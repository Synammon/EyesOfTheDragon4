using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RpgLibrary.Characters;
using RpgLibrary.TileEngine;
using SharedProject.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject.Characters
{
    public class Villager : ICharacter
    {
        public string Name { get; set; }
        public AnimatedSprite AnimatedSprite { get; private set; }
        public Point Tile { get; set; }
        public Rectangle Bounds { get; private set; }
        public string Conversation { get; set; }
        public string Scene { get; set; }
        public bool Enabled { get; set; }
        public bool Visible { get; set; }
        public Vector2 Position { get; set; }

        public Villager(AnimatedSprite sprite, Point tile)
        {
            Enabled = true;
            Visible = true;
            Position = new();
            Tile = new();
            AnimatedSprite = sprite;
            Tile = tile;
            Bounds = new(Engine.PointToWorld(Tile), new(Engine.TileWidth, Engine.TileHeight));
        }

        public void Update(GameTime gameTime)
        {
            AnimatedSprite.Position = Position;
            AnimatedSprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            AnimatedSprite.Draw(spriteBatch);
        }
    }
}