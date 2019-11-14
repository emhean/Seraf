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
        public bool Debug { get; set; } = true;

        /// <summary>
        /// Builds an Entity from a TObject and loads its XML file.
        /// </summary>
        public Entity CreateFromFile(TObject obj, string filePath)
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

            AddComponentsFromFile(_ent, filePath);

            if (Debug)// Debugging
                Console.WriteLine("*** Building Done ***");

            return (Entity)ent;

            //foreach (XmlElement element in xml.DocumentElement.ChildNodes)
            //{
            //    if (element.Name.Equals("include"))
            //    {
            //        foreach(Component component in CreateFromFile(obj, "Content/" + element.GetAttribute("file")).components)
            //        {
            //            _ent.AddComponent(component);
            //        }
            //    }
            //    else
            //    {
            //        Type type = this.ComponentParser.GetTypeFromString(element.Name);

            //        // Initialize
            //        var inst = Activator.CreateInstance(type, ent);
            //        (inst as Component).Initialize(element);
            //        (ent as Entity).AddComponent((Component)inst);
            //        Console.WriteLine(inst.ToString());
            //    }
            //}
        }

        public void AddComponentsFromFile(Entity entity, string filePath, List<string> args = null)
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
                    bool foo = false;
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
                        foo = true;
                    }

                    List<string> vs = null;
                    int x = 0;
                    foreach(XmlAttribute arg in element.Attributes)
                    {
                        if(arg.Name.Equals("include") == false)
                        {
                            if (vs == null)
                                vs = new List<string>();

                            vs.Add(arg.Value);
                        }
                    }

                    if(foo)
                    {
                        AddComponentsFromFile(entity, path, vs); // Absolute path in Content
                    }
                    else
                    {
                        AddComponentsFromFile(entity, "Content/" + path, vs); // Absolute path in Content
                    }

                    

                }
                else
                {
                    Type type = this.ComponentParser.GetTypeFromString(element.Name);

                    if(args != null)
                    {
                        string[] a = new string[] { "$0", "$1", "$2", "$3", "$4", "$5", "$6", "$7", "$8", "$9" };

                        foreach (XmlAttribute val in element.Attributes)
                        {
                            foreach(var foobar in a)
                            {
                                if (val.Value.Equals(foobar))
                                {
                                    foreach(var arg in args)
                                    {
                                        if (arg.Equals(foobar))
                                            element.SetAttribute(val.Name, arg); //val.Value = arg;
                                    }
                                }
                            }
                            
                        }
                    }
                    

                    // Initialize
                    var inst = Activator.CreateInstance(type, entity);
                    (inst as Component).Initialize(element);

                    // Add
                    entity.AddComponent((Component)inst);
                }
            }
        }
        /// <summary>
        /// Creates a TObject from an Entity.
        /// </summary>
        public TObject CreateTObject(Entity entity)
        {
            TObject tobj = new TObject(entity.uuid, entity.name, entity.type, entity.pos, entity.size, entity.Properties);
            return tobj;
        }
    }
}
