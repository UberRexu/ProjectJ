using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Content;
using MonoGame.Extended.ViewportAdapters;


namespace ProjectJ
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        //Map
        public Hallway hallwayScreen;
        int hallwayWidth = 2160;
        int hallwayHeight = 1440;
        public screen currentScreen;
        //Viewport
        public static int ScreenWidth = 1920;
        public static int ScreenHeight = 1080;
        int MapWidth;
        int MapHeight;

        public OrthographicCamera _camera;
        public Vector2 _cameraPosition;
        public Vector2 _bgPosition;
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
            var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, ScreenWidth, ScreenHeight);
            _camera = new OrthographicCamera(viewportadapter);
            _bgPosition = new Vector2();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            hallwayScreen = new Hallway(this, new EventHandler(GameplayScreenEvent));
            currentScreen = hallwayScreen;
            currentScreen.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if(currentScreen == hallwayScreen)
            {
                MapWidth = hallwayWidth;
                MapHeight = hallwayHeight;
                _bgPosition.X = 1000;
                _bgPosition.Y = 800;
            }
            currentScreen.Update(gameTime);
            _camera.LookAt(_bgPosition + _cameraPosition);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            var transformMatrix = _camera.GetViewMatrix();
            spriteBatch.Begin(transformMatrix: transformMatrix);
            currentScreen.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

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
        public void StopCamera()
        {
            _cameraPosition.X = 0;
            _cameraPosition.Y = 0;
        }
        public float GetCameraPosX()
        {
            return _cameraPosition.X;
        }
        public float GetCameraPosY()
        {
            return _cameraPosition.Y;
        }
        public int GetScreenWidth()
        {
            return ScreenWidth;
        }
        public int GetScreenHeight()
        {
            return ScreenHeight;
        }
    }
}
