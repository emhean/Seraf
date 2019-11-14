using Microsoft.Xna.Framework;
using Seraf.XNA.Tiled;
using System;
using System.Collections.Generic;

namespace Seraf.XNA.NSECS
{
    public class Entity : ITiledProperties
    {
        public TProperties Properties { get; set; } 

        public readonly int uuid;
        public string type;
        public string name;

        public Vector2 pos;
        public Vector2 size;

        public List<Component> components;

        public bool IsActive; // If updated
        bool isVisible, isCollidble;

        public bool IsVisible { get => isVisible; set => isVisible = value; }

        public event EventHandler BeforeExpired, AfterExpired;
        internal void Expire()
        {
            BeforeExpired?.Invoke(this, EventArgs.Empty);
            Expired = true;
            AfterExpired?.Invoke(this, EventArgs.Empty);
        }

        public bool IsCollidble { get => isCollidble; set => isCollidble = value; }
        public bool Expired;

        Entity() { }
        public Entity(int uuid)
        {
            this.uuid = uuid;
            this.components = new List<Component>();
            this.Properties = new TProperties();

            IsVisible = true;
            IsActive = true;
            IsCollidble = true;
            Expired = false;
        }

        public Entity(int uuid, Vector2 pos, Vector2 size) : this(uuid)
        {
            this.pos = pos;
            this.size = size;
        }

        /// <summary>
        /// Entity is initialized after when fully constructed.
        /// </summary>
        internal protected virtual void Initialize()
        {
        }


        #region Get/Set Component stuff
        public void AddComponent(Component component)
        {
            component.SetEntity(this); // Add entity of component to this instance.
            this.components.Add(component);
        }
        public T GetComponent<T>() where T : Component
        {
            for(int i = 0; i < components.Count; ++i)
            {
                if(components[i] is T)
                {
                    return (T)components[i];
                }
            }

            return null;
        }
        public void RemoveComponent(Component component)
        {
            this.components.Remove(component);
        }
        #endregion


        public Vector2 GetCenter() => new Vector2(pos.X + size.X / 2, pos.Y + size.Y / 2);
        public Vector2 GetRight() => new Vector2(pos.X + size.X, pos.Y + size.Y / 2);
        public Vector2 GetLeft() => new Vector2(pos.X, pos.Y + size.Y / 2);
        public Vector2 GetTop() => new Vector2(pos.X + size.X / 2, pos.Y);
        public Vector2 GetBottom() => new Vector2(pos.X + size.X / 2, pos.Y + size.Y);
    }
}
