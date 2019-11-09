using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Seraf.XNA.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seraf.XNA.NSECS.Components
{
    public class Player : Component
    {
        KeyboardState prev_kstate, kstate;
        Physics physics;
        SpriteAnim spriteAnim;
        bool still = true;
        bool jumping = false;

        Sprite[] anim_still, anim_walk, anim_jump;

        public Player(Entity entity, SpriteAnim spriteAnim, Physics physics, Collider collider) : base(entity)
        {
            this.physics = physics;
            this.spriteAnim = spriteAnim;

            Texture2D tex = ContentPipeline.Instance.Load<Texture2D>("sprites/mario");

            // Mario's width is 16 and height 21
            int mario_w = 16;
            int mario_h = 21;

            anim_still = new Sprite[]
            {
                new Sprite(entity, tex, new Rectangle(0, 0, mario_w, mario_h)),
            };
            anim_walk = new Sprite[]
            {
                new Sprite(entity, tex, new Rectangle(0, 0, mario_w, mario_h)),
                new Sprite(entity, tex, new Rectangle(0 + mario_w, 0, mario_w, mario_h))
            };
            anim_jump = new Sprite[]
            {
                new Sprite(entity, tex, new Rectangle(32, 0, mario_w, mario_h)),
                new Sprite(entity, tex, new Rectangle(32, 0, mario_w, mario_h))
            };

            spriteAnim.SetSprites(anim_still);


            collider.Collided += CheckCollision;
        }

        private void CheckCollision(object sender, CollisionArgs e)
        {
            if(e.Side == COLLISION_SIDE.Top)
            {
                if (jumping)
                {
                    if (spriteAnim.Sprites.Equals(this.anim_jump))
                    {
                        spriteAnim.SetSprites(anim_still);
                    }

                    jumping = false;
                }
            }
            //else if (e.Side == COLLISION_SIDE.Left)
            //{
            //    physics.vel = Vector2.Zero;
            //    still = true;
            //}
            //else if (e.Side == COLLISION_SIDE.Right)
            //{
            //    physics.vel = Vector2.Zero;
            //    still = true;
            //}
            //else if (e.Side == COLLISION_SIDE.Left)
            //{
            //}
        }

        public override void Update(float delta)
        {
            prev_kstate = kstate;
            kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Space) && prev_kstate.IsKeyUp(Keys.Space))
            {
                if (!jumping && physics.Jump())
                {
                    //Entity.GetComponent<Collider>().Update(delta);
                    jumping = true;
                    spriteAnim.SetSprites(anim_jump);
                }
            }

            if (kstate.IsKeyDown(Keys.A))
            {
                if (physics.vel.X > -2)
                {
                    physics.vel.X -= 0.1f;
                    still = false;
                }
                spriteAnim.spriteEffects = SpriteEffects.FlipHorizontally;

                if(!jumping && !spriteAnim.Sprites.Equals(anim_jump))
                    spriteAnim.SetSprites(anim_walk);
            }
            else if (kstate.IsKeyDown(Keys.D))
            {
                if (physics.vel.X < 2)
                {
                    physics.vel.X += 0.1f;
                    still = false;
                }
                spriteAnim.spriteEffects = SpriteEffects.None;

                if(!jumping && !spriteAnim.Sprites.Equals(anim_jump))
                    spriteAnim.SetSprites(anim_walk);
            }
            else
            {
                bool foo = false;
                bool bar = false;

                if(!still)
                {
                    if (physics.vel.X < 0)
                    {
                        physics.vel.X += 0.1f;
                        foo = true;
                    }

                    if (physics.vel.X > 0)
                    {
                        physics.vel.X += -0.1f;
                        bar = true;
                    }

                    if (foo && bar)
                    {
                        if (!jumping)
                        {
                            spriteAnim.SetSprites(anim_still);
                        }

                        physics.vel.X = 0;
                        still = true;
                    }
                }
            }
        }
    }
}
