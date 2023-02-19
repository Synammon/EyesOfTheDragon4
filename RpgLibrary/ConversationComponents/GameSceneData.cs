using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace RpgLibrary.ConversationComponents
{
    public partial class GameSceneData
    {
        #region Field Region

        protected string text;
        protected List<SceneOption> options;

        #endregion

        #region Property Region


        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public List<SceneOption> Options
        {
            get { return options; }
            set { options = value; }
        }

        #endregion

        #region Constructor Region
        #endregion

        #region Method Region
        #endregion
    }
}
