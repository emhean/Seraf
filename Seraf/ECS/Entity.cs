using System.Collections.Generic;
using System.Linq;

namespace Seraf.ECS
{
    public class Entity
    {
        readonly int uuid;
        private List<Component> components;
        
        public Entity(int uuid)
        {
            this.uuid = uuid;
            this.components = new List<Component>();
        }

        public void AddComponent(Component component)
        {
            this.components.Add(component);
        }

        public void RemoveComponent() { }

        /// <summary>
        /// Get component from entity. Returns null if no component of that type exists.
        /// </summary>
        public T GetComponent<T>() where T : Component
        {
            //var comp = components.Find(x => (x is T));
            //return (T)comp;
            for (int i = 0; i < components.Count; ++i)
            {
                if (components[i] is T)
                    return (T)components[i];
            }

            return null;
            //throw new Exception("Crash! Expected component was not found!");
        }

        public void ClearComponents()
        {
            components.Clear();
        }
    }
}
