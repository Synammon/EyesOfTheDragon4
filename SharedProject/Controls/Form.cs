using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace SharedProject.Controls
{
    public abstract class Form : GameState
    {
        private ControlManager _controls;
        protected readonly GraphicsDeviceManager _graphicsDevice;
        private Point _size;
        private Rectangle _bounds;
        private string _title;

        public string Title { get { return _title; } set { _title = value; } }
        public ControlManager Controls { get => _controls; set => _controls = value; }
        public Point Size { get => _size; set => _size = value; }
        public bool FullScreen { get; set; }
        public PictureBox Background { get; private set; }
        public PictureBox TitleBar { get; private set; }
        public Button CloseButton { get; private set; }
        public Rectangle Bounds { get => _bounds; protected set => _bounds = value; }
        public Vector2 Position { get; set; }

        public Form(Game game, Vector2 position, Point size) : base(game)
        {
            Enabled = true;
            Visible = true;
            FullScreen = false;
            _size = size;

            Position = position;
            Bounds = new(Point.Zero, Size);
            _graphicsDevice = (GraphicsDeviceManager)Game.Services.GetService(
                typeof(GraphicsDeviceManager));

            Initialize();
            LoadContent();
            Title = "";
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            _controls = new((Game.Content.Load<SpriteFont>("Fonts/MainFont")));

            TitleBar = new(
                Game.Content.Load<Texture2D>("GUI/TitleBar"),
                new(0, 0, Size.X, 20));

            Background = new(
                Game.Content.Load<Texture2D>("GUI/Form"),
                new(
                    0,
                    20,
                    Bounds.Width,
                    Bounds.Height))
            { FillMethod = FillMethod.Fill };

            CloseButton = new(
                Game.Content.Load<Texture2D>("GUI/CloseButton"),
                ButtonRole.Cancel)
            { Position = Vector2.Zero, Color = Color.White, Text = "" };

            CloseButton.Click += CloseButton_Click;

            if (FullScreen)
            {
                TitleBar.Height = 0;
                Background.Position = new();
                Background.Height = _graphicsDevice.PreferredBackBufferHeight;
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            StateManager.PopState();
        }

        public override void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                CloseButton.Update(gameTime);
                Controls.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (!Visible) return;

            if (!FullScreen)
            {
                Vector2 size = ControlManager.SpriteFont.MeasureString(Title);
                Vector2 position = new((Bounds.Width - size.X) / 2, 0);
                Label label = new()
                {
                    Text = _title,
                    Color = Color.White,
                    Position = position
                };

                Matrix m = Matrix.CreateTranslation(new Vector3(Position, 0));

                SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, null, null, null, m);

                Background.Draw(SpriteBatch);
                TitleBar.Draw(SpriteBatch);

                CloseButton.Draw(SpriteBatch);

                SpriteBatch.End();

                SpriteBatch.Begin();

                label.Position = Helper.NearestInt(position + Position);
                label.Color = Color.White;
                label.Draw(SpriteBatch);

                SpriteBatch.End();

                m = Matrix.CreateTranslation(new Vector3(0, 20, 0) + new Vector3(Position, 0));

                SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, null, null, null, m);

                _controls.Draw(SpriteBatch);

                SpriteBatch.End();
            }
            else
            {
                SpriteBatch.Begin();

                Background.DestinationRectangle = new(
                    0,
                    0,
                    _graphicsDevice.PreferredBackBufferWidth,
                    _graphicsDevice.PreferredBackBufferHeight);

                _controls.Draw(SpriteBatch);

                SpriteBatch.End();
            }
        }
    }
}
