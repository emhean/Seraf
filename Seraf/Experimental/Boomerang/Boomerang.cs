using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Seraf.Experimental.Boomerang
{
    public class Boomerang
    {
        public Texture2D tex;
        public Vector2 pos;
        public float rot;
        public Color color;
        public Vector2 orig;
        public Vector2 scale;

        public Vector2 spd, dir;

        public Boomerang(Texture2D texture)
        {
            this.tex = texture;

            this.orig = new Vector2(tex.Width / 2, tex.Height / 2); // center
            this.scale = Vector2.One;
            this.color = Color.White;
        }
    }
}
