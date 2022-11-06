using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace ProjectJ
{
    public class Pause : screen
    {
        Texture2D menuTexture;
        Texture2D pauseTexture;
        Game1 game;
        int currentMenu = 1;
        bool keyActiveUp = false;
        bool keyActiveDown = false;
        KeyboardState old_keyboard;
        KeyboardState keyboard;
        Song song;
        //SFX
        List<SoundEffect> soundEffects;
        public Pause(Game1 game, EventHandler theScreenEvent) : base(theScreenEvent)
        {
            this.game = game;
            pauseTexture = game.Content.Load<Texture2D>("Ui/pause");
            menuTexture = game.Content.Load<Texture2D>("Ui/allbutton");
            soundEffects = new List<SoundEffect>();
            soundEffects.Add(game.Content.Load<SoundEffect>("SFX/FlippingPaperCut"));
            soundEffects.Add(game.Content.Load<SoundEffect>("SFX/FlippingPaperCut2"));
            soundEffects.Add(game.Content.Load<SoundEffect>("SFX/Wrong"));
        }
        public override void LoadContent()
        {
            base.LoadContent();
        }
        public override void Update(GameTime theTime)
        {
            KeyboardState old_keyboard = keyboard;
            keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Left))
            {
                if (keyActiveUp == true)
                {
                    soundEffects[0].CreateInstance().Play();
                    if (currentMenu > 1)
                    {
                        currentMenu = currentMenu - 1;
                        keyActiveUp = false;
                    }
                }
            }
            if (keyboard.IsKeyDown(Keys.Right))
            {
                if (keyActiveDown == true)
                {
                    soundEffects[0].CreateInstance().Play();
                    if (currentMenu < 2)
                    {
                        currentMenu++;
                        keyActiveDown = false;
                    }
                }
            }
            if (keyboard.IsKeyDown(Keys.Enter) && old_keyboard.IsKeyUp(Keys.Enter))
            {
                if (currentMenu == 1)
                {
                    soundEffects[1].CreateInstance().Play();
                    game.stop_time = false;
                    game.pause = false;
                    ScreenEvent.Invoke(game.old_currentScreen, new EventArgs());
                }
                if (currentMenu == 2)
                {
                    game.Exit();
                }
            }
            if (keyboard.IsKeyUp(Keys.Left))
            {
                keyActiveUp = true;
            }
            if (keyboard.IsKeyUp(Keys.Right))
            {
                keyActiveDown = true;
            }
            base.Update(theTime);
        }

        public override void Draw(SpriteBatch theBatch)
        {
            theBatch.Draw(menuTexture, Vector2.Zero, Color.White);
            theBatch.Draw(pauseTexture, new Vector2((game._bgPosition.X + game._cameraPosition.X) - 960, (game._bgPosition.Y + game._cameraPosition.Y) - 540), Color.White);
            if (currentMenu == 1)
            {
                theBatch.Draw(menuTexture, new Vector2((game._bgPosition.X + game._cameraPosition.X) - 397, (game._bgPosition.Y + game._cameraPosition.Y) + 144), new Rectangle(900, 250, 300, 250), Color.White);
                theBatch.Draw(menuTexture, new Vector2((game._bgPosition.X + game._cameraPosition.X) + 203, (game._bgPosition.Y + game._cameraPosition.Y) + 144), new Rectangle(600, 0, 300, 250), Color.White);
            }
            if (currentMenu == 2)
            {
                theBatch.Draw(menuTexture, new Vector2((game._bgPosition.X + game._cameraPosition.X) - 397, (game._bgPosition.Y + game._cameraPosition.Y) + 144), new Rectangle(900, 0, 300, 250), Color.White);
                theBatch.Draw(menuTexture, new Vector2((game._bgPosition.X + game._cameraPosition.X) + 203, (game._bgPosition.Y + game._cameraPosition.Y) + 144), new Rectangle(600, 250, 300, 250), Color.White);
            }
            base.Draw(theBatch);
        }
    }
}
