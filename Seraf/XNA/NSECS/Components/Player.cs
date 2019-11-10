using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        Collider collider;
        bool still = true;

        Sprite[] anim_still, anim_walk, anim_jump, anim_fall, anim_death;

        SoundEffectInstance bgm, sfx_jump;

        public Player(Entity entity, SpriteAnim spriteAnim, Physics physics, Collider collider) : base(entity)
        {
            this.physics = physics;
            this.spriteAnim = spriteAnim;
            this.collider = collider;

            Texture2D tex = ContentPipeline.Instance.Load<Texture2D>("sprites/mario");

            var sfx = ContentPipeline.Instance.Load<SoundEffect>("sfx/smw_jump");
            sfx_jump = sfx.CreateInstance();

            bgm = ContentPipeline.Instance.Load<SoundEffect>("bgm/underground").CreateInstance();
            bgm.Volume = 0.2f;
            bgm.IsLooped = true;
            bgm.Play();


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
                new Sprite(entity, tex, new Rectangle(16, 0, mario_w, mario_h))
            };
            anim_jump = new Sprite[]
            {
                new Sprite(entity, tex, new Rectangle(32, 0, mario_w, mario_h))
            };
            anim_fall = new Sprite[]
            {
                new Sprite(entity, tex, new Rectangle(48, 0, mario_w, mario_h))
            };

            anim_death = new Sprite[]
            {
                new Sprite(entity, ContentPipeline.Instance.Load<Texture2D>("sprites/death"), new Rectangle(0, 0, mario_w, 24))
            };

            spriteAnim.SetSprites(anim_still);


            collider.Collided += CheckCollision;
        }

        private void CheckCollision(object sender, CollisionArgs e)
        {
            if (death)
                return;

            if (e.Side == COLLISION_SIDE.Top)
            {
                //if( !physics.jumping)
                //{
                //    if (still)
                //    {
                //        spriteAnim.SetSprites(anim_still);
                //    }
                //}

                if (physics.isJumping)
                {
                    if (spriteAnim.Sprites.Equals(this.anim_jump))
                    {
                        spriteAnim.SetSprites(anim_still);
                    }
                    physics.isJumping = false;
                }
                else if (physics.isFalling)
                {
                    spriteAnim.SetSprites(anim_still);

                    physics.Land();
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

        bool death;

        public void Kill()
        {
            death = true;
            physics.Enabled = false;
            collider.Enabled = false;

            bgm.Stop();
            bgm.Dispose();
            bgm = ContentPipeline.Instance.Load<SoundEffect>("bgm/death").CreateInstance();
            bgm.Volume = 1f;
            bgm.Play();
        }

        public void Respawn()
        {
            death = false;
            physics.Enabled = true;
            collider.Enabled = true;

            physics.Land();
            physics.lethalFall = false;
            physics.fall_time = 0;
            physics.isJumping = false;
            physics.isFalling = true;

            spriteAnim.SetSprites(anim_fall);

            bgm.Stop();
            bgm.Dispose();

            bgm = ContentPipeline.Instance.Load<SoundEffect>("bgm/underground").CreateInstance();
            bgm.Volume = 0.2f;
            bgm.IsLooped = true;
            bgm.Play();
        }


        public override void Update(float delta)
        {
            prev_kstate = kstate;
            kstate = Keyboard.GetState();

            if (death)
            {
                spriteAnim.SetSprites(anim_death);
                Entity.pos.Y += 2;
                return;
            }

            if (!death && physics.lethalFall)
            {
                Kill();
            }


            if (kstate.IsKeyDown(Keys.Space) && prev_kstate.IsKeyUp(Keys.Space))
            {
                if (!physics.isJumping && physics.Jump())
                {
                    //Entity.GetComponent<Collider>().Update(delta);
                    //physics.isJumping = true;
                    spriteAnim.SetSprites(anim_jump);
                    if(sfx_jump.State == SoundState.Playing)
                    {
                        sfx_jump.Stop();
                    }
                    sfx_jump.Play();
                }
            }

            if (kstate.IsKeyDown(Keys.A))
            {
                if (kstate.IsKeyDown(Keys.LeftShift))
                    physics.vel_x = -2;
                else physics.vel_x = -1;

                still = false;

                physics.UpdateVel();

                spriteAnim.spriteEffects = SpriteEffects.FlipHorizontally;

                if (!physics.isJumping)
                    spriteAnim.SetSprites(anim_walk);
            }
            else if (kstate.IsKeyDown(Keys.D))
            {
                if (kstate.IsKeyDown(Keys.LeftShift))
                    physics.vel_x = 2;
                else physics.vel_x = 1;

                still = false;
                physics.UpdateVel();

                spriteAnim.spriteEffects = SpriteEffects.None;

                if (!physics.isJumping)
                    spriteAnim.SetSprites(anim_walk);
            }
            else
            {
                still = true;
            }

            if (still)
            {
                if (physics.vel_x < 0)
                {
                    physics.vel_x += 0.5f;
                    if (physics.vel_x > 0)
                    {
                        physics.vel_x = 0;
                    }
                }
                else if (physics.vel_x > 0)
                {
                    physics.vel_x -= 0.5f;
                    if (physics.vel_x < 0)
                    {
                        physics.vel_x = 0;
                    }
                }

                if (physics.vel_x == 0)
                    if (!physics.isJumping)
                        spriteAnim.SetSprites(anim_still);
            }

            if (!physics.isJumping && physics.isFalling)
            {
                spriteAnim.SetSprites(anim_fall);
            }

        }
    }
}
