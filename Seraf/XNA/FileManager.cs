using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Seraf.XNA
{
    public class FileManager<T>
    {
        /// <summary>
        /// Exports serializable object to a binary XML file.
        /// </summary>
        public void Export(string filePath, T obj)
        {
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                IFormatter f = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                f.Serialize(stream, obj);
            }
        }

        /// <summary>
        /// Imports a binary XML file and creates an instance of T, which should be serializable.
        /// </summary>
        public T Import(string filePath)
        {
            T obj;
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IFormatter f = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                obj = (T)f.Deserialize(stream);
            };
            return obj;
        }

        /// <summary>
        /// Imports a raw-text XML file and creates an instance of T, which should be serializable.
        /// </summary>
        public T ImportRaw(string filePath)
        {
            T obj;
            using (var reader = new StreamReader(filePath))
            {
                var deseriz = new XmlSerializer(typeof(T));
                obj = (T)deseriz.Deserialize(reader);
            };
            return obj;
        }
    }
}