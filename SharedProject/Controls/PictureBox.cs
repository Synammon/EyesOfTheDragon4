using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject.Controls
{
    public enum FillMethod { Clip, Fill, Original, Center }

    public class PictureBox : Control
    {
        #region Field Region

        private Texture2D _image;
        private Rectangle _sourceRect;
        private Rectangle _destRect;
        private FillMethod _fillMethod;
        private int _width;
        private int _height;

        #endregion

        #region Property Region

        public Texture2D Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public Rectangle SourceRectangle
        {
            get { return _sourceRect; }
            set { _sourceRect = value; }
        }

        public Rectangle DestinationRectangle
        {
            get { return _destRect; }
            set { _destRect = value; }
        }

        public FillMethod FillMethod
        {
            get { return _fillMethod; }
            set { _fillMethod = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        #endregion

        #region Constructors

        public PictureBox(Texture2D image, Rectangle destination)
        {
            Image = image;

            DestinationRectangle = destination;

            Width = destination.Width;
            Height = destination.Height;

            if (image != null)
                SourceRectangle = new Rectangle(0, 0, image.Width, image.Height);
            else
                SourceRectangle = new Rectangle(0, 0, 0, 0);

            Color = Color.White;

            _fillMethod = FillMethod.Original;

            if (SourceRectangle.Width > DestinationRectangle.Width)
            {
                _sourceRect.Width = DestinationRectangle.Width;
            }

            if (SourceRectangle.Height > DestinationRectangle.Height)
            {
                _sourceRect.Height = DestinationRectangle.Height;
            }
        }

        public PictureBox(Texture2D image, Rectangle destination, Rectangle source)
            : this(image, destination)
        {
            SourceRectangle = source;
            Color = Color.White;

            _fillMethod = FillMethod.Original;

            if (SourceRectangle.Width > DestinationRectangle.Width)
            {
                _sourceRect.Width = DestinationRectangle.Width;
            }

            if (SourceRectangle.Height > DestinationRectangle.Height)
            {
                _sourceRect.Height = DestinationRectangle.Height;
            }
        }

        #endregion

        #region Abstract Method Region

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_image != null)
            {
                switch (_fillMethod)
                {
                    case FillMethod.Original:
                        _fillMethod = FillMethod.Original;

                        if (SourceRectangle.Width > DestinationRectangle.Width)
                        {
                            _sourceRect.Width = DestinationRectangle.Width;
                        }

                        if (SourceRectangle.Height > DestinationRectangle.Height)
                        {
                            _sourceRect.Height = DestinationRectangle.Height;
                        }
                        spriteBatch.Draw(Image, DestinationRectangle, SourceRectangle, Color);
                        break;
                    case FillMethod.Clip:
                        if (DestinationRectangle.Width > SourceRectangle.Width)
                        {
                            _destRect.Width = SourceRectangle.Width;
                        }

                        if (_destRect.Height > DestinationRectangle.Height)
                        {
                            _destRect.Height = DestinationRectangle.Height;
                        }
                        spriteBatch.Draw(Image, DestinationRectangle, SourceRectangle, Color);
                        break;
                    case FillMethod.Fill:
                        _sourceRect = new(0, 0, Image.Width, Image.Height);
                        spriteBatch.Draw(Image, DestinationRectangle, null, Color);
                        break;
                    case FillMethod.Center:
                        _sourceRect.Width = Image.Width;
                        _sourceRect.Height = Image.Height;
                        _sourceRect.X = 0;
                        _sourceRect.Y = 0;

                        Rectangle dest = new(0, 0, Width, Height);

                        if (Image.Width >= Width)
                        {
                            dest.X = DestinationRectangle.X;
                        }
                        else
                        {
                            dest.X = DestinationRectangle.X + (Width - Image.Width) / 2;
                        }

                        if (Image.Height >= Height)
                        {
                            dest.Y = DestinationRectangle.Y;
                        }
                        else
                        {
                            dest.Y = DestinationRectangle.Y + (Height - Image.Height) / 2;
                        }
                        spriteBatch.Draw(Image, dest, SourceRectangle, Color);
                        break;
                }
            }
        }

        public override void HandleInput()
        {
        }

        #endregion

        #region Picture Box Methods

        public void SetPosition(Vector2 newPosition)
        {
            _destRect = new Rectangle(
                (int)newPosition.X,
                (int)newPosition.Y,
                _sourceRect.Width,
                _sourceRect.Height);
        }

        #endregion
    }
}
