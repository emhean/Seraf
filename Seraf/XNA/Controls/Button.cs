using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Seraf.XNA.Controls
{
    public class Button : Control
    { 
        public Button(Text text, Rectangle bounds) : base(bounds)
        {
            this.Text = text;
        }

        public Button(Rectangle bounds) : base(bounds)
        {
            this.Text = null;
        }

        public Text Text { get; set; }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Bounds, Color);

            // Draw text on top of control
            if(Text != null)
                spriteBatch.DrawString(Text.Font, Text, Text.GetVector2(), Text.TextColor);
        }
    }
}
