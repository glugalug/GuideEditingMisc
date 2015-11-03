using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
namespace SchedulesDirectGrabber
{
    public class KeywordCache
    {
        private KeywordCache()
        {
            keywords = new MXFKeywords();
            keywordGroups = new MXFKeywordGroups(keywords);
        }
        public static KeywordCache instance { get { return instance_; } }
        private static KeywordCache instance_ = new KeywordCache();

        public readonly MXFKeywords keywords;
        public readonly MXFKeywordGroups keywordGroups;

        [DataContract]
        public class MXFKeyword
        {
            public MXFKeyword() { throw new NotImplementedException(); }
            internal MXFKeyword(string id, string word)
            {
                this.id = id;
                this.word = word;
            }

            [DataMember(IsRequired = true), XmlAttribute(AttributeName = "id")]
            public string id { get; set; }

            [DataMember(IsRequired = true), XmlAttribute(AttributeName = "word")]
            public string word { get; set; }
        }

        [DataContract]
        public class MXFKeywordGroup
        {
            public MXFKeywordGroup() { throw new NotImplementedException(); }
            internal MXFKeywordGroup(string groupName, string uid, string keywords)
            {
                this.groupName = groupName;
                this.uid = uid;
                this.keywords = keywords;
            }
            //private readonly Keywords.TopLevelKeywordInfo keywordInfo_;

            [DataMember(IsRequired = true), XmlAttribute(AttributeName = "groupName")]
            public string groupName { get; set; }

            [DataMember(IsRequired = true), XmlAttribute(AttributeName = "uid")]
            public string uid { get; set; }

            [DataMember(IsRequired = true), XmlAttribute(AttributeName = "keywords")]
            public string keywords { get; set; }
        }

        [DataContract]
        public class MXFKeywords
        {
            [DataMember, XmlArray(ElementName = "Keywords"), XmlArrayItem("Keyword")]
            public IEnumerable<MXFKeyword> keywords {
                get
                {
                    foreach (var topLevel in topLevelKeywordIndex_.Values)
                    {
                        yield return new MXFKeyword(topLevel.GetId(), topLevel.GetKeyword());
                        foreach (var secondLevel in topLevel.GetSecondLevelKeywordEnumeration())
                        {
                            yield return secondLevel;
                        }
                    }
                }
            }

            public IEnumerable<MXFKeywordGroup> GetKeywordGroupEnumeration()
            {
                foreach (var topLevel in topLevelKeywordIndex_.Values)
                {
                    yield return topLevel.ToKeywordGroup();
                }
            }

            public string GetTopLevelKeywordId(string topLevel)
            {
                return GetTopLevelKeywordInfo(topLevel).GetId();
            }

            public string GetSecondLevelKeywordId(string topLevel, string secondLevel)
            {
                return GetTopLevelKeywordInfo(topLevel).GetSecondLevelId(secondLevel);
            }
            public void AddKeywords(string topLevel, IEnumerable<string> secondLevel)
            {
                if (!topLevelKeywordIndex_.ContainsKey(topLevel))
                {
                    topLevelKeywordIndex_[topLevel] = new TopLevelKeywordInfo(topLevel, topLevelKeywordIndex_.Count + 1);
                }
                TopLevelKeywordInfo keywordInfo = topLevelKeywordIndex_[topLevel];
                foreach (string keyword in secondLevel)
                {
                    keywordInfo.AddSecondLevelKeyword(keyword);
                }
            }

            private TopLevelKeywordInfo GetTopLevelKeywordInfo(string keyword)
            {
                if (!topLevelKeywordIndex_.ContainsKey(keyword))
                {
                    topLevelKeywordIndex_[keyword] = new TopLevelKeywordInfo(keyword, topLevelKeywordIndex_.Count + 1);
                }
                return topLevelKeywordIndex_[keyword];
            }

            Dictionary<string, TopLevelKeywordInfo> topLevelKeywordIndex_ = new Dictionary<string, TopLevelKeywordInfo>();
            internal class TopLevelKeywordInfo
            {
                public TopLevelKeywordInfo(string keyword, int id)
                {
                    keyword_ = keyword;
                    id_ = id;
                    AddSecondLevelKeyword("All");
                }

                public MXFKeywordGroup ToKeywordGroup()
                {
                    string groupName = this.GetId();
                    string uid = string.Format("!KeywordGroup!GSD{0}", keyword_.Replace(" ", ""));
                    List<string> kvIds = new List<string>();
                    foreach (int key in secondLevelKeywordIndex_.Values)
                    {
                        kvIds.Add(SecondLevelIndexToID(key));
                    }
                    kvIds.Sort();
                    string keywords = string.Join(",", kvIds);
                    return new MXFKeywordGroup(groupName, uid, keywords);
                }

                internal IEnumerable<MXFKeyword> GetSecondLevelKeywordEnumeration()
                {
                    foreach(var secondLevel in secondLevelKeywordIndex_)
                    {
                        yield return new MXFKeyword(GetSecondLevelId(secondLevel.Key), secondLevel.Key);
                    }
                }

                internal string GetId() { return string.Format("k{0}", id_); }
                internal string GetKeyword() { return keyword_; }
                internal string GetSecondLevelId(string keyword)
                {
                    AddSecondLevelKeyword(keyword);
                    return SecondLevelIndexToID(secondLevelKeywordIndex_[keyword]);
                }
                internal void AddSecondLevelKeyword(string keyword)
                {
                    if (secondLevelKeywordIndex_.ContainsKey(keyword)) return;
                    secondLevelKeywordIndex_[keyword] = secondLevelKeywordIndex_.Count + 1;
                }
                private string SecondLevelIndexToID(int index)
                {
                    if (index < 100) return string.Format("k{0}", id_ * 100 + index);
                    if (index < 10000) return string.Format("k{0}", id_ * 10000 + index);
                    return string.Format("k{0}", id_ * 1000000 + index);
                }
                private int id_;
                private Dictionary<string, int> secondLevelKeywordIndex_ = new Dictionary<string, int>();
                private readonly string keyword_;
            }
        }

        [DataContract]
        public class MXFKeywordGroups
        {
            public MXFKeywordGroups(MXFKeywords keywords)
            {
                keywords_ = keywords;
            }

            [IgnoreDataMember]
            private MXFKeywords keywords_;

            [DataMember, XmlArray(ElementName = "KeywordGroup")]
            public IEnumerable<MXFKeywordGroup> keywordGroups { get { return keywords_.GetKeywordGroupEnumeration(); } }
        }

    }
}
