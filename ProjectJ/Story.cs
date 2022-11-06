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
    public class Story : screen
    {
        Game1 game;
        Texture2D Story1;
        Texture2D Story2;
        Texture2D Story3;
        Texture2D Story4;
        Texture2D Story5;
        Song song;
        List<SoundEffect> soundEffects;
        KeyboardState old_keyboard;
        KeyboardState keyboard;
        int currentStory = 1;
        public bool playStoryMusic = false;

        public Story(Game1 game, EventHandler theScreenEvent) : base(theScreenEvent)
        {
            this.game = game;
            this.song = game.Content.Load<Song>("SFX/Ghost-Story");
        }
        public override void Initialize()
        {
            
        }
        public override void LoadContent()
        {
            Story1 = game.Content.Load<Texture2D>("Story/Story1");
            Story2 = game.Content.Load<Texture2D>("Story/Story2");
            Story3 = game.Content.Load<Texture2D>("Story/Story3");
            Story4 = game.Content.Load<Texture2D>("Story/Story4");
            Story5 = game.Content.Load<Texture2D>("Story/Story5");
            soundEffects = new List<SoundEffect>();
            soundEffects.Add(game.Content.Load<SoundEffect>("SFX/FlippingPaperCut"));
        }
        public override void Update(GameTime theTime)
        {
            KeyboardState old_keyboard = keyboard;
            keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Enter) && old_keyboard.IsKeyUp(Keys.Enter))
            {
                soundEffects[0].CreateInstance().Play();
                currentStory++;
            }
            if(currentStory == 6)
            {
                MediaPlayer.Stop();
                ScreenEvent.Invoke(game.hallwayScreen, new EventArgs());
            }
        }
        public override void Draw(SpriteBatch theBatch)
        {
            if(currentStory == 1)
            {
                MediaPlayer.Volume = 0.2f;
                MediaPlayer.Play(song);
                theBatch.Draw(Story1, new Vector2(0, 0), new Rectangle(0, 0, 1920, 1080), Color.White);
            }
            if (currentStory == 2)
            {
                theBatch.Draw(Story2, new Vector2(0, 0), new Rectangle(0, 0, 1920, 1080), Color.White);
            }

            if (currentStory == 3)
            {
                theBatch.Draw(Story3, new Vector2(0, 0), new Rectangle(0, 0, 1920, 1080), Color.White);
            }
            if (currentStory == 4)
            {
                theBatch.Draw(Story4, new Vector2(0, 0), new Rectangle(0, 0, 1920, 1080), Color.White);
            }
            if (currentStory == 5)
            {
                theBatch.Draw(Story5, new Vector2(0, 0), new Rectangle(0, 0, 1920, 1080), Color.White);
            }

        }
    }
}
