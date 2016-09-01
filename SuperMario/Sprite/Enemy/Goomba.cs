using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMario
{
    class Goomba:Enemy
    {
        public enum State { alive,die}
        public State state { get; set; }
        private Rectangle dieFrame;
        private FrameSelector moveFrame;
        private float stateTimer;
        public DateTime dieTime;
        public Goomba(int x, int y,bool moveRight=true)
        {
            name = "goomba";
            positionRectangle = new Rectangle(x, y, 16, 16);
            state = State.alive;
            if (moveRight)
            {
                xVelocity = 1;
            }
            else
            {
                xVelocity = -1;
            }
            init();
        }
        public override void update(GameTime gameTime, List<Sprite> sprites)
        {
            if (state != State.die)
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
            }
            else
                if ((DateTime.Now - dieTime).TotalMilliseconds > 500)
                {
                    canRemove = true;
                }

        }
        public override void xCollision(Sprite s)
        {
            switch (s.name)
            {
                case "gps":
                case "brick":
                case "itemBlock": xVelocity = -xVelocity;
                break;
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
                            yVelocity = 0;
                        }
                        else
                        {
                            yVelocity = 0;
                        }
                        break;
                    }
            }
        }
        public void die(bool blow=false)
        {
            if (blow)
            {
                canNotCollide = true;
                yVelocity = -2;
                dieTime = DateTime.Now;
            }
            else
            {
                canNotCollide = true;
                state = State.die;
                dieTime = DateTime.Now;
            }
        }
        public void init()
        {
            List<Rectangle> moveFrames = new List<Rectangle>();
            moveFrames.Add(new Rectangle(0, 16, 16, 16));
            moveFrames.Add(new Rectangle(16, 16, 16, 16));
            moveFrame = new FrameSelector(0.3f, moveFrames);
            dieFrame = new Rectangle(32, 16, 16, 16);
        }
        public Rectangle getFrame()
        {
            switch(state)
            {
                case State.alive:
                    {
                        stateTimer += 0.01f;
                        return moveFrame.GetFrame(ref stateTimer);
                    }
                case State.die: return dieFrame;
                default: return new Rectangle();
            }
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, positionRectangle, getFrame(), Color.White);
        }
    }
}
