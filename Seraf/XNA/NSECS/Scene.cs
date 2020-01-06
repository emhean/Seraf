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

        public List<Entity> Entities { get; set; }
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

        public void Render(Vector2 pos, Experimental.SpriteData spriteData)
        {
            SpriteBatch.Draw(spriteData.tex, pos, spriteData.clip, spriteData.color, spriteData.rot, spriteData.origin, Vector2.One, spriteData.spriteEffects, spriteData.layerDepth);
        }

        public void Update(float delta)
        {
            foreach (var c in Controls)
                c.Update(delta, Mouse.GetState());
        }

        public void RenderControls() // TODO: Move this into Engine.Render(scene)
        {
            foreach (var c in Controls)
                if(c.IsVisible)
                    c.Render(SpriteBatch);
        }

        public void RenderEntities()
        {
            for (int i = 0; i < Entities.Count; ++i)
            {
                if (Entities[i].IsVisible)
                {
                    for (int j = 0; j < Entities[i].GetComponentCount(); ++j)
                    {
                        Entities[i][j].Render(this);
                    }
                }
            }
        }


        public void RenderControl(Control control)
        {
            control.Render(this.SpriteBatch);
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
