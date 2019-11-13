using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Seraf.XNA
{
    public struct SpriteData
    {
        public Texture2D tex;
        public Rectangle clip;
        public Color color;
        public float rot;
        public Vector2 origin;
        public SpriteEffects spriteEffects;
        public float layerDepth;

        public SpriteData(Texture2D tex, Rectangle clip, Color color, Vector2 origin, float rot, SpriteEffects spriteEffects, float layerDepth)
        {
            this.tex = tex;
            this.clip = clip;
            this.color = color;
            this.origin = origin;
            this.rot = rot;
            this.spriteEffects = spriteEffects;
            this.layerDepth = layerDepth;
        }
        public SpriteData(Texture2D tex, Rectangle clip)
        {
            this.tex = tex;
            this.clip = clip;

            this.color = Color.White;
            this.origin = Vector2.Zero;
            this.rot = 0f;
            this.spriteEffects = SpriteEffects.None;
            this.layerDepth = 0f;
        }
    }
}
