using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RpgLibrary.Sprites;

namespace RpgLibrary.TileEngine
{
    public class Camera
    {
        #region Field Region

        private Vector2 _position;
        private Rectangle _viewportRectangle;

        public Vector3 Translation { get { return new(-_position, 0); } }
        public Matrix Transformation { get { return Matrix.CreateTranslation(Translation); } }

        #endregion

        #region Property Region

        public Point PositionAsPoint
        {
            get { return new Point((int)_position.X, (int)_position.Y); }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Rectangle ViewportRectangle
        {
            get
            {
                return new Rectangle(
                _viewportRectangle.X,
                _viewportRectangle.Y,
                _viewportRectangle.Width,
                _viewportRectangle.Height);
            }
        }

        #endregion

        #region Constructor Region

        public Camera(Rectangle viewportRect)
        {
            _viewportRectangle = viewportRect;
        }

        public Camera(Rectangle viewportRect, Vector2 position)
        {
            _viewportRectangle = viewportRect;
            Position = position;
        }

        #endregion

        #region Method Region

        public void SnapToPosition(Vector2 newPosition, TileMap map)
        {
            _position.X = newPosition.X - _viewportRectangle.Width / 2;
            _position.Y = newPosition.Y - _viewportRectangle.Height / 2;
            LockCamera(map);
        }

        public void LockToSprite(ISprite sprite, TileMap map)
        {
            _position.X = (sprite.Position.X + sprite.Width / 2)
                - (Engine.ViewportRectangle.Width / 2);

            _position.Y = (sprite.Position.Y + sprite.Height / 2)
                - (Engine.ViewportRectangle.Height / 2);

            LockCamera(map);
        }

        public void LockCamera(TileMap map)
        {
            if (map != null)
            {
                _position.X = MathHelper.Clamp(_position.X,
                    0,
                    map.WidthInPixels - _viewportRectangle.Width);
                _position.Y = MathHelper.Clamp(_position.Y,
                    0,
                    map.HeightInPixels - _viewportRectangle.Height);
            }
        }

        #endregion
    }
}
