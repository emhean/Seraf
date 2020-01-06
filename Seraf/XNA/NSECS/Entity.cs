using Microsoft.Xna.Framework;
using Seraf.XNA.Tiled;
using System;
using System.Collections.Generic;

namespace Seraf.XNA.NSECS
{
    public class Entity : ITiledProperties
    {
        #region Fields
        /// <summary>
        /// The unique unchanged identifier.
        /// </summary>
        public readonly int uuid;
        /// <summary>
        /// The entity type.
        /// </summary>
        public string type;
        /// <summary>
        /// The name of entity.
        /// </summary>
        public string name;
        /// <summary>
        /// Position of entity.
        /// </summary>
        public Vector2 pos;
        /// <summary>
        /// Size of entity.
        /// </summary>
        public Vector2 size;

        List<Component> components;
        bool isActive = true;
        bool isVisible = true;
        bool isCollidble = true;
        #endregion

        #region Properties
        /// <summary>
        /// The properties from the Tiled editor.
        /// </summary>
        public TProperties Properties { get; set; }
        /// <summary>
        /// Whether the Update method will be called. Default value is true.
        /// </summary>
        public bool IsActive { get => isActive; set => isActive = value; }
        /// <summary>
        /// Whether the Render method will be called. Default value is true.
        /// </summary>
        public bool IsVisible { get => isVisible; set => isVisible = value; }
        /// <summary>
        /// Whether the Entity is collidble. Default value is true.
        /// </summary>
        public bool IsCollidble { get => isCollidble; set => isCollidble = value; }
        /// <summary>
        /// If true then Entity will be removed. Default value is false, Use Expire() to set to true.
        /// </summary>
        public bool Expired { get; private set; }
        #endregion

        #region Events
        public event EventHandler BeforeExpired;
        public event EventHandler AfterExpired;
        #endregion


        #region Constructors
        Entity() { }
        public Entity(int uuid)
        {
            this.uuid = uuid;
            this.components = new List<Component>();
            this.Properties = new TProperties();

            IsVisible = true;
            IsActive = true;
            IsCollidble = true;
        }
        public Entity(int uuid, Vector2 pos, Vector2 size) : this(uuid)
        {
            this.pos = pos;
            this.size = size;
        }
        #endregion

        /// <summary>
        /// Invokes BeforeExpired event, sets Expired to true then invokes AfterExpired.
        /// </summary>
        internal void Expire()
        {
            BeforeExpired?.Invoke(this, EventArgs.Empty);
            Expired = true;
            AfterExpired?.Invoke(this, EventArgs.Empty);
        }

        #region Get/Set Component stuff
        public Component this[int index] => components[index];


        public int GetComponentCount() => components.Count;

        public IEnumerable<Component> EnumerateComponents()
        {
            for (int i = 0; i < components.Count; ++i)
                yield return components[i];
        }
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
        public bool HasComponent<T>() where T : Component => (GetComponent<T>() != null);
        public bool HasComponent<T>(out T component) where T : Component
        {
            component = GetComponent<T>();
            return (component != null);
        }
        #endregion


        public Vector2 GetCenter() => new Vector2(pos.X + size.X / 2, pos.Y + size.Y / 2);
        public Vector2 GetRight() => new Vector2(pos.X + size.X, pos.Y + size.Y / 2);
        public Vector2 GetLeft() => new Vector2(pos.X, pos.Y + size.Y / 2);
        public Vector2 GetTop() => new Vector2(pos.X + size.X / 2, pos.Y);
        public Vector2 GetBottom() => new Vector2(pos.X + size.X / 2, pos.Y + size.Y);
    }
}
