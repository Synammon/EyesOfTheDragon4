using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgLibrary.TileEngine;

namespace Rpglibrary.TileEngine
{
    public class TileLayer : ILayer
    {
        #region Field Region

        private readonly Tile[,] _layer;

        #endregion

        #region Property Region

        public int Width
        {
            get { return _layer.GetLength(1); }
        }

        public int Height
        {
            get { return _layer.GetLength(0); }
        }

        #endregion

        #region Constructor Region

        public TileLayer(Tile[,] map)
        {
            this._layer = (Tile[,])map.Clone();
        }

        public TileLayer(int width, int height)
        {
            _layer = new Tile[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    _layer[y, x] = new Tile(0, 0);
                }
            }
        }

        #endregion

        #region Method Region

        public Tile GetTile(int x, int y)
        {
            return _layer[y, x];
        }

        public void SetTile(int x, int y, Tile tile)
        {
            _layer[y, x] = tile;
        }

        public void SetTile(int x, int y, int tileIndex, int tileset, bool visible = true, int rotation = 0)
        {
            _layer[y, x] = new Tile(tileIndex, tileset, visible, rotation);
        }

        public void Update(GameTime gameTime)
        {
        }

        Rectangle destination = new(0, 0, Engine.TileWidth, Engine.TileHeight);
        Tile tile;

        Point min;
        Point max;
        Vector2 origin;

        public void Draw(SpriteBatch spriteBatch, Camera camera, List<Tileset> tilesets)
        {
            Point cameraPoint = Engine.VectorToCell(camera.Position);
            Point viewPoint = Engine.VectorToCell(
                new Vector2(
                    (camera.Position.X + camera.ViewportRectangle.Width),
                    (camera.Position.Y + camera.ViewportRectangle.Height)));

            min.X = Math.Max(0, cameraPoint.X - 1);
            min.Y = Math.Max(0, cameraPoint.Y - 1);
            max.X = Math.Min(viewPoint.X + 1, Width);
            max.Y = Math.Min(viewPoint.Y + 1, Height);

            for (int y = min.Y; y < max.Y; y++)
            {
                destination.Y = y * Engine.TileHeight;

                for (int x = min.X; x < max.X; x++)
                {
                    tile = GetTile(x, y);

                    if (tile.TileIndex == -1 || tile.Tileset == -1 || tile.TileIndex >= tilesets[tile.Tileset].SourceRectangles.Length)
                        continue;

                    destination.X = x * Engine.TileWidth;

                    if (tile.TileRotation == 0)
                    {
                        spriteBatch.Draw(
                            tilesets[tile.Tileset].Texture,
                            destination,
                            tilesets[tile.Tileset].SourceRectangles[tile.TileIndex],
                            Color.White);
                    }
                    else
                    {
                        origin.X = (float)tilesets[tile.Tileset].SourceRectangles[tile.TileIndex].Width / 2f;
                        origin.Y = (float)tilesets[tile.Tileset].SourceRectangles[tile.TileIndex].Height / 2f;

                        Rectangle dest2 = new(destination.X, destination.Y, destination.Width, destination.Height);
                        dest2.X += Engine.TileWidth / 2;
                        dest2.Y += Engine.TileHeight / 2;

                        spriteBatch.Draw(
                            tilesets[tile.Tileset].Texture,
                            dest2,
                            tilesets[tile.Tileset].SourceRectangles[tile.TileIndex],
                            Color.White,
                            MathHelper.ToRadians(tile.TileRotation),
                            origin,
                            SpriteEffects.None,
                            1f);
                    }
                }
            }
        }

        #endregion
    }
}
