using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgLibrary.Characters;
using SharedProject.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject
{
    public class Character : ICharacter
    {
        private string _name;
        private AnimatedSprite _sprite;
        private string _spriteName;

        public string Name => _name;

        public bool Enabled { get; set; }
        public bool Visible { get; set; }
        public Vector2 Position { get; set; }
        public Point Tile { get; set; }

        private Character()
        {
            Enabled = true;
            Visible = true;
            Position = new();
            Tile = new();
        }

        public Character(string name, AnimatedSprite animatedSprite, string spriteName)
        {
            _name = name;
            _sprite = animatedSprite;
            _spriteName = spriteName;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            _sprite.Position= Position;
            _sprite.Update(gameTime);
        }
    }
}
