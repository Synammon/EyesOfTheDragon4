using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rpglibrary.TileEngine;
using RpgLibrary.Characters;
using RpgLibrary.TileEngine;
using SharedProject;
using SharedProject.Characters;
using SharedProject.Controls;
using SharedProject.GamesScreens;
using SharedProject.Sprites;
using SharedProject.StateManagement;
using SharpFont.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;

namespace SharedProject.GameScreens
{
    public class GamePlayState : GameState, IGamePlayState
    {
        readonly Camera camera;
        TileMap map;
        readonly Engine engine;
        RenderTarget2D renderTarget;
        AnimatedSprite sprite;
        private Button upButton, downButton, leftButton, rightButton;
        private bool inMotion = false;
        private Rectangle collision = new();
        private float speed;
        private Vector2 motion;

        public Player Player { get; private set; }

        private IConversationManager _conversationManager;
        private IConversationState _conversationState;

        public GamePlayState(Game game) : base(game)
        {
            Game.Services.AddService<IGamePlayState>(this);
            camera = new(Settings.BaseRectangle);
            engine = new(32, 32, Settings.BaseRectangle);
        }

        public GameState Tag => this;

        public override void Initialize()
        {
            speed = 96;

            base.Initialize();
        }

    protected override void LoadContent()
    {
        base.LoadContent();

        Player = (Player)Game.Services.GetService<IPlayer>();

        _conversationState = Game.Services.GetService<IConversationState>();
        _conversationManager = Game.Services.GetService<IConversationManager>();

        renderTarget = new(GraphicsDevice, Settings.BaseWidth, Settings.BaseHeight);

        Texture2D texture = Game.Content.Load<Texture2D>(@"Tiles/tileset1");

        List<Tileset> tilesets = new()
        {
            new(texture, 8, 8, 32, 32),
        };

        TileLayer layer = new(100, 100);

        map = new("test", tilesets[0], layer);

        Dictionary<string, Animation> animations = new();

        Animation animation = new(3, 32, 32, 0, 0) { CurrentFrame = 0, FramesPerSecond = 8 };
        animations.Add("walkdown", animation);

        animation = new(3, 32, 32, 0, 32) { CurrentFrame = 0, FramesPerSecond = 8 };
        animations.Add("walkleft", animation);

        animation = new(3, 32, 32, 0, 64) { CurrentFrame = 0, FramesPerSecond = 8 };
        animations.Add("walkright", animation);

        animation = new(3, 32, 32, 0, 96) { CurrentFrame = 0, FramesPerSecond = 8 };
        animations.Add("walkup", animation);

        texture = Game.Content.Load<Texture2D>(@"PlayerSprites/femalepriest");

        sprite = new(texture, animations)
        {
            CurrentAnimation = "walkdown",
            IsActive = true,
            IsAnimating = true,
        };

        texture = Game.Content.Load<Texture2D>(@"PlayerSprites/femalefighter");

        AnimatedSprite rio = new(texture, animations) 
        { 
            CurrentAnimation = "walkdown",
            IsAnimating = true,
        };

        CharacterLayer chars = new();

        chars.Characters.Add(
            new Villager(rio, new(10, 10)) 
            { 
                Position = new(320, 320),
                Tile = new(10, 10),
                Visible= true,
                Enabled=true,
                Conversation="Rio"
            });

        map.AddLayer(chars);

        rightButton = new(Game.Content.Load<Texture2D>("GUI/g21245"), ButtonRole.Menu)
        {
            Position = new(80, Settings.BaseHeight - 80),
            Size = new(32, 32),
            Text = "",
            Color = Color.White,
        };

        rightButton.Down += RightButton_Down;
        ControlManager.Add(rightButton);

        upButton = new(Game.Content.Load<Texture2D>("GUI/g21263"), ButtonRole.Menu)
        {
            Position = new(48, Settings.BaseHeight - 48 - 64),
            Size = new(32, 32),
            Text = "",
            Color = Color.White,
        };

        upButton.Down += UpButton_Down;
        ControlManager.Add(upButton);

        downButton = new(Game.Content.Load<Texture2D>("GUI/g21272"), ButtonRole.Menu)
        {
            Position = new(48, Settings.BaseHeight - 48),
            Size = new(32, 32),
            Text = "",
            Color = Color.White,
        };

        downButton.Down += DownButton_Down;
        ControlManager.Add(downButton);

        leftButton = new(Game.Content.Load<Texture2D>("GUI/g22987"), ButtonRole.Menu)
        {
            Position = new(16, Settings.BaseHeight - 80),
            Size = new(32, 32),
            Text = "",
            Color = Color.White,
        };

        leftButton.Down += LeftButton_Down;

        ControlManager.Add(leftButton);
    }

        private void LeftButton_Down(object sender, EventArgs e)
        {
            if (!inMotion)
            {
                MoveLeft();
            }
        }

        private void MoveLeft()
        {
            motion = new(-1, 0);
            inMotion = true;
            sprite.CurrentAnimation = "walkleft";
            collision = new(
                (sprite.Tile.X - 2) * Engine.TileWidth,
                sprite.Tile.Y * Engine.TileHeight,
                Engine.TileWidth,
                Engine.TileHeight);
        }

        private void RightButton_Down(object sender, EventArgs e)
        {
            if (!inMotion)
            {
                MoveRight();
            }
        }

        private void MoveRight()
        {
            motion = new(1, 0);
            inMotion = true;
            sprite.CurrentAnimation = "walkright";
            collision = new(
                (sprite.Tile.X + 2) * Engine.TileWidth,
                sprite.Tile.Y * Engine.TileHeight,
                Engine.TileWidth,
                Engine.TileHeight);
        }

        private void DownButton_Down(object sender, EventArgs e)
        {
            if (!inMotion)
            {
                MoveDown();
            }
        }

        private void MoveDown()
        {
            motion = new(0, 1);
            Point newTile = sprite.Tile + new Point(0, 2);
            inMotion = true;
            sprite.CurrentAnimation = "walkdown";
            collision = new(
                newTile.X * Engine.TileWidth,
                newTile.Y * Engine.TileHeight,
                Engine.TileWidth,
                Engine.TileHeight);
        }

        private void UpButton_Down(object sender, EventArgs e)
        {
            if (!inMotion)
            {
                MoveUp();
            }
        }

        private void MoveUp()
        {
            motion = new(0, -1);
            inMotion = true;
            sprite.CurrentAnimation = "walkup";
            collision = new(
                sprite.Tile.X * Engine.TileWidth,
                (sprite.Tile.Y - 2) * Engine.TileHeight,
                Engine.TileWidth,
                Engine.TileHeight);
        }

        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime);

            sprite.Update(gameTime);
            map.Update(gameTime);

            if (Xin.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) && !inMotion)
            {
                MoveLeft();
            }
            else if (Xin.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) && !inMotion)
            {
                MoveRight();
            }

            if (Xin.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) && !inMotion)
            {
                MoveUp();
            }
            else if (Xin.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) && !inMotion)
            {
                MoveDown();
            }

            if (Xin.WasKeyReleased(Microsoft.Xna.Framework.Input.Keys.F) && !inMotion)
            {
                HandleConversation();
            }

            if (motion != Vector2.Zero)
            {
                motion.Normalize();
            }
            else
            {
                inMotion = false;
                return;
            }

            if (!sprite.LockToMap(new(99 * Engine.TileWidth, 99 * Engine.TileHeight), ref motion))
            {
                inMotion = false;
                return;
            }

            Vector2 newPosition = sprite.Position + motion * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Rectangle nextPotition = new Rectangle(
                (int)newPosition.X,
                (int)newPosition.Y,
                Engine.TileWidth,
                Engine.TileHeight);

            if (nextPotition.Intersects(collision))
            {
                inMotion = false;
                motion = Vector2.Zero;
                sprite.Position = new((int)sprite.Position.X, (int)sprite.Position.Y);
                return;
            }

            if (map.PlayerCollides(nextPotition))
            {
                inMotion = false;
                motion = Vector2.Zero;
                return;
            }

            sprite.Position = newPosition;
            sprite.Tile = Engine.VectorToCell(newPosition);

            camera.LockToSprite(sprite, map);

            base.Update(gameTime);
        }

        private void HandleConversation()
        {
            var layer = map.Layers.FirstOrDefault(x => x is CharacterLayer);

            if (layer is CharacterLayer characterLayer)
            {
                foreach (ICharacter c in characterLayer.Characters)
                {
                    if (c.Tile.X == sprite.Tile.X && Math.Abs(sprite.Tile.Y - c.Tile.Y) == 1 ||
                        (c.Tile.Y == sprite.Tile.Y && Math.Abs(sprite.Tile.X - c.Tile.X) == 1))
                    {
                        if (c is Villager villager)
                        {
                            _conversationState.SetConversation(Player, villager.Conversation);
                            StateManager.PushState((ConversationState)_conversationState);
                            //Conversation conversation = (Conversation)_conversationManager.GetConversation(villager.Conversation);
                            //if (conversation != null)
                            //{
                            //}
                        }
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin(SpriteSortMode.Immediate,
                                BlendState.AlphaBlend,
                                SamplerState.PointClamp,
                                null,
                                null,
                                null,
                                camera.Transformation);

            map.Draw(gameTime, SpriteBatch, camera);
            sprite.Draw(SpriteBatch);

            SpriteBatch.End();

            SpriteBatch.Begin();

            base.Draw(gameTime);

            SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            SpriteBatch.Begin(SpriteSortMode.Immediate,
                                BlendState.AlphaBlend,
                                SamplerState.PointClamp);

            SpriteBatch.Draw(renderTarget, Settings.TargetRectangle, Color.White);

            SpriteBatch.End();
        }
    }
}
