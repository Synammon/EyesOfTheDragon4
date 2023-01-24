using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharedProject.Controls;

namespace SharedProject
{
    public abstract partial class GameState : DrawableGameComponent
    {
        #region Fields and Properties

        protected ControlManager controls;

        private readonly List<GameComponent> childComponents;
        
        protected SpriteBatch SpriteBatch { get; set; }

        public ControlManager ControlManager { get { return controls; } }

        public List<GameComponent> Components
        {
            get { return childComponents; }
        }

        readonly GameState tag;

        public GameState Tag
        {
            get { return tag; }
        }

        protected GameStateManager StateManager;

        #endregion

        #region Constructor Region

        public GameState(Game game)
        : base(game)
        {
            childComponents = new List<GameComponent>();
            tag = this;
        }

        #endregion

        #region MG Drawable Game Component Methods

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            SpriteBatch = Game.Services.GetService<SpriteBatch>();
            controls = new(Game.Content.Load<SpriteFont>(@"Fonts/ControlFont"));
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameComponent component in childComponents)
            {
                if (component.Enabled)
                    component.Update(gameTime);
            }

            ControlManager.Update(gameTime, PlayerIndex.One);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            DrawableGameComponent drawComponent;

            foreach (GameComponent component in childComponents)
            {
                if (component is DrawableGameComponent)
                {
                    drawComponent = component as DrawableGameComponent;
                    if (drawComponent.Visible)
                        drawComponent.Draw(gameTime);
                }
            }

            ControlManager.Draw(SpriteBatch);

            base.Draw(gameTime);
        }

        #endregion

        #region GameState Method Region

        internal protected virtual void StateChange(object sender, EventArgs e)
        {
            StateManager ??= Game.Services.GetService<GameStateManager>();

            if (StateManager.CurrentState == Tag)
                Show();
            else
                Hide();
        }

        protected virtual void Show()
        {
            Visible = true;
            Enabled = true;

            foreach (GameComponent component in childComponents)
            {
                component.Enabled = true;

                if (component is DrawableGameComponent child)
                {
                    child.Visible = true;
                }
            }
        }

        protected virtual void Hide()
        {
            Visible = false;
            Enabled = false;

            foreach (GameComponent component in childComponents)
            {
                component.Enabled = false;

                if (component is DrawableGameComponent child)
                {
                    child.Visible = false;
                }
            }
        }

        #endregion
    }
}
