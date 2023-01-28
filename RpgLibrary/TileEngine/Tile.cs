using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgLibrary.TileEngine
{
    public struct Tile
    {
        #region Field Region

        private int _tileIndex;
        private int _tileset;
        private int _rotation;
        private bool _visible;

        #endregion

        #region Property Region

        [ContentSerializer]
        public int TileIndex
        {
            get { return _tileIndex; }
            private set { _tileIndex = value; }
        }

        [ContentSerializer]
        public int Tileset
        {
            get { return _tileset; }
            private set { _tileset = value; }
        }

        [ContentSerializer(Optional = true)]
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        [ContentSerializer(Optional = true)]
        public int TileRotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        #endregion

        #region Constructor Region

        public Tile()
        {
            _tileIndex = -1;    
            _tileset = -1;
            _visible = true;
            _rotation = 0;
        }

        public Tile(int tileIndex, int tileset, bool visible = true, int rotation = 0)
        {
            _visible = visible;
            _tileIndex = tileIndex;
            _tileset = tileset;
            _rotation = rotation;
        }

        #endregion
    }
}
