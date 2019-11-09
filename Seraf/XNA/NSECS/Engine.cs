using System.Collections.Generic;

namespace NSECS
{
    public class Engine
    {
        public List<Entity> entities;
        UUID _uuid;

        public Engine()
        {
            this.entities = new List<Entity>();
            this._uuid = new UUID();
        }

        public Scene Scene { get; set; }


        public Entity GetEntity(int uuid)
        {
            for (int i = 0; i < entities.Count; ++i)
            {
                if (entities[i].uuid == uuid)
                    return entities[i];
            }
            throw new System.Exception("Entity not found!");
        }

        public void AddEntity(Entity entity)
        {
            this.entities.Add(entity);
            _uuid.GetUUID(entity.uuid);

            System.Console.WriteLine("Added Entity of UUID: " + entity.uuid);
        }

        public void RemoveEntity(int uuid)
        {
            for (int i = 0; i < entities.Count; ++i)
                if (entities[i].uuid == uuid)
                {
                    entities.RemoveAt(i);
                    _uuid.FreeUUID(uuid);

                    System.Console.WriteLine("Removed Entity of UUID: " + uuid);
                }
        }

        public void Update(float delta)
        {
            for (int i = 0; i < entities.Count; ++i)
            {
                for (int x = 0; x < entities.Count; ++x)
                {
                    if (i != x)
                    {
                        if (entities[i].body.collider is BoxCollider b)
                        {
                            if (entities[x].body.collider is BoxCollider b2)
                            {
                                if (b.CheckIfColliding(b2))
                                {
                                    b.UpdateCollision(b2);
                                }
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < entities.Count; ++i)
            {
                if (entities[i].HasBody())
                    entities[i].body.Update(delta);
                
                for (int j = 0; j < entities[i].components.Count; ++j)
                {
                    entities[i].components[j].Update(delta);

                    if (entities[i].components[j].Expired) // Remove expired components
                    {
                        entities[i].RemoveComponent(entities[i].components[j]);
                    }
                }

                if (entities[i].Expired) // Remove entity if expired.
                    this.RemoveEntity(entities[i].uuid);
            }
        }

        public void Render(Scene scene)
        {
            for (int i = 0; i < entities.Count; ++i)
            {
                if (entities[i].HasBody())
                {
                    if (entities[i].body.HasSprite())
                        entities[i].body.Render(scene);
                }

                foreach (var c in entities[i].components)
                {
                    c.Render(scene);
                }
            }
        }
    }
}
