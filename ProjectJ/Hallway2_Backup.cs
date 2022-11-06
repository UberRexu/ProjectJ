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
    public class Hallway2_Backup : screen
    {
        //Game1
        Game1 game;

        //Map Properties
        TiledMap Hallway2_backup_tiledMap;
        TiledMapRenderer Hallway2_backup_tiledMapRenderer;
        TiledMapObjectLayer Hallway2_backup_Collider;
        TiledMapObjectLayer Hallway2_backup_Door;
        TiledMapObjectLayer Hallway2_backup_Locked_Door;
        TiledMapObjectLayer Hallway2_backup_c205;
        const int MapWidth = 2400;
        const int MapHeight = 1560;
        public readonly CollisionComponent collisionComponent;
        private readonly List<IEntity> entities = new List<IEntity>();

        //Player
        PlayerEntity player;
        Rectangle Player;
        SpriteSheet playerSheet;

        //In out parameter
        public bool havelabkey = false;
        public bool On_enterHallway2 = false;
        public bool On_exitHallway2 = false;
        public bool On_enterHallway2_Upper = false;
        public bool On_enterHallway2_Lower = false;
        public bool On_exitHallway2_Upper = false;
        public bool On_exitHallway2_Lower = false;

        public Hallway2_Backup(Game1 game, EventHandler theScreenEvent) : base(theScreenEvent)
        {
            this.game = game;
            collisionComponent = new CollisionComponent(new RectangleF(0, 0, MapWidth, MapHeight));
            //Load map
            Hallway2_backup_tiledMap = game.Content.Load<TiledMap>("Hallway2/Map-Hallway2");
            Hallway2_backup_tiledMapRenderer = new TiledMapRenderer(game.GraphicsDevice, Hallway2_backup_tiledMap);
            //Collider
            foreach (TiledMapObjectLayer layer in Hallway2_backup_tiledMap.ObjectLayers)
            {
                if (layer.Name == "collider")
                {
                    Hallway2_backup_Collider = layer;
                }
                if (layer.Name == "door")
                {
                    Hallway2_backup_Door = layer;
                }
                if (layer.Name == "locked_door")
                {
                    Hallway2_backup_Locked_Door = layer;
                }
                if (layer.Name == "c205")
                {
                    Hallway2_backup_c205 = layer;
                }
            }
            foreach (TiledMapObject obj in Hallway2_backup_Collider.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Collider(game, new RectangleF(position, obj.Size)));
            }
            foreach (TiledMapObject obj in Hallway2_backup_Door.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Door(game, new RectangleF(position, obj.Size)));
            }
            foreach (TiledMapObject obj in Hallway2_backup_Locked_Door.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Locked_door(game, new RectangleF(position, obj.Size)));
            }
            foreach (TiledMapObject obj in Hallway2_backup_c205.Objects)
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
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public override void LoadContent()
        {
            base.LoadContent();
        }
        public override void Update(GameTime theTime)
        {
            collisionComponent.Update(theTime);
            Hallway2_backup_tiledMapRenderer.Update(theTime);
            base.Update(theTime);
        }
        public override void Draw(SpriteBatch theBatch)
        {
            var transformMatrix = game._camera.GetViewMatrix();
            Hallway2_backup_tiledMapRenderer.Draw(transformMatrix);
            base.Draw(theBatch);
        }
    }
}
