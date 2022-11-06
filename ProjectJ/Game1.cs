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
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public SpriteFont font;
        public SpriteFont pixel_font;
        public SpriteFont pixel_font_small;

        //Map
        public TitleScreen titleScreen;

        public Hallway hallwayScreen;
        int hallwayWidth = 3840;
        int hallwayHeight = 2160;

        public Lab labScreen;
        int labWidth = 1920;
        int labHeight = 1080;

        public Office officeScreen;
        int officeWidth = 1920;
        int officeHeight = 1080;

        public Lecture lectureScreen;
        int lectureWidth = 1920;
        int lectureHeight = 1080;

        public Theatre theatreScreen;
        int theatreWidth = 1920;
        int theatreHeight = 1080;

        public Story storyScreen;
        public GameOver gameoverScreen;
        public Win winScreen;
        public Pause pauseScreen;
        public screen currentScreen;
        public screen old_currentScreen;
        
        //Viewport
        public static int ScreenWidth = 1920;
        public static int ScreenHeight = 1080;
        public int MapWidth;
        public int MapHeight;

        //Camera
        public OrthographicCamera _camera;
        public Vector2 _cameraPosition;
        public Vector2 _bgPosition;

        //Timer
        public int counter = 1;
        public int hour;
        int minute;
        int ten_minute;
        int limit = 60;
        float countDuration = 0.01f;
        float currentTime = 0f;
        string time;
        public bool stop_time = false;
        string timesup;

        //Ui
        Texture2D ui_bar;
        Texture2D key1;
        Texture2D key2;
        Texture2D flashlight;

        //KeyItem
        public bool p_havekey1 = false;
        public bool p_havekey2 = false;
        public bool p_havekey3 = false;
        public bool p_havebook = false;

        //Random
        Random r = new Random();
        public int random;

        //Objective
        public int objective = 1;
        string objective1;
        string objective2;
        string objective3;
        string objective4;
        string objective5;
        public bool pause = false;

        //Screen Color
        Texture2D blackRectangle;
        //Work
        public bool work_done = false;
        //Open book
        public bool openbook = false;
        //Keyboard State
        KeyboardState old_keyboard;
        KeyboardState keyboard;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.ApplyChanges();
            var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 1920, 1080);
            _camera = new OrthographicCamera(viewportadapter);
            _bgPosition = new Vector2();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //LoadMap
            /*Title*/ titleScreen = new TitleScreen(this, new EventHandler(GameplayScreenEvent));
            /*Story*/ storyScreen = new Story(this, new EventHandler(GameplayScreenEvent));
            /*Office*/ officeScreen = new Office(this, new EventHandler(GameplayScreenEvent));
            /*Theater*/ theatreScreen = new Theatre(this, new EventHandler(GameplayScreenEvent));
            /*Lecture*/ lectureScreen = new Lecture(this, new EventHandler(GameplayScreenEvent));
            /*Lab*/ labScreen = new Lab(this, new EventHandler(GameplayScreenEvent));
            /*Hallway1*/ hallwayScreen = new Hallway(this, new EventHandler(GameplayScreenEvent));
            /*GameOver*/ gameoverScreen = new GameOver(this, new EventHandler(GameplayScreenEvent));
            /*Win*/ winScreen = new Win(this, new EventHandler(GameplayScreenEvent));
            /*Pause*/ pauseScreen = new Pause(this, new EventHandler(GameplayScreenEvent));
            currentScreen = titleScreen;
            currentScreen.LoadContent();
            storyScreen.LoadContent();
            font = Content.Load<SpriteFont>("ArialFont");
            pixel_font = Content.Load<SpriteFont>("ZFPixelus");
            pixel_font_small = Content.Load<SpriteFont>("ZFPixelus_Small");
            ui_bar = Content.Load<Texture2D>("Ui/bar");
            key1 = Content.Load<Texture2D>("KeyItem/keys");
            key2 = Content.Load<Texture2D>("KeyItem/keys2");
            flashlight = Content.Load<Texture2D>("KeyItem/flashlight");
            blackRectangle = new Texture2D(GraphicsDevice, 1, 1);
            blackRectangle.SetData(new[] { Color.Black });
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            //Timer
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (currentScreen == titleScreen || currentScreen == storyScreen)
            {
                counter = 0;
            }
            if (currentTime >= countDuration && stop_time == false)
            {
                counter++;
                currentTime -= countDuration;
            }
            if (counter >= limit)
            {
                minute++;
                counter = 0;
            }
            if (minute == 10)
            {
                ten_minute++;
                minute = 0;
            }
            if (ten_minute == 6)
            {
                hour++;
                ten_minute = 0;
            }
            if (hour == 6)
            {
                stop_time = true;
            }
            //Update Map
            if (currentScreen == hallwayScreen)
            {
                MapWidth = hallwayWidth;
                MapHeight = hallwayHeight;
                _bgPosition.X = 1196;
                _bgPosition.Y = 146;
                _camera.LookAt(_bgPosition + _cameraPosition);
            }
            if (currentScreen == officeScreen)
            {
                MapWidth = officeWidth;
                MapHeight = officeHeight;
                _bgPosition.X = 1346;
                _bgPosition.Y = 735;
                _camera.LookAt(_bgPosition + _cameraPosition);
            }
            if (currentScreen == lectureScreen)
            {
                MapWidth = lectureWidth;
                MapHeight = lectureHeight;
                _bgPosition.X = 1300;
                _bgPosition.Y = 800;
                _camera.LookAt(_bgPosition + _cameraPosition);
            }
            if (currentScreen == theatreScreen)
            {
                MapWidth = theatreWidth;
                MapHeight = theatreHeight;
                _bgPosition.X = 1184;
                _bgPosition.Y = 859;
                _camera.LookAt(_bgPosition + _cameraPosition);
            }
            if (currentScreen == labScreen)
            {
                MapWidth = labWidth;
                MapHeight = labHeight;
                _bgPosition.X = 936;
                _bgPosition.Y = 550;
                _camera.LookAt(_bgPosition + _cameraPosition);
            }
            if (currentScreen == gameoverScreen)
            {
                _bgPosition.X = 950;
                _bgPosition.Y = 540;
                _cameraPosition = new Vector2(0, 0);
                _camera.LookAt(_bgPosition + _cameraPosition);
            }
            if (officeScreen.player.havekey1 == true)
            {
                p_havekey1 = true;
            }
            if (theatreScreen.player.havekey2 == true)
            {
                p_havekey2 = true;
            }
            if (lectureScreen.player.havekey3 == true)
            {
                p_havekey3 = true;
            }
            KeyboardState old_keyboard = keyboard;
            keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Tab) && p_havebook == true)
            {
                openbook = true;
            }
            else
            {
                openbook = false;
            }
            if (pause == false)
            {
                old_currentScreen = currentScreen;
            }
            currentScreen.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            var transformMatrix = _camera.GetViewMatrix();
            spriteBatch.Begin(transformMatrix: transformMatrix);
            currentScreen.Draw(spriteBatch);
            if (currentScreen != titleScreen && currentScreen != storyScreen && currentScreen != gameoverScreen && hallwayScreen.draw_chat == false && hour !=6 && openbook == false && currentScreen != pauseScreen && currentScreen != winScreen)
            {
                time = "Current Time = 0" + hour + ":" + ten_minute + minute + ":" + counter;
                spriteBatch.DrawString(pixel_font, time, new Vector2((_bgPosition.X + _cameraPosition.X) - 400, (_bgPosition.Y + _cameraPosition.Y) - 500), Color.Cornsilk);
            }
            timesup = ("Times is up!");
            objective1 = ("Objective: Go talk to security.");
            objective2 = ("Objective: Get Lab(C205) key.");
            objective3 = ("Objective: Work and survive.");
            objective4 = ("Objective: Leave this place.");
            objective5 = ("Objective: Survive!");
            //Objective
            if (objective == 1 && hallwayScreen.draw_chat == false && currentScreen != gameoverScreen && openbook == false && currentScreen != pauseScreen && currentScreen != winScreen)
            {
                spriteBatch.DrawString(pixel_font_small, objective1, new Vector2((_bgPosition.X + _cameraPosition.X) - 300, (_bgPosition.Y + _cameraPosition.Y) - 400), Color.Cornsilk);
            }
            if (objective == 2 && officeScreen.draw_chat == false && hallwayScreen.draw_chat == false && currentScreen != gameoverScreen && openbook == false && currentScreen != pauseScreen && currentScreen != winScreen)
            {
                spriteBatch.DrawString(pixel_font_small, objective2, new Vector2((_bgPosition.X + _cameraPosition.X) - 300, (_bgPosition.Y + _cameraPosition.Y) - 400), Color.Cornsilk);
            }
            if (objective == 3 && currentScreen != gameoverScreen && openbook == false && currentScreen != pauseScreen && currentScreen != winScreen)
            {
                spriteBatch.DrawString(pixel_font_small, objective3, new Vector2((_bgPosition.X + _cameraPosition.X) - 300, (_bgPosition.Y + _cameraPosition.Y) - 400), Color.Cornsilk);
            }
            if (objective == 4 && currentScreen != gameoverScreen && openbook == false && currentScreen != pauseScreen && currentScreen != winScreen)
            {
                spriteBatch.DrawString(pixel_font_small, objective4, new Vector2((_bgPosition.X + _cameraPosition.X) - 300, (_bgPosition.Y + _cameraPosition.Y) - 400), Color.Cornsilk);
            }
            if (objective == 5 && currentScreen != gameoverScreen && openbook == false && currentScreen != pauseScreen && currentScreen != winScreen)
            {
                spriteBatch.DrawString(pixel_font_small, objective5, new Vector2((_bgPosition.X + _cameraPosition.X) - 180, (_bgPosition.Y + _cameraPosition.Y) - 400), Color.Cornsilk);
            }
            if (hour == 6 && currentScreen != gameoverScreen && openbook == false && currentScreen != pauseScreen && currentScreen != winScreen)
            {
                spriteBatch.DrawString(pixel_font, timesup, new Vector2((_bgPosition.X + _cameraPosition.X) - 200, (_bgPosition.Y + _cameraPosition.Y) - 500), Color.Red);
                objective = 5;
            }
            //Ui for hallway1
            if (currentScreen == hallwayScreen && hallwayScreen.draw_chat == false && openbook == false)
            {
                spriteBatch.Draw(ui_bar, new Vector2(_cameraPosition.X + 900, _cameraPosition.Y + 500), Color.White);
                if (officeScreen.player.havekey1 == true)
                {
                    spriteBatch.Draw(key1, new Vector2(_cameraPosition.X + 1090, _cameraPosition.Y + 515), Color.White);
                }
                if (theatreScreen.player.havekey2 == true)
                {
                    spriteBatch.Draw(key2, new Vector2(_cameraPosition.X + 1156, _cameraPosition.Y + 515), Color.White);
                }
                if (lectureScreen.player.havekey3 == true)
                {
                    spriteBatch.Draw(flashlight, new Vector2(_cameraPosition.X + 1222, _cameraPosition.Y + 515), Color.White);
                }
            }
            //Ui for office screen
            if (currentScreen == officeScreen && officeScreen.draw_chat == false && openbook == false)
            {
                spriteBatch.Draw(ui_bar, new Vector2(_bgPosition.X - 600, _bgPosition.Y + 120), Color.White);

                if(p_havekey1 == true)
                {
                    spriteBatch.Draw(key1, new Vector2(_bgPosition.X - 410, _bgPosition.Y + 135), Color.White);
                }
                if(p_havekey2 == true)
                {
                    spriteBatch.Draw(key2, new Vector2(_bgPosition.X - 344, _bgPosition.Y + 135), Color.White);
                }
                if(p_havekey3 == true)
                {
                    spriteBatch.Draw(flashlight, new Vector2(_bgPosition.X - 278, _bgPosition.Y + 135), Color.White);
                }
            }
            //Ui for lecture screen
            if (currentScreen == lectureScreen && openbook == false)
            {
                spriteBatch.Draw(ui_bar, new Vector2(_bgPosition.X - 600, _bgPosition.Y + 120), Color.White);

                if (p_havekey1 == true)
                {
                    spriteBatch.Draw(key1, new Vector2(_bgPosition.X - 410, _bgPosition.Y + 135), Color.White);
                }
                if (p_havekey2 == true)
                {
                    spriteBatch.Draw(key2, new Vector2(_bgPosition.X - 344, _bgPosition.Y + 135), Color.White);
                }
                if (p_havekey3 == true)
                {
                    spriteBatch.Draw(flashlight, new Vector2(_bgPosition.X - 278, _bgPosition.Y + 135), Color.White);
                }
            }
            //Ui for theatre
            if (currentScreen == theatreScreen && openbook == false)
            {
                spriteBatch.Draw(ui_bar, new Vector2(_bgPosition.X - 600, _bgPosition.Y + 120), Color.White);

                if (p_havekey1 == true)
                {
                    spriteBatch.Draw(key1, new Vector2(_bgPosition.X - 410, _bgPosition.Y + 135), Color.White);
                }
                if (p_havekey2 == true)
                {
                    spriteBatch.Draw(key2, new Vector2(_bgPosition.X - 344, _bgPosition.Y + 135), Color.White);
                }
                if (p_havekey3 == true)
                {
                    spriteBatch.Draw(flashlight, new Vector2(_bgPosition.X - 278, _bgPosition.Y + 135), Color.White);
                }
            }
            //Ui for Lab
            if (currentScreen == labScreen && openbook == false)
            {
                spriteBatch.Draw(ui_bar, new Vector2(_bgPosition.X - 300, _bgPosition.Y + 300), Color.White);

                if (p_havekey1 == true)
                {
                    spriteBatch.Draw(key1, new Vector2(_bgPosition.X - 110, _bgPosition.Y + 315), Color.White);
                }
                if (p_havekey2 == true)
                {
                    spriteBatch.Draw(key2, new Vector2(_bgPosition.X - 0, _bgPosition.Y + 315), Color.White);
                }
                if (p_havekey3 == true)
                {
                    spriteBatch.Draw(flashlight, new Vector2(_bgPosition.X + 72, _bgPosition.Y + 315), Color.White);
                }
            }
            spriteBatch.Draw(blackRectangle, new Rectangle(0, 0, 6144, 2304), Color.Black * 0.2f);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        //Other Method
        public void GameplayScreenEvent(object obj, EventArgs e)
        {
            currentScreen = (screen)obj;
        }
        public int GetMapWidth()
        {
            return MapWidth;
        }
        public int GetMapHeight()
        {
            return MapHeight;
        }
        public void UpdateCamera(Vector2 move)
        {
            _cameraPosition += move;
        }
        public float GetCameraPosX()
        {
            return _cameraPosition.X;
        }
        public float GetCameraPosY()
        {
            return _cameraPosition.Y;
        }
        public float GetScreenWidth()
        {
            return ScreenWidth;
        }
        public int GetScreenHeight()
        {
            return ScreenHeight;
        }
        public int Random()
        {
            random = r.Next(0, 5);
            return random;
        }
    }
}
