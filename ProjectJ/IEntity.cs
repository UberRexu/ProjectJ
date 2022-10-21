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
    public interface IEntity : ICollisionActor
    {
        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);
    }
}
