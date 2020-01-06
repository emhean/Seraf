using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Seraf.XNA;
using Seraf.XNA.Tiled;
using Seraf.XNA.NSECS;
using Seraf.XNA.NSECS.Components;
using Seraf.XNA.Controls;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Seraf.XNA.Debug;
using Seraf.XNA.Collision;

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

        Entity player;
        Vector2 spawn;
        public SpriteFont Font_UI { get; private set; }


        //TileCursor tileCursor;

        List<Camera2DCutscene> camera2DCutscenes;
        Camera2DControlled cam;
        Scene scene;
        KeyboardState keyboardState, prev_keyboardState;
        RandomizedColors textRenderer;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowAltF4 = true;
            //Window.IsBorderless = true;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 960;
            graphics.ApplyChanges();
        }
        protected override void Initialize()
        {
            ContentPipeline.CreateInstance(this.Content);
            base.Initialize();
        }

        #region Content stuff
        /// <summary>
        /// Loads a map. Example: "maps/testmap/", "testmap.tmx"
        /// </summary>
        void LoadMap(string fileFolder, string fileName)
        {
            var fm = new FileManager<TTileMap>();
            map = fm.ImportRaw("Content/" + fileFolder + fileName);

            engine = new Engine(map);
            cam = new Camera2DControlled();
            cam.Zoom = 3f;
            camera2DCutscenes = new List<Camera2DCutscene>();

            for (int i = 0; i < map.tileSets.Count; ++i)
            {
                var ts_fm = new FileManager<TTileSet>();
                var loaded_set = ts_fm.ImportRaw("Content/" + fileFolder + map.tileSets[i].source);
                loaded_set.source = map.tileSets[i].source;
                loaded_set.firstgid = map.tileSets[i].firstgid;
                var file = loaded_set.image.image_source.Remove(loaded_set.image.image_source.Length - 4, 4);
                loaded_set.image.texture = Content.Load<Texture2D>("maps/testmap/" + file);

                map.tileSets[i] = loaded_set; // Set new reference
                map.tileSets[i].Properties.CopyFrom(loaded_set.Properties); // Copy the properties
                map.tileSets[i].CreateClips(); // Create clips after loading the *.tsx file.
            }

            foreach (var l in map.tileLayers)
            {
                l.tileData.ConstructTiles(map.tileWidth, l.width, l.height);
            }

            this.player = CreatePlayer();
            engine.AddEntity(player);

            var builder = new EntityBuilder();
            foreach (var ogroups in map.objectGroups)
            {
                foreach (var obj in ogroups.objects)
                {
                    var ent = builder.CreateFromFile(obj, "Content/entities/" + obj.type);
                    engine.AddEntity(ent);

                    if (ent.type.Equals("Spawn") && ent.name.Equals("spawn_start"))
                    {
                        this.spawn = ent.pos;
                        player.pos = spawn;
                    }

                    if (ent.type.Equals("LoadZone"))
                    {
                        var collider = ent.GetComponent<Collider>();
                        collider.Collided += LoadZone_LoadMap;
                    }
                }
            }
        }

        private void LoadZone_LoadMap(object sender, CollisionArgs e)
        {
            var collider = (Collider)sender;
            var ent = (Entity)collider.Entity;

            LoadMap("maps/testmap/", ent.Properties.GetPropertyValue("Map"));

            // Find the target spawn of the loadzone.
            string targetSpawnName = ent.Properties.GetPropertyValue("TargetSpawn");
            bool set = false;

            foreach (var o in engine.entities)
            {
                if (o.type == "Spawn" && o.name == targetSpawnName)
                {
                    // Set position to target position of loadingzone.
                    var p_coll = player.GetComponent<Collider>();
                    // Vector2.UnitX because we want to center X only.
                    p_coll.SetPosition((o.pos - (player.size / 2) * Vector2.UnitX));

                    Console.WriteLine("Player position set to: " + o.pos);
                    set = true;
                    break;
                }

                if (set)
                    break;
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            scene = new Scene(GraphicsDevice, spriteBatch);

            Font_UI = ContentPipeline.Instance.Load<SpriteFont>("fonts/ui");

            #region Test stuff
            // Debug tool: Tile Cursor
            //tileCursor = new TileCursor(Content.Load<Texture2D>("debug/tilecursor"));
            // TEXT TEST
            textRenderer = new RandomizedColors("Testing", Font_UI, new Color[] { Color.Blue, Color.AliceBlue });
            #endregion


            LoadMap("maps/testmap/", "testmap.tmx");



            #region Buttons
            //#region Respawn Button
            //var tex = ContentPipeline.Instance.Load<Texture2D>("sprites/button");
            //var tex_hover = ContentPipeline.Instance.Load<Texture2D>("sprites/button_hover");
            //var tex_click = ContentPipeline.Instance.Load<Texture2D>("sprites/button_click");

            //var button_respawn = new Button(new Text(Font_UI,
            //    new Rectangle(0, 0, 0, 0), "Respawn"), new Rectangle(50, 50, tex.Width * 2, tex.Height * 2))
            //{
            //    Texture = tex,
            //};

            //button_respawn.Text.Parent = button_respawn;
            //button_respawn.Text.TextAlignX = TextAlignX.Center;
            //button_respawn.Text.TextAlignY = TextAlignY.Center;

            //button_respawn.CursorEnter += (object o, EventArgs e) => button_respawn.Texture = tex_hover;
            //button_respawn.CursorLeave += (object o, EventArgs e) => button_respawn.Texture = tex;
            //button_respawn.ClickHold += (object o, EventArgs e) => button_respawn.Texture = tex_click;
            //button_respawn.Clicked += delegate (object o, EventArgs e)
            //{
            //    button_respawn.Texture = tex;

            //    player.pos = spawn;
            //    var p = player.GetComponent<Player>();
            //    p.Respawn();
            //};

            //scene.Controls.Add(button_respawn);
            //#endregion

            //#region Save Button
            //var button_save = new Button(new Text(Font_UI, new Rectangle(0, 0, 0, 0), "Save"),
            //    new Rectangle(50, 60 + tex.Height + tex.Height, tex.Width * 2, tex.Height * 2))
            //{
            //    Texture = tex
            //};
            //button_save.Text.Parent = button_save;
            //button_save.Text.TextAlignX = TextAlignX.Center;
            //button_save.Text.TextAlignY = TextAlignY.Center;

            //button_save.CursorEnter += (object o, EventArgs e) => button_save.Texture = tex_hover;
            //button_save.CursorLeave += (object o, EventArgs e) => button_save.Texture = tex;
            //button_save.ClickHold += (object o, EventArgs e) => button_save.Texture = tex_click;
            //button_save.Clicked += delegate (object o, EventArgs e)
            //{
            //    button_save.Texture = tex;

            //    //spawn = mario.pos; // Set current position to new spawn position.
            //    //var entityParser = new EntityBuilder();
            //    //foreach (var groups in map.objectGroups)
            //    //{
            //    //    foreach (var ent in engine.entities)
            //    //    {
            //    //        for (int i = 0; i < groups.objects.Count; ++i)
            //    //        {
            //    //            if (ent.uuid.Equals(groups.objects[i].id))
            //    //                groups.objects[i] = entityParser.CreateTObject(ent);
            //    //        }
            //    //    }
            //    //}
            //    //map.Save("testmap.tmx");
            //};
            //scene.Controls.Add(button_save);
            //#endregion



            //var tex_button_inv = ContentPipeline.Instance.Load<Texture2D>("sprites/inventory/inv_btn");
            //var button_inv = new Button(new Rectangle(50, 250, tex_button_inv.Width * 4, tex_button_inv.Height * 4))
            //{
            //    Texture = tex_button_inv
            //};
            //scene.Controls.Add(button_inv);


            //var tex_inv_btn = ContentPipeline.Instance.Load<Texture2D>("sprites/inventory/inv_container");
            //var tex_inv_btn_hover = ContentPipeline.Instance.Load<Texture2D>("sprites/inventory/inv_container_hover");
            //var tex_inv_btn_click = ContentPipeline.Instance.Load<Texture2D>("sprites/inventory/inv_container_click");
            //for (int i = 0; i < 4; ++i)
            //{
            //    for(int j = 0; j < 5; ++j)
            //    {
            //        var inv_btn = new Button(
            //            new Rectangle(50 + ( (i * tex_inv_btn.Width) * 4),
            //            (250 + button_inv.Texture.Height * 4) + ((j * tex_inv_btn.Height) * 4),
            //            tex_inv_btn.Width * 4, tex_inv_btn.Height * 4))
            //        {
            //            Texture = tex_inv_btn,
            //            Tag = "inv_container",
            //            IsVisible  = false
            //        };

            //        inv_btn.CursorEnter += (object o, EventArgs e) => inv_btn.Texture = tex_inv_btn_hover;
            //        inv_btn.CursorLeave += (object o, EventArgs e) => inv_btn.Texture = tex_inv_btn;
            //        inv_btn.ClickHold += (object o, EventArgs e) => inv_btn.Texture = tex_inv_btn_click;

            //        inv_btn.Clicked += delegate (object o, EventArgs e)
            //        {
            //            inv_btn.Texture = tex_inv_btn;

            //            Console.WriteLine("[Inventory Container, Contains: " + inv.slots[0] + "]");
            //        };

            //        scene.Controls.Add(inv_btn);
            //    }

            //}

            //button_inv.Clicked += delegate (object o, EventArgs e)
            //{
            //    if (inv.IsVisible)
            //    {
            //        inv.IsVisible = false;
            //        foreach(var b in scene.Controls)
            //        {
            //            if(b.Tag != null && b.Tag.Equals("inv_container"))
            //                b.IsVisible = false;
            //        }
            //    }
            //    else
            //    {
            //        inv.IsVisible = true;
            //        foreach (var b in scene.Controls)
            //        {
            //            if (b.Tag != null && b.Tag.Equals("inv_container"))
            //                b.IsVisible = true;
            //        }
            //    }
            //};
            #endregion
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        Entity CreatePlayer()
        {
            var e = new Entity(0, new Vector2(70, 70), new Vector2(16, 21));
            var anim = new SpriteAnim(e, null);
            var collider = new Collider(e);
            var phys = new Physics(e);
            e.AddComponent(phys);
            e.AddComponent(collider);
            e.AddComponent(new Player(e, anim, phys, collider));
            e.AddComponent(anim);
            var inv = new Inventory(e);
            e.AddComponent(inv);
            return e;
        }
        #endregion

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            engine.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            cam.UpdateControls((float)gameTime.ElapsedGameTime.TotalSeconds);
            

            if(keyboardState.IsKeyDown(Keys.F) && prev_keyboardState.IsKeyUp(Keys.F))
                graphics.ToggleFullScreen();

            if (keyboardState.IsKeyDown(Keys.C) && prev_keyboardState.IsKeyUp(Keys.C))
            {
                if(camera2DCutscenes.Count == 0)
                {
                    Camera2DCutscene cutscene = new Camera2DCutscene(cam, spawn);
                    cutscene.DestinationReached += delegate (object o, EventArgs e)
                    {
                        var soundEffect = ContentPipeline.Instance.Load<SoundEffect>("bgm/solution");
                        soundEffect.Play();
                    };
                    camera2DCutscenes.Add(cutscene);
                }
            }

            if (camera2DCutscenes.Count == 0)
            {
                cam.Position = player.pos + (player.size / 2);
            }
            else
            {
                camera2DCutscenes[0].UpdatePosition((float)gameTime.ElapsedGameTime.TotalSeconds);

                if (camera2DCutscenes[0].Finished)
                {
                    camera2DCutscenes.RemoveAt(0);
                }
            }


            //tileCursor.Update(cam, GraphicsDevice);
            //foreach(var layer in engine.map.tileLayers)
            //{
            //    foreach (var tile in engine.GetRelevantTiles(layer, tileCursor.GetVector2(), 4))
            //    {
            //        tileCursor.CheckCollision(tile.rect);

            //        //Console.WriteLine("Collided");
            //    }
            //}
            
            scene.Camera = cam;
            scene.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            var p = player.GetComponent<Physics>();
            //var player = mario.GetComponent<Player>();

            Window.Title = string.Format("isJumping={0}, isFalling={1}, jump={2}", p.isJumping, p.isFalling, p.jump);
            //Window.Title = string.Format("mrect: {0}, rect: {1}", tileCursor.mRect, tileCursor.rect);

            prev_keyboardState = keyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.GraphicsDevice.Clear(map.backgroundColor);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, cam.GetTransformation(GraphicsDevice));


            foreach (var layer in map.imageLayers)
                spriteBatch.Draw(layer.image.texture, layer.offset, Color.White * layer.opacity);

            engine.Render(scene);

            //tileCursor.Render(spriteBatch);
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            scene.RenderControls();
            textRenderer.Render(spriteBatch, new Vector2(250, 100));
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
