using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMario
{
    class Turtle:Enemy
    {
        public enum State { alive,shell}
        public State state { get; set; }
        private Rectangle shellFrame;
        private FrameSelector moveFrame;
        private float stateTimer;
        public bool stoping { get; private set; }
        public DateTime dieTime;
        public Turtle(int x, int y,bool moveRight=true)
        {
            name = "turtle";
            positionRectangle = new Rectangle(x, y, 16, 21);
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
        public override void xCollision(Sprite s)
        {
            switch (s.name)
            {
                case "gps":
                case "brick":
                case "itemBlock": xVelocity = -xVelocity;
                break;
                case "goomba":
                {
                    if(state ==State.shell)
                    ((Goomba)s).die(true);
                    break;
                }
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
                case "goomba":
                    {
                        if(state==State.shell)
                        ((Goomba)s).die(true);
                        break;
                    }
            }
        }
        public void stop()
        {
            xVelocity = 0;
            stoping = true;
        }
        public void kick(bool right)
        {
            if (right) xVelocity = 2;
            else xVelocity = -2;
            stoping = false;
        }
        public void toShell()
        {
            state = State.shell;
            positionRectangle.Height = 14;
            xVelocity = 0;
            stoping = true;
        }
        public void die()
        {
            canNotCollide = true;
            yVelocity = -4;
        }
        public void init()
        {
            List<Rectangle> moveFrames = new List<Rectangle>();
            moveFrames.Add(new Rectangle(96, 10, 16, 21));
            moveFrames.Add(new Rectangle(112, 10, 16, 21));
            moveFrame = new FrameSelector(0.3f, moveFrames);
            shellFrame = new Rectangle(161, 17, 16, 14);
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
                case State.shell: return shellFrame;
                default: return new Rectangle();
            }
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, positionRectangle, getFrame(), Color.White);
        }
    }
}
