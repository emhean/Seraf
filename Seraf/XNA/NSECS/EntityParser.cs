using System;
using System.Collections.Generic;
using System.Xml;
using System.Reflection;
using Microsoft.Xna.Framework;
using Seraf.XNA.Tiled;

namespace Seraf.XNA.NSECS
{
    public class EntityParser
    {
        static IEnumerable<Type> GetTypesWithHelpAttribute(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(EntityBlueprint), true).Length > 0)
                {
                    yield return type;
                }
            }
        }

        Dictionary<string, Type> entityType_dict;
        public bool Debug { get; set; }

        public EntityParser()
        {
            entityType_dict = new Dictionary<string, Type>();
            foreach (var type in GetTypesWithHelpAttribute(System.Reflection.Assembly.GetExecutingAssembly()))
            {
                foreach (var att in Attribute.GetCustomAttributes(type))
                    if (att is EntityBlueprint blueprint)
                        entityType_dict.Add(blueprint.Type, type);
            }


            if (Debug)// Debugging
            {
                foreach (var foo in entityType_dict)
                    Console.WriteLine(foo.Key + ", " + foo.Value);
            }
        }

        public Entity CreateEntityFromTObject(TObject obj)
        {
            Type t = entityType_dict[obj.type];
            var ent = Activator.CreateInstance(t, obj.id, obj.pos, obj.size);
            return (Entity)ent;
        }

        public Entity CreateEntityFromFile(string filePath)
        {
            var xml = new XmlDocument();
            xml.Load(filePath);

            if(Debug)// Debugging
                Console.WriteLine("*** Importing Entity ***");

            string type = xml.DocumentElement.GetAttribute("type");
            int uuid = Convert.ToInt32(xml.DocumentElement.GetAttribute("id"));

            Vector2 pos = new Vector2(
                Convert.ToInt32(xml.DocumentElement.GetAttribute("x")),
                Convert.ToInt32(xml.DocumentElement.GetAttribute("y")));

            Vector2 size = new Vector2(
                float.Parse(xml.DocumentElement.GetAttribute("width")),
                float.Parse(xml.DocumentElement.GetAttribute("height")));

            if(Debug)// Debugging
                Console.WriteLine(type + ", " + uuid + ", " + pos + ", " + size);

            Type t = entityType_dict[type];
            var ent = Activator.CreateInstance(t, uuid, pos, size);

            if(Debug)// Debugging
                Console.WriteLine("*** Importing Done ***"); 

            return (Entity)ent;
        }
    }
}
