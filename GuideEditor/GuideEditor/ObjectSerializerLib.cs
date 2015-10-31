using Microsoft.MediaCenter.Guide;
using Microsoft.MediaCenter.Store;
using System.Xml;

namespace ObjectSerializerLib
{
    class StoredObjectSerializer<T> where T:StoredObject
    {
        public virtual void SerializeToXmlElement<T>(T stored_object, XmlElement element) {
            StoredObject so = stored_object as StoredObject;
            element.Name = stored_object.GetType().Name;
            element.SetAttribute("Id", so.Id);
            element.SetAttribute("IsCached", so.IsCached);
            element.SetAttribute("IsIdOnly", so.IsIdOnly);
            element.SetAttribute("IsLatestVersion", so.IsLatestVersion);
            element.SetAttribute("LockCount", so.LockCount);
            // todo: serialize objectstore?
            element.SetAttribute("ObjectStore", so.ObjectStore.StoreName);
            element.SetAttribute("ObjectTypeId", so.ObjectType.TypeId);
            // todo: serialize provider?
            element.SetAttribute("Provider", so.Provider.Name);
            // todo: serialize restriction?
            element.SetAttribute("RestrictionId", so.Restriction.Id);
            element.SetAttribute("Revision", so.Revision);
            XmlDocument owner_doc = element.OwnerDocument;
            XmlElement uidsElement = owner_doc.CreateElement("UIDs");
            element.AppendChild(uidsElement);
            foreach (UId uid in so.UIds)
            {
                XmlElement uidElement = owner_doc.CreateElement("UID");
                SerializeToXmlElement<UId>(uid, uidElement);
                uidsElement.AppendChild(uidElement);
                uidElement.SetAttribute("IdValue", uid.IdValue);
            }
        }
    }
}