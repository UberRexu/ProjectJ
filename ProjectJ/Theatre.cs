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
    public class Theatre : screen
    {
        Game1 game;
        //Map Properties
        TiledMap Theatre_tiledMap;
        TiledMapRenderer Theatre_tiledMapRenderer;
        TiledMapObjectLayer Theatre_Collider;
        TiledMapObjectLayer Theatre_Door;
        TiledMapObjectLayer Theatre_Key_Locker;
        const int MapWidth = 1920;
        const int MapHeight = 1080;
        public readonly CollisionComponent collisionComponent;
        private readonly List<IEntity> entities = new List<IEntity>();
        //Map Object
        Rectangle exitDoor;
        Rectangle Key_locker;
        //Player
        public PlayerEntity player;
        Rectangle Player;
        SpriteSheet playerSheet;

        public bool On_enterTheatre = false;
        public bool On_exitTheatre = false;
        //SFX
        List<SoundEffect> soundEffects;
        //Button
        Texture2D button;
        //Ghost
        Texture2D ghost;
        Rectangle ghostPoint;
        Texture2D RulesBook;
        public Theatre(Game1 game, EventHandler theScreenEvent) : base(theScreenEvent)
        {
            this.game = game;
            collisionComponent = new CollisionComponent(new RectangleF(0, 0, MapWidth, MapHeight));
            //Load map
            Theatre_tiledMap = game.Content.Load<TiledMap>("Theatre/Map-Theatre");
            Theatre_tiledMapRenderer = new TiledMapRenderer(game.GraphicsDevice, Theatre_tiledMap);
            //Collider
            foreach (TiledMapObjectLayer layer in Theatre_tiledMap.ObjectLayers)
            {
                if (layer.Name == "collider")
                {
                    Theatre_Collider = layer;
                }
                if (layer.Name == "door")
                {
                    Theatre_Door = layer;
                }
                if (layer.Name == "table")
                {
                    Theatre_Key_Locker = layer;
                }
            }
            foreach (TiledMapObject obj in Theatre_Collider.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Collider(game, new RectangleF(position, obj.Size)));
            }
            foreach (TiledMapObject obj in Theatre_Door.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Door(game, new RectangleF(position, obj.Size)));
            }
            foreach (TiledMapObject obj in Theatre_Key_Locker.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Key_locker(game, new RectangleF(position, obj.Size)));
            }
            //Setup Player
            playerSheet = game.Content.Load<SpriteSheet>("Character/IP_Walk.sf", new JsonContentLoader());
            player = new PlayerEntity(game, new RectangleF(new Point2(1346, 735), new Size2(40, 68)), new AnimatedSprite(playerSheet));
            entities.Add(player);
            foreach (IEntity entity in entities)
            {
                collisionComponent.Insert(entity);
            }
            //SFX
            soundEffects = new List<SoundEffect>();
            soundEffects.Add(game.Content.Load<SoundEffect>("SFX/DoorOpen"));
            soundEffects.Add(game.Content.Load<SoundEffect>("SFX/ItemCollect"));
            //Button
            button = game.Content.Load<Texture2D>("Ui/bte");
            //Ghost
            ghost = game.Content.Load<Texture2D>("Ghost/ghost");
            RulesBook = game.Content.Load<Texture2D>("RulesBook/rules-book2");

        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public override void LoadContent()
        {

        }
        public override void Update(GameTime theTime)
        {
            //Enter Point
            if (game.hallwayScreen.On_enterHallway == false && On_enterTheatre == true)
            {
                player.PlayerPos = new Vector2(1184, 800);
                On_enterTheatre = false;
            }
            //ExitDoor Point
            exitDoor = new Rectangle(1173, 912, 96, 24);
            //Locker Point
            Key_locker = new Rectangle(572, 336, 98, 59);
            //Player Point
            Player = new Rectangle((int)player.PlayerPos.X, (int)player.PlayerPos.Y, 40, 68);
            //Ghost Point
            if (game.random == 2 || game.random == 4)
            {
                ghostPoint = new Rectangle(940, 648, 38, 64);
            }
            else
            {
                ghostPoint = new Rectangle(0, 0, 0, 0);
            }
            if (Player.Intersects(ghostPoint))
            {
                ScreenEvent.Invoke(game.gameoverScreen, new EventArgs());
            }
            //Interaction
            if (Player.Intersects(Key_locker) && player.interact == true && player.havekey2 == false && game.hour == 6)
            {
                soundEffects[1].CreateInstance().Play();
                player.havekey2 = true;
                game.lectureScreen.p_havekey2 = true;
            }
            //Switch to hallway
            if (Player.Intersects(exitDoor))
            {
                On_enterTheatre = false;
                On_exitTheatre = true;
                game.hallwayScreen.On_enterHallway = true;
                soundEffects[0].CreateInstance().Play();
                ScreenEvent.Invoke(game.hallwayScreen, new EventArgs());
                return;
            }
            //Lock camera
            game._cameraPosition.X = -298;
            game._cameraPosition.Y = -227;
            //Update
            foreach (IEntity entity in entities)
            {
                entity.Update(theTime);
            }
            if (game.pause == true)
            {
                ScreenEvent.Invoke(game.pauseScreen, new EventArgs());
            }
            collisionComponent.Update(theTime);
            Theatre_tiledMapRenderer.Update(theTime);
        }
        public override void Draw(SpriteBatch theBatch)
        {
            var transformMatrix = game._camera.GetViewMatrix();
            Theatre_tiledMapRenderer.Draw(transformMatrix);
            foreach (IEntity entity in entities)
            {
                entity.Draw(theBatch);
            }
            if (Player.Intersects(Key_locker) && game.hour == 6)
            {
                theBatch.Draw(button, new Vector2(610, 336), Color.White);
            }
            if (game.random == 2 || game.random == 4)
            {
                theBatch.Draw(ghost, new Vector2(940, 648), Color.White);
            }
            if (player.openbook == true && game.p_havebook == true)
            {
                theBatch.Draw(RulesBook, new Vector2(game._cameraPosition.X + 250, game._cameraPosition.Y + 350), new Rectangle(0, 0, 1920, 1080), Color.White);
            }
            /*string playerPosX;
            string playerPosY;
            string cameraPosX;
            string cameraPosY;
            string CPX;
            string CPY;
            playerPosX = "PlayerPosX = " + (int)player.PlayerPos.X;
            playerPosY = "PlayerPosY = " + (int)player.PlayerPos.Y;
            cameraPosX = "CameraPosX = " + (int)game._cameraPosition.X;
            cameraPosY = "CameraPosY = " + (int)game._cameraPosition.Y;
            CPX = "CPX = " + (player.PlayerPos.X - (int)game._cameraPosition.X);
            CPY = "CPY = " + (player.PlayerPos.Y - (int)game._cameraPosition.Y);
            theBatch.DrawString(game.font, playerPosX, new Vector2(player.PlayerPos.X, player.PlayerPos.Y - 15), Color.Cornsilk);
            theBatch.DrawString(game.font, playerPosY, new Vector2(player.PlayerPos.X, player.PlayerPos.Y), Color.Cornsilk);
            theBatch.DrawString(game.font, cameraPosX, new Vector2(player.PlayerPos.X, player.PlayerPos.Y - 30), Color.Cornsilk);
            theBatch.DrawString(game.font, cameraPosY, new Vector2(player.PlayerPos.X, player.PlayerPos.Y - 45), Color.Cornsilk);
            theBatch.DrawString(game.font, CPX, new Vector2(player.PlayerPos.X, player.PlayerPos.Y - 60), Color.Cornsilk);
            theBatch.DrawString(game.font, CPY, new Vector2(player.PlayerPos.X, player.PlayerPos.Y - 75), Color.Cornsilk);*/
            base.Draw(theBatch);
        }
    }
}
