using System.Collections.Generic;
using System.Linq;

namespace Seraf.ECS
{
    public class Engine
    {
        private List<System> systems;
        private List<Entity> entities;

        public Engine()
        {
            this.systems = new List<System>();
            this.entities = new List<Entity>();
        }

        public void Update(float delta)
        {
            for(int i = 0; i < systems.Count; ++i)
                systems[i].Update(delta);
        }

        public void ClearEntity(int uuid)
        {
            entities[uuid].ClearComponents();
        }

        public Entity GetEntity(int uuid)
        {
            return entities[uuid];
        }

        public Entity CreateEntity()
        {
            Entity entity = new Entity(entities.Count);
            entities.Add(entity);
            return entity;
        }

        public void RegisterSystem(System system)
        {
            systems.Add(system);
        }

        public void RemoveSystem() { }

        public T GetSystem<T>()
        {
            return (T)systems.Where(x => x is T);
        }
    }
}
