using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Seraf.XNA;
using Seraf.XNA.Tiled;
using Seraf.XNA.NSECS;
using Seraf.XNA.NSECS.Components;

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
        Camera2DControlled cam;
        Scene scene;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            ContentPipeline.CreateInstance(this.Content);

            map = new TiledMap("testmap/");
            engine = new Engine(map);
            cam = new Camera2DControlled();

            base.Initialize();
        }

        Entity mario;
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            scene = new Scene(GraphicsDevice, spriteBatch);

            map.Load("testmap.tmx");




            mario = new Entity(0, new Vector2(70, 70), new Vector2(16, 21));

            var anim = new SpriteAnim(mario, null);
            var collider = new Collider(mario);

            mario.AddComponent(anim);
            mario.AddComponent(new Physics(mario));


            mario.AddComponent(new Player(mario, anim, mario.GetComponent<Physics>(), collider));
            mario.AddComponent(collider);

            engine.AddEntity(mario);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            engine.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            cam.UpdateControls((float)gameTime.ElapsedGameTime.TotalSeconds);
            cam.Position = mario.pos + (mario.size / 2);


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

            engine.Render(scene);


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
