using System;
using System.Collections.Generic;

namespace Seraf.ECS
{
    public delegate EventHandler<Entity> SystemArgs(object sender, Entity entity);

    public abstract class System //<T> where T : Node
    {
        //private List<T> nodes = new List<T>();
        //protected List<Node> nodes = new List<Node>();

        //public void AddNode(Node node)
        //{
        //    this.nodes.Add(node);
        //}

        public abstract void Update(float delta);

        public void AddEntity(Entity entity)
        {
            OnEntityAdded(entity);
        }

        public event SystemArgs EntityAdded;
        protected virtual void OnEntityAdded(Entity entity)
        {
            EntityAdded?.Invoke(this, entity);
        }


    }
}
