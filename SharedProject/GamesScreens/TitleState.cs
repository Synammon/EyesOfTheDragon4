using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharedProject.Controls;

namespace SharedProject.GameScreens
{
    public interface ITitleState
    {
        GameState Tag { get; }
    }

    public class TitleState : GameState, ITitleState
    {
        #region Field region

        Texture2D backgroundImage;

        readonly GameState tag;

        public GameState Tag
        {
            get { return tag; }
        }

        #endregion

        #region Constructor region

        public TitleState(Game game)
            : base(game)
        {
            Game.Services.AddService<ITitleState>(this);
            SpriteBatch = Game.Services.GetService<SpriteBatch>();
        }

        #endregion

        #region XNA Method region

        protected override void LoadContent()
        {
            base.LoadContent();

            ContentManager Content = Game.Content;
            backgroundImage = Content.Load<Texture2D>(@"Backgrounds\titlescreen");

            LinkLabel startLabel = new();
            startLabel.Position = new Vector2(350, 600);
            startLabel.Text = "Press ENTER to begin";
            startLabel.Color = Color.White;
            startLabel.TabStop = true;
            startLabel.HasFocus = true;
            startLabel.Selected += StartLabel_Selected; ;

            ControlManager.Add(startLabel);
        }

        private void StartLabel_Selected(object sender, EventArgs e)
        {
            StartMenuState state = (StartMenuState)Game.Services.GetService<IStartMenuState>().Tag;
            StateManager.ChangeState(state);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            SpriteBatch.Draw(
                backgroundImage,
                Settings.BaseRectangle,
                Color.White);

            base.Draw(gameTime);

            SpriteBatch.End();
        }

        protected override void Show()
        {
            base.Show();

            LoadContent();
        }

        #endregion
    }
}
