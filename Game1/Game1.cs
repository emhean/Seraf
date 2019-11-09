using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Seraf.XNA;
using Seraf.XNA.Tiled;
using Seraf.XNA.NSECS;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        TiledMap map;
        Engine engine;
        Camera2D cam;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            ContentPipeline.CreateInstance(this.Content);

            cam = new Camera2D();
            map = new TiledMap("testmap/");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            map.Load("testmap.tmx");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.GraphicsDevice.Clear(map.backgroundColor);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, cam.GetTransformation(GraphicsDevice));


            foreach (var layer in map.imageLayers)
            {
                spriteBatch.Draw(layer.image.texture, layer.offset, Color.White * layer.opacity);
            }


            for (int i = 0; i < map.tileLayers.Count; ++i)//for (int i = (map.tileLayers.Count - 1); i > -1; --i)
            {
                foreach (var tile in map.tileLayers[i].tiles)
                {
                    foreach (var ts in map.tileSets)
                    {
                        if (ts.IsTileIDPartOfSet(tile.id))
                        {
                            if (tile.id != 0)
                            {
                                spriteBatch.Draw(ts.image.texture, tile.rect, ts.tile_data[tile.id - ts.firstgid].clip[0], Color.White * map.tileLayers[i].Opacity);
                            }
                            //else spriteBatch.Draw(ts.image.texture, tile.rect, ts.tile_clips[tile.id], Color.White * map.tileLayers[i].Opacity);
                        }
                    }
                }
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
