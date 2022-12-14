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
    public class Lecture : screen
    {
        Game1 game;
        //Map Properties
        TiledMap Lecture_tiledMap;
        TiledMapRenderer Lecture_tiledMapRenderer;
        TiledMapObjectLayer Lecture_Collider;
        TiledMapObjectLayer Lecture_Door;
        TiledMapObjectLayer Lecture_Key_Locker;
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
        public bool p_havekey2 = false;
        public bool On_enterLecture = false;
        public bool On_exitLecture = false;
        //SFX
        List<SoundEffect> soundEffects;
        //Button
        Texture2D button;
        //Ghost
        Texture2D ghost;
        Rectangle ghostPoint;
        Texture2D RulesBook;
        public Lecture(Game1 game, EventHandler theScreenEvent) : base(theScreenEvent)
        {
            this.game = game;
            collisionComponent = new CollisionComponent(new RectangleF(0, 0, MapWidth, MapHeight));
            //Load map
            Lecture_tiledMap = game.Content.Load<TiledMap>("Lecture/Map-Lecture");
            Lecture_tiledMapRenderer = new TiledMapRenderer(game.GraphicsDevice, Lecture_tiledMap);
            //Collider
            foreach (TiledMapObjectLayer layer in Lecture_tiledMap.ObjectLayers)
            {
                if (layer.Name == "collider")
                {
                    Lecture_Collider = layer;
                }
                if (layer.Name == "door")
                {
                    Lecture_Door = layer;
                }
                if (layer.Name == "table")
                {
                    Lecture_Key_Locker = layer;
                }
            }
            foreach (TiledMapObject obj in Lecture_Collider.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Collider(game, new RectangleF(position, obj.Size)));
            }
            foreach (TiledMapObject obj in Lecture_Door.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Door(game, new RectangleF(position, obj.Size)));
            }
            foreach (TiledMapObject obj in Lecture_Key_Locker.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Key_locker(game, new RectangleF(position, obj.Size)));
            }
            //Setup Player
            playerSheet = game.Content.Load<SpriteSheet>("Character/IP_Walk.sf", new JsonContentLoader());
            player = new PlayerEntity(game, new RectangleF(new Point2(1195, 146), new Size2(40, 68)), new AnimatedSprite(playerSheet));
            entities.Add(player);
            foreach (IEntity entity in entities)
            {
                collisionComponent.Insert(entity);
            }
            //SFX
            soundEffects = new List<SoundEffect>();
            soundEffects.Add(game.Content.Load<SoundEffect>("SFX/DoorOpen"));
            soundEffects.Add(game.Content.Load<SoundEffect>("SFX/ItemCollect"));
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
            if (game.hallwayScreen.On_enterHallway == false && On_enterLecture == true)
            {
                player.PlayerPos = new Vector2(1487, 840);
                On_enterLecture = false;
            }
            //Lock camera
            game._cameraPosition.X = -298;
            game._cameraPosition.Y = -227;
            //Player Rectangle
            Player = new Rectangle((int)player.PlayerPos.X, (int)player.PlayerPos.Y, 40, 68);
            //Exit Door Rectangle
            exitDoor = new Rectangle(1464, 936, 96, 24);
            Key_locker = new Rectangle(888, 288, 120, 72);
            //Ghost Point
            if (game.random == 2 || game.random == 4)
            {
                ghostPoint = new Rectangle(720, 312, 38, 64);
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
            if (Player.Intersects(Key_locker) && player.interact == true && player.havekey3 == false && p_havekey2 == true)
            {
                soundEffects[1].CreateInstance().Play();
                player.havekey3 = true;
            }
            //Switch to hallway
            if (Player.Intersects(exitDoor))
            {
                On_enterLecture = false;
                On_exitLecture = true;
                game.hallwayScreen.On_enterHallway = true;
                soundEffects[0].CreateInstance().Play();
                ScreenEvent.Invoke(game.hallwayScreen, new EventArgs());
                return;
            }
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
            Lecture_tiledMapRenderer.Update(theTime);
            base.Update(theTime);
        }
        public override void Draw(SpriteBatch theBatch)
        {
            var transformMatrix = game._camera.GetViewMatrix();
            Lecture_tiledMapRenderer.Draw(transformMatrix);
            foreach (IEntity entity in entities)
            {
                entity.Draw(theBatch);
            }
            if (Player.Intersects(Key_locker) && game.p_havekey2 == true)
            {
                theBatch.Draw(button, new Vector2(900, 288), Color.White);
            }
            if (game.random == 2 || game.random == 4)
            {
                theBatch.Draw(ghost, new Vector2(720, 312), Color.White);
            }
            if (player.openbook == true && game.p_havebook == true)
            {
                theBatch.Draw(RulesBook, new Vector2(game._cameraPosition.X + 350, game._cameraPosition.Y + 300), new Rectangle(0, 0, 1920, 1080), Color.White);
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
