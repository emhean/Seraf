using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seraf.XNA
{
    public class TextEffect
    {
        protected string text;
        protected SpriteFont font;
        protected Color color;

        public TextEffect(string text, SpriteFont font, Color color)
        {
            this.text = text;
            this.font = font;
            this.color = color;
        }

        public virtual void Render(SpriteBatch spriteBatch, Vector2 pos)
        {
            spriteBatch.DrawString(font, text, pos, color);
        }
    }

    public class RandomizedColors : TextEffect
    {
        public float t;
        Color[] rndColor_pool;
        Color[] rndColors;

        public RandomizedColors(string text, SpriteFont font, Color[] colors) : base(text, font, Color.White)
        {
            this.rndColors = new Color[text.Length];
            this.rndColor_pool = colors;
        }

        //public void SetColors(Color[] colors)
        //{
        //    rndColors = colors;
        //    rndColor_pool = colors;
        //}

        //public Color GetRandomColor()
        //{
        //    return rndColor_pool[Rng.GetInt(0, rndColor_pool.Length)];
        //}

        private void RandomizeColors()
        {
            for(int x = 0; x < 2; ++x)
            {
                for (int i = 0; i < text.Length; ++i)
                {
                    rndColors[i] = rndColor_pool[Rng.GetInt(0, rndColor_pool.Length)];
                }
            }
        }

        public override void Render(SpriteBatch spriteBatch, Vector2 pos)
        {
            t += 0.1f;

            if(t > 1)
            {
                RandomizeColors();
                t = 0;
            }

            var sB = new StringBuilder();

            for (int i = 0; i < text.Length; ++i)
            {
                sB.Append(text[i]);

                spriteBatch.DrawString(font, sB, pos, rndColors[i]);

                pos.X += font.MeasureString(sB).X;

                sB.Clear();
            }
        }
    }

    public class TextRenderer
    {
        string text;
        SpriteFont font;
        Color color;

        public TextRenderer(string text, SpriteFont font, Color color)
        {
            this.text = text;
            this.font = font;
            this.color = color;
        }

        public void Render(SpriteBatch spriteBatch, Vector2 pos)
        {
            spriteBatch.DrawString(font, text, pos, color);
        }
    }
}
