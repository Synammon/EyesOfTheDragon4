using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Rpglibrary.TileEngine;
using RpgLibrary.Characters;
using RpgLibrary.TileEngine;
using SharpFont;
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

        public bool Alive => Enemies.Any(x => x.Health.Current > 0) && Allies.Any(x => x.Health.Current > 0); 

        public Encounter(ICharacter player) 
        {
            Allies.Add(player);
        }

        public void Update(GameTime gameTime)
        {
            Map?.Update(gameTime);

            foreach (var character in Enemies.Where(x => x.Health.Current > 0))
            {
                character.Update(gameTime);
            }

            foreach (var character in Allies.Where(x => x.Health.Current > 0))
            {
                character.Update(gameTime);
            }
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

        public void RandomMap(ContentManager content)
        {
            Random random = new();

            Texture2D texture = content.Load<Texture2D>(@"Tiles/tileset1");

            List<Tileset> tilesets = new()
            {
                new(texture, 8, 8, 32, 32),
            };

            TileLayer layer = new(1280 / 64 + 1, 720 / 64 + 1);

            Map = new("test", tilesets[0], layer);

            layer = new(1280 / 64 + 1, 720 / 64 + 1);
            CollisionLayer collisions = new();

            for (int y = 0; y < 720 / 64 + 1; y++)
                for (int x = 0; x < 1280 / 64 + 1; x++)
                {
                    layer.SetTile(x, y, new Tile(-1, -1));
                }

            for (int i = 0; i < 20; i++)
            {
                int x;
                int y;

                do
                {
                    x = random.Next(1 + 1280 / 64);
                    y = random.Next(1 + 720 / 64);
                } while (Allies.Any(z => z.Tile.X == x && z.Tile.Y == y) ||
                            Enemies.Any(z => z.Tile.X == x && z.Tile.Y == y));

                collisions.Collisions.Add(new(
                    new(x * Engine.TileWidth, y * Engine.TileHeight), 
                    new(Engine.TileWidth, Engine.TileHeight)),
                    CollisionValue.Impassible);

                layer.SetTile(x, y, new Tile(random.Next(3, 14), 0));
            }

            Map.Layers.Add(layer);
            Map.Layers.Add(collisions);
        }
    }
}
