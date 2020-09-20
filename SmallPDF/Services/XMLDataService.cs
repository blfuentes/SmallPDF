using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SmallPDF.Services
{
    public class XMLDataService
    {
        public T GetElements<T>(string filepath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            FileStream fs = new FileStream(filepath, FileMode.Open);
            TextReader reader = new StreamReader(fs);
            return (T)serializer.Deserialize(reader);
        }

        public void AddElement<T>(string filepath, string collection, string rootName, Dictionary<string, object> attributes)
        {
            XDocument doc = XDocument.Load(filepath);
            XElement root = new XElement(rootName);
            foreach (var att in attributes)
            {
                root.Add(new XAttribute(att.Key, att.Value));
            }
            doc.Element(collection).Add(root);
            doc.Save(filepath);
        }

        public void UpdateElement<T>(string filepath, string rootName, string idProperty, string idValue, Dictionary<string, object> attributes)
        {
            XDocument doc = XDocument.Load(filepath);
            var element = doc.Descendants(rootName)
                            .Single(r => r.Attribute(idProperty).Value == idValue);
            if(element != null)
            {
                foreach (var att in attributes)
                {
                    element.Attribute(att.Key).Value = att.Value.ToString();
                }
            }
            doc.Save(filepath);
        }

        public void RemoveElement<T>(string filepath, string rootName, string idProperty, string idValue)
        {
            XDocument doc = XDocument.Load(filepath);
            var element = doc.Descendants(rootName)
                            .Single(r => r.Attribute(idProperty).Value == idValue);
            if (element != null)
            {
                element.Remove();
                doc.Save(filepath);
            }
        }

    }
}
