using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject.Controls
{
    public class ListBox : Control
    {
        #region Event Region

        public event EventHandler SelectionChanged;
        public event EventHandler Enter;
        public event EventHandler Leave;

        #endregion

        #region Field Region

        private readonly List<string> _items = new();

        private int _startItem;
        private int _lineCount;

        private readonly Texture2D _image;
        private readonly Texture2D _cursor;

        private Color _selectedColor = Color.Red;
        private int _selectedItem;
        private readonly Button _upButton, _downButton;
        private double timer;

        private bool _mouseOver;

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

        public new bool HasFocus
        {
            get { return hasFocus; }
            set
            {
                hasFocus = value;

                if (hasFocus)
                    OnEnter(null);
                else
                    OnLeave(null);
            }
        }

        public Rectangle Bounds
        {
            get { return new(Helper.V2P(Position), Helper.V2P(Size)); }
        }
        #endregion

        #region Constructor Region

        public ListBox(Texture2D background, Texture2D downButton, Texture2D upButton, Texture2D cursor)
            : base()
        {
            HasFocus = false;
            TabStop = true;

            _upButton = new(upButton, ButtonRole.Menu) { Text = "" };
            _downButton = new(downButton, ButtonRole.Menu) { Text = "" };

            _upButton.Click += UpButton_Click;
            _downButton.Click += DownButton_Click;

            this._image = background;
            this.Size = new Vector2(_image.Width, _image.Height);
            this._cursor = cursor;

            _startItem = 0;
            Color = Color.Black;
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            if (_selectedItem != _items.Count - 1 && timer > 0.1)
            {
                timer = 0;
                _selectedItem++;
                OnSelectionChanged(null);
            }
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            if (_selectedItem > 0 && timer > 0.1)
            {
                timer = 0;
                _selectedItem--;
                OnSelectionChanged(null);
            }
        }

        #endregion

        #region Abstract Method Region

        public override void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.TotalSeconds;

            _upButton.Update(gameTime);
            _downButton.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _lineCount = (int)(Size.Y / SpriteFont.LineSpacing);
            Rectangle d = new((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            spriteBatch.Draw(_image, d, Color.White);
            Point position = Xin.MouseAsPoint;
            Rectangle destination = new(0, 0, 100, (int)SpriteFont.LineSpacing);
            _mouseOver = false;

            for (int i = 0; i < _lineCount; i++)
            {
                if (_startItem + i >= _items.Count)
                {
                    break;
                }

                destination.X = (int)Position.X;
                destination.Y = (int)(Position.Y + i * SpriteFont.LineSpacing);

                if ((destination.Contains(position) && 
                    Xin.MouseState.LeftButton == ButtonState.Pressed) ||
                    (destination.Contains(Xin.TouchLocation) &&
                    Xin.TouchPressed()))
                {
                    _mouseOver = true;
                    _selectedItem = _startItem + i;
                    OnSelectionChanged(null);
                }

                float length = 0;
                int j = 0;
                string text = "";

                while (length <= Size.X - _upButton.Width && j < _items[i].Length)
                {
                    j++;
                    length = SpriteFont.MeasureString(_items[i].Substring(0, j)).X;
                    text = _items[i].Substring(0, j);
                }

                if (_startItem + i == _selectedItem)
                {
                    spriteBatch.DrawString(
                        SpriteFont,
                        text,
                        new Vector2(Position.X + 3, Position.Y + i * SpriteFont.LineSpacing + 2),
                        SelectedColor);
                }
                else
                {
                    spriteBatch.DrawString(
                        SpriteFont,
                        text,
                        new Vector2(Position.X + 3, Position.Y + i * SpriteFont.LineSpacing + 2),
                        Color);
                }
            }

            _upButton.Position = new(Position.X + Size.X - _upButton.Width, Position.Y);
            _downButton.Position = new(Position.X + Size.X - _downButton.Width, Position.Y + Size.Y - _downButton.Height);

            _upButton.Draw(spriteBatch);
            _downButton.Draw(spriteBatch);
        }

        public override void HandleInput()
        {
            if (!HasFocus)
            {
                return;
            }

            if (Xin.WasKeyReleased(Keys.Down))
            {
                if (_selectedItem < _items.Count - 1)
                {
                    _selectedItem++;

                    if (_selectedItem >= _startItem + _lineCount)
                    {
                        _startItem = _selectedItem - _lineCount + 1;
                    }

                    OnSelectionChanged(null);
                }
            }
            else if (Xin.WasKeyReleased(Keys.Up))
            {
                if (_selectedItem > 0)
                {
                    _selectedItem--;

                    if (_selectedItem < _startItem)
                    {
                        _startItem = _selectedItem;
                    }

                    OnSelectionChanged(null);
                }
            }
            if (Xin.WasMouseReleased(MouseButtons.Left) && _mouseOver)
            {
                HasFocus = true;
                OnSelectionChanged(null);
            }
            if (Xin.WasKeyReleased(Keys.Enter))
            {
                HasFocus = false;
                OnSelected(null);
            }

            if (Xin.WasKeyReleased(Keys.Escape))
            {
                HasFocus = false;
                OnLeave(null);
            }
        }

        #endregion

        #region Method Region

        protected virtual void OnSelectionChanged(EventArgs e)
        {
            SelectionChanged?.Invoke(this, e);
        }

        protected virtual void OnEnter(EventArgs e)
        {
            Enter?.Invoke(this, e);
        }

        protected virtual void OnLeave(EventArgs e)
        {
            Leave?.Invoke(this, e);
        }

        #endregion
    }
}
