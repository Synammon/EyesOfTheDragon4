using Assimp.Configs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RpgLibrary.Characters;
using RpgLibrary.TileEngine;
using SharedProject.Sprites;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace SharedProject.Mobs
{
    public class Mob : ICharacter
    {
        private Mob() { }
        public AnimatedSprite AnimatedSprite { get; set; }

        private string _name;
        private string _description;

        public string Name { get => _name; private set => _name = value; }
        public string Description { get => _description; private set => _description = value; }
        public int Strength { get; set; }
        public int Perception { get; set; }
        public int Endurance { get; set; }
        public int Charisma { get; set; }
        public int Intellect { get; set; }
        public int Agility { get; set; }
        public int Luck { get; set; }

        public AttributePair Health { get; set; }
        public AttributePair Mana { get; set; }

        public int Gold { get; set; }
        public int Experience { get; set; }
        public bool Enabled { get; set; }
        public bool Visible { get; set; }
        public Vector2 Position { get; set; }
        public Point Tile { get; set; }

        public bool RollD10(string attribute)
        {
            PropertyInfo info = this.GetType().GetProperty(attribute);

            if (info != null )
            {
                if (info.GetType() == typeof(int))
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

                    if (int.TryParse(info.GetValue(this).ToString(), out int value))
                    {
                        return (roll <= value);
                    }
                }
            }

            return false;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            AnimatedSprite.Draw(spriteBatch);
        }

        public virtual void Update(GameTime gameTime)
        {
            AnimatedSprite.Update(gameTime);
        }

        public static Mob FromString(string data, ContentManager content)
        {
            Mob mob = new();
            string[] parts = data.Split(',');

            foreach (string part in parts)
            {
                string[] pair = part.Split('=');

                PropertyInfo info = mob.GetType().GetProperty(pair[0]);

                if (info != null)
                {
                    Type type = info.PropertyType;
                    if (type != typeof(AttributePair) &&
                        type != typeof(AnimatedSprite) &&
                        type != typeof(Vector2) &&
                        type != typeof(Point))
                    {
                        if (typeof(String) == type)
                        {
                            info.SetValue(mob, pair[1], null);
                        }
                        else
                        {
                            info.SetValue(mob, int.Parse(pair[1]), null);
                        }
                    }
                    else if (type == typeof(AttributePair))
                    {
                        info.SetValue(mob, new AttributePair(int.Parse(pair[1])), null);
                    }
                    else if (type == typeof(AnimatedSprite))
                    {
                        CreateAnimatedSprite(content, mob, pair[1]);
                    }
                    else if (type == typeof(Vector2))
                    {
                        string[] vector = pair[1].Split(':');

                        if (float.TryParse(vector[0], out float x) && float.TryParse(vector[1], out float y))
                        {
                            info.SetValue(mob, new Vector2(x, y));
                        }
                    }
                    else if (type == typeof(Point))
                    {
                        string[] point = pair[1].Split(':');

                        if (int.TryParse(point[0], out int x) && int.TryParse(point[1], out int y))
                        {
                            info.SetValue(mob, new Point(x, y));
                        }
                    }

                }            
            }

            mob.AnimatedSprite.Position = new(mob.Tile.X * Engine.TileWidth, mob.Tile.Y * Engine.TileHeight);
            return mob;
        }

        private static void CreateAnimatedSprite(ContentManager content, Mob mob, string v)
        {
            string[] parts = v.Split(';');
            Texture2D texture = content.Load<Texture2D>($"Mobs/{parts[0]}");
            Dictionary<string, Animation> animations = new();

            for (int i = 1;  i < parts.Length - 1; i++)
            {
                string[] animationDesc = parts[i].Split(":");

                if (int.TryParse(animationDesc[1], out int xoffset) && 
                    int.TryParse(animationDesc[2], out int yoffset) &&
                    int.TryParse(animationDesc[3], out int width) &&
                    int.TryParse(animationDesc[4], out int height) &&
                    int.TryParse(animationDesc[5], out int count))
                {
                    Animation animation = new(count, width, height, xoffset, yoffset);

                    animations.Add(animationDesc[0], animation);
                }
            }

            mob.AnimatedSprite = new(texture, animations)
            {
                CurrentAnimation = parts[^1],
                IsActive = true,
                IsAnimating = true
            };
        }
    }
}
