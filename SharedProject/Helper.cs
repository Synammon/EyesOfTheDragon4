using Microsoft.Xna.Framework;
using RpgLibrary.Characters;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SharedProject
{
    public static class Helper
    {
        private static readonly Random _random = new();

        public static bool RollDie(ICharacter character, string attribute)
        {
            PropertyInfo info = character.GetType().GetProperty(attribute);

            if (info != null)
            {
                if (info.PropertyType == typeof(Int32))
                {
                    int roll = Helper.Random.Next(1, 11);

                    if (roll == 10)
                    {
                        return false;
                    }

                    if (roll == 1)
                    {
                        return true;
                    }

                    if (int.TryParse(info.GetValue(character).ToString(), out int value))
                    {
                        return (roll <= value);
                    }
                }
            }

            return false;
        }

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
