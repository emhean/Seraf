using System;
using System.Collections.Generic;
using System.Xml;
using NSECS;
using System.Reflection;
using Microsoft.Xna.Framework;

namespace Seraf.ScriptingEngine
{
    public class EntityParser
    {
        Dictionary<string, Type> entityType_dict;

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

        public EntityParser()
        {
            entityType_dict = new Dictionary<string, Type>();
            foreach (var type in GetTypesWithHelpAttribute(System.Reflection.Assembly.GetExecutingAssembly()))
            {
                foreach (var att in Attribute.GetCustomAttributes(type))
                    if (att is EntityBlueprint blueprint)
                        entityType_dict.Add(blueprint.Type, type);
            }

            // Debugging
            foreach(var foo in entityType_dict)
                Console.WriteLine(foo.Key + ", " + foo.Value);
        }


        public Entity CreateEntityFromFile(string filePath)
        {
            var xml = new XmlDocument();
            xml.Load(filePath);

            Console.WriteLine("*** Importing Entity ***");

            string type = xml.DocumentElement.GetAttribute("type");
            int uuid = Convert.ToInt32(xml.DocumentElement.GetAttribute("id"));

            Vector2 pos = new Vector2(
                Convert.ToInt32(xml.DocumentElement.GetAttribute("x")),
                Convert.ToInt32(xml.DocumentElement.GetAttribute("y")));

            Vector2 size = new Vector2(
                float.Parse(xml.DocumentElement.GetAttribute("width")),
                float.Parse(xml.DocumentElement.GetAttribute("height")));

            Console.WriteLine(type + ", " + uuid + ", " + pos + ", " + size);

            Type t = entityType_dict[type];
            var ent = Activator.CreateInstance(t, uuid, pos, size);

            Console.WriteLine("*** Importing Done ***"); 

            return (Entity)ent;
        }
    }
}
