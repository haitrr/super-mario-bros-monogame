using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMario
{
    class Coin:Item
    {
        private static Rectangle frame;
        float stateTimer;
        DateTime showTime;
        int animation;
        public Coin(int x, int y)
        {
            name = "coin";
            positionRectangle = new Rectangle(x, y, 16, 16);
            init();
        }
        public void init()
        {
            //List<Rectangle> frames = new List<Rectangle>(), listTemp = new List<Rectangle>();
            //frames.Add(new Rectangle(0,112 , 16, 16));
            //frames.Add(new Rectangle(0,96, 16, 16));
            //frame = new FrameSelector(0.3f, frames);
            frame = new Rectangle(0, 112, 16, 16);
        }
        //public Rectangle getFrame()
        //{
        //    stateTimer += 0.05f;
        //    return frame.GetFrame(ref stateTimer);
        //}
        public override void update(GameTime gameTime, List<Sprite> sprites)
        {
            if(!hiden)
            {
                if ((DateTime.Now - showTime).TotalMilliseconds > animation*100)
                {
                    if(animation<3)
                    positionRectangle.Y -= 16;
                    else
                    positionRectangle.Y += 16;
                    animation++;
                }
                if (animation == 6) canRemove = true;

            }
        }
        public override void showItem()
        {
            if(hiden)
            {
                hiden = false;
                showTime = DateTime.Now;
                animation = 0;
            }
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            if(!hiden) spriteBatch.Draw(texture, positionRectangle, frame, Color.White);
        }
    }
}
