using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Content;
using MonoGame.Extended.ViewportAdapters;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace ProjectJ
{
    public class Win : screen
    {
        Texture2D WinScreenTexture;
        Game1 game;
        KeyboardState old_keyboard;
        KeyboardState keyboard;
        public Song Outro;

        public Win(Game1 game, EventHandler theScreenEvent) : base(theScreenEvent)
        {
            this.game = game;
            WinScreenTexture = game.Content.Load<Texture2D>("GameOver/end");
            Outro = game.Content.Load<Song>("SFX/Outro");
            
        }
        public override void LoadContent()
        {
            base.LoadContent();
        }
        public override void Update(GameTime theTime)
        {
            KeyboardState old_keyboard = keyboard;
            keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Enter) && old_keyboard.IsKeyUp(Keys.Enter))
            {
                game.Exit();
            }
            base.Update(theTime);
        }
        public override void Draw(SpriteBatch theBatch)
        {
            theBatch.Draw(WinScreenTexture, new Vector2((game._bgPosition.X + game._cameraPosition.X) - 1000, (game._bgPosition.Y + game._cameraPosition.Y) - 500), Color.White);
            base.Draw(theBatch);
        }
    }
}
