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

        bool isActive = true;
        bool isVisible = true;
        public bool IsActive { get => isActive; set => isActive = value; }
        public bool IsVisible { get => isVisible; set => isVisible = value; }
        public bool Expired { get; private set; }
        /// <summary>
        /// If this is enabled then debug stuff will happen.
        /// </summary>
        public bool DebugMode { get; set; } = true; 

        /// <summary>
        /// Component is initialized after when fully constructed.
        /// </summary>
        public virtual void Initialize(XmlElement e) { } // TODO: Make internal?

        public event EventHandler BeforeExpired;
        public event EventHandler AfterExpired;

        public virtual void Update(float delta) { }
        public virtual void Render(Scene scene) { }

        /// <summary>
        /// The entity that holds this component.
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

        ///// <summary>
        ///// Set the Entity that owns this component.
        ///// </summary>
        public void SetEntity(Entity entity)
        {
            this.Entity = entity;
        }
    }
}
