using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject
{
    public interface IPlayer
    {
        string Name { get; }
    }

    public sealed class Player : DrawableGameComponent, IPlayer
    {
        private string _name;

        public Player(Game game) : base(game)
        {
            Game.Services.AddService<IPlayer>(this);
        }

        public string Name => _name;
    }
}
