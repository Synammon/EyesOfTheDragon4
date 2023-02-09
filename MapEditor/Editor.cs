using MapEditor.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharedProject;

namespace MapEditor
{
    public class Editor : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly GameStateManager _manager;

        public MainForm MainForm { get; private set; }

        public Editor()
        {
            _graphics = new(this)
            {
                PreferredBackBufferWidth = 1900,
                PreferredBackBufferHeight = 1000
            };

            _graphics.ApplyChanges();

            Services.AddService<GraphicsDeviceManager>(_graphics);

            _manager = new(this);
            Components.Add(_manager);

            Services.AddService<GameStateManager>(_manager);

            Components.Add(new Xin(this));

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService<SpriteBatch>(_spriteBatch);

            // TODO: use this.Content to load your game content here
            MainForm = new(this, Vector2.Zero, new(1900, 1000))
            {
                FullScreen= false,
            };

            _manager.PushState(MainForm);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}