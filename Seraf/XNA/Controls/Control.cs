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
        public Control Parent { get; set; }
        public Color Color { get; set; }
        public Vector2 GetVector2() => new Vector2(bounds.X, bounds.Y);

        public event EventHandler Clicked, ClickHold, RightClicked, RightClickHold, CursorEnter, CursorLeave; //CursorHover
        public event EventHandler PositionChanged, SizeChanged;

        public object Tag { get; set; }
        public Texture2D Texture { get; set; }
        public bool IsVisible { get; set; } = true;

        protected Rectangle bounds;
        public Rectangle Bounds
        {
            get => bounds;
            set
            {
                if(bounds.X != value.X || bounds.Y != value.Y)
                {
                    bounds.X = value.X;
                    bounds.Y = value.Y;
                    OnPositionChanged(EventArgs.Empty);
                }

                if (bounds.Width != value.Width || bounds.Height != value.Height)
                {
                    bounds.Width = value.Width;
                    bounds.Height = value.Height;
                    OnSizeChanged(EventArgs.Empty);
                }
            }
        }
        protected MouseState prev_mouseState;
        public Control()
        {
            this.Color = Color.White;
        }
        public Control(Rectangle bounds)
        {
            this.bounds = bounds;
            this.Color = Color.White;
        }

        protected virtual void OnClicked(EventArgs e) => Clicked?.Invoke(this, e);
        protected virtual void OnClickHold(EventArgs e) => ClickHold?.Invoke(this, e);
        protected virtual void OnRightClicked(EventArgs e) => RightClicked?.Invoke(this, e);
        protected virtual void OnRightClickHold(EventArgs e) => RightClickHold?.Invoke(this, e);
        //protected virtual void OnCursorHover(EventArgs e) => CursorHover?.Invoke(this, e);
        protected virtual void OnCursorEnter(EventArgs e) => CursorEnter?.Invoke(this, e);
        protected virtual void OnCursorLeave(EventArgs e) => CursorLeave?.Invoke(this, e);
        protected virtual void OnPositionChanged(EventArgs e) => PositionChanged?.Invoke(this, e);
        protected virtual void OnSizeChanged(EventArgs e) => SizeChanged?.Invoke(this, e);

        bool entered;
        bool left_hold;
        bool right_hold;

        public virtual void Update(float delta, MouseState mouseState)
        {
            Rectangle cursorRect = new Rectangle((int)mouseState.X, (int)mouseState.Y, 1, 1);

            if(bounds.Intersects(cursorRect))
            {
                if(entered == false)
                {
                    entered = true;
                    OnCursorEnter(EventArgs.Empty);
                }
                //OnCursorHover(EventArgs.Empty);

                if(mouseState.LeftButton == ButtonState.Pressed) // Left Button
                {
                    // This statement first to avoid the 1 frame bug where clicking does nothing
                    if (prev_mouseState.LeftButton == ButtonState.Released) 
                    {
                        left_hold = false;
                        OnClicked(EventArgs.Empty);
                    }
                    else if (!left_hold)
                    {
                        left_hold = true;
                        OnClickHold(EventArgs.Empty);
                    }
                }
                else if (mouseState.RightButton == ButtonState.Pressed) // Right Button
                {
                    // This statement first to avoid the 1 frame bug where clicking does nothing
                    if (prev_mouseState.RightButton == ButtonState.Released)
                    {
                        right_hold = false;
                        OnRightClicked(EventArgs.Empty);
                    }
                    else if (!right_hold)
                    {
                        right_hold = true;
                        OnRightClickHold(EventArgs.Empty);
                    }
                }
            }
            else
            {
                if(entered)
                {
                    entered = false;
                    left_hold = false;
                    right_hold = false;
                    OnCursorLeave(EventArgs.Empty);
                }
            }

            prev_mouseState = mouseState;
        }

        //public abstract void Render(SpriteBatch spriteBatch);
        public virtual void Render(SpriteBatch spriteBatch) { }
    }
}
