using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject.GameScreens
{
    public interface IStartMenuState
    {
        GameState Tag { get; }
    }

    public class StartMenuState : GameState, IStartMenuState
    {
        public StartMenuState(Game game) : base(game)
        {
            Game.Services.AddService<IStartMenuState>(this);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
