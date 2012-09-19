using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CityEngine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameMain : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private SpriteFont defaultFont;
        private int totalFrames = 0;
        private float elapsedTime = 0.0f;
        private int fps = 0;

        public GameModBase Mod { get; set; }

        public GameMain()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;

            this.IsMouseVisible = true;
            this.IsFixedTimeStep = true;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine("initializing CityEngine");

            // Get all mods
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            if (directory.Exists)
            {
                var mods = new Dictionary<string, Type>();
                var files = directory.GetFiles("mod-*.dll");

                foreach (var file in files)
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFrom(file.FullName);
                        Type[] types = assembly.GetTypes();

                        foreach (Type type in types)
                        {
                            var attributes = type.GetCustomAttributes(typeof(GameModAttribute), true);
                            if (attributes.Length > 0)
                            {
                                var a = attributes[0] as GameModAttribute;
                                mods.Add(a.name, type);

                                Debug.WriteLine("found mod {0} [{1}]", a.name, a.version);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                }

                if (mods.Count > 0)
                {
                    // Just get the first found mod for now
                    var selectedMod = mods.First();
                    Debug.WriteLine("loading mod: " + selectedMod.Key);

                    this.Mod = (GameModBase)selectedMod.Value.GetConstructor(Type.EmptyTypes).Invoke(null);
                }
            }

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.defaultFont = this.Content.Load<SpriteFont>("DefaultFont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            if (this.Mod != null)
            {
                this.Mod.Unload();
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            this.elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (this.elapsedTime >= 1000.0f)
            {
                this.fps = this.totalFrames;
                this.totalFrames = 0;
                this.elapsedTime = 0;
            }

            if (gameTime.TotalGameTime.Ticks != 0)
            {
                if (!this.Mod.IsInitialized)
                {
                    this.Mod.Game = this;
                    this.Mod.Load();
                }

                this.Mod.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Reference page contains code sample.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Draw.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.totalFrames++;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (this.Mod.IsInitialized)
            {
                this.Mod.Draw(gameTime);
            }

            spriteBatch.Begin();
            spriteBatch.DrawString(this.defaultFont, string.Format("{0}", this.fps), new Vector2(0, 0), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
