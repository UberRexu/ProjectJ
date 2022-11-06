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
    public class Lab : screen
    {
        Game1 game;
        //Map Properties
        TiledMap Lab_tiledMap;
        TiledMapRenderer Lab_tiledMapRenderer;
        TiledMapObjectLayer Lab_Collider;
        TiledMapObjectLayer Lab_Door;
        TiledMapObjectLayer Lab_Computer;
        TiledMapObjectLayer Lab_server_right;
        TiledMapObjectLayer Lab_server_left;
        const int MapWidth = 1920;
        const int MapHeight = 1080;
        public readonly CollisionComponent collisionComponent;
        private readonly List<IEntity> entities = new List<IEntity>();
        //Map Object
        Rectangle exitDoor;
        Rectangle server_right;
        Rectangle server_left;
        Rectangle computer;
        //Player
        PlayerEntity player;
        Rectangle Player;
        SpriteSheet playerSheet;
        //In out parameter
        public bool On_enterLab = false;
        public bool On_exitLab = false;
        //Interaction parameter
        public bool turn_ac25 = false;
        public bool turn_ac0 = false;
        //AC Timer
        int ac_timer;
        bool ac_start;
        string temp;
        //SFX
        List<SoundEffect> soundEffects;
        //Work
        public float work_progress = 0;
        string work_pro;
        Texture2D RulesBook;
        public Lab(Game1 game, EventHandler theScreenEvent) : base(theScreenEvent)
        {
            this.game = game;
            collisionComponent = new CollisionComponent(new RectangleF(0, 0, MapWidth, MapHeight));
            //Load map
            Lab_tiledMap = game.Content.Load<TiledMap>("Lab/Map-Lab");
            Lab_tiledMapRenderer = new TiledMapRenderer(game.GraphicsDevice, Lab_tiledMap);
            //Collider
            foreach (TiledMapObjectLayer layer in Lab_tiledMap.ObjectLayers)
            {
                if (layer.Name == "collider")
                {
                    Lab_Collider = layer;
                }
                if (layer.Name == "computer")
                {
                    Lab_Computer = layer;
                }
                if (layer.Name == "door")
                {
                    Lab_Door = layer;
                }
                if (layer.Name == "server_right")
                {
                    Lab_server_right = layer;
                }
                if (layer.Name == "server_left")
                {
                    Lab_server_left = layer;
                }
            }
            foreach (TiledMapObject obj in Lab_Collider.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Collider(game, new RectangleF(position, obj.Size)));
            }
            foreach (TiledMapObject obj in Lab_Computer.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Collider(game, new RectangleF(position, obj.Size)));
            }
            foreach (TiledMapObject obj in Lab_Door.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Door(game, new RectangleF(position, obj.Size)));
            }
            foreach (TiledMapObject obj in Lab_server_right.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Key_locker(game, new RectangleF(position, obj.Size)));
            }
            foreach (TiledMapObject obj in Lab_server_left.Objects)
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
            soundEffects.Add(game.Content.Load<SoundEffect>("SFX/adjust_ac"));
            soundEffects.Add(game.Content.Load<SoundEffect>("SFX/typing"));
            soundEffects.Add(game.Content.Load<SoundEffect>("SFX/positive"));
            work_pro = ("Work progress 0%");
            temp = ("A/C : Off");
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
            if (game.hallwayScreen.On_enterHallway == false && On_enterLab == true)
            {
                player.PlayerPos = new Vector2(936, 840);
                On_enterLab = false;
                ac_start = true;
            }
            //ExitDoor Point
            exitDoor = new Rectangle(888, 960, 144, 24);
            //Player Point
            Player = new Rectangle((int)player.PlayerPos.X, (int)player.PlayerPos.Y, 40, 68);
            //Server
            server_right = new Rectangle(1656, 168, 144, 120);
            server_left = new Rectangle(120, 168, 144, 120);
            //Computer
            computer = new Rectangle(1680, 744, 120, 96);
            //Interaction
            if (Player.Intersects(server_right) && player.interact == true && turn_ac25 == false)
            {
                soundEffects[2].CreateInstance().Play();
                temp = ("Temperature = 25 Celcius");
                turn_ac25 = true;
                turn_ac0 = false;
            }
            if (Player.Intersects(server_left) && player.interact == true && turn_ac0 == false)
            {
                soundEffects[2].CreateInstance().Play();
                temp = ("Temperature = 0 Celcius");
                turn_ac25 = false;
                turn_ac0 = true;
            }
            if (Player.Intersects(computer) && player.interact == true)
            {
                work_progress++;
            }
            switch (work_progress)
            {
                case 100:
                    work_pro = ("Work progress 2%");
                    soundEffects[3].CreateInstance().Play();
                    break;
                case 200:
                    work_pro = ("Work progress 4%");
                    break;
                case 300:
                    work_pro = ("Work progress 6%");
                    break;
                case 400:
                    work_pro = ("Work progress 8%");
                    break;
                case 500:
                    work_pro = ("Work progress 10%");
                    soundEffects[3].CreateInstance().Play();
                    break;
                case 600:
                    work_pro = ("Work progress 12%");
                    break;
                case 700:
                    work_pro = ("Work progress 14%");
                    break;
                case 800:
                    work_pro = ("Work progress 16%");
                    break;
                case 900:
                    work_pro = ("Work progress 18%");
                    soundEffects[3].CreateInstance().Play();
                    break;
                case 1000:
                    work_pro = ("Work progress 20%");
                    break;
                case 1100:
                    work_pro = ("Work progress 22%");
                    break;
                case 1200:
                    work_pro = ("Work progress 24%");
                    break;
                case 1300:
                    work_pro = ("Work progress 26%");
                    soundEffects[3].CreateInstance().Play();
                    break;
                case 1400:
                    work_pro = ("Work progress 28%");
                    break;
                case 1500:
                    work_pro = ("Work progress 30%");
                    break;
                case 1600:
                    work_pro = ("Work progress 32%");
                    break;
                case 1700:
                    work_pro = ("Work progress 34%");
                    soundEffects[3].CreateInstance().Play();
                    break;
                case 1800:
                    work_pro = ("Work progress 36%");
                    break;
                case 1900:
                    work_pro = ("Work progress 38%");
                    break;
                case 2000:
                    work_pro = ("Work progress 40%");
                    break;
                case 2100:
                    work_pro = ("Work progress 42%");
                    soundEffects[3].CreateInstance().Play();
                    break;
                case 2200:
                    work_pro = ("Work progress 44%");
                    break;
                case 2300:
                    work_pro = ("Work progress 46%");
                    break;
                case 2400:
                    work_pro = ("Work progress 48%");
                    break;
                case 2500:
                    work_pro = ("Work progress 50%");
                    soundEffects[3].CreateInstance().Play();
                    break;
                case 2600:
                    work_pro = ("Work progress 52%");
                    break;
                case 2700:
                    work_pro = ("Work progress 54%");
                    break;
                case 2800:
                    work_pro = ("Work progress 56%");
                    break;
                case 2900:
                    work_pro = ("Work progress 58%");
                    soundEffects[3].CreateInstance().Play();
                    break;
                case 3000:
                    work_pro = ("Work progress 60%");
                    break;
                case 3100:
                    work_pro = ("Work progress 62%");
                    break;
                case 3200:
                    work_pro = ("Work progress 64%");
                    break;
                case 3300:
                    work_pro = ("Work progress 66%");
                    soundEffects[3].CreateInstance().Play();
                    break;
                case 3400:
                    work_pro = ("Work progress 68%");
                    break;
                case 3500:
                    work_pro = ("Work progress 70%");
                    break;
                case 3600:
                    work_pro = ("Work progress 72%");
                    break;
                case 3700:
                    work_pro = ("Work progress 74%");
                    soundEffects[3].CreateInstance().Play();
                    break;
                case 3800:
                    work_pro = ("Work progress 76%");
                    break;
                case 3900:
                    work_pro = ("Work progress 78%");
                    break;
                case 4000:
                    work_pro = ("Work progress 80%");
                    break;
                case 4100:
                    work_pro = ("Work progress 82%");
                    soundEffects[3].CreateInstance().Play();
                    break;
                case 4200:
                    work_pro = ("Work progress 84%");
                    break;
                case 4300:
                    work_pro = ("Work progress 86%");
                    break;
                case 4400:
                    work_pro = ("Work progress 88%");
                    break;
                case 4500:
                    work_pro = ("Work progress 90%");
                    soundEffects[3].CreateInstance().Play();
                    break;
                case 4600:
                    work_pro = ("Work progress 92%");
                    break;
                case 4700:
                    work_pro = ("Work progress 94%");
                    break;
                case 4800:
                    work_pro = ("Work progress 96%");
                    soundEffects[3].CreateInstance().Play();
                    break;
                case 4900:
                    work_pro = ("Work progress 98%");
                    break;
                case 5000:
                    work_pro = ("Work progress 100%");
                    soundEffects[4].CreateInstance().Play();
                    game.objective = 4;
                    game.work_done = true;
                    break;
            }
            //Switch to hallway2
            if (Player.Intersects(exitDoor))
            {
                On_enterLab = false;
                On_exitLab = true;
                game.hallwayScreen.On_enterHallway = true;
                soundEffects[0].CreateInstance().Play();
                ac_timer = 0;
                ac_start = false;
                ScreenEvent.Invoke(game.hallwayScreen, new EventArgs());
                return;
            }
            //Lock camera
            game._cameraPosition.X = 0;
            game._cameraPosition.Y = 0;
            //Update
            foreach (IEntity entity in entities)
            {
                entity.Update(theTime);
            }
            if (ac_start == true && turn_ac25 == false)
            {
                ac_timer++;
            }
            if (ac_timer == 3000)
            {
                ScreenEvent.Invoke(game.gameoverScreen, new EventArgs());
            }
            if (game.pause == true)
            {
                ScreenEvent.Invoke(game.pauseScreen, new EventArgs());
            }
            collisionComponent.Update(theTime);
            Lab_tiledMapRenderer.Update(theTime);
            base.Update(theTime);
        }
        public override void Draw(SpriteBatch theBatch)
        {
            var transformMatrix = game._camera.GetViewMatrix();
            Lab_tiledMapRenderer.Draw(transformMatrix);
            foreach (IEntity entity in entities)
            {
                entity.Draw(theBatch);
            }
            if (player.openbook == true && game.p_havebook == true)
            {
                theBatch.Draw(RulesBook, new Vector2(game._cameraPosition.X + 0, game._cameraPosition.Y + 50), new Rectangle(0, 0, 1920, 1080), Color.White);
            }
            if(player.openbook == false && game.p_havebook == true)
            {
                theBatch.DrawString(game.pixel_font_small, work_pro, new Vector2((game._bgPosition.X + game._cameraPosition.X) - 300, (game._bgPosition.Y + game._cameraPosition.Y) + 250), Color.Cornsilk);
                theBatch.DrawString(game.pixel_font_small, temp, new Vector2((game._bgPosition.X + game._cameraPosition.X) - 300, (game._bgPosition.Y + game._cameraPosition.Y) + 220), Color.Cornsilk);
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
