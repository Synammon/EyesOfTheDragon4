using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Rpglibrary.TileEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgLibrary.TileEngine
{
    public class TileMap
    {
        #region Field Region

        private readonly string _name;
        private readonly List<Tileset> _tilesets;
        private readonly List<ILayer> _mapLayers;

        private int _mapWidth;
        private int _mapHeight;

        #endregion

        #region Property Region

        public string Name
        {
            get { return _name; }
        }

        public int WidthInPixels
        {
            get { return _mapWidth * Engine.TileWidth; }
        }

        public int HeightInPixels
        {
            get { return _mapHeight * Engine.TileHeight; }
        }

        public List<ILayer> Layers { get { return _mapLayers; } }

        #endregion

        #region Constructor Region

        public TileMap(string name, List<Tileset> tilesets, TileLayer baseLayer, TileLayer buildingLayer, TileLayer splatterLayer)
        {
            this._name = name;
            this._tilesets = tilesets;
            this._mapLayers = new List<ILayer>();

            _mapLayers.Add(baseLayer);

            AddLayer(buildingLayer);
            AddLayer(splatterLayer);

            _mapWidth = baseLayer.Width;
            _mapHeight = baseLayer.Height;
        }

        public TileMap(string name, Tileset tileset, TileLayer baseLayer)
        {
            this._name = name;
            _tilesets = new List<Tileset>
            {
                tileset
            };

            _mapLayers = new List<ILayer>
            {
                baseLayer
            };

            _mapWidth = baseLayer.Width;
            _mapHeight = baseLayer.Height;
        }

        #endregion

        #region Method Region

        public void AddLayer(ILayer layer)
        {
            if (layer is TileLayer layer1)
            {
                if (!(layer1.Width == _mapWidth && layer1.Height == _mapHeight))
                    throw new Exception("Map layer size exception");
            }

            _mapLayers.Add(layer);
        }

        public void AddTileset(Tileset tileset)
        {
            _tilesets.Add(tileset);
        }

        public void Update(GameTime gameTime)
        {
            foreach (ILayer layer in _mapLayers)
            {
                layer.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Camera camera)
        {
            foreach (ILayer layer in _mapLayers)
            {
                if (layer is TileLayer layer1)
                {
                    layer1.Draw(spriteBatch, camera, _tilesets);
                }
                else if (layer is CharacterLayer characters)
                {
                    characters.Draw(spriteBatch, camera, _tilesets);
                }
            }
        }

        #endregion

        public void Resize(List<ILayer> layers)
        {
            _mapWidth = ((TileLayer)layers[0]).Width;
            _mapHeight = ((TileLayer)layers[0]).Height;

            _mapLayers.Clear();

            foreach (ILayer layer in layers)
                _mapLayers.Add(layer);
        }

        public bool PlayerCollides(Rectangle nextPotition)
        {
            CharacterLayer layer = _mapLayers.Where(x => x is CharacterLayer).FirstOrDefault() as CharacterLayer;

            if (layer != null)
            {
                foreach (var character in layer.Characters)
                {
                    Rectangle rectangle = new(
                        new(character.Tile.X * Engine.TileWidth, character.Tile.Y * Engine.TileHeight), 
                        new(Engine.TileWidth, Engine.TileHeight));
                    if (rectangle.Intersects(nextPotition))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
