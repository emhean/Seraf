using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Seraf.XNA.Controls
{
    public class Button : Control
    {
        public string Text { get; set; }

        public Button(Rectangle bounds, string text) : base(bounds)
        {
            this.Text = text;
        }
    }
}
