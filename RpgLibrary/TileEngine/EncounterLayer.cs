using Assimp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgLibrary.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgLibrary.TileEngine
{
    public class EncounterLayer : ILayer
    {
        public Dictionary<ISprite, Encounter> Encounters { get; private set; } = new();

        public EncounterLayer() { }

        public void Draw(SpriteBatch spriteBatch, Camera camera, List<Tileset> tilesets)
        {
            foreach (ISprite sprite in Encounters.Keys) 
            {
                if (Encounters[sprite].Alive)
                {
                    sprite.Draw(spriteBatch);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (ISprite sprite in Encounters.Keys)
            {
                if (Encounters[sprite].Alive)
                {
                    sprite.Update(gameTime);
                }
            }
        }
    }
}
