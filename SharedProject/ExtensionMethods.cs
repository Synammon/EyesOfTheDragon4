using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject
{
    public static class ExtensionMethods
    {
        public static void Fill(this Texture2D texture2D, Color color) 
        {
            Color[] data = new Color[texture2D.Width * texture2D.Height];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = color;
            }

            texture2D.SetData(data);
        }

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Point destination, Rectangle? source, Color color)
        {
            spriteBatch.Draw(texture, new Vector2(destination.X, destination.Y), source, color);
        }

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Point destination, Color color)
        {
            spriteBatch.Draw(texture, new Vector2(destination.X, destination.Y), color);
        }

        public static void DrawString(this SpriteBatch spriteBatch, SpriteFont font, string text, Point destination, Color tint)
        {
            spriteBatch.DrawString(font, text, new Vector2(destination.X, destination.Y), tint);
        }

        public static Rectangle Grow(this Rectangle r, int size)
        {
            return new(r.X - size,
                       r.Y - size,
                       r.Width + size * 2,
                       r.Height + size * 2);
        }

        public static Rectangle Scale(this Rectangle rect, Vector2 scale)
        {
            return new Rectangle(
                (int)(rect.X * scale.X),
                (int)(rect.Y * scale.Y),
                (int)(rect.Width * scale.X),
                (int)(rect.Height * scale.Y));
        }
       
    }
}
