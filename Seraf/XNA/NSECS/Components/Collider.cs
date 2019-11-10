using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Seraf.XNA.Collision;
using System;
using System.Collections.Generic;

namespace Seraf.XNA.NSECS
{
    public class Collider : Component
    {
        public Rectangle rect;

        public Collider(Entity entity) : base(entity)
        {
            rect = new Rectangle((int)entity.pos.X, (int)entity.pos.Y, (int)entity.size.X, (int)entity.size.Y);
        }

        public override void Update(float delta)
        {
            if (!Enabled)
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

        Texture2D tex = ContentPipeline.Instance.Load<Texture2D>("sprites/box");

        public override void Render(Scene scene)
        {
            scene.SpriteBatch.Draw(tex, rect, Color.Red * 0.2f);
        }
    }



}
