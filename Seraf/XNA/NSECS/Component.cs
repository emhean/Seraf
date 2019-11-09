namespace Seraf.XNA.NSECS
{
    public class Component
    {
        public Component() { }
        public virtual void Update(float delta) { }
        public virtual void Render(Scene scene) { }

        /// <summary>
        /// The entity instance that holds this component.
        /// </summary>
        public Entity Entity { get; private set; }

        /// <summary>
        /// If this is true then the component is expired and will be removed. Use Expire() to set true.
        /// </summary>
        public bool Expired { get; private set; }

        public void Expire()
        {
            this.Expired = true;
        }
        
        /// <summary>
        /// Set the Entity that owns this component.
        /// </summary>
        public void SetEntity(Entity entity)
        {
            this.Entity = entity;
        }
    }
}
