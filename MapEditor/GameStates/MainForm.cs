using Microsoft.Xna.Framework;
using SharedProject;
using SharedProject.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.GameStates
{
    public interface IMainForm
    {
        Form Target { get; }
    }

    public class MainForm : Form, IMainForm
    {
        public Form Target => this;

        public MainForm(Game game, Vector2 position, Point size)
            : base(game, position, size)
        {
            Game.Services.AddService<IMainForm>(this);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
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
