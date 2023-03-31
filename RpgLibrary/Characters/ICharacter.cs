using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgLibrary.Characters
{
    public struct AttributePair
    {
        public int Current;
        public int Maximum;

        public AttributePair()
        {
            Current = 10;
            Maximum = 10;
        }

        public AttributePair(int maximum)
        {
            Current = Maximum = maximum;
        }

        public void Adjust(int amount)
        {
            Current += amount;

            if (Current > Maximum)
            {
                Current = Maximum;
            }
        }
    }

    public interface ICharacter
    {
        string Name { get; }

        int Stength { get; set; }
        int Perception { get; set; }
        int Endurance { get; set; }
        int Charisma { get; set; }
        int Intellect { get; set; }
        int Agility { get; set; }
        int Luck { get; set; }

        AttributePair Health { get; set; }
        AttributePair Mana { get; set; }

        int Gold { get; set; }
        int Experience { get; set; }

        bool Enabled { get; set; }
        bool Visible { get; set; }
        Vector2 Position { get; set; }
        Point Tile { get; set; }
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}
