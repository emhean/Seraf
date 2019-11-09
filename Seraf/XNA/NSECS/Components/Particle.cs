using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NSECS.Components
{
    public class Particle : Component
    {
        public float lifeSpan;
        public SpriteAnim anim;

        public Particle(SpriteAnim anim, float lifeSpan)
        {
            this.anim = anim;
            this.lifeSpan = lifeSpan;
        }

        public override void Update(float delta)
        {
            if (lifeSpan < 0)
            {
                this.Expire();
            }
            else
            {
                anim.Update(delta);
                lifeSpan -= delta;
            }

            base.Update(delta);
        }

        public override void Render(Scene scene)
        {
            scene.Render(anim.tex, Entity.pos, anim.clip, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);

            base.Render(scene);
        }
    }
}
