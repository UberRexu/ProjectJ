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
    public class Hallway : screen
    {
        //Game1
        Game1 game;
        //Map Properties
        TiledMap Hallway_tiledMap;
        TiledMapRenderer Hallway_tiledMapRenderer;
        TiledMapObjectLayer Hallway_Collider;
        TiledMapObjectLayer Door;
        TiledMapObjectLayer Locked_door;
        TiledMapObjectLayer Table;
        const int MapWidth = 6144;
        const int MapHeight = 2304;
        public readonly CollisionComponent collisionComponent;
        private readonly List<IEntity> entities = new List<IEntity>();
        //Map Object
        Rectangle b201;
        Rectangle b202;
        Rectangle b203;
        Rectangle c201;
        Rectangle c202;
        Rectangle c203;
        Rectangle c205;
        Rectangle table;
        Rectangle win_time;
        //Player
        public PlayerEntity player;
        Rectangle Player;
        SpriteSheet playerSheet;

        public bool On_enterHallway = false;
        public bool On_exitHallway = false;

        public bool On_enterHallway_Upper = false;
        public bool On_enterHallway_Lower = false;
        public bool On_exitHallway_Upper = false;
        public bool On_exitHallway_Lower = false;

        //Interaction
        public bool draw_chat = false;
        public bool talk_secu = false;
        int currentChat = 1;
        Texture2D secureChat1;
        Texture2D secureChat2;
        Texture2D secureChat3;
        Texture2D secureChat4;
        Texture2D instruction1;
        Texture2D instruction2;

        //Keyboard State
        KeyboardState old_keyboard;
        KeyboardState keyboard;

        //SFX
        List<SoundEffect> soundEffects;
        Song song;
        public float Volume { get; set; }

        //Ghost
        Texture2D ghost;
        Rectangle ghostPoint;
        Rectangle deadzone;

        //Button
        Texture2D button;

        Texture2D RulesBook;
        public bool start_wind;
        bool win = false;
        public Hallway(Game1 game, EventHandler theScreenEvent) : base(theScreenEvent)
        {
            this.game = game;
            collisionComponent = new CollisionComponent(new RectangleF(0, 0, MapWidth, MapHeight));
            //Load map
            Hallway_tiledMap = game.Content.Load<TiledMap>("Hallway/Map-Hallway");
            Hallway_tiledMapRenderer = new TiledMapRenderer(game.GraphicsDevice, Hallway_tiledMap);
            //Collider
            foreach (TiledMapObjectLayer layer in Hallway_tiledMap.ObjectLayers)
            {
                if (layer.Name == "collider")
                {
                    Hallway_Collider = layer;
                }
                if (layer.Name == "door_collider")
                {
                    Door = layer;
                }
                if (layer.Name == "table")
                {
                    Table = layer;
                }
                if (layer.Name == "locked_door")
                {
                    Locked_door = layer;
                }
            }
            foreach (TiledMapObject obj in Hallway_Collider.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Collider(game, new RectangleF(position, obj.Size)));
            }
            foreach (TiledMapObject obj in Door.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Door(game, new RectangleF(position, obj.Size)));
            }
            foreach (TiledMapObject obj in Table.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Table(game, new RectangleF(position, obj.Size)));
            }
            foreach (TiledMapObject obj in Locked_door.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Locked_door(game, new RectangleF(position, obj.Size)));
            }
            //Setup Player
            playerSheet = game.Content.Load<SpriteSheet>("Character/IP_Walk.sf", new JsonContentLoader());
            player = new PlayerEntity(game, new RectangleF(new Point2(1195, 146), new Size2(40, 68)), new AnimatedSprite(playerSheet));
            entities.Add(player);
            foreach (IEntity entity in entities)
            {
                collisionComponent.Insert(entity);
            }
            //Chat
            secureChat1 = game.Content.Load<Texture2D>("Chat/ChatSecu11");
            secureChat2 = game.Content.Load<Texture2D>("Chat/ChatSecu22");
            secureChat3 = game.Content.Load<Texture2D>("Chat/ChatSecu33");
            secureChat4 = game.Content.Load<Texture2D>("Chat/ChatSecu44");
            instruction1 = game.Content.Load<Texture2D>("Chat/Instruction1");
            instruction2 = game.Content.Load<Texture2D>("Chat/Instruction2");
            //SFX
            soundEffects = new List<SoundEffect>();
            soundEffects.Add(game.Content.Load<SoundEffect>("SFX/DoorOpen"));
            soundEffects.Add(game.Content.Load<SoundEffect>("SFX/LockDoor"));
            soundEffects.Add(game.Content.Load<SoundEffect>("SFX/Wind"));
            //Ghost
            ghost = game.Content.Load<Texture2D>("Ghost/ghost");
            button = game.Content.Load<Texture2D>("Ui/bte");
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
            if (game.counter == 1)
            {
                var instance = soundEffects[2].CreateInstance();
                instance.IsLooped = true;
                instance.Play();
                instance.Volume = 0.25f;
                if(win == true)
                {
                    instance.Volume = 0f;
                }
            }
            //Enter Point
            //Office
            if (On_enterHallway == true && game.officeScreen.On_exitOffice == true)
            {
                player.PlayerPos = new Vector2 (1885,836);
                game._bgPosition.X = 1885;
                game._bgPosition.Y = 836;
                game._cameraPosition.X = 693;
                game._cameraPosition.Y = 729;
                On_enterHallway = false;
                game.officeScreen.On_exitOffice = false;
            }
                //Lecture
            if (On_enterHallway == true && game.lectureScreen.On_exitLecture == true)
            {
                player.PlayerPos = new Vector2(2614, 856);
                game._bgPosition.X = 2614;
                game._bgPosition.Y = 856;
                game._cameraPosition.X = 1419;
                game._cameraPosition.Y = 710;
                On_enterHallway = false;
                game.lectureScreen.On_exitLecture = false;
            }
                //Theatre
            if (On_enterHallway == true && game.theatreScreen.On_exitTheatre == true)
            {
                player.PlayerPos = new Vector2(3341, 832);
                game._bgPosition.X = 3341;
                game._bgPosition.Y = 832;
                game._cameraPosition.X = 2147;
                game._cameraPosition.Y = 686;
                On_enterHallway = false;
                game.lectureScreen.On_exitLecture = false;
            }
                //Lab
            if (On_enterHallway == true && game.labScreen.On_exitLab == true)
            {
                player.PlayerPos = new Vector2(4777, 1649);
                game._bgPosition.X = 4777;
                game._bgPosition.Y = 1649;
                game._cameraPosition.X = 3582;
                game._cameraPosition.Y = 1503;
                On_enterHallway = false;
                game.labScreen.On_exitLab = false;
            }
            //Player Point
            Player = new Rectangle((int)player.PlayerPos.X, (int)player.PlayerPos.Y, 40, 68);
            //Room Point
                //Room B
            b201 = new Rectangle(1872, 769, 96, 47);
            b202 = new Rectangle(2592, 769, 96, 47);
            b203 = new Rectangle(3312, 769, 96, 47);
                //Room C
            c201 = new Rectangle(1872, 1584, 96, 47);
            c202 = new Rectangle(2592, 1584, 96, 47);
            c203 = new Rectangle(3312, 1584, 96, 47);
            c205 = new Rectangle(4752, 1584, 96, 47);
            //Win point
            win_time = new Rectangle(954, 30, 486, 1);

            //Table
            table = new Rectangle(984, 696, 72, 81);
            //Interaction
            if (Player.Intersects(table) && player.interact == true && talk_secu == false)
            {
                draw_chat = true;
                talk_secu = true;
                player.playerCanMove = false;
                currentChat = 1;
                game.stop_time = true;
                game.objective = 2;
            }
            //Switch Room
                //B201(Ofice)
            if (Player.Intersects(b201) && player.enterRoom == true)
            {
                game.Random();
                game.officeScreen.On_enterOffice = true;
                player.enterRoom = false;
                soundEffects[0].CreateInstance().Play();
                ScreenEvent.Invoke(game.officeScreen, new EventArgs());
                return;
            }
                //B202(Lecture)
            if (Player.Intersects(b202) && player.enterRoom == true)
            {
                game.Random();
                game.lectureScreen.On_enterLecture = true;
                player.enterRoom = false;
                soundEffects[0].CreateInstance().Play();
                ScreenEvent.Invoke(game.lectureScreen, new EventArgs());
                return;
            }
                //B203(Theatre)
            if (Player.Intersects(b203) && player.enterRoom == true)
            {
                game.Random();
                game.theatreScreen.On_enterTheatre = true;
                player.enterRoom = false;
                soundEffects[0].CreateInstance().Play();
                ScreenEvent.Invoke(game.theatreScreen, new EventArgs());
                return;
            }
                //C205(lab)
            if (Player.Intersects(c205) && player.enterRoom == true && game.p_havekey1 == true)
            {
                game.labScreen.On_enterLab = true;
                On_exitHallway = true;
                player.enterRoom = false;
                soundEffects[0].CreateInstance().Play();
                ScreenEvent.Invoke(game.labScreen, new EventArgs());
                return;
            }
            //Win with time still on
            if(Player.Intersects(win_time) && game.labScreen.work_progress >= 5000 && game.hour < 6)
            {
                win = true;
                MediaPlayer.Play(game.winScreen.Outro);
                ScreenEvent.Invoke(game.winScreen, new EventArgs());
            }
            //Interact
            KeyboardState old_keyboard = keyboard;
            keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Enter) && old_keyboard.IsKeyUp(Keys.Enter))
            {
                //soundEffects[0].CreateInstance().Play();
                currentChat++;
            }
            foreach (IEntity entity in entities)
            {
                entity.Update(theTime);
            }
            if (game.hour == 6)
            {
                deadzone = new Rectangle(960, 48, 480, 24);
                ghostPoint = new Rectangle(1176, 48, 48, 72);
            }
            if (Player.Intersects(deadzone) && game.p_havekey3 == true)
            {
                ScreenEvent.Invoke(game.winScreen, new EventArgs());
            }
            if (Player.Intersects(deadzone) && game.p_havekey3 == false)
            {
                game._cameraPosition = new Vector2(0, 0);
                game._bgPosition = new Vector2(0, 0);
                ScreenEvent.Invoke(game.gameoverScreen, new EventArgs());
            }
            if (game.pause == true)
            {
                game.stop_time = true;
                ScreenEvent.Invoke(game.pauseScreen, new EventArgs());
            }

            collisionComponent.Update(theTime);
            Hallway_tiledMapRenderer.Update(theTime);
            base.Update(theTime);
        }
        public override void Draw(SpriteBatch theBatch)
        {
            var transformMatrix = game._camera.GetViewMatrix();
            Hallway_tiledMapRenderer.Draw(transformMatrix);
            foreach (IEntity entity in entities)
            {
                entity.Draw(theBatch);
            }
            if (draw_chat == true && currentChat == 1)
            {
                theBatch.Draw(secureChat1, new Vector2 (100, 100) , Color.White);
            }
            if (draw_chat == true && currentChat == 2)
            {
                theBatch.Draw(secureChat2, new Vector2(100, 100), Color.White);
            }
            if (draw_chat == true && currentChat == 3)
            {
                theBatch.Draw(secureChat3, new Vector2(100, 100), Color.White);
            }
            if (draw_chat == true && currentChat == 4)
            {
                theBatch.Draw(secureChat4, new Vector2(100, 100), Color.White);
            }
            if (draw_chat == true && currentChat == 5)
            {
                theBatch.Draw(instruction1, new Vector2(100, 100), Color.White);
            }
            if (draw_chat == true && currentChat == 6)
            {
                theBatch.Draw(instruction2, new Vector2(100, 100), Color.White);
            }
            if (draw_chat == true && currentChat == 7)
            {
                player.havebook = true;
                game.p_havebook = true;
                draw_chat = false;
                player.playerCanMove = true;
                currentChat = 1;
                game.stop_time = false;
            }
            if (game.hour == 6)
            {
                theBatch.Draw(ghost, new Vector2(1176, 48), Color.White);
            }
            if (Player.Intersects(table) && talk_secu == false)
            {
                theBatch.Draw(button, new Vector2(984, 696), Color.White);
            }
            if(player.openbook == true && player.havebook == true)
            {
                theBatch.Draw(RulesBook, new Vector2(game._cameraPosition.X + 250, game._cameraPosition.Y - 400), new Rectangle(0, 0, 1920, 1080), Color.White);
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
            theBatch.DrawString(game.font, playerPosX, new Vector2(player.PlayerPos.X, player.PlayerPos.Y -15), Color.Cornsilk);
            theBatch.DrawString(game.font, playerPosY, new Vector2(player.PlayerPos.X, player.PlayerPos.Y), Color.Cornsilk);
            theBatch.DrawString(game.font, cameraPosX, new Vector2(player.PlayerPos.X, player.PlayerPos.Y - 30), Color.Cornsilk);
            theBatch.DrawString(game.font, cameraPosY, new Vector2(player.PlayerPos.X, player.PlayerPos.Y - 45), Color.Cornsilk);
            theBatch.DrawString(game.font, CPX, new Vector2(player.PlayerPos.X, player.PlayerPos.Y - 60), Color.Cornsilk);
            theBatch.DrawString(game.font, CPY, new Vector2(player.PlayerPos.X, player.PlayerPos.Y - 75), Color.Cornsilk);*/
            base.Draw(theBatch);
        }
    }
}
