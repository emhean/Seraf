using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Seraf.XNA.NSECS.Components
{
    public class Sprite : Component
    {
        public Texture2D tex;
        public Rectangle clip;


        public Sprite(Entity entity, Texture2D tex, Rectangle clip) : base(entity)
        {
            this.tex = tex;
            this.clip = clip;
        }

        public override void Update(float delta)
        {
        }

        public override void Render(Scene scene)
        {
            scene.Render(tex, Entity.pos, clip, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}
