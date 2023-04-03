using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgLibrary.Characters;
using SharedProject.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject
{
    public interface IPlayer : ICharacter
    {
        AnimatedSprite Sprite { get; }
    }

    public sealed class Player : DrawableGameComponent, IPlayer
    {
        private string _name;
        private AnimatedSprite _sprite;

        public Player(Game game, AnimatedSprite animatedSprite) : base(game)
        {
            Game.Services.AddService<IPlayer>(this);
            _sprite = animatedSprite;
        }

        public string Name => _name;

        public int Strength { get; set; }
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
        public Vector2 Position { get; set; }
        public Point Tile { get; set; }

        public AnimatedSprite Sprite => _sprite;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch);
        }
    }
}
