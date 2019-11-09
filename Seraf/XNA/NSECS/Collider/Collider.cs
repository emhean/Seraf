using Microsoft.Xna.Framework;

namespace NSECS
{
    public abstract class Collider
    {
        /// <summary>
        /// The position of the collider.
        /// </summary>
        //Vector2 pos;

        public Collider()
        {
        }

        public virtual void Update(float delta)
        {
            //this.pos = entity.pos;
        }

        //public virtual void CheckCollision<T>(T other)
        //{
        //}
    }
}
