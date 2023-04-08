using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgLibrary.Characters;
using RpgLibrary.TileEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgLibrary
{
    public class Encounter
    {
        public List<ICharacter> Allies { get; set; } = new();
        public List<ICharacter> Enemies { get; set; } = new();
        public TileMap Map { get; set; }

        public bool Alive => Enemies.Count > 0 && Allies.Count > 0;

        public Encounter(ICharacter player) 
        {
            Allies.Add(player);
        }

        public void Update(GameTime gameTime)
        {
            Map?.Update(gameTime);

            foreach (var character in Enemies) 
            { 
                character.Update(gameTime); 
            }

            Enemies.RemoveAll(x => x.Health.Current <= 0);
            Allies.RemoveAll(x => x.Health.Current <= 0);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera) 
        {
            Map?.Draw(gameTime, spriteBatch, camera);

            foreach (var character in Enemies)
            {
                character.Draw(spriteBatch);
            }

            foreach (var character in Allies)
            {
                character.Draw(spriteBatch);
            }
        }
    }
}
