using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgLibrary;
using RpgLibrary.TileEngine;
using SharedProject.Controls;
using SharedProject.Mobs;
using SharedProject.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedProject.GamesScreens
{
    public interface IEncounterState
    {
        void SetEncounter(Player player, Encounter encounter);
    }

    public class EncounterState : GameState, IEncounterState
    {
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

        public EncounterState(Game game) : base(game)
        {
            Game.Services.AddService(typeof(IEncounterState), this);
            Camera = new(new(Point.Zero, new(Settings.BaseWidth, Settings.BaseHeight)));
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            renderTarget = new(GraphicsDevice, Settings.BaseWidth, Settings.BaseHeight);
        }
        
        public void SetEncounter(Player player, Encounter encounter)
        {
            this.encounter = encounter;

            encounter.RandomMap(Game.Content);

            PlayerTile = player.Tile;
            PlayerPosition = player.Position;
            PlayerAnimation = player.Sprite.CurrentAnimation;

            Point newTile = new(3, 5);

            this.encounter.Allies[0].Tile = newTile;
            ((Player)this.encounter.Allies[0]).Sprite.Position = new(newTile.X * Engine.TileWidth, newTile.Y * Engine.TileHeight);
            ((Player)this.encounter.Allies[0]).Sprite.CurrentAnimation = "walkright";
            ((Player)this.encounter.Allies[0]).Sprite.IsAnimating = true;

            newTile = new(16, 5);

            this.encounter.Enemies[0].Tile = newTile;
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
        }

        private void MoveRight()
        {
            motion = new(1, 0);
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
            Point newTile = Player.Sprite.Tile + new Point(0, 2);
            inMotion = true;
            Player.Sprite.CurrentAnimation = "walkdown";
            collision = new(
                newTile.X * Engine.TileWidth,
                newTile.Y * Engine.TileHeight,
                Engine.TileWidth,
                Engine.TileHeight);
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
        }

        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime);

            Player.Update(gameTime);
            encounter.Map.Update(gameTime);

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

            if (!Player.Sprite.LockToMap(new(20 * Engine.TileWidth, 10 * Engine.TileHeight), ref motion))
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
                Player.Sprite.Position = new((int)Player.Sprite.Position.X, (int)Player.Sprite.Position.Y);
                return;
            }

            if (encounter.Map.PlayerCollides(nextPotition))
            {
                inMotion = false;
                motion = Vector2.Zero;
                return;
            }

            Player.Sprite.Position = newPosition;
            Player.Sprite.Tile = Engine.VectorToCell(newPosition);

            base.Update(gameTime);

            encounter?.Update(gameTime);
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

            SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            SpriteBatch.Begin();
            SpriteBatch.Draw(renderTarget, Vector2.Zero, Color.White);
            SpriteBatch.End();
        }
    }

}
