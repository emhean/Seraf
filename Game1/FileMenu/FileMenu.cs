using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Seraf.XNA.FileMenu
{
    /// <summary>
    /// A file menu where the player can choose between save files.
    /// </summary>
    public class FileMenu
    {
        ///////////////////////////////////////////////////////
        // !!! WARNING !!!
        // THIS CODE IS A NIGHTMARE TO WORK WITH.
        // DO NOT MAKE ANY MAJOR CHANGES WITHOUT DEBUGGING.
        ///////////////////////////////////////////////////////


        #region Private classes
        /// <summary>
        /// The file menu buttons.
        /// </summary>
        private class FileMenuButton
        {
            public Texture2D texture;
            public Rectangle bounds;
            public Rectangle[] clips;
            /// <summary>
            /// 0 = idle, 1 = hover
            /// </summary>
            public int state;

            public string text;
            public Vector2 text_pos;

            public FileMenuButton(Texture2D texture, int x, int y, string text)
            {
                this.texture = texture;

                int h = texture.Height / 2; // To Quick fix for clips
                this.bounds = new Rectangle(x, y, texture.Width, h);
                this.clips = new Rectangle[]
                {
                    new Rectangle(0, 0, texture.Width, h),
                    new Rectangle(0, h, texture.Width, h)
                };
                this.state = 0;
                this.text = text;
            }
        }
        /// <summary>
        /// The tiled scrolling background behind the file menu.
        /// </summary>
        private class Background
        {
            Texture2D texture;
            Rectangle[] tiles;

            public Background(Texture2D texture)
            {
                this.texture = texture;
                this.tiles = GetRectangles();
            }

            Rectangle[] GetRectangles()
            {
                int w = texture.Width;
                int h = texture.Height;

                // DO ONLY TWEAK THIS IF SIZE OF TILED BACKGROUND IS CHANGED!
                return new Rectangle[]
                {
                        new Rectangle(0, 0, w, h),
                        new Rectangle(w, 0, w, h),
                        new Rectangle(w * 2, 0, w, h),
                        new Rectangle(w * 3, 0, w, h),
                        new Rectangle(w * 4, 0, w, h),
                        new Rectangle(w * 5, 0, w, h),
                        //////////////////////////////////
                        new Rectangle(0, h, w, h),
                        new Rectangle(w, h, w, h),
                        new Rectangle(w * 2, h, w, h),
                        new Rectangle(w * 3, h, w, h),
                        new Rectangle(w * 4, h, w, h),
                        new Rectangle(w * 5, h, w, h),
                        ///////////////////////////////////////
                        new Rectangle(0, h * 2, w, h),
                        new Rectangle(w, h * 2, w, h),
                        new Rectangle(w * 2, h * 2, w, h),
                        new Rectangle(w * 3, h * 2, w, h),
                        new Rectangle(w * 4, h * 2, w, h),
                        new Rectangle(w * 5, h * 2, w, h),
                };
            }

            public void Move()
            {
                for (int i = 0; i < tiles.Length; ++i)
                {
                    tiles[i].X -= 1;
                    //tiles[i].Y -= 1;
                }
            }

            public void Update()
            {
                if (tiles[0].X == -texture.Width)
                    this.tiles = GetRectangles();
                
                Move();
            }

            public void Render(SpriteBatch spriteBatch, Color color)
            {
                for (int i = 0; i < tiles.Length; ++i)
                    spriteBatch.Draw(texture, tiles[i], color);
            }
        }
        #endregion


        #region Constants
        public const int SAVEFILE_COUNT = 3; // The number of save files. Changing this, if wanted, should be safe.
        public const string STR_TITLE = "Please select a file.";
        public const string STR_START = "Start?";
        #endregion

        #region FileMenu variables
        FileMenuButton[] fileMenuButtons; // The File buttons along with Erase and Options
        FileMenuButton[] yesNo_btns; // The Yes/No buttons after selecting a file
        Background background; // The tiled scrolling background
        Texture2D menu; // The menu texture
        float menu_opacity; // The opacity of the menu texture.
        SpriteFont font; // The font of everything
        Color color_hover; // Color of hovered button
        Color color_selected; // Color of selected button
        Vector2 str_title_pos; // Position of the title on the menu
        Vector2 str_start_pos; // Position of the Start? question after selecting a file

        KeyboardState kState; // State of frame
        KeyboardState p_kState; // Previous state
        bool file_isSelected;
        int selected_file; // The zero-based index of the selected file
        float t; // Time variable that is incremented by delta time
        //bool blink; // Whether the selected button is blinking. Maybe not needed?
        //float t_blink; // Time variable for blink
        int currentButton; // Index of current button. Used for File buttons along with Erase and Options.
        int currentChoice; // Index of current choice. Used with Yes/No buttons.

        Matrix matrix; // Transformation matrix to center the menu 
        Vector2 pos; // Position of menu.
        float rot; // Rotation of menu.
        float zoom; // Zoom of menu.
        bool fadeOut, fadeIn; // Whether menu is fading in or fading out.
        float fadeValue; // The opacity value of fade, render color is multipled with it.
        #endregion

        #region Cursor variables
        float cos = 0; // The sin value.
        float cos_t = 0; // Time variable.
        Texture2D cursor; // Texture of cursor.
        Vector2 cursor_pos; // Position of cursor.
        bool cursor_visible;
        // Sinus variables to avoid shit ton of local variables.
        Vector2 cos_v; // Sinus vector.
        Vector2 cos_min; // Minimum value of sinus vector.
        float cos_val; // The final result of clamping sinus with sinus minimum.
        #endregion

        #region Cursor functions
        public void ShowCursor()
        {
            cursor_visible = true;
        }
        public void HideCursor()
        {
            cursor_visible = false;
        }
        #endregion


        /// <summary>
        /// Occurs all the time something happens but with an enum for the differect scenarios.
        /// </summary>
        public event EventHandler<FileMenuArgs> MenuUpdated;
        protected void OnMenuUpdate(FileMenuArgs args) => MenuUpdated?.Invoke(this, args);

        public event EventHandler<FileMenuArgs> FileSelected;
        protected void OnFileSelected(FileMenuArgs args) => FileSelected?.Invoke(this, args);

        public event EventHandler<FileMenuArgs> FileChosen;
        protected void OnFileChosen(FileMenuArgs args) => FileChosen?.Invoke(this, args);

        public event EventHandler<FileMenuArgs> FadeOutExpired;
        public event EventHandler<FileMenuArgs> FadeInExpired;
        protected void OnFadeOutExpired() => FadeOutExpired?.Invoke(this, new FileMenuArgs(-1, FileMenuChoice.None));
        protected void OnFadeInExpired() => FadeInExpired?.Invoke(this, new FileMenuArgs(-1, FileMenuChoice.None));

        public FileMenu(ContentManager content)
        {
            this.background = new Background(content.Load<Texture2D>("filemenu/background"));
            this.menu = content.Load<Texture2D>("filemenu/menu");
            this.menu_opacity = 0.9f;
            this.font = content.Load<SpriteFont>("filemenu/font");
            this.color_hover = Color.DarkGray;
            this.color_selected = Color.Yellow;
            this.str_title_pos = new Vector2(14, 7);
            this.cursor = content.Load<Texture2D>("filemenu/hand_r");

            var button_tex = content.Load<Texture2D>("filemenu/button_38x28w");
            this.fileMenuButtons = new FileMenuButton[]
            {
                    new FileMenuButton(button_tex, 14, 24, "File 1"),
                    new FileMenuButton(button_tex, 14, 40, "File 2"),
                    new FileMenuButton(button_tex, 14, 56, "File 3"),
                    new FileMenuButton(button_tex, 14, 79, "Erase"),
                    new FileMenuButton(button_tex, 14, 95, "Options")
            };

            this.yesNo_btns = new FileMenuButton[]
            {
                new FileMenuButton(button_tex, menu.Width - (18 + button_tex.Width * 2), 95, "No"),
                new FileMenuButton(button_tex, menu.Width - (16 + button_tex.Width), 95, "Yes"),
            };
            this.str_start_pos = new Vector2( (yesNo_btns[0].bounds.X + button_tex.Width / 2) + STR_START.Length / 2, 82);

            #region Adjust the text positions
            Vector2 measure = font.MeasureString("A"); // Measurement
            foreach (var b in fileMenuButtons)
                b.text_pos = new Vector2(b.bounds.X + font.MeasureString("A").X / 2, b.bounds.Y);

            foreach (var b in yesNo_btns)
                b.text_pos = new Vector2(b.bounds.X + font.MeasureString("A").X / 2, b.bounds.Y);
            #endregion

            this.pos = new Vector2(menu.Bounds.Width / 2, menu.Bounds.Height / 2);
            this.zoom = 3.5f;
            this.rot = 0f;

            this.fadeIn = true;
            this.fadeValue = 1.0f;
        }

        public FileMenuChoice GetFileEnumFromInt(int choice)
        {
            if (choice == 0)
                return FileMenuChoice.FileOne;
            else if (choice == 1)
                return FileMenuChoice.FileTwo;
            else if (choice == 2)
                return FileMenuChoice.FileThree;

            throw new Exception("Invalid parameter!");
        }

        /// <summary>
        /// Updates the file menu.
        /// </summary>
        public void Update(float delta)
        {
            t += delta;

            background.Update();

            cos_t += delta;
            cos = (float)(Math.Cos(cos_t * 10));

            p_kState = kState;
            kState = Keyboard.GetState();

            if(fadeIn)
            {
            }
            else if(fadeOut)
            {
            }
            else
            {
                if (!file_isSelected)
                {
                    // Cursor stuff
                    cursor_pos.X = fileMenuButtons[currentButton].bounds.X - cursor.Width;
                    cursor_pos.Y = fileMenuButtons[currentButton].bounds.Y;
                    cursor_pos.X += cos;

                    if (t > 0.2f)
                    {
                        if (kState.IsKeyDown(Keys.S))
                        {
                            currentButton += 1;
                            if (currentButton == fileMenuButtons.Length)
                                currentButton = 0;

                            OnMenuUpdate(new FileMenuArgs(currentButton, FileMenuChoice.SelectedButton));
                            t = 0; // Reset time
                        }
                        else if (kState.IsKeyDown(Keys.W))
                        {
                            currentButton -= 1;
                            if (currentButton == -1)
                                currentButton = fileMenuButtons.Length - 1;

                            OnMenuUpdate(new FileMenuArgs(currentButton, FileMenuChoice.SelectedButton));
                            t = 0; // Reset time
                        }
                        else if (kState.IsKeyDown(Keys.E) && p_kState.IsKeyUp(Keys.E))
                        {
                            //blink = (blink) ? false : true; // Let's forget about blink shit for now
                            t = 0; // Reset time

                            file_isSelected = true;
                            OnMenuUpdate(new FileMenuArgs(currentButton, FileMenuChoice.SelectedFile));


                            // I know this is terrible
                            // TODO: Fix this
                            OnFileSelected(new FileMenuArgs(currentButton, GetFileEnumFromInt(currentButton)));



                            selected_file = currentButton;// Selected file because amount of files is equal to amount of buttons
                            //currentButton = 0; // This might not be needed?
                            currentChoice = 0;
                        }

                        for (int i = 0; i < fileMenuButtons.Length; ++i)
                            fileMenuButtons[i].state = 0;
                        fileMenuButtons[currentButton].state = 1;
                    }
                } // end of !file_isSelected
                else
                {
                    // Cursor stuff
                    cursor_pos.X = yesNo_btns[currentChoice].bounds.X - cursor.Width;
                    cursor_pos.Y = yesNo_btns[currentChoice].bounds.Y;
                    cursor_pos.X += cos;

                    if (t > 0.2f)
                    {
                        // File is selected
                        if (kState.IsKeyDown(Keys.A))
                        {
                            currentChoice += 1;
                            if (currentChoice == yesNo_btns.Length)
                                currentChoice = 0;

                            OnMenuUpdate(new FileMenuArgs(currentChoice, FileMenuChoice.SelectedButton));
                            t = 0; // Reset time
                        }
                        else if (kState.IsKeyDown(Keys.D))
                        {
                            currentChoice -= 1;
                            if (currentChoice == -1)
                                currentChoice = yesNo_btns.Length - 1;

                            OnMenuUpdate(new FileMenuArgs(currentChoice, FileMenuChoice.SelectedButton));
                            t = 0; // Reset time
                        }
                        else if (kState.IsKeyDown(Keys.E) && p_kState.IsKeyUp(Keys.E))
                        {
                            if (currentChoice == 1)
                            {
                                fadeOut = true;
                                fadeValue = 0f;
                                // Instead of invoking the event here lets do it after the fade out. It's in the Render logic.
                                //OnFileSelected(new FileMenuArgs(selected_file, FileMenuChoice.Yes));
                            }
                            else
                            {
                                file_isSelected = false;
                                currentButton = selected_file;
                                
                                if(currentButton > 2)
                                    OnMenuUpdate(new FileMenuArgs(selected_file, FileMenuChoice.FileNo));
                            }

                            t = 0; // Reset time
                        }
                    }

                    for (int i = 0; i < yesNo_btns.Length; ++i)
                        yesNo_btns[i].state = 0;

                    yesNo_btns[currentChoice].state = 1; 
                }
            }
        }

        /// <summary>
        /// Renders the file menu. DO send a SpriteBatch without a Begin() call! Let this method do all the work.
        /// </summary>
        public void Render(SpriteBatch spriteBatch, Viewport viewport)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            background.Render(spriteBatch, Color.White);
            spriteBatch.End();

            matrix = GetTranslationMatrix(viewport);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, matrix);
            spriteBatch.Draw(menu, Vector2.Zero, Color.White * menu_opacity);

            if ( !file_isSelected)
            {
                for (int i = 0; i < fileMenuButtons.Length; ++i)
                {
                    if (i == currentButton)
                        spriteBatch.Draw(fileMenuButtons[i].texture, fileMenuButtons[i].bounds, fileMenuButtons[i].clips[fileMenuButtons[i].state], color_hover);
                    else
                        spriteBatch.Draw(fileMenuButtons[i].texture, fileMenuButtons[i].bounds, fileMenuButtons[i].clips[fileMenuButtons[i].state], Color.White);

                    // Render text
                    if(fileMenuButtons[i].state == 1)
                        spriteBatch.DrawString(font, fileMenuButtons[i].text, fileMenuButtons[i].text_pos + Vector2.UnitY, Color.Black);
                    else
                        spriteBatch.DrawString(font, fileMenuButtons[i].text, fileMenuButtons[i].text_pos, Color.Black);
                }
            }
            else
            {
                // Selected file because amount of files is equal to amount of buttons
                spriteBatch.Draw(fileMenuButtons[selected_file].texture, fileMenuButtons[selected_file].bounds,
                    fileMenuButtons[selected_file].clips[fileMenuButtons[selected_file].state], color_selected);

                spriteBatch.DrawString(font, fileMenuButtons[selected_file].text, fileMenuButtons[selected_file].text_pos + Vector2.UnitY, Color.Black);

                for (int i = 0; i < fileMenuButtons.Length; ++i)
                {
                    if (i != selected_file)
                    {
                        spriteBatch.Draw(fileMenuButtons[i].texture, fileMenuButtons[i].bounds, fileMenuButtons[i].clips[fileMenuButtons[i].state], Color.White);

                        // Render text
                        if(fileMenuButtons[i].state == 1)
                            spriteBatch.DrawString(font, fileMenuButtons[i].text, fileMenuButtons[i].text_pos + Vector2.UnitY, Color.Black);
                        else
                            spriteBatch.DrawString(font, fileMenuButtons[i].text, fileMenuButtons[i].text_pos, Color.Black);
                    }
                }

                cos_v = new Vector2(0, cos * 2);
                cos_min = new Vector2(0.9f, 0.9f);
                cos_val = Vector2.Clamp(cos_v, cos_min, Vector2.One ).Y;

                if (currentChoice == 1)
                    spriteBatch.Draw(yesNo_btns[1].texture, yesNo_btns[1].bounds, yesNo_btns[1].clips[yesNo_btns[1].state], Color.Yellow * cos_val);
                else
                    spriteBatch.Draw(yesNo_btns[1].texture, yesNo_btns[1].bounds, yesNo_btns[1].clips[yesNo_btns[1].state], Color.White);

                if (currentChoice == 0)
                    spriteBatch.Draw(yesNo_btns[0].texture, yesNo_btns[0].bounds, yesNo_btns[0].clips[yesNo_btns[0].state], Color.Red * cos_val);
                else
                    spriteBatch.Draw(yesNo_btns[0].texture, yesNo_btns[0].bounds, yesNo_btns[0].clips[yesNo_btns[0].state], Color.White);

                // Render text
                if (yesNo_btns[0].state == 1)
                    spriteBatch.DrawString(font, yesNo_btns[0].text, yesNo_btns[0].text_pos + Vector2.UnitY, Color.Black);
                else
                    spriteBatch.DrawString(font, yesNo_btns[0].text, yesNo_btns[0].text_pos, Color.Black);

                if (yesNo_btns[1].state == 1)
                    spriteBatch.DrawString(font, yesNo_btns[1].text, yesNo_btns[1].text_pos + Vector2.UnitY, Color.Black);
                else
                    spriteBatch.DrawString(font, yesNo_btns[1].text, yesNo_btns[1].text_pos, Color.Black);
            }

            // Render the title
            spriteBatch.DrawString(font, STR_TITLE, str_title_pos, Color.White);

            if(file_isSelected) // Render the Start? text
            {
                spriteBatch.DrawString(font, STR_START, str_start_pos, Color.White);
            }

            if(cursor_visible) // Render cursor if visible
                spriteBatch.Draw(cursor, cursor_pos, Color.White);

            spriteBatch.End();

            if(fadeIn)
            {
                spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                background.Render(spriteBatch, Color.Black * fadeValue);
                spriteBatch.End();
                fadeValue -= 0.01f;
                if(fadeValue < 0)
                {
                    fadeValue = 0f;
                    fadeIn = false;

                    OnFadeInExpired();
                }
            }
            else if(fadeOut)
            {
                spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                background.Render(spriteBatch, Color.Black * fadeValue);
                fadeValue += 0.01f;
                if (fadeValue > 1)
                {
                    fadeValue = 0f;

                    if (selected_file < SAVEFILE_COUNT)
                    {
                        OnMenuUpdate(new FileMenuArgs(selected_file, FileMenuChoice.FileYes));
                        OnFileChosen(new FileMenuArgs(selected_file, GetFileEnumFromInt(selected_file)));
                    }
                    else
                    {
                        OnMenuUpdate(new FileMenuArgs(currentButton, FileMenuChoice.SelectedButton));
                    }

                    fadeOut = false;
                    OnFadeOutExpired();
                }

                spriteBatch.End();
            }
        }
        

        protected Matrix GetTranslationMatrix(Viewport viewport)
        {
            return Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0))
                * Matrix.CreateRotationZ(rot)
                * Matrix.CreateScale(new Vector3(zoom, zoom, 1))
                * Matrix.CreateTranslation(new Vector3(viewport.Width * 0.5f, viewport.Height * 0.5f, 0));
        }
    }
}
