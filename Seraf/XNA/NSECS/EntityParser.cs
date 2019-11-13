using System;
using System.Collections.Generic;
using System.Xml;
using System.Reflection;
using Microsoft.Xna.Framework;
using Seraf.XNA.Tiled;
using System.Xml.Serialization;
using System.IO;

namespace Seraf.XNA.NSECS
{
    public class EntityParser : TypeDictionary<EntityBlueprint>
    {
        public EntityParser()
        {
            foreach(var type in this.EnumerateTypes())
            {
                foreach (var att in Attribute.GetCustomAttributes(type))
                    if (att is EntityBlueprint blueprint)
                    dict.Add(blueprint.TypeName, type);
            }

            if (Debug) // Debugging
            {
                foreach (var foo in dict)
                    Console.WriteLine(foo.Key + ", " + foo.Value);
            }
        }


        #region Shit
        //public static string SerializeToXml(object obj)
        //{
        //    XmlSerializer serializer = new XmlSerializer(obj.GetType());
        //    using (StringWriter writer = new StringWriter())
        //    {
        //        serializer.Serialize(writer, obj);
        //        return writer.ToString();
        //    }
        //}

        ///// <summary>
        ///// Saves an entity to a xml file.
        ///// </summary>
        //public void Save(Entity entity, string fileName)
        //{
        //    using (var writer = new System.IO.StreamWriter(fileName))
        //    {
        //        var type = entity.GetType();

        //        var serializer = new XmlSerializer(type);
        //        serializer.Serialize(writer, this);
        //        writer.Flush();
        //    }
        //}

        ///// <summary>
        ///// Load en entity from a XML file.
        ///// </summary>
        //public Entity Load(string fileName, string type)
        //{
        //    using (var stream = System.IO.File.OpenRead(fileName))
        //    {
        //        var t = this.GetTypeFromString(type);

        //        var serializer = new XmlSerializer(t.GetType());
        //        return (Entity)serializer.Deserialize(stream);
        //    }
        //}
        #endregion


        public Entity CreateEntityFromTObject(TObject obj)
        {
            Type t = dict[obj.type];
            var ent = Activator.CreateInstance(t, obj.id, obj.pos, obj.size);

            Entity _ent = (Entity)ent;
            _ent.Properties = obj.Properties;
            _ent.type = obj.type;
            _ent.name = obj.name;
            _ent.Initialize(); // Initialize

            return _ent;
        }


        public TObject CreateTObjectFromEntity(Entity entity)
        {
            TObject tobj = new TObject(entity.uuid, entity.name, entity.type, entity.pos, entity.size, entity.Properties);
            return tobj;
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

            Type t = dict[type];
            var ent = Activator.CreateInstance(t, uuid, pos, size);

            if(Debug)// Debugging
                Console.WriteLine("*** Importing Done ***"); 

            return (Entity)ent;
        }
    }
}
