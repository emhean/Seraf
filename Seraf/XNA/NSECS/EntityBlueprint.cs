using System;

namespace Seraf.XNA.NSECS
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class EntityBlueprint : Attribute
    {
        public EntityBlueprint(string type)
        {
            this.Type = type;
        }

        public string Type { get; }
    }

    //[AttributeUsage(AttributeTargets.Method)]
    //public class EntityTemplate : Attribute
    //{
    //    public EntityTemplate(string type)
    //    {
    //        this.Type = type;
    //    }

    //    public string Type { get; }
    //}

    //public delegate Entity EntityTemplateDelegate();
}
