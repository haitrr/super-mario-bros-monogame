using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMario
{
    class Star:Item
    {
        FrameSelector frame;
        float stateTimer;
        public Star(int x, int y)
        {
            name = "star";
            positionRectangle = new Rectangle(x, y, 16, 16);
            xVelocity=0;
            yVelocity=0;
            yAcceleration = 0.1f;
            init();
        }
        public override void update(GameTime gameTime, List<Sprite> sprites)
        {
            if (!canNotCollide)
            {
                Sprite s;
                positionRectangle.X += (int)xVelocity;
                s = checkCollision(sprites);
                if (s != null) xCollision(s);
                else
                {
                    positionRectangle.X -= (int)xVelocity;
                    positionRectangle.Y += (int)yVelocity;
                    s = checkCollision(sprites);
                    if (s != null) yCollision(s);
                    else
                    {
                        positionRectangle.Y += 1;
                        s = checkCollision(sprites);
                        if (s == null) yVelocity += 1;
                        positionRectangle.Y -= 1;
                    }
                }
                positionRectangle.X += (int)xVelocity;
                yVelocity += yAcceleration;
            }
        }
        public override void xCollision(Sprite s)
        {
            switch (s.name)
            {
                case "gps":
                case "brick":
                case "itemBlock": xVelocity = -xVelocity; yVelocity = -yVelocity; break;
            }
        }
        public override void yCollision(Sprite s)
        {
            switch (s.name)
            {
                case "gps":
                case "brick":
                case "itemBlock":
                    {
                        if (positionRectangle.Bottom > s.positionRectangle.Top)
                        {
                            positionRectangle.Y = s.positionRectangle.Top - positionRectangle.Height;
                            yVelocity = -7;
                        }
                        else
                        {
                            yVelocity = 0;
                        }
                        break;
                    }
            }
        }
        public void init()
        {
            List<Rectangle> frames = new List<Rectangle>(), listTemp = new List<Rectangle>();
            frames.Add(new Rectangle(0, 48, 16, 16));
            frames.Add(new Rectangle(16, 48, 16, 16));
            frames.Add(new Rectangle(32, 48, 16, 16));
            frames.Add(new Rectangle(48, 48, 16, 16));
            frame = new FrameSelector(0.3f, frames);
        }
        public Rectangle getFrame()
        {
            stateTimer += 0.01f;
            return frame.GetFrame(ref stateTimer);
        }
        public override void showItem()
        {
            if (hiden)
            {
                hiden = false;
                canNotCollide = false;
                xVelocity = 1;
                yVelocity = -4;
            }
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            if(!hiden) spriteBatch.Draw(texture, positionRectangle, getFrame(), Color.White);
        }
    }
}
