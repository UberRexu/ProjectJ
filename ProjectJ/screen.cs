using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectJ
{
    public class screen
    {
        protected EventHandler ScreenEvent;
        public screen(EventHandler theScreenEvent)
        {
            ScreenEvent = theScreenEvent;
        }
        public virtual void Initialize()
        {
            
        }
        public virtual void LoadContent()
        {

        }
        public virtual void Update(GameTime theTime)
        {

        }
        public virtual void Draw(SpriteBatch theBatch)
        {

        }
        public virtual void Getmapwidth()
        {

        }
        public virtual void Getmapheight()
        {

        }
        protected virtual void UnloadContent()
        {
            
        }
    }
}
