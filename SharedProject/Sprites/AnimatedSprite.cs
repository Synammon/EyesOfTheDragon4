using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RpgLibrary.TileEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject.Sprites
{
    public class AnimatedSprite : Sprite
    {
        #region Field Region

        readonly Dictionary<string, Animation> animations;
        string currentAnimation;
        bool isAnimating;
        readonly Texture2D texture;

        #endregion

        #region Property Region

        public bool IsActive { get; set; }

        public string CurrentAnimation
        {
            get { return currentAnimation; }
            set { currentAnimation = value; }
        }

        public bool IsAnimating
        {
            get { return isAnimating; }
            set { isAnimating = value; }
        }

        #endregion

        #region Constructor Region

        public AnimatedSprite(Texture2D sprite, Dictionary<string, Animation> animation)
        {
            texture = sprite;
            animations = new();

            foreach (string key in animation.Keys)
                animations.Add(key, (Animation)animation[key].Clone());
        }

        #endregion

        #region Method Region

        public void ResetAnimation()
        {
            animations[currentAnimation].Reset();
        }

        public override void Update(GameTime gameTime)
        {
            if (isAnimating)
                animations[currentAnimation].Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture,
                new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    Engine.TileWidth,
                    Engine.TileHeight),
                animations[currentAnimation].CurrentFrameRect,
                Color.White);
        }

        public bool LockToMap(Point mapSize, ref Vector2 motion)
        {
            Position = new(
                MathHelper.Clamp(Position.X, 0, mapSize.X - Width),
                MathHelper.Clamp(Position.Y, 0, mapSize.Y - Height));

            if (Position.X == 0 && motion.X < 0 || Position.Y == 0 && motion.Y < 0)
            {
                motion = Vector2.Zero;
                return false;
            }

            if (Position.X == mapSize.X - Width && motion.X > 0)
            {
                motion = Vector2.Zero;
                return false;
            }

            if (Position.Y == mapSize.Y - Width && motion.Y > 0)
            {
                motion = Vector2.Zero;
                return false;
            }

            return true;
        }

        #endregion
    }
}
