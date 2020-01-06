using Seraf.XNA.Tiled;
using System;
using System.Collections.Generic;
using System.Text;
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
        public bool DebugMode { get; set; } = true;

        /// <summary>
        /// Builds an entity from a TObject and loads entitys XML file.
        /// </summary>
        public Entity CreateFromFile(TObject obj, string filePath)
        {
            var xml = new XmlDocument();
            if( !filePath.EndsWith(".xml"))
                xml.Load(filePath + ".xml");
            else xml.Load(filePath);

            if (DebugMode)// Debugging
                Console.WriteLine("*** Building Entity ***");

            string att_type = xml.DocumentElement.GetAttribute("type");
            string att_isCollidble = xml.DocumentElement.GetAttribute("isCollidble");
            string att_isActive = xml.DocumentElement.GetAttribute("isActive");
            string att_isVisible = xml.DocumentElement.GetAttribute("isVisible");

            Type ent_type = typeof(Entity);
            var ent = Activator.CreateInstance(ent_type, obj.id, obj.pos, obj.size);

            Entity _ent = (ent as Entity);
            _ent.type = obj.type;
            _ent.name = obj.name;

            if (att_isCollidble.Equals("false"))
                _ent.IsCollidble = false;
            if (att_isActive.Equals("false"))
                _ent.IsActive = false;
            if (att_isVisible.Equals("false"))
                _ent.IsVisible = false;

            if (obj.Properties != null)
            {
                if(_ent.Properties != null)
                    _ent.Properties.CopyFrom(obj.Properties); // Copy the properties from TObject to the new entity
            }
            

            AddComponentsFromFile(_ent, filePath);

            if (DebugMode)// Debugging
                Console.WriteLine("*** Building Done ***");

            return (Entity)ent;
        }

        List<Component> Get_Init_ComponentsFromInclusion(Entity entity, string filePath, List<string> args)
        {
            var xml = new XmlDocument();
            if (!filePath.EndsWith(".xml"))
                xml.Load(filePath + ".xml");
            else xml.Load(filePath);

            List<Component> components = new List<Component>();
            string[] vars = new string[] { "$0", "$1", "$2", "$3", "$4", "$5", "$6", "$7", "$8", "$9" };

            foreach (XmlElement element in xml.DocumentElement.ChildNodes)
            {
                foreach (XmlAttribute att in element.Attributes)
                {
                    if (att.Value.StartsWith("$"))
                    {
                        int var_index = int.Parse(att.Value.Trim('$'));
                        att.Value = args[var_index]; // Set value to the list of variables

                        Console.WriteLine("Var: " + att.Value);
                    }
                }
                Type type = this.ComponentParser.GetTypeFromString(element.Name);
                var inst = Activator.CreateInstance(type, entity);
                (inst as Component).Initialize(element);
                components.Add((Component)inst);
            }

            return components;
        }

        public void AddComponentsFromFile(Entity entity, string filePath)
        {
            var xml = new XmlDocument();
            if (!filePath.EndsWith(".xml"))
                xml.Load(filePath + ".xml");
            else xml.Load(filePath);

            List<Component> components = new List<Component>();

            foreach (XmlElement element in xml.DocumentElement.ChildNodes)
            {
                if (element.Name.Equals("include"))
                {
                    string path = element.GetAttribute("file");

                    if (path.StartsWith("./"))
                    {
                        string[] s = filePath.Split('/');
                        StringBuilder sb = new StringBuilder();
                        // Add path 
                        // -1 to not add the last element which is filename
                        for (int i = 0; +i < (s.Length - 1); ++i)
                        {
                            sb.Append(s[i]);
                            sb.Append("/");
                        }
                        
                        sb.Append(path);

                        path = sb.ToString();
                    }

                    List<string> include_args = new List<string>();
                    foreach(XmlAttribute arg in element.Attributes)
                    {
                        if(arg.Name.Equals("include") == false)
                            include_args.Add(arg.Value);
                    }

                    foreach (Component component in Get_Init_ComponentsFromInclusion(entity, path, include_args))
                        entity.AddComponent(component);
                }
                else
                {
                    Type type = this.ComponentParser.GetTypeFromString(element.Name);

                    // Initialize
                    var inst = Activator.CreateInstance(type, entity);
                    (inst as Component).Initialize(element);

                    // Add
                    entity.AddComponent((Component)inst);
                }
            }
        }

        ///// <summary>
        ///// Creates a TObject from an Entity.
        ///// </summary>
        //public TObject CreateTObject(Entity entity)
        //{
        //    TObject tobj = new TObject(entity.uuid, entity.name, entity.type, entity.pos, entity.size, entity.Properties);
        //    return tobj;
        //}
    }
}
