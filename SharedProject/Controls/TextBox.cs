using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject.Controls
{
    public class TextBox : Control
    {
        private readonly Texture2D _background;
        private readonly Texture2D _border;
        private readonly Texture2D _caret;
        private double timer;
        private Color _tint;
        private readonly List<string> validChars = new();
        public bool ReadOnly { get; set; }

        public TextBox(Texture2D background, Texture2D caret, Texture2D brdr)
            : base()
        {
            Text = "";
            _background = background;
            _border = brdr;
            _caret = caret;
            _tint = Color.Black;

            ReadOnly = false;

            foreach (char c in "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTWXYZ0123456789 -_".ToCharArray())
            {
                validChars.Add(c.ToString());
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 dimensions = ControlManager.SpriteFont.MeasureString(Text);
            dimensions.Y = 0;
            spriteBatch.Draw(_border, 
                             new Rectangle(Helper.V2P(Position), Helper.V2P(Size)).Grow(1), 
                             Color.White);
            spriteBatch.Draw(_background,
                             new Rectangle(
                                 Helper.V2P(Position), 
                                 Helper.V2P(Size)),
                             Color.White);
            spriteBatch.DrawString(
                ControlManager.SpriteFont,
                Text,
                Helper.NearestInt(Position + Vector2.One * 5),
                Color.Black,
                0,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                1f);
            spriteBatch.Draw(
                _caret,
                Helper.NearestInt(Position + dimensions + Vector2.One * 5),
                _tint);
        }

        public override void HandleInput()
        {
            if (!HasFocus)
            {
                return;
            }

            List<Keys> keys = Xin.KeysPressed();

            foreach (Keys key in keys)
            {
                string value = Enum.GetName(typeof(Keys), key);

                if (value == "Back" && Text.Length > 0)
                {
                    Text = Text.Substring(0, Text.Length - 1);
                    return;
                }
                else if (value == "Back")
                {
                    Text = "";
                    return;
                }

                if (value.Length == 2 && value.Substring(0, 1) == "D")
                {
                    value = value.Substring(1);
                }

                if (!Xin.IsKeyDown(Keys.LeftShift) && !Xin.IsKeyDown(Keys.RightShift) && !Xin.KeyboardState.CapsLock)
                {
                    value = value.ToLower();
                }

                if (validChars.Contains(value))
                {
                    if (ControlManager.SpriteFont.MeasureString(Text + value).X < Size.X)
                        Text += value;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            timer += 3 * gameTime.ElapsedGameTime.TotalSeconds;
            double sine = Math.Sin(timer);

            _tint = Color.Black * (int)Math.Round(Math.Abs(sine));
        }
    }
}
