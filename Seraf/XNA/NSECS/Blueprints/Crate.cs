using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Seraf.XNA.Collision;
using Seraf.XNA.NSECS;
using Seraf.XNA.NSECS.Components;

namespace Seraf.XNA.NSECS.Blueprints
{
    [EntityBlueprint("Crate")]
    public class Crate : Entity
    {
        Collider collider;

        public Crate(int uuid, Vector2 pos, Vector2 size) : base(uuid, pos, size)
        {
            var tex = ContentPipeline.Instance.Load<Texture2D>("sprites/crate");

            var sprites = new Sprite[]
            {
                new Sprite(this, tex, new Rectangle(0, 0, 16, 16)),
            };

            var anim = new SpriteAnim(this, sprites);
            

            this.AddComponent(anim);

            this.AddComponent(new Physics(this));
            this.collider = new Collider(this);
            this.AddComponent(collider);



            collider.Collided += GetPushed;
        }

        private void GetPushed(object sender, CollisionArgs e)
        {
            if(e.Side == COLLISION_SIDE.Left)
            {
                this.pos.X += 1;
            }
            else if (e.Side == COLLISION_SIDE.Right)
            {
                this.pos.X -= 1;
            }

            collider.Update(0.16f);
        }
    }
}
