using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Seraf.XNA.Controls;
using System.Collections.Generic;

namespace Seraf.XNA.NSECS
{
    public class Scene
    {
        public Scene(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            this.GraphicsDevice = graphicsDevice;
            this.SpriteBatch = spriteBatch;
            this.Controls = new List<Control>();
        }

        public List<Control> Controls { get; set; } 
        public GraphicsDevice GraphicsDevice { get; }
        public SpriteBatch SpriteBatch { get; }
        public Camera2D Camera { get; set; }

        public void Render(Texture2D tex, Vector2 pos, Rectangle clip, Color color, float rot, Vector2 orig, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            SpriteBatch.Draw(tex, pos, clip, color, rot, orig, scale, effects, layerDepth);
        }

        public void Render(Texture2D tex, Rectangle pos, Rectangle clip, Color color, float rot, Vector2 orig, SpriteEffects effects, float layerDepth)
        {
            SpriteBatch.Draw(tex, pos, clip, color, rot, orig, effects, layerDepth);
        }

        //public void Render(Entity entity)
        //{
        //    var d_rect = new Rectangle((int)entity.pos.X, (int)entity.pos.Y, (int)entity.size.X, (int)entity.size.Y);

        //    SpriteBatch.Draw(entity.body.sprite.tex, d_rect, entity.body.sprite.clip,
        //        Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        //}

        public void Update(float delta)
        {
            foreach (var c in Controls)
                c.Update(delta, Mouse.GetState());
        }

        public void RenderControls()
        {
            foreach(var c in Controls)
            {
                SpriteBatch.Draw(c.Texture, c.Bounds, Color.White);
            }
        }

        public void Clear()
        {
            GraphicsDevice.Clear(Color.Blue);
        }

        public void Begin()
        {
            SpriteBatch.Begin();
        }

        public void End()
        {
            SpriteBatch.End();
        }
    }
}
