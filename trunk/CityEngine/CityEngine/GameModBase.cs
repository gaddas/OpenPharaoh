// -----------------------------------------------------------------------
// <copyright file="BaseMod.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace CityEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CityEngine.Files;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The base Mod class
    /// </summary>
    public abstract class GameModBase
    {
        /// <summary>
        /// Gets or sets the device.
        /// </summary>
        /// <value>
        /// The device.
        /// </value>
        public Game Game { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitialized { get; set; }

        /// <summary>
        /// Loads the specified device.
        /// </summary>
        public virtual void Load()
        {
            this.IsInitialized = true;
        }

        /// <summary>
        /// Unloads this instance.
        /// </summary>
        public abstract void Unload();

        /// <summary>
        /// Updates the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draws the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public abstract void Draw(GameTime gameTime);

        /// <summary>
        /// Loads the texture2d.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="imageIndex">Index of the image.</param>
        /// <returns>The texture</returns>
        public Texture2D LoadTexture2d(ContainerSG3 container, int imageIndex)
        {
            return container.LoadTexture2d(this.Game.GraphicsDevice, (ushort) imageIndex);
        }
    }
}
