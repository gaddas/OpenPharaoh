using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CityEngine.Files;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CityEngine
{
    [GameMod("OpenPharaoh", "1.0 Alpha")]
    public class OpenPharaoh : GameModBase
    {
        public Dictionary<PharaohTextureIDs, Texture2D> generalTextures;
        private SpriteBatch sb;

        public enum PharaohTextureIDs
        {
            SPR_TITLE_SCREEN = 201,
        }

        public override void Load()
        {
            this.generalTextures = new Dictionary<PharaohTextureIDs, Texture2D>();
            this.sb = new SpriteBatch(this.Game.GraphicsDevice);

            var container = new ContainerSG3(@"C:\Users\bbdnet6039\Downloads\OpenPharaoh\Pharaoh - Cleopatra\Data\Pharaoh_Unloaded.sg3");
            this.generalTextures.Add(PharaohTextureIDs.SPR_TITLE_SCREEN, this.LoadTexture2d(container, 201));
            
            base.Load();
        }

        public override void Unload()
        {
            this.sb.Dispose();
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            //sb.Draw(generalTextures[PharaohTextureIDs.SPR_TITLE_SCREEN], new Vector2(0, 0), Color.White);
            sb.Draw(generalTextures[PharaohTextureIDs.SPR_TITLE_SCREEN], new Rectangle(0, 0, 800, 600), Color.White);
            sb.End();
        }
    }
}
