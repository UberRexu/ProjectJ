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
    public class Hallway2 : screen
    {
        //Game1
        Game1 game;

        //Map Properties
        TiledMap Hallway2_tiledMap;
        TiledMapRenderer Hallway2_tiledMapRenderer;
        TiledMapObjectLayer Hallway2_Collider;
        TiledMapObjectLayer Door;
        TiledMapObjectLayer Locked_Door;
        TiledMapObjectLayer c205;
        const int MapWidth = 2400;
        const int MapHeight = 1560; 
        public readonly CollisionComponent collisionComponent;
        private readonly List<IEntity> entities = new List<IEntity>();

        //Map Object
        Rectangle GotoHallway_Upper;
        Rectangle GotoHallway_Lower;
        Rectangle c205_rec;

        //Player
        PlayerEntity player;
        Rectangle Player;
        SpriteSheet playerSheet;
        public bool havelabkey = false;
        public bool On_enterHallway2 = false;
        public bool On_exitHallway2 = false;
        public bool On_enterHallway2_Upper = false;
        public bool On_enterHallway2_Lower = false;
        public bool On_exitHallway2_Upper = false;
        public bool On_exitHallway2_Lower = false;

        //SFX
        List<SoundEffect> soundEffects;

        public Hallway2(Game1 game, EventHandler theScreenEvent) : base(theScreenEvent)
        {
            this.game = game;
            collisionComponent = new CollisionComponent(new RectangleF(0, 0, MapWidth, MapHeight));
            //Load map
            Hallway2_tiledMap = game.Content.Load<TiledMap>("Hallway2/Map-Hallway2");
            Hallway2_tiledMapRenderer = new TiledMapRenderer(game.GraphicsDevice, Hallway2_tiledMap);
            //Collider
            foreach (TiledMapObjectLayer layer in Hallway2_tiledMap.ObjectLayers)
            {
                if (layer.Name == "collider")
                {
                    Hallway2_Collider = layer;
                }
                if (layer.Name == "door")
                {
                    Door = layer;
                }
                if (layer.Name == "locked_door")
                {
                    Locked_Door = layer;
                }
                if (layer.Name == "c205")
                {
                    c205 = layer;
                }
            }
            foreach (TiledMapObject obj in Hallway2_Collider.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Collider(game, new RectangleF(position, obj.Size)));
            }
            foreach (TiledMapObject obj in Door.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Door(game, new RectangleF(position, obj.Size)));
            }
            foreach (TiledMapObject obj in Locked_Door.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Locked_door(game, new RectangleF(position, obj.Size)));
            }
            foreach (TiledMapObject obj in c205.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new LabDoor(game, new RectangleF(position, obj.Size)));
            }
            //Setup Player
            playerSheet = game.Content.Load<SpriteSheet>("Character/IP_Walk.sf", new JsonContentLoader());
            player = new PlayerEntity(game, new RectangleF(new Point2(0, 0), new Size2(40, 68)), new AnimatedSprite(playerSheet));
            player.PlayerPos = new Vector2(48, 240);
            entities.Add(player);
            foreach (IEntity entity in entities)
            {
                collisionComponent.Insert(entity);
            }
            //SFX
            soundEffects = new List<SoundEffect>();
            soundEffects.Add(game.Content.Load<SoundEffect>("SFX/DoorOpen"));
            soundEffects.Add(game.Content.Load<SoundEffect>("SFX/LockDoor"));

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
                //Hallway Upper
            if (On_enterHallway2_Upper == true && game.hallwayScreen.On_exitHallway_Upper == true)
            {
                player.PlayerPos = new Vector2(48, 240);
                game._bgPosition.X = 48;
                game._bgPosition.Y = 240;
                game._cameraPosition.X = 0;
                game._cameraPosition.Y = 0;
                On_enterHallway2_Upper = false;
                game.hallwayScreen.On_exitHallway_Upper = false;
            }
                //Hallway Lower
            if (On_enterHallway2_Lower == true && game.hallwayScreen.On_exitHallway_Lower == true)
            {
                player.PlayerPos = new Vector2(48, 1056);
                game._bgPosition.X = 48;
                game._bgPosition.Y = 1056;
                game._cameraPosition.X = 0;
                game._cameraPosition.Y = 816;
                On_enterHallway2_Lower = false;
                game.hallwayScreen.On_exitHallway_Lower = false;
            }
                //C205
            if (On_enterHallway2 == true && game.labScreen.On_exitLab == true)
            {
                player.PlayerPos = new Vector2(48, 1056);
                game._bgPosition.X = 1154;
                game._bgPosition.Y = 970;
                game._cameraPosition.X = 1149;
                game._cameraPosition.Y = 914;
                On_enterHallway2 = false;
                game.labScreen.On_exitLab = false;
            }
            if (game.hallwayScreen.player.havekey1 == true)
            {
                havelabkey = true;
            }
            //Player Point
            Player = new Rectangle((int)player.PlayerPos.X, (int)player.PlayerPos.Y, 40, 68);
            //Door Point
            GotoHallway_Upper = new Rectangle(0, 120, 24, 288);
            GotoHallway_Lower = new Rectangle(0, 936, 24, 288);
            c205_rec = new Rectangle(1128, 888, 96, 66);
            //Switch Room
                //Go to Hallway Upper
            if (Player.Intersects(GotoHallway_Upper) && player.enterRoom == true)
            {
                game.hallwayScreen.On_enterHallway_Upper = true;
                On_exitHallway2_Upper = true;
                player.enterRoom = false;
                ScreenEvent.Invoke(game.hallwayScreen, new EventArgs());
                return;
            }
            if (Player.Intersects(GotoHallway_Lower) && player.enterRoom == true)
            {
                game.hallwayScreen.On_enterHallway_Lower = true;
                On_exitHallway2_Lower = true;
                player.enterRoom = false;
                ScreenEvent.Invoke(game.hallwayScreen, new EventArgs());
                return;
            }
                //C205
            if (Player.Intersects(c205_rec) && player.enterRoom == true && havelabkey == true)
            {
                game.labScreen.On_enterLab = true;
                On_exitHallway2 = true;
                player.enterRoom = false;
                soundEffects[0].CreateInstance().Play();
                ScreenEvent.Invoke(game.labScreen, new EventArgs());
                return;
            }
            collisionComponent.Update(theTime);
            Hallway2_tiledMapRenderer.Update(theTime);
            base.Update(theTime);
        }
        public override void Draw(SpriteBatch theBatch)
        {
            var transformMatrix = game._camera.GetViewMatrix();
            Hallway2_tiledMapRenderer.Draw(transformMatrix);
            foreach (IEntity entity in entities)
            {
                entity.Draw(theBatch);
            }
            /*
            string playerPosX;
            string playerPosY;
            string cameraPosX;
            string cameraPosY;
            string CPX;
            string CPY;
            playerPosX = "PlayerPosX = " + (int)player.PlayerPos.X;
            playerPosY = "PlayerPosY = " + (int)player.PlayerPos.Y;
            cameraPosX = "CameraPosX = " + (int)game._cameraPosition.X;
            cameraPosY = "CameraPosY = " + (int)game._cameraPosition.Y;
            CPX = "CPX = " + ((int)player.PlayerPos.X - (int)game._cameraPosition.X);
            CPY = "CPY = " + ((int)player.PlayerPos.Y - (int)game._cameraPosition.Y);
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
