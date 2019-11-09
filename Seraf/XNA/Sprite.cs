using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Seraf.XNA
{
    public class Sprite
    {
        public Texture2D tex;
        public Rectangle clip;


        public Sprite(Texture2D tex, Rectangle clip)
        {
            this.tex = tex;
            this.clip = clip;
        }

        public virtual void Update(float delta)
        {
        }

        //public virtual void Render(Scene scene)
        //{
        //    scene.Render(tex, collider.pos, clip, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        //}
    }
}
