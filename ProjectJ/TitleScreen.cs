using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace ProjectJ
{
    public class TitleScreen : screen
    {
        Texture2D menuTexture;
        Texture2D TitleScreenTexture;
        Game1 game;
        int currentMenu = 1;
        bool keyActiveUp = false;
        bool keyActiveDown = false;
        Color color;
        KeyboardState old_keyboard;
        KeyboardState keyboard;
        Song song;

        //SFX
        List<SoundEffect> soundEffects;

        public TitleScreen(Game1 game, EventHandler theScreenEvent) : base(theScreenEvent)
        {
            this.game = game;
            TitleScreenTexture = game.Content.Load<Texture2D>("TitleScreen/ttscreen");
            menuTexture = game.Content.Load<Texture2D>("TitleScreen/allbutton");
        }
        public override void LoadContent()
        {
            color = new Color(255, 255, 255);
            soundEffects = new List<SoundEffect>();
            this.song = game.Content.Load<Song>("SFX/Come-Play-with-Me");
            MediaPlayer.Play(song);
            soundEffects.Add(game.Content.Load<SoundEffect>("SFX/FlippingPaperCut"));
            soundEffects.Add(game.Content.Load<SoundEffect>("SFX/FlippingPaperCut2"));
            soundEffects.Add(game.Content.Load<SoundEffect>("SFX/Wrong"));
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
                    if (currentMenu < 3)
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
                    MediaPlayer.Stop();
                    game.storyScreen.playStoryMusic = true;
                    ScreenEvent.Invoke(game.storyScreen, new EventArgs());
                }
                if (currentMenu == 2)
                {
                    soundEffects[2].CreateInstance().Play();
                }
                if (currentMenu == 3)
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
            theBatch.Draw(TitleScreenTexture, new Vector2(0, 0), Color.White);
            if (currentMenu == 1)
            {
                theBatch.Draw(menuTexture, new Vector2(558, 684), new Rectangle(0, 250, 300, 250), Color.White);
                theBatch.Draw(menuTexture, new Vector2(858, 684), new Rectangle(300, 0, 300, 250), Color.White);
                theBatch.Draw(menuTexture, new Vector2(1158, 684), new Rectangle(600, 0, 300, 250), Color.White);
            }
            if (currentMenu == 2)
            {
                theBatch.Draw(menuTexture, new Vector2(558, 684), new Rectangle(0, 0, 300, 250), Color.White);
                theBatch.Draw(menuTexture, new Vector2(858, 684), new Rectangle(300, 250, 300, 250), Color.White);
                theBatch.Draw(menuTexture, new Vector2(1158, 684), new Rectangle(600, 0, 300, 250), Color.White);
            }
            if (currentMenu == 3)
            {
                theBatch.Draw(menuTexture, new Vector2(558, 684), new Rectangle(0, 0, 300, 250), Color.White);
                theBatch.Draw(menuTexture, new Vector2(858, 684), new Rectangle(300, 0, 300, 250), Color.White);
                theBatch.Draw(menuTexture, new Vector2(1158, 684), new Rectangle(600, 250, 300, 250), Color.White);
            }
            base.Draw(theBatch);
        }
    }
}
