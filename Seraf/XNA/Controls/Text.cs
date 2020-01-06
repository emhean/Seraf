using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seraf.XNA.Controls
{
    public enum TextAlignX
    {
        Left, // Default because Rectangle(x:0,y:0) is at top left.
        Right,
        Center
    }
    public enum TextAlignY
    {
        Top, // Default because Rectangle(x:0,y:0) is at top left.
        Bottom,
        Center,
    }

    public class Text : Control
    {
        string str;
        TextAlignX textAlignX;
        TextAlignY textAlignY;
        SpriteFont font;


        public Color TextColor { get; set; }
        public Vector2 GetFontSize() => font.MeasureString("A");
        public Vector2 GetTextSize() => font.MeasureString(str);

        public event EventHandler FontChanged, TextChanged, TextAlignChanged;
        private void OnFontChanged(EventArgs e) => FontChanged?.Invoke(this, e);
        private void OnTextChanged(EventArgs e) => TextChanged?.Invoke(this, e);
        private void OnTextAlignChanged(EventArgs e) => TextAlignChanged?.Invoke(this, e);

        public Text(SpriteFont font, Rectangle bounds, string value)
        {
            this.font = font;
            this.bounds = bounds;
            this.str = value;
            this.TextColor = Color.Black;
        }

        public TextAlignX TextAlignX
        {
            get => textAlignX;
            set
            {
                if(textAlignX != value)
                {
                    textAlignX = value;

                    if (textAlignX == TextAlignX.Left)
                        this.bounds.X = this.Parent.Bounds.X; // + ((int)GetTextSize().X);

                    else if (textAlignX == TextAlignX.Right)
                        this.bounds.X = this.Parent.Bounds.X + ((int)GetTextSize().X);

                    else if (textAlignX == TextAlignX.Center)
                        this.bounds.X = this.Parent.Bounds.Center.X - ((int)GetTextSize().X / 2);

                    OnTextAlignChanged(EventArgs.Empty);
                }
            }
        }
        public TextAlignY TextAlignY
        {
            get => textAlignY;
            set
            {
                if (textAlignY != value)
                {
                    textAlignY = value;

                    if (textAlignY == TextAlignY.Center)
                        this.bounds.Y = this.Parent.Bounds.Center.Y - ((int)GetFontSize().Y / 2);

                    else if (textAlignY == TextAlignY.Top)
                        this.bounds.Y = this.Parent.Bounds.Y;// + (int)GetFontSize().Y;

                    else if (textAlignY == TextAlignY.Bottom)
                        this.bounds.Y = this.Parent.Bounds.Y + (int)GetFontSize().Y;

                    OnTextAlignChanged(EventArgs.Empty);
                }
            }
        }

        public SpriteFont Font
        {
            get => font;
            set
            {
                if (font != value)
                {
                    this.font = value;
                    OnFontChanged(EventArgs.Empty);
                }
            }
        }

        public static implicit operator string(Text text)
        {
            return text.str;
        }
        //public static explicit operator Text(string value)
        //{
        //    return new Text(value);
        //}

        public void SetText(string value)
        {
            if (!this.str.Equals(value))
            {
                this.str = value;
                OnTextChanged(EventArgs.Empty);
            }
        }
    }
}
