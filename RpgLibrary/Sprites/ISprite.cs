using Microsoft.Xna.Framework;
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
    }
}
