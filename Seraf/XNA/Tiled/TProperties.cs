using System.Collections;
using System.Collections.Generic;

namespace Seraf.XNA.Tiled
{
    public interface ITiledProperties
    {
        TProperties Properties { get; }
    }

    public class TProperties : IEnumerable<KeyValuePair<string, string>>
    {
        private Dictionary<string, string> properties;

        public TProperties()
        {
            this.properties = new Dictionary<string, string>();
        }

        /// <summary>
        /// Get/Set accesor. If set and property does not exist, will create it for you.
        /// </summary>
        public string this[string name]
        {
            get => properties[name];
            set
            {
                if (properties.ContainsKey(name))
                    properties[name] = value;
                else
                    properties.Add(name, value);
            }
        }

        public string GetPropertyValue(string name)
        {
            return properties[name];
        }

        public void AddProperty(string name, string value)
        {
            properties.Add(name, value);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach (var p in properties)
                yield return p;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var p in properties)
                yield return p;
        }
    }
}
