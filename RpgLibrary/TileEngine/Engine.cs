using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgLibrary.TileEngine
{
    public class Engine
    {
        #region Field Region

        private static int _tileWidth;
        private static int _tileHeight;
        private static Rectangle _viewPortRectangle;

        #endregion

        #region Property Region

        public static int TileWidth
        {
            get { return _tileWidth; }
        }

        public static int TileHeight
        {
            get { return _tileHeight; }
        }

        public static Rectangle ViewportRectangle
        {
            get { return _viewPortRectangle; }
            set { _viewPortRectangle = value; }
        }

        #endregion

        #region Constructors

        public Engine(int tileWidth, int tileHeight, Rectangle viewportRectangle)
        {
            Engine._tileWidth = tileWidth;
            Engine._tileHeight = tileHeight;
            Engine._viewPortRectangle = viewportRectangle;
        }

        #endregion

        #region Methods

        public static Point VectorToCell(Vector2 position)
        {
            return new Point((int)position.X / _tileWidth, (int)position.Y / _tileHeight);
        }

        public static Point PointToWorld(Point point)
        {
            return new Point(point.X * _tileWidth, point.Y * _tileHeight);
        }
        #endregion
    }
}
