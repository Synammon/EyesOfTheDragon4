using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject.Controls
{
    public enum ButtonRole { Accept, Cancel, Menu }

    public class Button : Control
    {
        #region

        public event EventHandler Click;
        public event EventHandler Down;

        #endregion
        #region Field Region

        private readonly Texture2D _background;
        float _frames;

        public ButtonRole Role { get; set; }
        public int Width { get { return _background.Width; } }
        public int Height { get { return _background.Height; } }

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region  

        public Button(Texture2D background, ButtonRole role)
        {
            Role = role;
            _background = background;
            Size = new(background.Width, background.Height);
            Text = "";
        }

        #endregion

        #region Method Region

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destination = new(
            (int)Position.X,
            (int)Position.Y,
            (int)Size.X,
            (int)Size.Y);

            spriteBatch.Draw(_background, destination, Color.White);

            spriteFont = ControlManager.SpriteFont;

            Vector2 size = spriteFont.MeasureString(Text);
            Vector2 offset = new((Size.X - size.X) / 2, ((Size.Y - size.Y) / 2));

            spriteBatch.DrawString(spriteFont, Text, Helper.NearestInt((Position + offset)), Color);
        }

        public override void HandleInput()
        {
            MouseState mouse = Mouse.GetState();
            Point position = new(mouse.X, mouse.Y);
            Rectangle destination = new(
                (int)(Position.X),
                (int)(Position.Y),
                (int)Size.X,
                (int)Size.Y);

            if ((Role == ButtonRole.Accept && Xin.WasKeyReleased(Keys.Enter)) ||
                (Role == ButtonRole.Accept && Xin.WasKeyReleased(Keys.Space)))
            {
                OnClick();
                return;
            }

            if (Role == ButtonRole.Cancel && Xin.WasKeyReleased(Keys.Escape))
            {
                OnClick();
                return;
            }

            if (Xin.WasMouseReleased(MouseButtons.Left) && _frames >= 5)
            {
                Rectangle r = destination.Scale(Settings.Scale);

                if (r.Contains(position))
                {
                    OnClick();
                    return;
                }
            }

            if (Xin.TouchReleased() && _frames >= 5)
            {
                Rectangle rectangle = destination.Scale(Settings.Scale);

                if (rectangle.Contains(Xin.TouchReleasedAt))
                {
                    OnClick();
                    return;
                }
            }

            if (Xin.IsMouseDown(MouseButtons.Left))
            {
                Rectangle rectangle = destination.Scale(Settings.Scale);

                if (rectangle.Contains(Xin.MouseAsPoint))
                {
                    OnDown();
                    return;
                }
            }

            if (Xin.TouchLocation != new Vector2(-1, -1))
            {
                Rectangle rectangle = destination.Scale(Settings.Scale);

                if (rectangle.Contains(Xin.TouchLocation))
                {
                    OnDown();
                    return;
                }
            }
        }

        private void OnDown()
        {
            Down?.Invoke(this, null);
        }

        private void OnClick()
        {
            Click?.Invoke(this, null);
        }

        public override void Update(GameTime gameTime)
        {
            _frames++;
            HandleInput();
        }

        public void Show()
        {
            _frames = 0;
        }

        #endregion
    }
}
