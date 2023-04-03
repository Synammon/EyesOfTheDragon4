using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProject.Controls
{
    public enum Direction { Down, Up}

    public class DirectionEventArgs : EventArgs
    {
        public Direction Direction;
    }

    public class RightLeftSelector : Control
    {
        #region Event Region

        public event EventHandler<DirectionEventArgs> SelectionChanged;

        #endregion

        #region Field Region

        private readonly List<string> _items = new();

        private readonly Texture2D _leftTexture;
        private readonly Texture2D _rightTexture;

        private Color _selectedColor = Color.Red;
        private int _maxItemWidth;
        private int _selectedItem;
        private Rectangle _leftSide = new();
        private Rectangle _rightSide = new();
        private int _yOffset;

        #endregion

        #region Property Region

        public Color SelectedColor
        {
            get { return _selectedColor; }
            set { _selectedColor = value; }
        }

        public int SelectedIndex
        {
            get { return _selectedItem; }
            set { _selectedItem = (int)MathHelper.Clamp(value, 0f, _items.Count); }
        }

        public string SelectedItem
        {
            get { return Items[_selectedItem]; }
        }

        public List<string> Items
        {
            get { return _items; }
        }

        public int MaxItemWidth
        {
            get { return _maxItemWidth; }
            set { _maxItemWidth = value; }
        }

        #endregion

        #region Constructor Region

        public RightLeftSelector(Texture2D leftArrow, Texture2D rightArrow)
        {
            _leftTexture = leftArrow;
            _rightTexture = rightArrow;
            TabStop = true;
            Color = Color.White;
        }

        #endregion

        #region Method Region

        public void SetItems(string[] items, int maxWidth)
        {
            this._items.Clear();

            foreach (string s in items)
                this._items.Add(s);

            _maxItemWidth = maxWidth;
        }

        protected void OnSelectionChanged(Direction direction)
        {
            DirectionEventArgs e = new() { Direction = direction };
            SelectionChanged?.Invoke(this, e);
        }

        #endregion

        #region Abstract Method Region

        public override void Update(GameTime gameTime)
        {
            HandleMouseInput();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 drawTo = Position;

            SpriteFont = ControlManager.SpriteFont;

            _yOffset = (int)((_leftTexture.Height - SpriteFont.MeasureString("W").Y) / 2);
            _leftSide = new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                _leftTexture.Width,
                _leftTexture.Height);

            spriteBatch.Draw(_leftTexture, _leftSide, Color.White);

            drawTo.X += _leftTexture.Width + 5f;

            float itemWidth = SpriteFont.MeasureString(_items[_selectedItem]).X;
            float offset = (_maxItemWidth - itemWidth) / 2;

            Vector2 off = new((int)offset, (int)_yOffset);

            if (HasFocus)
                spriteBatch.DrawString(SpriteFont, _items[_selectedItem], drawTo + off, _selectedColor);
            else
                spriteBatch.DrawString(SpriteFont, _items[_selectedItem], drawTo + off, Color);

            drawTo.X += _maxItemWidth + 5f;

            _rightSide = new Rectangle((int)drawTo.X, (int)drawTo.Y, _rightTexture.Width, _rightTexture.Height);
            
            spriteBatch.Draw(_rightTexture, _rightSide, Color.White);
        }

        public override void HandleInput()
        {
            if (_items.Count == 0)
                return;

            if (Xin.WasKeyReleased(Keys.Left) && _selectedItem != 0)
            {
                _selectedItem--;
                OnSelectionChanged(Direction.Down);
            }

            if (Xin.WasKeyReleased(Keys.Right) && _selectedItem != _items.Count - 1)
            {
                _selectedItem++;
                OnSelectionChanged(Direction.Up);
            }
        }

        private void HandleMouseInput()
        {
            if (Xin.WasMouseReleased(MouseButtons.Left))
            {
                Point mouse = Xin.MouseAsPoint;

                if (_leftSide.Scale(Settings.Scale).Contains(mouse) && _selectedItem != 0)
                {
                    _selectedItem--;
                    OnSelectionChanged(Direction.Down);
                }

                if (_rightSide.Scale(Settings.Scale).Contains(mouse) && _selectedItem != _items.Count - 1)
                {
                    _selectedItem++;
                    OnSelectionChanged(Direction.Up);
                }
            }

            if (Xin.TouchReleased())
            {
                if (_leftSide.Scale(Settings.Scale).Contains(Xin.TouchLocation) && _selectedItem != 0)
                {
                    _selectedItem--;
                    OnSelectionChanged(Direction.Down);
                }

                if (_rightSide.Scale(Settings.Scale).Contains(Xin.TouchLocation) && _selectedItem != _items.Count - 1)
                {
                    _selectedItem++;
                    OnSelectionChanged(Direction.Up);
                }
            }
        }

        #endregion
    }
}
