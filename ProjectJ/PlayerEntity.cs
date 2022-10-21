using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Sprites;

namespace ProjectJ
{
    public class PlayerEntity : IEntity
    {
        private readonly Game1 _game;
        public int Velocity = 4;
        Vector2 move;
        public IShapeF Bounds { get; }
        private KeyboardState currentKey;
        private KeyboardState oldKey;
        private AnimatedSprite playerSprite;
        string animation;
        public PlayerEntity(Game1 game, IShapeF circleF, AnimatedSprite playerSprite)
        {
            _game = game;
            Bounds = circleF;
            animation = "idle_down";
            playerSprite.Play(animation);
            this.playerSprite = playerSprite;
        }
        public virtual void Update(GameTime gameTime)
        {
            currentKey = Keyboard.GetState();
            //Walk Up
            if (currentKey.IsKeyDown(Keys.W) && Bounds.Position.Y > 0)
            {
                animation = "walk_up";
                playerSprite.Effect = SpriteEffects.None;
                move = new Vector2(0, -Velocity) * gameTime.GetElapsedSeconds() * 50;
                if (Bounds.Position.X - _game.GetCameraPosY() <= _game.GetMapHeight() && _game.GetCameraPosY() > 0)
                {
                    _game.UpdateCamera(move);
                }
                Bounds.Position += move;
            }
            else if (oldKey.IsKeyDown(Keys.W) && currentKey.IsKeyUp(Keys.W))
            {
                animation = "idle_up";
                playerSprite.Effect = SpriteEffects.None;
            }

            //Walk Down
            if (currentKey.IsKeyDown(Keys.S) && Bounds.Position.Y < _game.GetMapHeight() - ((RectangleF)Bounds).Height)
            {
                animation = "walk_down";
                playerSprite.Effect = SpriteEffects.None;
                move = new Vector2(0, Velocity) * gameTime.GetElapsedSeconds() * 50;
                if (Bounds.Position.Y - _game.GetCameraPosY() <= _game.GetMapWidth() && _game.GetCameraPosY() < _game.GetMapHeight() - _game.GetScreenHeight())
                {
                    _game.UpdateCamera(move);
                }
                Bounds.Position += move;
            }
            else if (oldKey.IsKeyDown(Keys.S) && currentKey.IsKeyUp(Keys.S))
            {
                animation = "idle_down";
                playerSprite.Effect = SpriteEffects.None;
            }

            //Walk Left
            if (currentKey.IsKeyDown(Keys.A) && Bounds.Position.X > 0)
            {
                animation = "walk_left";
                playerSprite.Effect = SpriteEffects.None;
                move = new Vector2(-Velocity, 0) * gameTime.GetElapsedSeconds() * 50;
                if (Bounds.Position.X - _game.GetCameraPosX() <= _game.GetMapWidth() && _game.GetCameraPosX() > 0)
                {
                    _game.UpdateCamera(move);
                }
                Bounds.Position += move;
            }
            else if (oldKey.IsKeyDown(Keys.A) && currentKey.IsKeyUp(Keys.A))
            {
                animation = "idle_left";
                playerSprite.Effect = SpriteEffects.None;
            }

            //Walk Right
            if (currentKey.IsKeyDown(Keys.D) && Bounds.Position.X < _game.GetMapWidth() - ((RectangleF)Bounds).Width)
            {
                animation = "walk_right";
                playerSprite.Effect = SpriteEffects.None;
                move = new Vector2(Velocity, 0) * gameTime.GetElapsedSeconds() * 50;
                if (Bounds.Position.X - _game.GetCameraPosX() <= _game.GetMapWidth() && _game.GetCameraPosX() < _game.GetMapWidth() - _game.GetScreenWidth())
                {
                    _game.UpdateCamera(move);
                }
                Bounds.Position += move;
            }
            else if (oldKey.IsKeyDown(Keys.D) && currentKey.IsKeyUp(Keys.D))
            {
                animation = "idle_right";
                playerSprite.Effect = SpriteEffects.None;
            }

            oldKey = currentKey;
            playerSprite.Play(animation);
            playerSprite.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle((RectangleF)Bounds, Color.White, 0f);
            spriteBatch.Draw(playerSprite, ((RectangleF)Bounds).Center);
        }
        public void OnCollision(CollisionEventArgs collisionInfo)
        {

            if (collisionInfo.Other.ToString().Contains("Collider"))
            {
                Bounds.Position -= collisionInfo.PenetrationVector;
                _game._cameraPosition -= collisionInfo.PenetrationVector;
            }
        }
        public float GetPlayerPosX()
        {
            return Bounds.Position.X;
        }
        public float GetPlayerPosY()
        {
            return Bounds.Position.Y;
        }
    }
}
