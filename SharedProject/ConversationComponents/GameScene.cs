using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using SharedProject;
using SharedProject.Controls;

namespace RpgLibrary.ConversationComponents
{
    public class ButtonGroup
    {
        public Button Button { get; set; }
        public string Text { get; set; }
        public SceneAction Action { get; set; }
    }

    public class GameScene : GameSceneData
    {
        public event EventHandler<SelectedIndexEventArgs> ItemSelected;

        #region Field Region

        protected Game game;
        protected Texture2D border;
        protected Texture2D texture;
        protected SpriteFont font;
        protected Texture2D button;
        protected int selectedIndex;
        protected Color highLight;
        protected Color normal;
        protected Vector2 textPosition;
        protected static Texture2D selected;
        protected Vector2 menuPosition = new(50, 475);
        protected bool isOver;
        protected List<ButtonGroup> buttons = new();

        #endregion

        #region Property Region

        public bool IsOver { get; private set; }

        [ContentSerializerIgnore]
        protected Vector2 Size { get; set; }

        public static Texture2D Selected
        {
            get { return selected; }
        }

        [ContentSerializerIgnore]
        public SceneAction OptionAction
        {
            get { return options[selectedIndex].OptionAction; }
        }

        public string OptionScene
        {
            get { return options[selectedIndex].OptionScene; }
        }

        public string OptionText
        {
            get { return options[selectedIndex].OptionText; }
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
        }

        [ContentSerializerIgnore]
        public Color NormalColor
        {
            get { return normal; }
            set { normal = value; }
        }

        [ContentSerializerIgnore]
        public Color HighLightColor
        {
            get { return highLight; }
            set { highLight = value; }
        }

        public Vector2 MenuPosition
        {
            get { return menuPosition; }
        }

        #endregion

        #region Constructor Region

        private GameScene()
        {
        }

        public GameScene(Game game, string text, List<SceneOption> options)
        {
            this.game = game;
            this.text = text;

            LoadContent();

            Size = MeasureText(out this.text, text);
            Size += new Vector2(0, (options.Count + 5) * font.LineSpacing);

            this.options = new List<SceneOption>();

            int index = 0;

            foreach (var option in options)
            {
                this.options.Add(option);
                ButtonGroup buttonGroup = new()
                {
                    Button = new(button, ButtonRole.Menu)
                    {
                        Size = new(font.LineSpacing, font.LineSpacing),
                        Text = "",
                        Index = index++,
                    },
                    Text = option.OptionText,
                };

                buttonGroup.Button.Click += Button_Click;
                buttons.Add(buttonGroup);
            }

            highLight = Color.Red;
            normal = Color.Black;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn.Index.HasValue)
            {
                SelectedIndexEventArgs item = new() { Index = btn.Index.Value };
                ItemSelected?.Invoke(sender, item);
            }
        }

        #endregion

        #region Method Region

        public Vector2 MeasureText(out string parsedText, string text)
        {
            Vector2 measured = Vector2.Zero;
            StringBuilder sb = new();
            string textOut = string.Empty;

            float currentLength = 0f;

            if (font == null)
            {
                parsedText = string.Empty;
                return measured;
            }

            measured.Y = font.LineSpacing;

            string[] parts = text.Split(' ');

            foreach (string s in parts)
            {
                Vector2 size = font.MeasureString(s);

                if (currentLength + size.X < Settings.BaseWidth - 100f)
                {
                    sb.Append(s);
                    sb.Append(' ');

                    currentLength += size.X + font.MeasureString(" ").X;
                }
                else
                {
                    measured.Y += font.LineSpacing;
                    sb.Append('\n');
                    sb.Append(s);
                    sb.Append(' ');

                    currentLength = size.X + font.MeasureString(" ").X;
                }
            }

            parsedText = sb.ToString();

            measured.X = Settings.BaseWidth;
            return measured;
        }

        protected void LoadContent()
        {
            border = new(game.GraphicsDevice, Settings.BaseWidth, 100);
            texture = new(game.GraphicsDevice, Settings.BaseWidth, 100);

            border.Fill(Color.Blue);
            texture.Fill(Color.White);

            font = game.Content.Load<SpriteFont>(@"Fonts/scenefont");
            button = game.Content.Load<Texture2D>(@"GUI/g21245");
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (ButtonGroup group in buttons)
            {
                group.Button.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D portrait = null)
        {
            Rectangle portraitRect = new(25, 25, 425, 425);
            string parsedText = string.Empty;

            if (texture == null) LoadContent();

            Size = MeasureText(out parsedText, text);
            Size += new Vector2(0, buttons.Count * (font.LineSpacing + 20) + 50);

            textPosition = new(25, Settings.BaseHeight - Size.Y);

            Rectangle dest = new(
                0,
                (int)textPosition.Y,
                Settings.BaseWidth,
                (int)Size.Y);

            spriteBatch.Draw(border, dest, Color.White);
            spriteBatch.Draw(texture, dest.Grow(-2), Color.White);

            if (portrait != null)
                spriteBatch.Draw(portrait, portraitRect, Color.White);

            spriteBatch.DrawString(font,
                parsedText,
                textPosition,
                Color.Black);

            Vector2 location = new(25, Settings.BaseHeight - buttons[0].Button.Size.Y - 20);

            for (int i = buttons.Count - 1; i >= 0; i--)
            {
                buttons[i].Button.Position = location;

                buttons[i].Button.Draw(spriteBatch);

                spriteBatch.DrawString(font,
                    options[i].OptionText,
                    location + new Vector2(40, 0),
                    Color.Black);

                location.Y -= buttons[0].Button.Size.Y + 20;
            }
        }
        #endregion
    }
}
