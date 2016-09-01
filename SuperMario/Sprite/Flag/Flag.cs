using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMario
{
    class Flag:Item
    {
        public enum State { SlidingDown, Normal }; 
        private int x, test;        
        public State state;

        public Flag(int x,int y)
        {
            name = "flag";
            positionRectangle = new Rectangle(x, y, 16, 16);
            sourceRectangle = new Rectangle(128, 32, 16, 16);
            state = State.Normal;
        }
        public void slideDown()
        {
            positionRectangle.Y += 1;
        }

        public void Update(int camera)
        {

        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, positionRectangle, sourceRectangle, Color.White);
        }
    }
}
