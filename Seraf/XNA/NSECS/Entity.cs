﻿using Microsoft.Xna.Framework;
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

        //public Body body;

        public List<Component> components;

        bool isVisible;
        public bool IsVisible
        {
            get => isVisible;
            set => isVisible = value;
            //get
            //{
            //    return isVisible;
            //}
            //set
            //{
            //    if (body.sprite == null)
            //        isVisible = false;

            //}
        }
        public bool IsActive; // If updated

        bool isCollidble;
        public bool IsCollidble
        {
            get => isCollidble;
            set => isCollidble = value;
            //get
            //{
            //    return isCollidble;
            //}
            //set {
            //    if (body.collider == null)
            //    {
            //        isCollidble = false;
            //    }
            //    else
            //        isCollidble = true;
            //}
        }

        public bool Expired;


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

        //public bool HasBody()
        //{
        //    return (body != null);
        //}
    }
}
