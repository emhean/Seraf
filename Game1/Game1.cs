using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Seraf.XNA;
using Seraf.XNA.Tiled;
using Seraf.XNA.NSECS;
using Seraf.XNA.NSECS.Components;
using Seraf.XNA.Controls;
using System;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        TTileMap map;
        Engine engine;
        Camera2DControlled cam;
        Scene scene;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowAltF4 = true;
            Window.IsBorderless = true;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 960;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            ContentPipeline.CreateInstance(this.Content);

            map = new TTileMap("testmap/");
            engine = new Engine(map);
            cam = new Camera2DControlled();
            cam.Zoom = 3f;


            base.Initialize();
        }

        Entity mario;
        Vector2 spawn;

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            scene = new Scene(GraphicsDevice, spriteBatch);

            map.Load("testmap.tmx");

            mario = new Entity(0, new Vector2(70, 70), new Vector2(16, 21));

            var anim = new SpriteAnim(mario, null);
            var collider = new Collider(mario);
            var phys = new Physics(mario);

            mario.AddComponent(phys);
            mario.AddComponent(collider);
            mario.AddComponent(new Player(mario, anim, phys, collider));
            mario.AddComponent(anim);
            mario.AddComponent(new Seraf.Experimental.ParticleEmitter(mario));



            //var foo = new EntityParser();
            //foo.Save(mario, "ent_test");

            engine.AddEntity(mario);

            var builder = new EntityBuilder();
            foreach(var e in map.objectGroups)
            {
                foreach(var o in e.objects)
                {
                    //var ent = parser.CreateEntityFromTObject(o);

                    var ent = builder.BuildFromFile(o, "Content/entities/" + o.type);

                    engine.AddEntity(ent);

                    if (ent.type.Equals("Spawn"))// is Spawn spawn)
                    {
                       this.spawn = ent.pos;
                    }
                }
            }

            var tex = ContentPipeline.Instance.Load<Texture2D>("sprites/button");
            var button = new Button(new Rectangle(50, 50, tex.Width * 2, tex.Height * 2), string.Empty);
            button.Texture = tex;
            button.Clicked += delegate (object o, EventArgs e)
            {
                mario.pos = spawn;

                var p = mario.GetComponent<Player>();
                p.Respawn();
                
            };
            scene.Controls.Add(button);

            var tex_save = ContentPipeline.Instance.Load<Texture2D>("sprites/save");
            var button_save = new Button(new Rectangle(50, 60 + tex.Height + tex_save.Height, tex_save.Width * 2, tex_save.Height * 2), string.Empty);
            button_save.Texture = tex_save;
            button_save.Clicked += delegate (object o, EventArgs e)
            {
                //Vector2 spawn_pos = spawn.pos;
                spawn = mario.pos;

                var entityParser = new EntityParser();
                foreach (var groups in map.objectGroups)
                {
                    foreach (var ent in engine.entities)
                    {
                        for(int i = 0; i < groups.objects.Count; ++i)
                        {
                            if(ent.uuid.Equals(groups.objects[i].id))
                                groups.objects[i] = entityParser.CreateTObjectFromEntity(ent);
                        }
                    }
                }


                map.Save("testmap.tmx");
            };
            scene.Controls.Add(button_save);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            engine.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            cam.UpdateControls((float)gameTime.ElapsedGameTime.TotalSeconds);
            
            if(Keyboard.GetState().IsKeyDown(Keys.F))
                graphics.ToggleFullScreen();

            //var pos = new Vector2((int)mario.pos.X, (int)mario.pos.Y);

            cam.Position = mario.pos + (mario.size / 2);
            scene.Camera = cam;

            scene.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            var p = mario.GetComponent<Physics>();
            var player = mario.GetComponent<Player>();

            Window.Title = string.Format("isJumping={0}, isFalling={1}, jump={2}", p.isJumping, p.isFalling, p.jump);

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


            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            scene.RenderControls();
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
