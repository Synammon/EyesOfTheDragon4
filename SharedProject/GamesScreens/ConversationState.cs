using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgLibrary.ConversationComponents;
using SharedProject.Controls;
using SharpFont.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedProject.StateManagement
{
    public interface IConversationState
    {
        GameState GameState { get; }
        void SetConversation(Player player, string conversation);
        void StartConversation();
    }

    public class ConversationState : GameState, IConversationState
    {
        private IConversationManager _conversations;
        private Player player;
        private Conversation _conversation;
        public GameState GameState => this;
        public RenderTarget2D RenderTarget { get; set; }

        public ConversationState(Game game)
            : base(game)
        {
            Game.Services.AddService<IConversationState>(this);
            _conversations = Game.Services.GetService<IConversationManager>();
            _conversation = new();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            RenderTarget = new(GraphicsDevice, Settings.TargetWidth, Settings.TargetHeight);

            foreach (GameScene scene in _conversation.Scenes.Values)
            {
                scene.ItemSelected += Scene_ItemSelected;
            }
        }

        private void Scene_ItemSelected(object sender, SelectedIndexEventArgs e)
        {
            ButtonGroup btn = (ButtonGroup)sender;

            switch (btn.Action.Action)
            {
                case ActionType.End:
                    StateManager.PopState();
                    break;
                case ActionType.Talk:
                    _conversation.ChangeScene(btn.Action.Parameter);
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            _conversation.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            GraphicsDevice.SetRenderTarget(RenderTarget);
            GraphicsDevice.Clear(Color.Transparent);

            SpriteBatch.Begin();

            base.Draw(gameTime);

            _conversation.Draw(gameTime, SpriteBatch);

            SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            SpriteBatch.Begin();

            SpriteBatch.Draw(RenderTarget, Settings.TargetRectangle, Color.White);

            SpriteBatch.End();
        }

        public void SetConversation(Player player, string conversation)
        {
            this.player = player;
            this._conversation = (Conversation)_conversations.GetConversation(conversation);
        }

        public void StartConversation()
        {
            _conversation.StartConversation();
        }
    }
}
