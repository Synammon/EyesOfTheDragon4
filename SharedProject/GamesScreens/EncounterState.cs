using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgLibrary;
using RpgLibrary.Characters;
using RpgLibrary.TileEngine;
using SharedProject.Controls;
using SharedProject.Mobs;
using SharedProject.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;

namespace SharedProject.GamesScreens
{
    public enum Mode { Movement, Attack, Cast, Use, Over }

    public interface IEncounterState
    {
        void SetEncounter(Player player, Encounter encounter);
    }

    public class EncounterState : GameState, IEncounterState
    {
        private Mode _mode = Mode.Movement;
        private Player Player;
        public Point PlayerTile { get; private set; }
        public Vector2 PlayerPosition { get; private set; }
        public string PlayerAnimation { get; private set; }

        private Encounter encounter;
        private Camera Camera { get; set; }
        private RenderTarget2D renderTarget;

        private Rectangle collision;
        private bool inMotion;
        private Vector2 motion;
        private readonly float speed = 160;

        private bool _turn;
        private double _timer;
        private readonly Queue<string> _messages = new();
        private Texture2D _messageBox;

        public EncounterState(Game game) : base(game)
        {
            Game.Services.AddService(typeof(IEncounterState), this);
            Camera = new(new(Point.Zero, new(Settings.BaseWidth, Settings.BaseHeight)));
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            renderTarget = new(GraphicsDevice, Settings.BaseWidth, Settings.BaseHeight);
            _messageBox = new(GraphicsDevice, Settings.BaseWidth, 128);
            _messageBox.Fill(Color.White);
        }
        
        public void SetEncounter(Player player, Encounter encounter)
        {
            this.encounter = encounter;

            encounter.RandomMap(Game.Content);

            PlayerTile = player.Sprite.Tile;
            PlayerPosition = player.Sprite.Position;
            PlayerAnimation = player.Sprite.CurrentAnimation;

            Point newTile = new(3, 5);

            this.encounter.Allies[0].Tile = newTile;
            ((Player)this.encounter.Allies[0]).Sprite.Position = new(newTile.X * Engine.TileWidth, newTile.Y * Engine.TileHeight);
            ((Player)this.encounter.Allies[0]).Sprite.CurrentAnimation = "walkright";
            ((Player)this.encounter.Allies[0]).Sprite.IsAnimating = true;

            newTile = new(16, 5);

            ((Mob)this.encounter.Enemies[0]).AnimatedSprite.Tile = newTile;
            ((Mob)this.encounter.Enemies[0]).AnimatedSprite.Position = new(16 * Engine.TileWidth, 5 * Engine.TileHeight);
            ((Mob)this.encounter.Enemies[0]).AnimatedSprite.CurrentAnimation = "left";

            Player = player;
        }

        private void MoveLeft()
        {
            motion = new(-1, 0);
            inMotion = true;
            Player.Sprite.CurrentAnimation = "walkleft";
            collision = new(
                (Player.Sprite.Tile.X - 2) * Engine.TileWidth,
                Player.Sprite.Tile.Y * Engine.TileHeight,
                Engine.TileWidth,
                Engine.TileHeight);
            if (encounter.Map.Layers.FirstOrDefault(x => x is CollisionLayer) is CollisionLayer layer)
            {
                Dictionary<Rectangle, CollisionValue> collisions = layer.Collisions;
                if (!collisions.ContainsKey(new(Player.Sprite.Tile + new Point(-1, 0), new(Engine.TileWidth, Engine.TileHeight))))
                {
                    Player.Tile += new Point(-1, 0);
                }
            }
        }

        private void MoveRight()
        {
            motion = new(1, 0);
            if (encounter.Map.Layers.FirstOrDefault(x => x is CollisionLayer) is CollisionLayer layer)
            { 
                Dictionary<Rectangle, CollisionValue> collisions = layer.Collisions;
                if (!collisions.ContainsKey(new(Player.Sprite.Tile + new Point(1, 0), new(Engine.TileWidth, Engine.TileHeight))))
                {
                    Player.Tile += new Point(1, 0);
                }
            }
            Point newTile = Player.Sprite.Tile + new Point(1, 0);
            inMotion = true;
            Player.Sprite.CurrentAnimation = "walkright";
            collision = new(
                (Player.Sprite.Tile.X + 2) * Engine.TileWidth,
                Player.Sprite.Tile.Y * Engine.TileHeight,
                Engine.TileWidth,
                Engine.TileHeight);
        }

        private void MoveDown()
        {
            motion = new(0, 1);
            Point newTile = Player.Sprite.Tile + new Point(0, 1);
            inMotion = true;
            Player.Sprite.CurrentAnimation = "walkdown";
            collision = new(
                newTile.X * Engine.TileWidth,
                (newTile.Y +  1) * Engine.TileHeight,
                Engine.TileWidth,
                Engine.TileHeight);
            if (encounter.Map.Layers.FirstOrDefault(x => x is CollisionLayer) is CollisionLayer layer)
            {
                Dictionary<Rectangle, CollisionValue> collisions = layer.Collisions;
                if (!collisions.ContainsKey(new(Player.Sprite.Tile + new Point(1, 0), new(Engine.TileWidth, Engine.TileHeight))))
                {
                    Player.Tile += new Point(0, 1);
                }
            }
        }

        private void MoveUp()
        {
            motion = new(0, -1);
            inMotion = true;
            Player.Sprite.CurrentAnimation = "walkup";
            collision = new(
                Player.Sprite.Tile.X * Engine.TileWidth,
                (Player.Sprite.Tile.Y - 2) * Engine.TileHeight,
                Engine.TileWidth,
                Engine.TileHeight);
            if (encounter.Map.Layers.FirstOrDefault(x => x is CollisionLayer) is CollisionLayer layer)
            {
                Dictionary<Rectangle, CollisionValue> collisions = layer.Collisions;
                if (!collisions.ContainsKey(new(Player.Sprite.Tile + new Point(1, 0), new(Engine.TileWidth, Engine.TileHeight))))
                {
                    Player.Tile += new Point(0, -1);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime);

            encounter?.Update(gameTime);

            Player.Update(gameTime);
            encounter.Map.Update(gameTime);

            _timer += gameTime.ElapsedGameTime.TotalSeconds;

            if (!encounter.Alive && _timer > 3)
            {
                Player.Sprite.Tile = PlayerTile;
                Player.Sprite.Position = PlayerPosition;
                Player.Sprite.CurrentAnimation = PlayerAnimation;
                
                StateManager.PopState();
            }
            else if (!encounter.Alive)
            {
                _messages.Clear();

                if (Player.Health.Current > 0)
                {
                    _messages.Enqueue("The battle has been won!");
                }
                else
                {
                    _messages.Enqueue("Bitter defeat...");
                }
            }

            if (_turn && _timer > 0.25)
            {
                if (_mode == Mode.Attack)
                {
                    HandleAttack();
                    return;
                }

                if (_mode == Mode.Movement)
                {
                    if (Xin.WasKeyReleased(Microsoft.Xna.Framework.Input.Keys.F))
                    {
                        _mode = Mode.Attack;
                        _messages.Enqueue("Attack where...");
                        return;
                    }
                }
                if (Xin.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) && !inMotion)
                {
                    MoveLeft();
                    _turn = false;
                    _timer = 0;
                    _messages.Enqueue("Moving west...");
                }
                else if (Xin.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) && !inMotion)
                {
                    MoveRight();
                    _turn = false;
                    _timer = 0;
                    _messages.Enqueue("Moving east...");
                }

                if (Xin.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) && !inMotion)
                {
                    MoveUp();
                    _turn = false;
                    _timer = 0;
                    _messages.Enqueue("Moving north...");
                }
                else if (Xin.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) && !inMotion)
                {
                    MoveDown();
                    _turn = false;
                    _timer = 0;
                    _messages.Enqueue("Moving south...");
                }

                if (motion != Vector2.Zero)
                {
                    motion.Normalize();
                    Player.Sprite.IsAnimating = true;
                }
                else
                {
                    inMotion = false;
                    Player.Sprite.IsAnimating = false;
                    return;
                }

            }
            else if (!_turn && _timer > 0.25)
            {
                Player.Sprite.IsAnimating = false;

                HandleEnemies();
            }

            if (!Player.Sprite.LockToMap(new(19 * Engine.TileWidth, 7 * Engine.TileHeight), ref motion))
            {
                inMotion = false;
                return;
            }

            Vector2 newPosition = Player.Sprite.Position + motion * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Rectangle nextPotition = new(
                (int)newPosition.X,
                (int)newPosition.Y,
                Engine.TileWidth,
                Engine.TileHeight);

            if (nextPotition.Intersects(collision))
            {
                inMotion = false;
                motion = Vector2.Zero;    
                
                Player.Sprite.Position = new((int)Player.Sprite.Position.X,
                                             (int)Player.Sprite.Position.Y);
                Player.Sprite.IsAnimating = false;

                return;
            }

            if (encounter.Map.PlayerCollides(nextPotition.Grow(-1)))
            {
                _messages.Enqueue("Ouch!");
                inMotion = false;
                motion = Vector2.Zero;
                return;
            }

            Player.Sprite.Position = newPosition;
            Player.Sprite.Tile = Engine.VectorToCell(newPosition);

            base.Update(gameTime);
        }

        private void HandleAttack()
        {
            if (_timer > 0.5)
            {
                if (Xin.WasKeyReleased(Microsoft.Xna.Framework.Input.Keys.A))
                {
                    _timer = 0;
                    _turn = false;
                    _messages.Enqueue("   Attacking west...");
                    DoAttack(new(-1, 0));
                    _mode = Mode.Movement;
                }
                else if (Xin.WasKeyReleased(Microsoft.Xna.Framework.Input.Keys.D))
                {
                    _timer = 0;
                    _turn = false;
                    _messages.Enqueue("   Attacking east...");
                    DoAttack(new(1, 0));
                    _mode = Mode.Movement;
                }
                else if (Xin.WasKeyReleased(Microsoft.Xna.Framework.Input.Keys.W))
                {
                    _timer = 0;
                    _turn = false;
                    _messages.Enqueue("   Attacking north...");
                    DoAttack(new(0, -1));
                    _mode = Mode.Movement;
                }
                else if (Xin.WasKeyReleased(Microsoft.Xna.Framework.Input.Keys.S))
                {
                    _timer = 0;
                    _turn = false;
                    _messages.Enqueue("   Attacking south...");
                    DoAttack(new(0, 1));
                    _mode = Mode.Movement;
                }
            }
        }

        private void DoAttack(Point direction)
        {
            Point target = Player.Tile + direction;
            Rectangle destination = new(target, new(Engine.TileWidth, Engine.TileHeight));

            for (int i =  0; i < encounter.Enemies.Count; i++)
            {
                var enemy = encounter.Enemies[i];

                Point enemyTile = new((int)((Mob)enemy).AnimatedSprite.Position.X / Engine.TileWidth,
                    (int)((Mob)enemy).AnimatedSprite.Position.Y / Engine.TileHeight);

                Rectangle enemyDestination = new(enemyTile, new(Engine.TileWidth, Engine.TileHeight));

                if (enemyDestination.Intersects(destination) && Helper.RollDie(((ICharacter)Player),"Agility"))
                {
                    if (!Helper.RollDie(enemy, "Agility"))
                    {
                        _messages.Enqueue("   Enemy was hit...");

                        AttributePair health = new(enemy.Health.Current);

                        health.Current -= Helper.Random.Next(1, 7);
                        enemy.Health = health;
                    }
                    else
                    {
                        _messages.Enqueue("   Enemy dodges your attack...");
                    }
                }
                else
                {
                    _messages.Enqueue("   Your attack fell upon empty air...");
                }
            }
        }

        private void HandleEnemies()
        {
            _turn = !_turn;
            _timer = 0;

            foreach (ICharacter c in encounter.Enemies)
            {
                Mob mob = c as Mob;
                Point distance = mob.AnimatedSprite.Tile - Player.Tile;

                if ((Math.Abs(distance.X) == 1 && Math.Abs(distance.Y) == 0) || 
                    (Math.Abs(distance.Y) == 1 && Math.Abs(distance.X) == 0))
                {
                    bool roll = Helper.RollDie(c, "Agility");

                    if (roll)
                    {
                        if (!Helper.RollDie(Player, "Agility"))
                        {
                            _messages.Enqueue($"The {c.Name} swings and hits...");
                        }
                        else
                        {
                            _messages.Enqueue($"You nimbly dodge the {c.Name}'s attack");
                        }
                    }
                    else
                    {
                        _messages.Enqueue($"The {c.Name} swings and misses");
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin(SpriteSortMode.Deferred,
                                BlendState.AlphaBlend,
                                SamplerState.PointWrap);

            encounter?.Draw(gameTime, SpriteBatch, Camera);

            Point messageLocation = new(0, Settings.BaseHeight - _messageBox.Height + 5);

            SpriteBatch.Draw(_messageBox,
                             messageLocation,
                             Color.White);
            messageLocation.X += 10;
            if (_messages.Count > 4) 
            {
                _messages.Dequeue();
            }

            foreach (string s in _messages)
            {
                SpriteBatch.DrawString(ControlManager.SpriteFont, s, messageLocation, Color.Black);
                messageLocation.Y += ControlManager.SpriteFont.LineSpacing;
            }

            SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            SpriteBatch.Begin();
            SpriteBatch.Draw(renderTarget, Vector2.Zero, Color.White);
            SpriteBatch.End();
        }

        protected override void Show()
        {
            _turn = false;
            _timer = 0;
            _messages.Clear();

            base.Show();
        }
    }

}
