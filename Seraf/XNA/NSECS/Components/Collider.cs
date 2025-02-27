﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Seraf.XNA.Collision;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Seraf.XNA.NSECS
{
    [ComponentBlueprint("collider")]
    public class Collider : Component
    {
        public Rectangle rect;

        public Collider(Entity entity) : base(entity)
        {
            this.rect = new Rectangle((int)entity.pos.X, (int)entity.pos.Y, (int)entity.size.X, (int)entity.size.Y);
        }

        public override void Initialize(XmlElement e)
        {
            foreach(XmlElement eve in e.GetElementsByTagName("event"))
            {
                if (eve.GetAttribute("name").Equals("Collided"))
                {
                    if (eve.GetAttribute("invoke").Equals("GetPushed"))
                        this.Collided += GetPushed;
                    else if (eve.GetAttribute("invoke").Equals("GetCollected"))
                        this.Collided += GetCollected;
                }
            }
        }

        /// <summary>
        /// Sets position and updates the collider.
        /// </summary>
        public void SetPosition(Vector2 position)
        {
            Entity.pos.X = position.X;
            Entity.pos.Y = position.Y;
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;
        }
        /// <summary>
        /// Sets position and updates the collider.
        /// </summary>
        public void SetPosition(int x, int y)
        {
            Entity.pos.X = x;
            Entity.pos.Y = y;
            rect.X = x;
            rect.Y = y;
        }

        public override void Update(float delta)
        {
            if (!IsActive)
                return;

            //rect.X = (int)Math.Ceiling(Entity.pos.X);
            rect.X = (int)Math.Round(Entity.pos.X, 0);
            //rect.Y = (int)Math.Round(Entity.pos.Y, 0);
            //rect.Width = (int)Math.Round(Entity.size.X, 0);
            //rect.Height = (int)Math.Round(Entity.size.Y, 0);

            //rect.X = (int)Entity.pos.X;
            rect.Y = (int)Entity.pos.Y;
            rect.Width = (int)Entity.size.X;
            rect.Height = (int)Entity.size.Y;
        }

        public event EventHandler<CollisionArgs> Collided;

        public void OnCollided(CollisionArgs args)
        {
            Collided?.Invoke(this, args);
        }


        private void GetPushed(object sender, CollisionArgs e)
        {
            if (e.Side == COLLISION_SIDE.Left)
            {
                this.Entity.pos.X += 1;
            }
            else if (e.Side == COLLISION_SIDE.Right)
            {
                this.Entity.pos.X -= 1;
            }

            this.Update(0.16f);
        }

        private void GetCollected(object sender, CollisionArgs e)
        {
            if(e.Side != COLLISION_SIDE.None)
            {

            }
        }


        /// <summary>
        /// AABB math to get side of intersection.
        /// </summary>
        public COLLISION_SIDE GetIntersectionSide(Rectangle other)
        {
            Rectangle intersection = Rectangle.Intersect(rect, other);
            if(intersection == Rectangle.Empty)
                return COLLISION_SIDE.None;
            

            COLLISION_SIDE side;

            float wy = (rect.Width + other.Width) * (rect.Center.Y - other.Center.Y);
            float hx = (rect.Height + other.Height) * (rect.Center.X - other.Center.X);

            
            if (wy > hx)
            {
                if (wy > -hx)
                    side = COLLISION_SIDE.Bottom;
                else
                    side = COLLISION_SIDE.Left;
            }
            else
            {
                if (wy > -hx)
                    side = COLLISION_SIDE.Right;
                else
                    side = COLLISION_SIDE.Top;
            }

            return side;
        }

        /// <summary>
        /// AABB math to get side of intersection.
        /// </summary>
        public COLLISION_SIDE GetIntersectionSide(Collider other) => GetIntersectionSide(other.rect);

        #region Debug stuff
        Texture2D tex = ContentPipeline.Instance.Load<Texture2D>("sprites/box");
        public override void Render(Scene scene)
        {
            if(DebugMode)
            {
                scene.SpriteBatch.Draw(tex, rect, Color.Red * 0.2f);
            }
        }

        #endregion
    }



}
