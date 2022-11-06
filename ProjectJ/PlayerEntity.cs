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
        Game1 _game;
        public int Velocity = 4;
        Vector2 move;
        public Vector2 PlayerPos;
        public IShapeF Bounds { get;}
        private KeyboardState currentKey;
        private KeyboardState oldKey;
        private AnimatedSprite playerSprite;
        string animation;
        Rectangle Player;
        public bool enterRoom;
        public bool interact;
        public bool playerCanMove = true;
        public bool havebook = false;
        public bool openbook = false;
        public bool havekey1 = false; //For C205
        public bool havekey2 = false; //For Flashlight
        public bool havekey3 = false; //FlashLight
        public bool pause = false;
        Texture2D RulesBook;
        public PlayerEntity(Game1 game, IShapeF circleF, AnimatedSprite playerSprite)
        {
            _game = game;
            Bounds = circleF;
            animation = "idle_down";
            playerSprite.Play(animation);
            PlayerPos.X = 1195;
            PlayerPos.Y = 146;
            Bounds.Position = PlayerPos;
            this.playerSprite = playerSprite;
            RulesBook = game.Content.Load<Texture2D>("RulesBook/rules-book2");
        }
        public virtual void Initialize()
        {

        }
        public virtual void LoadContent()
        {

        }
        public virtual void Update(GameTime gameTime)
        {
            Player = new Rectangle((int)Bounds.Position.X, (int)Bounds.Position.Y, 40, 68);
            Bounds.Position = PlayerPos;
            currentKey = Keyboard.GetState();
            
            //Walk Up
            if (currentKey.IsKeyDown(Keys.W) && Bounds.Position.Y > 0 && playerCanMove == true)
            {
                animation = "walk_up";
                playerSprite.Effect = SpriteEffects.None;
                move = new Vector2(0, -Velocity) * gameTime.GetElapsedSeconds() * 50;
                _game.UpdateCamera(move);
                PlayerPos.Y += move.Y;
            }
            else if (oldKey.IsKeyDown(Keys.W) && currentKey.IsKeyUp(Keys.W))
            {
                animation = "idle_up";
                playerSprite.Effect = SpriteEffects.None;
            }

            //Walk Down
            if (currentKey.IsKeyDown(Keys.S) && Bounds.Position.Y < _game.GetMapHeight() && playerCanMove == true)
            {
                animation = "walk_down";
                playerSprite.Effect = SpriteEffects.None;
                move = new Vector2(0, Velocity) * gameTime.GetElapsedSeconds() * 50;
                _game.UpdateCamera(move);
                PlayerPos.Y += move.Y;
            }
            else if (oldKey.IsKeyDown(Keys.S) && currentKey.IsKeyUp(Keys.S))
            {
                animation = "idle_down";
                playerSprite.Effect = SpriteEffects.None;
            }

            //Walk Left
            if (currentKey.IsKeyDown(Keys.A) && Bounds.Position.X > 0 && playerCanMove == true)
            {
                animation = "walk_left";
                playerSprite.Effect = SpriteEffects.None;
                move = new Vector2(-Velocity, 0) * gameTime.GetElapsedSeconds() * 50;
                _game.UpdateCamera(move);
                PlayerPos.X += move.X;
            }
            else if (oldKey.IsKeyDown(Keys.A) && currentKey.IsKeyUp(Keys.A))
            {
                animation = "idle_left";
                playerSprite.Effect = SpriteEffects.None;
            }

            //Walk Right
            if (currentKey.IsKeyDown(Keys.D) && Bounds.Position.Y < _game.GetMapWidth() && playerCanMove == true)
            {
                animation = "walk_right";
                playerSprite.Effect = SpriteEffects.None;
                move = new Vector2(Velocity, 0) * gameTime.GetElapsedSeconds() * 50;
                _game.UpdateCamera(move);
                PlayerPos.X += move.X;
            }
            else if (oldKey.IsKeyDown(Keys.D) && currentKey.IsKeyUp(Keys.D))
            {
                animation = "idle_right";
                playerSprite.Effect = SpriteEffects.None;
            }
            playerSprite.Play(animation);
            playerSprite.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            oldKey = currentKey;

            //Interact
            if (currentKey.IsKeyDown(Keys.E))
            {
                interact = true;
            }
            else
            {
                interact = false;

            }
            if (currentKey.IsKeyDown(Keys.Tab))
            {
                openbook = true;
            }
            else
            {
                openbook = false;
            }
            if(currentKey.IsKeyDown(Keys.Escape))
            {
                pause = true;
                _game.pause = true;
            }
            else
            {
                pause = false;
                _game.pause = false;
            }
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
                PlayerPos.X -= collisionInfo.PenetrationVector.X;
                PlayerPos.Y -= collisionInfo.PenetrationVector.Y;
                _game._cameraPosition -= collisionInfo.PenetrationVector;
            }
            if (collisionInfo.Other.ToString().Contains("Door"))
            {
                enterRoom = true;
            }
            if (collisionInfo.Other.ToString().Contains("Table"))
            {
                PlayerPos.X -= collisionInfo.PenetrationVector.X;
                PlayerPos.Y -= collisionInfo.PenetrationVector.Y;
                _game._cameraPosition -= collisionInfo.PenetrationVector;
            }
            if (collisionInfo.Other.ToString().Contains("Key_locker"))
            {
                PlayerPos.Y -= collisionInfo.PenetrationVector.Y;
                PlayerPos.X -= collisionInfo.PenetrationVector.X;
            }
            if (collisionInfo.Other.ToString().Contains("Locked_door"))
            {
                PlayerPos.X -= collisionInfo.PenetrationVector.X;
                PlayerPos.Y -= collisionInfo.PenetrationVector.Y;
                _game._cameraPosition -= collisionInfo.PenetrationVector;
            }
            if (collisionInfo.Other.ToString().Contains("LabDoor"))
            {
                PlayerPos.X -= collisionInfo.PenetrationVector.X;
                PlayerPos.Y -= collisionInfo.PenetrationVector.Y;
                _game._cameraPosition -= collisionInfo.PenetrationVector;
            }
        }
        public bool OpenRuleBook()
        {
            if (openbook == true)
            {
                return true;
            }
            else
            {
                return false;
            }
                
        }
    }
}
