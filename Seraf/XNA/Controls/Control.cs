using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seraf.XNA.Controls
{
    public abstract class Control
    {
        public event EventHandler Clicked;
        public event EventHandler CursorHover;
        public event EventHandler CursorEnter;
        public event EventHandler CursorLeave;

        public object Tag { get; set; }
        public Texture2D Texture { get; set; }

        protected Rectangle bounds;
        public Rectangle Bounds
        {
            get => bounds;
            set
            {
                bounds.X = value.X;
                bounds.Y = value.Y;
            }
        }
        protected MouseState prev_mouseState;
        public Control() { }
        public Control(Rectangle bounds)
        {
            this.bounds = bounds;
        }

        protected void OnClicked(EventArgs e) => Clicked?.Invoke(this, e);
        protected void OnCursorHover(EventArgs e) => CursorHover?.Invoke(this, e);
        protected void OnCursorEnter(EventArgs e) => CursorEnter?.Invoke(this, e);
        protected void OnCursorLeave(EventArgs e) => CursorLeave?.Invoke(this, e);

        bool entered;

        public void Update(float delta, MouseState mouseState)
        {
            Rectangle cursorRect = new Rectangle((int)mouseState.X, (int)mouseState.Y, 1, 1);

            if(bounds.Intersects(cursorRect))
            {
                if(entered == false)
                {
                    entered = true;
                    OnCursorEnter(EventArgs.Empty);
                }

                if(mouseState.LeftButton == ButtonState.Pressed)
                {
                    if(prev_mouseState.LeftButton == ButtonState.Released)
                    {
                        OnClicked(EventArgs.Empty);
                    }
                }
            }
            else
            {
                if(entered)
                {
                    entered = false;
                    OnCursorLeave(EventArgs.Empty);
                }
            }

            prev_mouseState = mouseState;
        }
    }
}
