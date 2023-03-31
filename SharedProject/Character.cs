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
        public int Stength { get; set; }
        public int Perception { get; set; }
        public int Endurance { get; set; }
        public int Charisma { get; set; }
        public int Intellect { get; set; }
        public int Agility { get; set; }
        public int Luck { get; set; }
        public AttributePair Health { get; set; }
        public AttributePair Mana { get; set; }
        public int Gold { get; set; }
        public int Experience { get; set; }

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
