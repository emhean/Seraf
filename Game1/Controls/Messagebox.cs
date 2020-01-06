using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Seraf.XNA
{
    public class MessageboxBuffer
    {
        StringBuilder strBuilder;

        public MessageboxBuffer()
        {
            strBuilder = new StringBuilder();
        }

        public void Add(string text, int lineLengthBreak)
        {
            if (text.Length > lineLengthBreak) // If text is longer 
            {
                // Get number of lines
                //int n = text.Length / lineLengthBreak;
                int c = 0;
                for (int i = 0; i < text.Length; ++i)
                {
                    strBuilder.Append(text[i]);
                    c += 1;
                    if (c == lineLengthBreak)
                    {
                        strBuilder.AppendLine();
                        c = 0;
                    }
                }
            }
            else
                strBuilder.AppendLine(text);
        }

        public override string ToString() => strBuilder.ToString();
    }

    public class Messagebox
    {
        #region Box stuff
        Texture2D texture;
        Rectangle bounds; // Position and scaled size of box
        bool isOpen;
        #endregion

        #region Text stuff
        SpriteFont font;
        float text_speed = 0.03f;
        float t; // Time
        StringBuilder buffer;
        StringBuilder text;
        Vector2 pos; // Position of text
        Vector2 measure; // Measured size of font
        #endregion

        #region Camera stuff
        float rot = 0f;
        float zoom = 3.5f;
        Matrix matrix;
        #endregion

        #region Etcetera
        bool isClosing;
        float closeOnFinishTimeRemaining;
        float closeOnFinishTime;
        float bufferTimeoutRemaining = 0f;
        float bufferTimeout = 2f;
        bool keyPress, keyHold;
        KeyboardState p_kState;
        int holdFrames;
        #endregion

        public Messagebox(Texture2D texture, SpriteFont font, Rectangle bounds)
        {
            this.texture = texture;
            this.font = font;
            this.text = new StringBuilder();
            this.buffer = new StringBuilder();
            this.measure = font.MeasureString("A");
            this.bounds = bounds;
            //this.pos = new Vector2((int)bounds.X, (int)bounds.Y) + measure;
            this.pos = new Vector2(texture.Bounds.Width / 2, texture.Bounds.Height / 2);
        }
        public Messagebox(Texture2D texture, SpriteFont font, Rectangle bounds, float textSpeed)
            : this(texture, font, bounds)
        {
            this.text_speed = textSpeed;
        }


        #region Properties
        public bool IsOpen => isOpen;
        public bool CloseOnFinish { get; set; }
        public Rectangle Bounds => bounds;
        public SpriteFont Font => font;
        /// <summary>
        /// Adjusts the messagebox size to the content taken from buffer. Default value is true.
        /// </summary>
        public bool AdjustSizeByBuffer { get; set; } = true;
        public float CloseOnFinishTime
        {
            get => closeOnFinishTime;
            set
            {
                closeOnFinishTime = value;
                closeOnFinishTimeRemaining = value;
            }
        }
        public float BufferTimeOut
        {
            get => bufferTimeout;
            set
            {
                bufferTimeout = value;
            }
        }
        #endregion


        #region Event stuff
        public event EventHandler Finish, Finished;
        public event EventHandler Opening, Opened;
        public event EventHandler Closing, Closed;
        public event EventHandler Buffer, Buffered, BufferEmpty;
        public event EventHandler Flush, Flushed;
        protected void OnFinish() => Finish?.Invoke(this, EventArgs.Empty);
        protected void OnFinished() => Finished?.Invoke(this, EventArgs.Empty);
        protected void OnOpening() => Opening?.Invoke(this, EventArgs.Empty);
        protected void OnOpened() => Opened?.Invoke(this, EventArgs.Empty);
        protected void OnClosing() => Closing?.Invoke(this, EventArgs.Empty);
        protected void OnClosed() => Closed?.Invoke(this, EventArgs.Empty);
        protected void OnBuffered() => Buffered?.Invoke(this, EventArgs.Empty);
        protected void OnBuffer() => Buffer?.Invoke(this, EventArgs.Empty);
        protected void OnBufferEmpty() => BufferEmpty?.Invoke(this, EventArgs.Empty);
        protected void OnFlush() => Flush?.Invoke(this, EventArgs.Empty);
        protected void OnFlushed() => Flushed?.Invoke(this, EventArgs.Empty);
        #endregion


        public bool HasBuffer()
        {
            return (buffer.Length != 0);
        }
        public void FlushBuffer()
        {
            OnFlush();
            if (HasBuffer())
            {
                OnBuffer();
                text.Append(buffer);
                OnBuffered();
            }
            buffer.Clear();
            OnFlushed();
        }
        public bool TakeFromBuffer(int count)
        {
            if (bufferTimeoutRemaining > 0)
                return false;

            if (buffer[0].Equals('#'))
            {
                bufferTimeoutRemaining = bufferTimeout;
                buffer.Remove(0, 1);
                return false;
            }

            for (int i = 0; i < count; ++i)
            {
                OnBuffer();
                text.Append(buffer[0]);
                buffer.Remove(0, 1);
                OnBuffered();
            }

            return true;
        }
        public void AddToBuffer(string text)
        {
            OnBuffer();

            buffer.Append(text);

            if (AdjustSizeByBuffer)
                AdjustSizeToContent();

            // closeOnFinishTimeRemaining = CloseOnFinishTime; // Why?

            OnBuffered();
        }
        public void Clear()
        {
            text.Clear();
            buffer.Clear();
        }
        /// <summary>
        /// Adjusts the size of the messagebox to the content.
        /// </summary>
        public Messagebox AdjustSizeToContent()
        {
            // It just werks
            bounds.Height = 0;
            int c = 2; // Two because 1 for top and 1 for end so that last line dont have to end with end line.
            int magic = 1;
            foreach (char lin in buffer.ToString().Where(x => (x.Equals('\n'))))
                c += magic;

            bounds.Height += c * (int)measure.Y;
            return this;
        }
        public Messagebox Open(bool autoSize = false)
        {
            if (autoSize)
                AdjustSizeToContent();

            OnOpening();
            isOpen = true;
            OnOpened();
            return this;
        }
        public Messagebox Close()
        {
            OnClosing();
            isOpen = false;
            OnClosed();
            return this;
        }
        public Messagebox Show()
        {
            isOpen = true;
            return this;
        }
        public Messagebox Hide()
        {
            isOpen = false;
            return this;
        }


        /// <summary>
        /// If it's open: it's up to the user that the messagebox has buffer before calling this method. Otherwise it will finish and invoke events.
        /// </summary>
        public void Update(float delta, KeyboardState keyboardState)
        {
            keyHold = keyboardState.IsKeyDown(Keys.E);
            if (holdFrames < 15)
                keyPress = (p_kState.IsKeyDown(Keys.E) && keyboardState.IsKeyUp(Keys.E));


            if (keyHold)
            {
                holdFrames += 1;
                t += delta * 10;
            }
            else
            {
                holdFrames = 0;
                t += delta;
            }


            if (isClosing && keyPress)
            {
                OnClosing();
                Close();
                OnClosed();
            }

            if (t > text_speed)
            {
                if (HasBuffer())
                {
                    if (bufferTimeoutRemaining > 0)
                        bufferTimeoutRemaining -= delta;

                    if(TakeFromBuffer(1)) { } // Maybe do something with this?

                    if (!HasBuffer()) // If buffer is empty we're done.
                    {
                        OnFinish();
                        OnFinished();

                        if (CloseOnFinish && bufferTimeout <= 0)
                        {
                            isClosing = true;
                        }
                        else
                        {
                            //bufferTimeoutRemaining = bufferTimeout; ???????????????????????
                        }
                    }
                }
                else
                {
                    if (bufferTimeoutRemaining < 0)
                    {
                        OnBufferEmpty();
                        // If no buffer after that event then it's close time!
                        if (!HasBuffer())
                            isClosing = true;
                    }
                    else if (bufferTimeoutRemaining > 0)
                    {
                        bufferTimeoutRemaining -= (delta + t);
                    }
                    else if (isClosing)
                    {
                        if (closeOnFinishTimeRemaining < 0)
                        {
                            if(CloseOnFinish)
                            {
                                OnClosing();
                                Close();
                                OnClosed();
                            }
                        }
                        else
                        {
                            closeOnFinishTimeRemaining -= (delta + t);
                        }
                    }
                }

                t = 0; // Reset time
            }

            p_kState = keyboardState;

        }

        /// <summary>
        /// Render the box. Will only be rendered if it's open.
        /// </summary>
        public void Render(SpriteBatch spriteBatch, Viewport viewport)
        {
            if (!isOpen)
                return;

            matrix = GetTranslationMatrix(viewport);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, matrix);

            Rectangle render_b = new Rectangle(0 - bounds.Width / 2, 0 - bounds.Height / 2, bounds.Width, bounds.Height);
            spriteBatch.Draw(texture, render_b, Color.White);


            spriteBatch.DrawString(font, text, new Vector2(render_b.X + measure.X / 2, render_b.Y + measure.Y / 2), Color.White);

            spriteBatch.End();
        }

        protected Matrix GetTranslationMatrix(Viewport viewport)
        {
            matrix = Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0)) *
                                         Matrix.CreateRotationZ(rot) *
                                         Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(viewport.Width * 0.5f, viewport.Height * 0.5f, 0));
            return matrix;
        }
    }
}
