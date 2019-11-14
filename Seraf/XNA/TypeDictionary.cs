using System;
using System.Collections.Generic;
using System.Reflection;

namespace Seraf.XNA
{
    public class TypeDictionary<T>
    {
        public Dictionary<string, Type> dict;
        public bool Debug { get; set; }

        public TypeDictionary()
        {
            dict = new Dictionary<string, Type>();
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
