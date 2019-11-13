using System;
using System.Xml;

namespace Seraf.XNA.NSECS
{
    public class Component
    {
        public Component(Entity entity)
        {
            this.Entity = entity;
        }

        public virtual void Initialize(XmlElement e) { } // TODO: Make internal

        public bool Enabled { get; set; } = true;

        public event EventHandler BeforeExpired, AfterExpired;

        public virtual void Update(float delta) { }
        public virtual void Render(Scene scene) { }

        /// <summary>
        /// The entity instance that holds this component.
        /// </summary>
        public Entity Entity { get; private set; }

        /// <summary>
        /// If this is true then the component is expired and will be removed. Use Expire() to set true.
        /// </summary>
        public bool IsExpired { get; private set; }

        public void Expire()
        {
            BeforeExpired?.Invoke(this, EventArgs.Empty);
            IsExpired = true;
            AfterExpired?.Invoke(this, EventArgs.Empty);
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
