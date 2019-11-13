using System;

namespace Seraf.XNA.NSECS
{
    public class ComponentParser : TypeDictionary<ComponentBlueprint>
    {
        public ComponentParser()
        {
            foreach (var type in this.EnumerateTypes())
            {
                foreach (var att in Attribute.GetCustomAttributes(type))
                    if (att is ComponentBlueprint blueprint)
                        dict.Add(blueprint.TypeName, type);
            }

            if (Debug) // Debugging
            {
                foreach (var foo in dict)
                    Console.WriteLine(foo.Key + ", " + foo.Value);
            }
        }
    }
}
