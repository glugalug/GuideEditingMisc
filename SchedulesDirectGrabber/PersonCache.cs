using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SchedulesDirectGrabber
{
    public class PersonCache
    {
        public static PersonCache instance { get { return instance_; } }

        public IEnumerable<MXFPerson> GetPersonEnumeration() { return people_.Values; }

        public void AddPerson(string id, string name)
        {
            if (people_.ContainsKey(id))
            {
                if (name != people_[id].name)
                {
                    Console.Write("Conflicting names for personID {0}, {1} vs {2}", id, people_[id].name, name);
                }
            }
            else people_[id] = new MXFPerson(id, name);
        }

        public string GetMXFPersonIdForKey(string key)
        {
            if (!personKeyIndex_.ContainsKey(key))
            {
                personKeyIndex_[key] = personKeyIndex_.Count + 1;
            }
            return "p" + personKeyIndex_[key].ToString();
        }

        private Dictionary<string, MXFPerson> people_ = new Dictionary<string, MXFPerson>();
        private Dictionary<string, int> personKeyIndex_ = new Dictionary<string, int>();


        private static PersonCache instance_ = new PersonCache();
        private PersonCache() { }

        public class MXFPerson
        {
            public MXFPerson() { }
            public MXFPerson(string key, string name) { key_ = key;  this.name = name; }

            [XmlAttribute("id")]
            public string mxfId { get { return PersonCache.instance.GetMXFPersonIdForKey(key_); } set { throw new NotImplementedException(); } }
            private readonly string key_;

            [XmlAttribute("name")]
            public string name { get; set; }

            [XmlAttribute("uid")]
            public string uid { get { return "!Person!GSD" + key_; } set { throw new NotImplementedException(); } }
        }
    }
}
