using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Seraf.XNA.Tiled
{
    [Serializable()]
    [XmlType("objectgroup")]
    public class TObjectGroup : ITiledProperties
    {
        [XmlAttribute("id")]
        public int id;

        [XmlAttribute("name")]
        public string name;

        [XmlElement("object")]
        public List<TObject> objects;

        [XmlElement("properties")]
        public TProperties Properties { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Concat("[Group] id:", id, ", name:", name, ", count:", objects.Count));
            foreach (var o in objects)
            {
                sb.AppendLine(string.Concat("in group: {\n", o.ToString(), "}"));
            }

            return sb.ToString();
        }
    }
}
