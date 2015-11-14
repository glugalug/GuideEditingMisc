using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace SchedulesDirectGrabber
{
    using System.Collections;
    using SelectionAlgorithm = ConfigManager.SDGrabberConfig.ImageSelectionOptions.SelectionAlgorithm;

    public class ImageCache
    {
        public static ImageCache instance {get{return instance_;} }
        private static ImageCache instance_ = new ImageCache();

        public static string GetSDImageIDByProgramID(string programID) { return Misc.LimitString(programID, 10); }

        // get an MXF image ID for a programID or channel serviceID.
        public string FindOrCreateMXFImageId(string otherId)
        {
            if (!imageIdIndex_.ContainsKey(otherId))
            {
                imageIdIndex_[otherId] = imageIdIndex_.Count + 1;
            }
            return "i" + imageIdIndex_[otherId].ToString();
        }

        // numbers assigned to each imageID for counting them in the MXF.
        // In some cases the key may be an url rather than an id.
        private Dictionary<string, int> imageIdIndex_ = new Dictionary<string, int>();
        
        //TODO: Match EP* images Ids against SH* and vice versa.
        public bool isImageLoaded(string imageID)
        {
            return imageCache_.ContainsKey(imageID);
        }

        public IEnumerable<MXFGuideImage> GetMXFGuideImages()
        {
            // Enumerate images with full info
            foreach (var kv in imageCache_)
            {
                yield return new MXFGuideImage(FindOrCreateMXFImageId(kv.Key), kv.Value.GetBestImage().URL);
            }

            // Enumerate images with just url (station logos)
            foreach (string imageId in imageIdIndex_.Keys)
            {
                if (!imageCache_.ContainsKey(imageId))
                {
                    yield return new MXFGuideImage(FindOrCreateMXFImageId(imageId), imageId);
                }
            }
        }

        public IDictionary<string, SDProgramImageResponse> GetImages(ISet<string> imageIDs)
        {
            ISet<string> imagesNeeded = new HashSet<string>(imageIDs.Except(imageCache_.Keys));
            IDictionary<string, SDProgramImageResponse> dbImages = DBManager.instance.GetImagesByIds(imagesNeeded);
            foreach (var kv in dbImages)
                imageCache_[kv.Key] = kv.Value;
            imagesNeeded.ExceptWith(dbImages.Keys);
            foreach(var imageResponse in DownloadProgramImages(imagesNeeded))
            {
                imageCache_[imageResponse.programID] = imageResponse;
            }
            return imageCache_;
        }

        public IEnumerable<SDProgramImageResponse> GetAllImagesForPrograms(IEnumerable<ProgramCache.DBProgram> programs)
        {
            HashSet<string> imageIDs = new HashSet<string>();
            foreach (var program in programs)
                if (program.hasImages)
                    imageIDs.Add(GetSDImageIDByProgramID(program.programID));
            return GetImages(imageIDs).Values;
        }
        
        private Dictionary<string, SDProgramImageResponse> imageCache_ = new Dictionary<string, SDProgramImageResponse>();

        private IEnumerable<SDProgramImageResponse> DownloadProgramImages(IEnumerable<string> programIDs)
        {
            const int kBatchSize = 500;
            Console.WriteLine("Downloading list of program images to download.");
            HashSet<string> idsToFetch = new HashSet<string>();
            foreach (string programID in programIDs)
            {
                string key = GetSDImageIDByProgramID(programID);
                if (!cachedImageData_.ContainsKey(key)) idsToFetch.Add(key);
            }

            Console.WriteLine("Downloading program image URLs");
            List<string> batchProgramIds = new List<string>();

            List<SDProgramImageResponse> fetchedImages = new List<SDProgramImageResponse>();
            Func<int> DownloadBatch = new Func<int>(() => {
                List<SDProgramImageResponse> responses = JSONClient.GetJSONResponse<List<SDProgramImageResponse>>(
                    UrlBuilder.BuildWithAPIPrefix("/metadata/programs/"), batchProgramIds, SDTokenManager.token_manager.token);
                foreach(var response in responses)
                {
                    if (!response.OK())
                    {
                        Console.WriteLine("Some images failed to download, code: {0} programID: {2} message: {1}",
                            response.code, response.message, response.programID);
                        continue;
                    }
                    fetchedImages.Add(response);
                }
                batchProgramIds.Clear();
                return 0;
            });

            foreach(var programID in idsToFetch)
            {
                batchProgramIds.Add(programID);
                if (batchProgramIds.Count >= kBatchSize) DownloadBatch();
            }
            if (batchProgramIds.Count > 0) DownloadBatch();
            DBManager.instance.SaveProgramImages(fetchedImages);
            return fetchedImages;
        }

        private Dictionary<string, SDProgramImageResponse> cachedImageData_ = new Dictionary<string, SDProgramImageResponse>();

        [DataContract]
        public class SDProgramImageResponse
        {

            [DataMember(Name = "programID", IsRequired = true)]
            public string programID { get; set; }
            [DataMember(Name = "code")]
            public int code { get; set; }
            [DataMember(Name = "message")]
            public string message { get; set; }
            [IgnoreDataMember]
            public List<SDProgramImageData> imageData;
            [IgnoreDataMember]
            private object data_;
            [DataMember(Name = "data", IsRequired = true)]
            public object data
            {
                get { return data_; }
                set
                {
                    try
                    {
                        data_ = value;
                        imageData = JSONClient.Deserialize<List<SDProgramImageData>>(data_.ToString());
                        data_ = imageData;
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine("Failed to deserialize image info: {0}", value.ToString());
                    }
                }
            }
            

            public bool OK() { return code == 0 && imageData?.Count > 0; }

            public SDProgramImageData GetBestImage()
            {
                var options = ConfigManager.config.imageSelection;
                SDProgramImageData bestImage = null;
                Func<SDProgramImageData, bool> IsBetterImage =
                    delegate (SDProgramImageData image)
                {
                    if (bestImage == null) return true;
                    switch (options.selectionAlgorithm)
                    {
                        case SelectionAlgorithm.closestHeight:
                            return Math.Abs(image.height - options.preferredShowImageHeight) < Math.Abs(bestImage.height - options.preferredShowImageHeight);
                        case SelectionAlgorithm.closestWidth:
                            return Math.Abs(image.width - options.preferredShowImageWidth) < Math.Abs(bestImage.width - options.preferredShowImageWidth);
                        case SelectionAlgorithm.closestPixels:
                            return Math.Abs(image.totalPixels - options.preferredShowImagePixels) < Math.Abs(bestImage.totalPixels - options.preferredShowImagePixels);
                        default:
                            throw new Exception(string.Format("Unrecognized image selection algorithm {0}", options.selectionAlgorithm));
                    }
                };

                foreach(var image in imageData)
                    if (IsBetterImage(image)) bestImage = image;

                return bestImage;
            }

        }

        [DataContract]
        public class SDProgramImageData
        {
            [DataMember(Name = "width", IsRequired = true)]
            public string widthStr { get; set; }
            [DataMember(Name = "height", IsRequired = true)]
            public string heightStr { get; set; }
            [DataMember(Name = "caption")]
            public SDProgramImageCaption caption { get; set; }
            [DataMember(Name = "uri", IsRequired = true)]
            public string uri { get; set; }
            [DataMember(Name = "size")]
            public string size { get; set; }
            [DataMember(Name = "aspect")]
            public string aspect { get; set; }
            [DataMember(Name = "category")]
            public string category { get; set; }
            [DataMember(Name = "text")]
            public string text { get; set; }
            [DataMember(Name = "primary")]
            public string primary { get; set; }
            [DataMember(Name = "tier")]
            public string tier { get; set; }
            [DataMember(Name ="rootId")]
            public string rootId { get; set; }

            [IgnoreDataMember]
            public string URL
            {
                get
                {
                    if (uri.StartsWith("http")) return uri;
                    return UrlBuilder.BuildWithAPIPrefix("/image/" + uri);
                }
            }

            [IgnoreDataMember]
            public int width { get { return int.Parse(widthStr); } }
            [IgnoreDataMember]
            public int height { get { return int.Parse(heightStr); } }
            [IgnoreDataMember]
            public long totalPixels { get { return width * height; } }

            [DataContract]
            public class SDProgramImageCaption
            {
                [DataMember(Name = "content")]
                public string content { get; set; }
                [DataMember(Name = "lang")]
                public string lang { get; set; }
            }
        }

        private ImageCache() { }

        public class MXFGuideImage
        {
            public MXFGuideImage() { }
            public MXFGuideImage(string imageID, string imageUrl)
            {
                mxfId = imageID;
                this.imageUrl = imageUrl;
            }

            [XmlAttribute("id")]
            public string mxfId { get; set; }

            [XmlAttribute("imageUrl")]
            public string imageUrl { get; set; }
        }
    }
}
