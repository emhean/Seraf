using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Seraf.XNA.NSECS
{
    public class Scene
    {
        public Scene(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            this.GraphicsDevice = graphicsDevice;
            this.SpriteBatch = spriteBatch;
        }

        //public Camera2D Camera;
        public GraphicsDevice GraphicsDevice { get; }
        public SpriteBatch SpriteBatch { get; }

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
