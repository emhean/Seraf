using System;

namespace Seraf.XNA.NSECS
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class EntityBlueprint : Attribute
    {
        public EntityBlueprint(string typeName)
        {
            this.TypeName = typeName;
        }

        public string TypeName { get; }
    }
}
