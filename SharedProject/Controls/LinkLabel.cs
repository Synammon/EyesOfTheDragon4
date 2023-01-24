using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SharedProject.Controls
{
    public class LinkLabel : Control
    {
        #region Fields and Properties

        Color selectedColor = Color.Red;

        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }

        #endregion

        #region Constructor Region

        public LinkLabel()
        {
            TabStop = true;
            HasFocus = false;
            Position = Vector2.Zero;
        }

        #endregion

        #region Abstract Methods

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (hasFocus)
                spriteBatch.DrawString(SpriteFont, Text, Position, selectedColor);
            else
                spriteBatch.DrawString(SpriteFont, Text, Position, Color);
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
            if (!HasFocus)
            {
                return;
            }

            if (Xin.WasKeyReleased(Keys.Enter))
            {
                base.OnSelected(null);
                return;
            }

            if (Xin.WasMouseReleased(MouseButtons.Left))
            {
                size = SpriteFont.MeasureString(Text);

                Rectangle r = new(
                (int)Position.X,
                    (int)Position.Y,
                    (int)size.X,
                    (int)size.Y);

                if (r.Contains(Xin.MouseAsPoint))
                {
                    base.OnSelected(null);
                }
            }
        }

        #endregion
    }
}
