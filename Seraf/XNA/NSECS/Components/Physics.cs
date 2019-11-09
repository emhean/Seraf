using Microsoft.Xna.Framework;

namespace Seraf.XNA.NSECS.Components
{
    public class Physics : Component
    {
        public Physics(Entity entity) : base(entity)
        {
            this.vel = new Vector2();
        }

        float jump;
        public Vector2 vel;
        public Vector2 gravity = new Vector2(0, 2);
        public Vector2 prev_pos;

        public bool Jump()
        {
            if (jump <= 0)
            {
                jump = -6;
                Entity.pos -= gravity; // Counter gravity for the first frame.

                return true;
            }

            return false;
        }

        public override void Update(float delta)
        {
            prev_pos = Entity.pos;

            if (jump < 0)
            {
                jump += delta * 10;
            }
            else jump = 0;

            Entity.pos.Y += jump;
            Entity.pos += gravity;
            Entity.pos += vel;
        }
    }
}
