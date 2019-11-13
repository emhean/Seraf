using System;
using System.Collections.Generic;
using System.Reflection;

namespace Seraf.XNA.NSECS
{
    public class TypeDictionary<T>
    {
        public Dictionary<string, Type> dict;
        public bool Debug { get; set; }


        public TypeDictionary()
        {
            dict = new Dictionary<string, Type>();

            //foreach (Type type in GetTypesWithHelpAttribute<T>(Assembly.GetExecutingAssembly()))
            //{
            //    foreach (var att in Attribute.GetCustomAttributes(type))
            //        if (att is T blueprint)
            //            dict.Add(blueprint.TypeName, type);
            //}
        }

        protected IEnumerable<Type> EnumerateTypes()
        {
            var assembly = Assembly.GetExecutingAssembly();

            foreach (Type type in assembly.GetTypes())
                if (type.GetCustomAttributes(typeof(T), true).Length > 0)
                    yield return type;
        }

        public Type GetTypeFromString(string type)
        {
            return dict[type];
        }

        public string GetStringFromType(Type type)
        {
            foreach (var m in dict)
                if (m.Value.Equals(type))
                    return m.Key;

            throw new Exception("Type not found!");
        }
    }
}
