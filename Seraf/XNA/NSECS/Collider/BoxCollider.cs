using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace NSECS
{
    public class Collision
    {
        [Flags]
        public enum CollisionSide
        {
            /// <summary>No collision occurred.</summary>
            None = 0,
            /// <summary>Collision occurred at the top side.</summary>
            Top = 1,
            /// <summary>Collision occurred at the bottom side.</summary>
            Bottom = 2,
            /// <summary>Collision occurred at the left side.</summary>
            Left = 4,
            /// <summary>Collision occurred at the right side.</summary>
            Right = 8
        }

    }
    public class BoxCollider : Collider
    {
        Rectangle rect; // The "box". Position is set to the base class vector pos.
        Entity entity;

        public BoxCollider(Entity entity) : base()
        {
            this.entity = entity;
            this.rect = new Rectangle((int)entity.pos.X, (int)entity.pos.Y, (int)entity.size.X, (int)entity.size.Y);
        }

        public BoxCollider(Entity entity, Vector2 pos, Vector2 size) : base()
        {
            this.entity = entity;
            this.rect = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
        }

        public override void Update(float delta)
        {
            this.rect.X = (int)entity.pos.X;
            this.rect.Y = (int)entity.pos.Y;


            colliders.Clear();


            base.Update(delta);
        }

        public bool CheckIfColliding(BoxCollider other)
            => (Rectangle.Intersect(rect, other.rect) != Rectangle.Empty);


        List<BoxCollider> colliders = new List<BoxCollider>();

        public void UpdateCollision(BoxCollider other)
        {
            if (colliders.Contains(other))
                return;

            var isection = Rectangle.Intersect(rect, other.rect);

            Console.WriteLine("!!! Collision !!!");

            if (rect.Bottom > other.rect.Top)
            {
                rect.Y -= isection.Height;
            }
            else
            {
                rect.Y += isection.Height;
            }

            colliders.Add(other);
            other.colliders.Add(this);

            // Update the real position to the colliders adjusted position
            entity.pos.X = rect.X;
            entity.pos.Y = rect.Y;
        }
    }
}
