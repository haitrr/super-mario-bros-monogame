using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMario
{
    class FireBall:MovableSprite
    {
        public enum State { Falling, Bouncing, Exploding};
        #region Properties
        private static Texture2D texture;
        private static FrameSelector Bounce, Explode;
        public State state;
        private float StateTimer;
        public static TimeSpan createTimer=TimeSpan.FromMilliseconds(-100);
        //phys
        private const float fall_velocity = 1;
        #endregion

        public FireBall(int x,int y,GameTime gameTime,bool right)
        {
            if ((gameTime.TotalGameTime.TotalMilliseconds - createTimer.TotalMilliseconds) < Constants.timeBetweenToFireBall)
            {
                canRemove = true;
                return;
            }
            else createTimer = gameTime.TotalGameTime;
            name = "fireball";
            this.StateTimer = 0;
            this.state = State.Bouncing;
            if (right) xVelocity = 2;
            else xVelocity = -2;
            yVelocity = 2;
            positionRectangle = new Rectangle(x, y, 0, 0);
            init();
        }
        public void init()
        {
            List<Rectangle> bounceList = new List<Rectangle>();
            bounceList.Add(new Rectangle(0, 0, 8, 8));
            bounceList.Add(new Rectangle(8, 0, 8, 8));
            bounceList.Add(new Rectangle(0, 8, 8, 8));
            bounceList.Add(new Rectangle(8, 8, 8, 8));
            Bounce = new FrameSelector(0.1f, bounceList);

            List<Rectangle> explodeList = new List<Rectangle>();
            explodeList.Add(new Rectangle(20, 4, 8, 8));
            explodeList.Add(new Rectangle(18, 17, 12, 14));
            explodeList.Add(new Rectangle(16, 32, 16, 16));
            Explode = new FrameSelector(0.1f, explodeList, false);
        }
        private void GetFrametoDraw()
        {
            switch (state)
            {
                case State.Exploding:
                    xVelocity = yVelocity = 0;
                    if (Explode.curFrameID < 2)
                        sourceRectangle = Explode.GetFrame(ref StateTimer);
                    else
                        canRemove = true;
                    break;
                case State.Bouncing:
                    sourceRectangle = Bounce.GetFrame(ref StateTimer);
                    yVelocity += fall_velocity;
                    break;
                default:
                    sourceRectangle = Bounce.GetFrame(ref StateTimer);
                    break;
            }
            positionRectangle.Width = sourceRectangle.Width;
            positionRectangle.Height = sourceRectangle.Height;
        }

        public void BounceUp()
        {
            yVelocity = -6;
        }
        public override void update(GameTime gameTime, List<Sprite> sprites)
        {
            Sprite s;
            if (state != State.Exploding)
            {
                positionRectangle.X += (int)xVelocity;
                s = checkCollision(sprites);
                if (s != null) xCollision(s);
                else
                {
                    positionRectangle.X -= (int)xVelocity;
                    if (state != State.Exploding)
                    {
                        positionRectangle.Y += (int)yVelocity;
                        s = checkCollision(sprites);
                        if (s != null)  yCollision(s);
                    }
                }
                positionRectangle.X += (int)xVelocity;
            }
            else
            {
                
            }
            GetFrametoDraw();
            StateTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public override void xCollision(Sprite s)
        {
            state = State.Exploding;
            switch(s.name)
            {
                case "gps":
                case "brick":
                case "itemBlock": break;
                case "goomba":
                    {
                        ((Goomba)s).die(true);
                        break;
                    }
                case "turtle":
                    {
                        ((Turtle)s).die();
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
                    if (positionRectangle.Bottom > s.positionRectangle.Top)
                    {
                        positionRectangle.Y = s.positionRectangle.Top - positionRectangle.Height;
                        BounceUp();
                    }
                    break;
                case "goomba":
                    {
                        ((Goomba)s).die(true);
                        break;
                    }
                case "turtle":
                    {
                        ((Turtle)s).die();
                        break;
                    }
            }
        }
        public override void draw(SpriteBatch spriteBatch)
        {
                spriteBatch.Draw(texture, positionRectangle, sourceRectangle, Color.White);
        }
        public static void loadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("sprites/fireBall.png");
        }
    }
}
