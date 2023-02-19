using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgLibrary.ConversationComponents;
using SharedProject.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedProject.StateManagement
{
    public interface IConversationState
    {
        GameState GameState { get; }
    }

    public class ConversationState : GameState, IConversationState
    {
        private Conversation conversation;
        private Player player;

        public GameState GameState => this;
        public RenderTarget2D RenderTarget { get; set; }

        public ConversationState(Game game)
            : base(game)
        {
            Game.Services.AddService<IConversationState>(this);

            conversation = new();

            SceneAction action = new()
            {
                Action = ActionType.Talk,
                Parameter = "Help"
            };

            List<SceneOption> options = new()
            {
                new SceneOption("Help!", "Help", action)
            };

            action = new()
            {
                Action = ActionType.End,
                Parameter = ""
            };

            options.Add(new("Goodbye.", "Goodbye", action));

            GameScene scene = new(game, "Oh no! The unthinkable has happened! A thief has stolen Greynar's eyes. With out them he will not be able to animated and defend us. You have to do something or the monsters outside the village will crush us.", options);
            conversation.AddScene("Hello", scene);

            options = new();
            options.Add(new("Goodbye.", "Gooodbye", new() { Action = ActionType.End, Parameter = "" }));

            scene = new(game, "Oh thank the heavens for you!", options);
            conversation.AddScene("Help", scene);

            conversation.FirstScene = "Hello";
            conversation.StartConversation();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            foreach (GameScene scene in conversation.Scenes.Values)
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
                    conversation.ChangeScene(btn.Action.Parameter);
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            conversation.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            GraphicsDevice.SetRenderTarget(RenderTarget);
            GraphicsDevice.Clear(Color.Transparent);

            SpriteBatch.Begin();
          
            base.Draw(gameTime);

            conversation.Draw(gameTime, SpriteBatch);

            SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            SpriteBatch.Begin();

            SpriteBatch.Draw(RenderTarget, new Rectangle(Point.Zero, Settings.Resolution), Color.White);

            SpriteBatch.End();
        }

        public void SetConversation(Player player, string conversation)
        {
            this.player = player;
            //this.conversation = conversations.GetConversation(conversation);
        }

        public void StartConversation()
        {
            conversation.StartConversation();
        }
    }
}
