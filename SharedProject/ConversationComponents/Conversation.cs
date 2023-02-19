using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace RpgLibrary.ConversationComponents
{
    public class Conversation : ConversationData
    {
        #region Field Region

        private string currentScene;
        private readonly Dictionary<string, GameScene> gameScenes = new();

        #endregion

        #region Property Region

        public Dictionary<string, GameScene> Scenes
        {
            get { return gameScenes; }
        }

        public GameScene CurrentScene
        {
            get { return gameScenes[currentScene]; }
        }

        #endregion

        #region Constructor Region

        public Conversation()
            : base()
        {
            GameScenes = new();
        }

        #endregion

        #region Method Region

        public void Update(GameTime gameTime)
        {
            if (Scenes.ContainsKey(currentScene))
            {
                Scenes[currentScene].Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Scenes.ContainsKey(currentScene))
            {
                Scenes[currentScene].Draw(gameTime, spriteBatch);
            }
        }

        public void AddScene(string sceneName, GameSceneData scene)
        {
            if (!GameScenes.ContainsKey(sceneName))
            {
                GameScenes.Add(sceneName, scene);
                Scenes.Add(sceneName, (GameScene)scene);
            }
        }

        public GameSceneData GetScene(string sceneName)
        {
            if (GameScenes.ContainsKey(sceneName))
                return GameScenes[sceneName];

            return null;
        }

        public void StartConversation()
        {
            currentScene = FirstScene;
        }

        public void ChangeScene(string sceneName)
        {
            currentScene = sceneName;
        }

        #endregion
    }
}
