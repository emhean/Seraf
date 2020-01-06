using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Seraf.XNA.Tiled
{
    public interface ITiledProperties
    {
        TProperties Properties { get; }
    }

    public enum TPropertyValueType
    {
        ValueType_String,
        ValueType_Int,
        ValueType_Bool,
        ValueType_Float,
        ValueType_File,
        ValueType_Color
    }

    [Serializable]
    [XmlType("property")]
    public struct TProperty
    {
        [XmlAttribute("name")]
        /// <summary>
        /// The name of this property.
        /// </summary>
        public string Name { get; set; }

        [XmlAttribute("value")]
        /// <summary>
        /// The value of this property. 
        /// </summary>
        public string Value { get; set; }

        [XmlAttribute("type")]
        /// <summary>
        /// The type of this value. Default value type is string because it's parsable at most times.
        /// </summary>
        public string ValueType { get; set; }

        public TProperty(string name, string value)
        {
            this.Name = name;
            this.Value = value;
            this.ValueType = "string";
        }

        public TProperty(string name, string value, string valueType)
        {
            this.Name = name;
            this.Value = value;
            this.ValueType = valueType;
        }

        public override string ToString()
        {
            return string.Concat("Name:", Name, " | Value:", Value);
        }

        public TPropertyValueType GetValueType()
        {
            switch(ValueType)
            {
                case "string": return TPropertyValueType.ValueType_String;
                case "int": return TPropertyValueType.ValueType_Int;
                case "bool": return TPropertyValueType.ValueType_Bool;
                case "float": return TPropertyValueType.ValueType_Float;
                case "file": return TPropertyValueType.ValueType_File;
                case "color": return TPropertyValueType.ValueType_Color;
            }

            // Return string in case of weird type. Strings can hold any type of values as a string.
            return TPropertyValueType.ValueType_String;
        }

        #region Setters
        public void SetName(string name)
        {
            this.Name = name;
        }
        public void SetValue(string value)
        {
            this.Value = value;
        }
        public void SetValueType(string valueType)
        {
            this.ValueType = valueType;
        }
        #endregion
    }

    [Serializable]
    [XmlType("properties")]
    public class TProperties
    {
        [XmlElement("property")]
        public List<TProperty> properties; 

        public TProperties()
        {
            properties = new List<TProperty>();
        }


        /// <summary>
        /// Get/Set accesor. If set and property does not exist, will create it for you.
        /// </summary>
        public string this[string name]
        {
            get => GetPropertyValue(name);
            set
            {
                for(int i = 0; i < GetPropertyCount(); ++i)
                {
                    if(properties[i].Name.Equals(name))
                    {
                        properties[i].SetValue(value);
                        return; // We return here!
                    }
                }

                // We add it as a new property because it was not found in the for loop.
                properties.Add(new TProperty(name, value));
            }
        }

        public int GetPropertyCount()
        {
            return properties.Count;
        }

        public bool ContainsProperty(string name)
        {
            for(int i = 0; i < properties.Count; ++i)
            {
                if (properties[i].Name.Equals(name))
                    return true;
            }

            return false;
        }

        public string GetPropertyValue(string name)
        {
            for (int i = 0; i < properties.Count; ++i)
            {
                if (properties[i].Name.Equals(name))
                    return properties[i].Value;
            }

            throw new Exception("Property does not exist!");
        }
        public void SetPropertyValue(string name, string newValue)
        {
            for (int i = 0; i < GetPropertyCount(); ++i)
            {
                if (properties[i].Name.Equals(name))
                    properties[i].SetValue(newValue);
            }

            throw new Exception("Property does not exist!");
        }

        public TProperty GetProperty(string name)
        {
            for (int i = 0; i < properties.Count; ++i)
            {
                if (properties[i].Name.Equals(name))
                    return properties[i];
            }

            throw new Exception("Property does not exist!");
        }

        public TProperty GetProperty(int index)
        {
            return properties[index];
        }

        public void Add(string name, string value)
        {
            properties.Add(new TProperty(name, value));
        }

        public void Add(string name, string value, string valueType)
        {
            properties.Add(new TProperty(name, value, valueType));
        }

        public void CopyFrom(TProperties properties)
        {
            this.properties.Clear();
            for(int i = 0; i < properties.GetPropertyCount(); ++i)
            {
                this.properties.Add(properties.GetProperty(i));
            }
        }
    }
}
