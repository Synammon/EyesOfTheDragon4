using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharedProject.Controls;
using SharedProject.GamesScreens;
using SummonersTale.StateManagement;

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
        RenderTarget2D renderTarget;

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

            renderTarget = new(GraphicsDevice, Settings.BaseWidth, Settings.BaseHeight);

            ContentManager Content = Game.Content;
            backgroundImage = Content.Load<Texture2D>(@"Backgrounds\titlescreen");

            Label startLabel = new()
            {
                Position = new Vector2(350, 600),
                Text = "Tap to begin",
                Color = Color.White,
                TabStop = true,
                HasFocus = true
            };

            startLabel.Selected += StartLabel_Selected; ;

            ControlManager.Add(startLabel);
        }

        private void StartLabel_Selected(object sender, EventArgs e)
        {
            GameState state = Game.Services.GetService<INewGameState>().GameState;
            StateManager.ChangeState(state);
        }

        public override void Update(GameTime gameTime)
        {
            if (Xin.WasMouseReleased(MouseButtons.Left) || Xin.TouchReleased() || Xin.WasKeyPressed())
            {
                StartLabel_Selected(this, null);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            renderTarget.GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin();

            SpriteBatch.Draw(
                backgroundImage,
                Settings.BaseRectangle,
                Color.White);

            base.Draw(gameTime);

            SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            SpriteBatch.Begin();

            SpriteBatch.Draw(renderTarget, Settings.TargetRectangle, Color.White);

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
