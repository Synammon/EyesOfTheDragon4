using Microsoft.Xna.Framework;
using SharedProject.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.GameStates
{
    public class MainForm : Form
    {
        public MainForm(Game game, Vector2 position, Point size)
            : base(game, position, size)
        {
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
