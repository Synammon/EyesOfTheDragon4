using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using RpgLibrary.ConversationComponents;
using RpgLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharedProject
{
    public interface IConversationManager
    {
        ConversationData GetConversation(string key);
        void AddConversation(string key, ConversationData conversation);
        void LoadConversations(Game game);
        void WriteConversations();
        void ReadConversations();
    }

    public class ConversationManager : GameComponent, IConversationManager
    {
        private readonly Dictionary<string, ConversationData> _conversations = new();
        private readonly Game _game;

        public Dictionary<string, ConversationData> Conversations => _conversations;

        public ConversationManager(Game game) : base(game)
        {
            game.Services.AddService<IConversationManager>(this);
        }

        public void AddConversation(string key, ConversationData conversationData)
        {
            if (!string.IsNullOrEmpty(key) && !Conversations.ContainsKey(key))
            {
                Conversations.Add(key, conversationData);
            }
        }

        public ConversationData GetConversation(string key)
        {
            if (Conversations.ContainsKey(key))
            {
                return Conversations[key];
            }

            return null;
        }

        public void LoadConverstions(ContentManager Content)
        {
            try
            {
                string folder = string.Format("{0}/Conversations/", Content.RootDirectory);

                foreach (var f in Directory.GetDirectories(folder))
                {
                    string root = f.Replace(@"Content/", "");
                    string animation = root.Replace(string.Format(@"Conversations/"), "");

                    foreach (var r in Directory.GetFiles(f))
                    {
                        string path = Path.GetFileNameWithoutExtension(r);

                        string build = string.Format(@"{0}/Conversations/{1}", root, path);
                        ConversationData data = Content.Load<ConversationData>(build);
                        Conversations.Add(path, data);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void ReadConversations()
        {
            Conversations.Clear();

            try
            {
                foreach (var conversation in Directory.GetFiles(string.Format("{0}/Conversations/", _game.Content.RootDirectory)))
                {
                    if (Path.GetExtension(conversation).ToLower() == "xml")
                    {

                        ConversationData data = XnaSerializer.Deserialize<ConversationData>(conversation);
                        Conversations.Add(Path.GetFileNameWithoutExtension(conversation), data);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void WriteConversations()
        {
            try
            {
                foreach (string conversation in Conversations.Keys)
                {
                    XnaSerializer.Serialize<ConversationData>(conversation, Conversations[conversation]);
                }
            }
            catch (Exception ex)
            {

            }

        }

        public void LoadConversations(Game game)
        {
            Conversation conversation = new();

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

            options = new()
            {
                new("Goodbye.", "Gooodbye", new() { Action = ActionType.End, Parameter = "" })
            };

            scene = new(game, "Oh thank the heavens for you!", options);
            conversation.AddScene("Help", scene);

            conversation.FirstScene = "Hello";
            conversation.StartConversation();

            AddConversation("Rio", conversation);
        }
    }
}
