using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgLibrary.Sprites
{
    public interface ISprite
    {
        Vector2 Position { get; set; }
        Vector2 Size { get; set; }
        int Width { get; }
        int Height { get; }
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}
