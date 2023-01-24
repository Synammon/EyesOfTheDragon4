﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject
{
    public class Settings
    {
        public const int BaseWidth = 1280;
        public const int BaseHeight = 720;

        public static Rectangle BaseRectangle { get { return new(0, 0, BaseWidth, BaseHeight); } }
        public static int TargetWidth { get; set; } = BaseWidth;
        public static int TargetHeight { get; set; } = BaseHeight;
    }
}
