﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject
{
    public static class Helper
    {
        private static readonly Random _random = new();

        public static Vector2 NearestInt(Vector2 vector2)
        {
            return new((int)vector2.X, (int)vector2.Y);
        }

        public static Point V2P(Vector2 vector2)
        {
            return new((int)vector2.X, (int)vector2.Y);
        }

        public static Random Random 
        { 
            get { return _random; } 
        }
    }
}
