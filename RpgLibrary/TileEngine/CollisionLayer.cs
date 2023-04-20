using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgLibrary.TileEngine
{
    public enum CollisionValue { Impassible, Water }

    public class CollisionLayer : ILayer
    {
        public Dictionary<Rectangle, CollisionValue> Collisions { get; private set; } = new();

        public void Draw(SpriteBatch spriteBatch, Camera camera, List<Tileset> tilesets)
        {
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
