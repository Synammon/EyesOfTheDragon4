using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace RpgLibrary.ConversationComponents
{
    public class ConversationData
    {
        #region Field Region

        protected string name;
        protected string firstScene;
        protected Dictionary<string, GameSceneData> scenes;

        #endregion

        #region Property Region

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string FirstScene
        {
            get { return firstScene; }
            set { firstScene = value; }
        }

        public Dictionary<string, GameSceneData> GameScenes
        {
            get { return scenes; }
            set { scenes = value; }
        }

        #endregion

        #region Constructor Region

        protected ConversationData()
        {
        }

        public ConversationData(string name, string firstScene)
        {
            this.scenes = new();
            this.name = name;
            this.firstScene = firstScene;
        }

        #endregion

        #region Method Region
        #endregion
    }
}
