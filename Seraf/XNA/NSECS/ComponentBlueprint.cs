using System;

namespace Seraf.XNA.NSECS
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ComponentBlueprint : Attribute
    {
        public ComponentBlueprint(string typeName)
        {
            this.TypeName = typeName;
        }

        public string TypeName { get; private set; }

        //public virtual void InitializedFromXML(XmlElement e) { }
    }
}
