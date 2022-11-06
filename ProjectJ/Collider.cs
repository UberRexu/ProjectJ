using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace ProjectJ
{
    class Collider : IEntity
    {
        private readonly Game1 game;
        public IShapeF Bounds { get; }

        public Collider(Game1 game, RectangleF rectangleF)
        {
            this.game = game;
            Bounds = rectangleF;
        }
        public virtual void Update(GameTime gameTime)
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawRectangle((RectangleF)Bounds, Color.Blue, 3);
        }
        public void OnCollision(CollisionEventArgs collisionInfo)
        {

        }
    }
}
