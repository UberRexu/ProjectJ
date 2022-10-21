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

namespace ProjectJ
{
    public class Hallway : screen
    {
        Game1 game;
        TiledMap Hallway_tiledMap;
        TiledMapRenderer Hallway_tiledMapRenderer;
        TiledMapObjectLayer Hallway_Collider;
        PlayerEntity player;
        private readonly List<IEntity> entities = new List<IEntity>();
        public readonly CollisionComponent collisionComponent;
        const int MapWidth = 2160;
        const int MapHeight = 1440;
        Texture2D _texture;

        public Hallway(Game1 game, EventHandler theScreenEvent) : base(theScreenEvent)
        {
            this.game = game;
            collisionComponent = new CollisionComponent(new RectangleF(0, 0, MapWidth, MapHeight));
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public override void LoadContent()
        {
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
            }
            foreach (TiledMapObject obj in Hallway_Collider.Objects)
            {
                Point2 position = new Point2(obj.Position.X, obj.Position.Y);
                entities.Add(new Collider(game, new RectangleF(position, obj.Size)));
            }
            //Setup Player
            SpriteSheet playerSheet = game.Content.Load<SpriteSheet>("Character/IP_Walk.sf",new JsonContentLoader());
            entities.Add(player = new PlayerEntity(game, new RectangleF(new Point2(1000, 800), new Size2(40,68)), new AnimatedSprite(playerSheet)));
            foreach (IEntity entity in entities)
            {
                collisionComponent.Insert(entity);
            }
            _texture = new Texture2D(game.GraphicsDevice, 1, 1);
            _texture.SetData(new Color[] { Color.DarkSlateGray });
        }
        public override void Update(GameTime theTime)
        {
            foreach (IEntity entity in entities)
            {
                entity.Update(theTime);
            }
            collisionComponent.Update(theTime);
            Hallway_tiledMapRenderer.Update(theTime);
            base.Update(theTime);
        }
        public override void Draw(SpriteBatch theBatch)
        {
            var transformMatrix = game._camera.GetViewMatrix();
            Hallway_tiledMapRenderer.Draw(transformMatrix);
            theBatch.Draw(_texture, new Rectangle(1873, 770, 94, 100), Color.Red); 
            foreach (IEntity entity in entities)
            {
                entity.Draw(theBatch);
            }
            base.Draw(theBatch);
        }
    }
}
