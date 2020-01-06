using Microsoft.Xna.Framework;

namespace Seraf.XNA.NSECS.Components
{
    [ComponentBlueprint("physics")]
    public class Physics : Component
    {
        public Physics(Entity entity) : base(entity)
        {
        }

        public float gravity = 2;
        public bool isJumping;
        public bool isFalling;
        public float jump;
        public float vel_x, vel_y;
        public Vector2 prev_pos;

        public bool Jump()
        {
            if (isFalling)
                return false;

            if (jump <= 0)
            {
                _Jump();
                return true;
            }

            return false;
        }

        public void ForceJump() => _Jump();

        private void _Jump()
        {
            jump = -5;
            Entity.pos.Y -= 2; // Counter gravity for the first frame.
            isJumping = true;
            isFalling = false;
        }

        public void Land()
        {
            jump = 0;
            isFalling = false;
            isJumping = false;

            //if (jump < 0)
            //{
            //    jump = 0;
            //    isFalling = false;
            //    isJumping = false;
            //}
        }

        public void UpdateVel()
        {
            Entity.pos.X += vel_x;
            Entity.pos.Y += vel_y;
        }

        public float fall_time;
        public bool lethalFall;

        public override void Update(float delta)
        {
            if (prev_pos.Y != Entity.pos.Y)
            {
                if (!isJumping)
                {
                    isFalling = true;
                }
            }
            else isFalling = false;

            prev_pos = Entity.pos;


            if (jump < 0)
            {
                jump += delta * 10;
            }
            else
            {
                if (isJumping)
                {
                    isFalling = true;
                    isJumping = false;
                    jump = 0;
                }
            }

            Entity.pos.Y += jump;


            if(isFalling)
            {
                if(!lethalFall)
                {
                    fall_time += delta;

                    if (fall_time > 2)
                    {
                        lethalFall = true;
                        fall_time = 0;
                    }
                }
                
            }
            else
            {
                fall_time = 0;
                lethalFall = false;
            }
        }
    }
}
