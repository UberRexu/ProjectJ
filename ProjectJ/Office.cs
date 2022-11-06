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
    public class Office : screen
    {
        //Gmae1
        Game1 game;
        //Map Properties
        TiledMap Office_tiledMap;
        TiledMapRenderer Office_tiledMapRenderer;
        TiledMapObjectLayer Office_Collider;
        TiledMapObjectLayer Office_Door;
        TiledMapObjectLayer Office_Key_Locker;
        const int MapWidth = 1920;
        const int MapHeight = 1080;
        private readonly List<IEntity> entities = new List<IEntity>();
        public readonly CollisionComponent collisionComponent;
        //Map Object
        Rectangle exitDoor;
        Rectangle Key_locker;
        //Player
        public PlayerEntity player;
        Rectangle Player;
        SpriteSheet playerSheet;
        //In out parameter
        public bool On_enterOffice = false;
        public bool On_exitOffice = false;
        //SFX
        List<SoundEffect> soundEffects;
        //Screen Color
        Texture2D color_screen_red;
        int red_screen_counter;
        bool red_screen_occur;
        //Button
        Texture2D button;
        //Chat
        public bool draw_chat;
        int currentChat;
        Texture2D pchat1;
        Texture2D pchat2;
        Texture2D pchat3;
        Texture2D instruction3;
        Texture2D instruction4;
        //Keyboard State
        KeyboardState old_keyboard;
        KeyboardState keyboard;
        //Ghost
        Texture2D ghost;
        Rectangle ghostPoint;
        Texture2D RulesBook;
        public bool openbook = false;

        public Office(Game1 game, EventHandler theScreenEvent) : base(theScreenEvent)
        {
            this.game = game;
            collisionComponent = new CollisionComponent(new RectangleF(0, 0, MapWidth, MapHeight));
            //Load map
            Office_tiledMap = game.Content.Load<TiledMap>("Office/Map-Office");
            Office_tiledMapRenderer = new TiledMapRenderer(game.GraphicsDevice, Office_tiledMap);
            //Collider
            foreach (TiledMapObjectLayer layer in Office_tiledMap.ObjectLayers)
            {
                if (layer.Name == "collider")
                {
                    Office_Collider = layer;
                }
                if (layer.Name == "door")
                {
                    Office_Door = layer;
                }
                if (layer.Name == "key_locker")
                {
                    Office_Key_Locker = layer;
                }
            }
            foreach (TiledMapObject obj in Office_Collider.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Collider(game, new RectangleF(position, obj.Size)));
            }
            foreach (TiledMapObject obj in Office_Door.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Door(game, new RectangleF(position, obj.Size)));
            }
            foreach (TiledMapObject obj in Office_Key_Locker.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Key_locker(game, new RectangleF(position, obj.Size)));
            }
            //Setup Player
            playerSheet = game.Content.Load<SpriteSheet>("Character/IP_Walk.sf", new JsonContentLoader());
            player = new PlayerEntity(game, new RectangleF(new Point2(1346, 735), new Size2(40, 68)), new AnimatedSprite(playerSheet));
            entities.Add(player);
            player.Velocity = 4;
            foreach (IEntity entity in entities)
            {
                collisionComponent.Insert(entity);
            }
            //SFX
            soundEffects = new List<SoundEffect>();
            soundEffects.Add(game.Content.Load<SoundEffect>("SFX/DoorOpen"));
            soundEffects.Add(game.Content.Load<SoundEffect>("SFX/ItemCollect"));
            color_screen_red = game.Content.Load<Texture2D>("ScreenColor/Red");
            button = game.Content.Load<Texture2D>("Ui/bte");
            pchat1 = game.Content.Load<Texture2D>("Chat/ChatP11"); ;
            pchat2 = game.Content.Load<Texture2D>("Chat/ChatP22"); ;
            pchat3 = game.Content.Load<Texture2D>("Chat/ChatP33"); ;
            instruction3 = game.Content.Load<Texture2D>("Chat/Instruction3"); ;
            instruction4 = game.Content.Load<Texture2D>("Chat/Instruction4"); ;
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
            player.Velocity = 2;
            //Enter Point
            if (game.hallwayScreen.On_enterHallway == false && On_enterOffice == true)
            {
                player.PlayerPos = new Vector2 (1295, 725);
                On_enterOffice = false;
            }
            //ExitDoor Point
            exitDoor = new Rectangle(1272, 816, 96, 24);
            //Locker Point
            Key_locker = new Rectangle(480, 168, 72, 72);
            //Player Point
            Player = new Rectangle((int)player.PlayerPos.X, (int)player.PlayerPos.Y, 40, 68);
            //Ghost Point
            if (game.random == 2 || game.random == 4)
            {
                ghostPoint = new Rectangle(1224, 480, 38, 64);
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
            if (Player.Intersects(Key_locker) && player.interact == true && player.havekey1 == false)
            {
                soundEffects[1].CreateInstance().Play();
                game.p_havekey1 = true;
                player.havekey1 = true;
                draw_chat = true;
                player.playerCanMove = false;
                currentChat = 1;
                game.stop_time = true;
                game.objective = 3;
            }
            //Switch to hallway
            if (Player.Intersects(exitDoor))
            {
                On_enterOffice = false;
                On_exitOffice = true;
                game.hallwayScreen.On_enterHallway = true;
                red_screen_counter = 0;
                red_screen_occur = false;
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
            if (red_screen_occur == true)
            {
                red_screen_counter++;
            }
            if (red_screen_counter == 3000)
            {
                ScreenEvent.Invoke(game.gameoverScreen, new EventArgs());
            }
            //Interact
            old_keyboard = keyboard;
            keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Enter) && old_keyboard.IsKeyUp(Keys.Enter))
            {
                currentChat++;
            }
            if (player.openbook == true && game.p_havebook == true)
            {
                openbook = true;
            }
            if (game.pause == true)
            {
                ScreenEvent.Invoke(game.pauseScreen, new EventArgs());
            }
            foreach (IEntity entity in entities)
            {
                entity.Update(theTime);
            }
            collisionComponent.Update(theTime);
            Office_tiledMapRenderer.Update(theTime);
            base.Update(theTime);
        }
        public override void Draw(SpriteBatch theBatch)
        {
            var transformMatrix = game._camera.GetViewMatrix();
            Office_tiledMapRenderer.Draw(transformMatrix);
            foreach (IEntity entity in entities)
            {
                entity.Draw(theBatch);
            }
            if (game.random == 1 || game.random == 5)
            {
                red_screen_occur = true;
                theBatch.Draw(color_screen_red, new Vector2(0, 0), Color.White * 0.2f);
            }
            if (Player.Intersects(Key_locker))
            {
                theBatch.Draw(button, new Vector2(505, 168), Color.White);
            }
            if (draw_chat == true && currentChat == 1)
            {
                theBatch.Draw(pchat1, new Vector2(100, -10), Color.White);
            }
            if (draw_chat == true && currentChat == 2)
            {
                theBatch.Draw(pchat2, new Vector2(100, -10), Color.White);
            }
            if (draw_chat == true && currentChat == 3)
            {
                theBatch.Draw(pchat3, new Vector2(100, -10), Color.White);
            }
            if (draw_chat == true && currentChat == 4)
            {
                theBatch.Draw(instruction3, new Vector2(100, -10), Color.White);
            }
            if (draw_chat == true && currentChat == 5)
            {
                theBatch.Draw(instruction4, new Vector2(100, -10), Color.White);
            }
            if (draw_chat == true && currentChat == 6)
            {
                draw_chat = false;
                player.playerCanMove = true;
                currentChat = 1;
                game.stop_time = false;
            }
            if (game.random == 2 || game.random == 4)
            {
                theBatch.Draw(ghost, new Vector2(1224, 480), Color.White);
            }
            if (player.openbook == true && game.p_havebook == true)
            {
                theBatch.Draw(RulesBook, new Vector2(game._cameraPosition.X + 400, game._cameraPosition.Y + 200), new Rectangle(0, 0, 1920, 1080), Color.White);
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
