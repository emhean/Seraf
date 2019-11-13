using Seraf.XNA.Tiled;
using System;
using System.Xml;

namespace Seraf.XNA.NSECS
{
    public class EntityBuilder
    {
        public EntityBuilder()
        {
            this.ComponentParser = new ComponentParser();
        }

        ComponentParser ComponentParser { get; set; }
        public bool Debug { get; set; } = true;

        public Entity BuildFromFile(TObject obj, string filePath)
        {
            var xml = new XmlDocument();
            if( !filePath.EndsWith(".xml"))
                xml.Load(filePath + ".xml");
            else xml.Load(filePath);

            if (Debug)// Debugging
                Console.WriteLine("*** Building Entity ***");

            string att_type = xml.DocumentElement.GetAttribute("type");

            //Type ent_type = parser.dict[obj.type];
            Type ent_type = typeof(Entity);
            var ent = Activator.CreateInstance(ent_type, obj.id, obj.pos, obj.size);

            Entity _ent = (ent as Entity);
            _ent.type = obj.type;
            _ent.name = obj.name;

            foreach (XmlElement element in xml.DocumentElement.ChildNodes)
            {
                Type type = this.ComponentParser.GetTypeFromString(element.Name);

                // Initialize
                var inst = Activator.CreateInstance(type, ent);
                (inst as Component).Initialize(element);
                (ent as Entity).AddComponent((Component)inst);
                Console.WriteLine(inst.ToString());
            }

            //if (Debug)// Debugging
            //    Console.WriteLine(type + ", " + uuid + ", " + pos + ", " + size);

            if (Debug)// Debugging
                Console.WriteLine("*** Building Done ***");

            return (Entity)ent;
        }
    }
}
