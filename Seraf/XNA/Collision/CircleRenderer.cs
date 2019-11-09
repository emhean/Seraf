using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Seraf.XNA.Collision
{
    public class CircleRenderer
    {
        Rectangle drawRect;
        Texture2D circleTex;

        public CircleRenderer(Texture2D circleTexture)
        {
            this.circleTex = circleTexture;
        }

        public void DrawCircle(SpriteBatch spriteBatch, Vector2 pos, float radius)
        {
            drawRect.X = (int)pos.X - (int)radius;
            drawRect.Y = (int)pos.Y - (int)radius;
            drawRect.Width = (int)radius * 2;
            drawRect.Height = (int)radius * 2;

            spriteBatch.Draw(circleTex, drawRect, Color.White);
        }
    }
}
