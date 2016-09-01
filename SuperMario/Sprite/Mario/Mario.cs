using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace SuperMario
{
    class Mario:MovableSprite
    {
        public static Texture2D texture;
        public enum Level { Small, Big, Fire };
        public enum State { Standing, Walking, Turn, Jumping, Squat, Dying, Falling, toBig, toSmall, toFire, ThrowFire, SlidingDown, Slided, FallFormBase, WalkingtoCastle };
        private FrameSelector bigStand, bigWalk, bigTurn, bigJump, bigSquat, bigSwim, bigSlideDown;
        private FrameSelector smStand, smWalk, smTurn, smJump, smDie, smSwim, smSlideDown;
        private List<List<int>> YframeInvis;
        private List<Rectangle> toBig, toSmall;
        private float Transformation_Timer, InvisibleTimer;
        public Level level { get; set; }
        public State state { get; set; }
        public bool movingRight, Jumped, Speedup;
        public bool isTransforming, isInvisible, HurtInvisible;
        private const int InvisibleTime = 15000, GotHit_Invisible = 2000; 
        private int lastY, lastYframe;
        private float StateTimer;
        Pole pole;

        private KeyboardState KB_curState, KB_preState;
        bool stay=false;
        public static void loadContent(ContentManager content)
        {
            texture=content.Load<Texture2D>("sprites/mario.png");
        }
        public void init()
        {
            #region Initialize Animations
            #region Init BigMario
            int i = 0;
            //standing
            List<Rectangle> standList = new List<Rectangle>();
            standList.Add(new Rectangle(16 * (i++), 0, 16, 32)); //id = 0
            bigStand = new FrameSelector(0.1f, standList);

            //walking
            List<Rectangle> wkList = new List<Rectangle>();
            for (; i < 4; i++) // id = 1, 2, 3
            {
                wkList.Add(new Rectangle(16 * i, 0, 16, 32));
            }
            bigWalk = new FrameSelector(0.1f, wkList);

            //Turn
            List<Rectangle> turnList = new List<Rectangle>();
            turnList.Add(new Rectangle(16 * (i++), 0, 16, 32)); //id = 4
            bigTurn = new FrameSelector(0.1f, turnList);

            //Jump
            List<Rectangle> jumpList = new List<Rectangle>();
            jumpList.Add(new Rectangle(16 * (i++), 0, 16, 32)); //id = 5
            bigJump = new FrameSelector(0.1f, jumpList);

            //Squat
            List<Rectangle> squatList = new List<Rectangle>();
            squatList.Add(new Rectangle(16 * (i++), 0, 16, 32)); //id = 6
            bigSquat = new FrameSelector(0.1f, squatList);
            //SlideDown
            List<Rectangle> sdList = new List<Rectangle>();
            sdList.Add(new Rectangle(113, 0, 14, 32)); //id = 7
            sdList.Add(new Rectangle(129, 0, 14, 32)); //id = 8
            bigSlideDown = new FrameSelector(0.2f, sdList);
            //Swim
            i = 9;
            List<Rectangle> swimList = new List<Rectangle>();
            for (; i < 15; i++) // id = 1, 2, 3
            {
                swimList.Add(new Rectangle(16 * i, 0, 16, 32));
            }
            bigSwim = new FrameSelector(0.1f, swimList);
            #endregion

            #region Init SmallMario (y = 32x11 = 320)
            //standing
            List<Rectangle> stList = new List<Rectangle>();
            stList.Add(new Rectangle(0, 320, 12, 16)); //id = 0
            smStand = new FrameSelector(0.1f, stList);

            //walking 
            List<Rectangle> walkList = new List<Rectangle>();
            walkList.Add(new Rectangle(16, 320, 13, 16)); //id = 1
            walkList.Add(new Rectangle(32, 320, 11, 16)); //id = 2
            walkList.Add(new Rectangle(46, 320, 15, 16)); //id = 3
            smWalk = new FrameSelector(0.095f, walkList);

            //Turn
            List<Rectangle> tnList = new List<Rectangle>();
            tnList.Add(new Rectangle(64, 320, 13, 16)); //id = 4
            smTurn = new FrameSelector(0.1f, tnList);

            //Jump
            List<Rectangle> jpList = new List<Rectangle>();
            jpList.Add(new Rectangle(78, 320, 16, 16)); //id = 5
            smJump = new FrameSelector(0.1f, jpList);

            //Die
            List<Rectangle> dieList = new List<Rectangle>();
            dieList.Add(new Rectangle(95, 320, 14, 16)); //id = 6
            smDie = new FrameSelector(0.1f, dieList);
            //slidedown
            List<Rectangle> sdList_ = new List<Rectangle>();
            sdList_.Add(new Rectangle(112, 320, 13, 16)); //id = 7
            sdList_.Add(new Rectangle(127, 320, 13, 16)); //id = 8
            smSlideDown = new FrameSelector(0.2f, sdList_);

            //Swim
            List<Rectangle> swmList = new List<Rectangle>();
            swmList.Add(new Rectangle(143, 320, 13, 16)); //id = 9
            swmList.Add(new Rectangle(159, 320, 13, 16)); //id = 10
            swmList.Add(new Rectangle(175, 320, 13, 16)); //id = 11
            swmList.Add(new Rectangle(191, 320, 13, 16)); //id = 12
            swmList.Add(new Rectangle(206, 320, 14, 16)); //id = 13
            smSwim = new FrameSelector(0.1f, swmList);
            #endregion

            #region Tranformation
            this.toBig = new List<Rectangle>(3);
            toBig.Add(new Rectangle(15 * 16, 0, 16, 32)); // mid
            toBig.Add(new Rectangle(0, 320, 12, 16)); // small
            toBig.Add(new Rectangle(0, 0, 16, 32)); // big

            this.toSmall = new List<Rectangle>(2);
            toSmall.Add(new Rectangle(144, 0, 16, 32)); //big fall
            toSmall.Add(new Rectangle(143, 320, 13, 16)); //small fall
            #endregion

            #region InvisibleTransition
            YframeInvis = new List<List<int>>(2);
            YframeInvis.Add(new List<int>(new int[] { 64, 96, 128 })); // big
            YframeInvis.Add(new List<int>(new int[] { 352, 368, 384 })); //small
            #endregion
            #endregion
            this.Transformation_Timer = 0;
            this.InvisibleTimer = 0;
            this.level = Level.Small;
            this.state = State.Standing;
            this.Jumped = true;
            this.movingRight = true;
            this.KB_preState = Keyboard.GetState();
            this.StateTimer = 0;
        }
        private void GetFrametoDraw(float ElapsedGameTime)
        {
            if (level == Level.Small)
            {
                #region smallMario
                switch (state)
                {
                    case State.Walking:
                        Walking();
                        sourceRectangle = smWalk.GetFrame(ref StateTimer);
                        break;
                    case State.Turn:
                        Walking();
                        sourceRectangle = smTurn.GetFrame(ref StateTimer);
                        break;
                    case State.Jumping:
                        Jumping();
                        sourceRectangle = smJump.GetFrame(ref StateTimer);
                        break;
                    case State.Dying:
                        Dying();
                        sourceRectangle = smDie.GetFrame(ref StateTimer);
                        break;
                    case State.Falling:
                        Falling();
                        break;
                    case State.SlidingDown:
                        SlidingDown();
                        sourceRectangle = smSlideDown.GetFrame(ref StateTimer);
                        break;
                    case State.Slided:
                        Slided();
                        break;
                    case State.FallFormBase:
                        FallFromBase();
                        break;
                    case State.WalkingtoCastle:
                        WalkingtoCastle(ref StateTimer);
                        break;
                    case State.toBig:
                        SmalltoBig(ElapsedGameTime);
                        break;
                    default:
                        Standing();
                        sourceRectangle = smStand.GetFrame(ref StateTimer);
                        break;
                }
                #endregion

                if (isInvisible)
                {
                    sourceRectangle.Y = lastYframe;
                    Invisible(ElapsedGameTime, 1);
                }
                else if (HurtInvisible)
                    GetHit(ElapsedGameTime, 1);
            }
            else
            {
                #region bigMario
                switch (state)
                {
                    case State.Walking:
                        sourceRectangle = bigWalk.GetFrame(ref StateTimer);
                        Walking();
                        break;
                    case State.Turn:
                        sourceRectangle = bigTurn.GetFrame(ref StateTimer);
                        Walking();
                        break;
                    case State.Jumping:
                        sourceRectangle = bigJump.GetFrame(ref StateTimer);
                        Jumping();
                        break;
                    case State.Squat:
                        sourceRectangle = bigSquat.GetFrame(ref StateTimer);
                        break;
                    case State.Falling:
                        Falling();
                        break;
                    case State.SlidingDown:
                        SlidingDown();
                        sourceRectangle = bigSlideDown.GetFrame(ref StateTimer);
                        //positionRectangle.X = Flag.Pole.Right - desRect.Width;
                        break;
                    case State.Slided:
                        Slided();
                        break;
                    case State.FallFormBase:
                        FallFromBase();
                        break;
                    case State.WalkingtoCastle:
                        WalkingtoCastle(ref StateTimer);
                        break;
                    case State.toSmall:
                        BigtoSmall(ElapsedGameTime);
                        break;
                    case State.toFire:
                        BigtoFire(ElapsedGameTime);
                        break;
                    case State.ThrowFire:
                        if (StateTimer < 0.2)
                            sourceRectangle = new Rectangle(16 * 16, 64, 16, 32);
                        else
                        {
                            StateTimer = 0;
                            state = State.Standing;
                        }
                        break;
                    default:
                        sourceRectangle = bigStand.GetFrame(ref StateTimer);
                        Standing();
                        break;
                }
                #endregion

                if (!isInvisible)
                {
                    if (level == Level.Fire)
                        sourceRectangle.Y = 32;
                }
                else
                {
                    sourceRectangle.Y = lastYframe;
                    Invisible(ElapsedGameTime, 0);
                }
            }

            positionRectangle.Width = sourceRectangle.Width;
            positionRectangle.Height = sourceRectangle.Height;

            StateTimer += ElapsedGameTime / 1000;
        }
        private void Standing()
        {
            Jumped = KB_preState.IsKeyDown(Keys.W) ? true : false;
            yVelocity = xVelocity = 0;

            //if (KB_curState.IsKeyDown(Keys.G) && !KB_preState.IsKeyDown(Keys.G))
            //{
            //    if (level == Level.Fire)
            //    {
            //        if (Game.FireBalls.Count < 2)
            //        {
            //            Game.Add_NewFireball();
            //            state = State.ThrowFire;
            //        }
            //    }
            //}

            if (KB_curState.IsKeyDown(Keys.D))
            {
                movingRight = true;
                state = State.Walking;
            }
            else if (KB_curState.IsKeyDown(Keys.A))
            {
                movingRight = false;
                state = State.Walking;
            }
            else if (KB_curState.IsKeyDown(Keys.W))
            {
                if (!Jumped)
                {
                    lastY = positionRectangle.Y;
                    yVelocity = Constants.jump_velocity;
                    state = State.Jumping;
                }
            }
        }

        private void Walking()
        {
            Jumped = KB_preState.IsKeyDown(Keys.W) ? true : false;
            state = State.Walking;
            float max_v;
            if (KB_curState.IsKeyDown(Keys.G)) //run key
            {
                max_v = Constants.max_run_velocity;
                xAcceleration = Constants.run_accelerate;
                Speedup = true;
            }
            else
            {
                max_v = Constants.max_walk_velocity;
                xAcceleration = Constants.walk_accelerate;
                Speedup = false;
            }

            if (KB_curState.IsKeyDown(Keys.D))
            {
                //moving right
                movingRight = true;
                if (xVelocity < 0) //walking left earlier
                {
                    state = State.Turn;
                    xAcceleration += 0.3f;
                }

                if (xVelocity > max_v) //slow down if pre-state was spdup
                    xVelocity -= xAcceleration;
                else if (xVelocity < max_v) //speed up if not reach max_v
                    xVelocity += xAcceleration;
            }
            else if (KB_curState.IsKeyDown(Keys.A))
            {
                //moving left
                movingRight = false;
                if (xVelocity > 0) //walking right earlier
                {
                    state = State.Turn;
                    xAcceleration += 0.3f;
                }

                if (xVelocity < max_v * -1) //slow down if pre-state was spdup
                    xVelocity += xAcceleration;
                else if (xVelocity > max_v * -1) //speed up if not reach max_v
                    xVelocity -= xAcceleration;
            }
            else
            {
                //slow down (moving right or left)
                if (movingRight)
                {
                    if (xVelocity > 0) //slow down then stop
                        xVelocity -= xAcceleration * 5;
                    else // moving left earlier -> stop immediately
                    {
                        xVelocity = 0;
                        state = State.Standing;
                    }
                }
                else
                {
                    if (xVelocity < 0) //slow down then stop
                        xVelocity += xAcceleration * 5;
                    else // moving left earlier -> stop immediately
                    {
                        xVelocity = 0;
                        state = State.Standing;
                    }
                }
            }
            if (KB_curState.IsKeyDown(Keys.W))
            {
                if (!Jumped)
                {
                    lastY =  positionRectangle.Y;
                    yVelocity = Constants.jump_velocity;
                    state = State.Jumping;
                }
            }
        }

        private void Jumping()
        {
            Jumped = true;
            yVelocity += Constants.fall_velocity; //decrease yVelocity gradually
            if ((lastY - positionRectangle.Y) > 55)
            {
                state = State.Falling;

            }

            //if (KB_curState.IsKeyDown(Keys.G))
            //{
            //    if (level == Level.Fire && !KB_preState.IsKeyDown(Keys.G))
            //    {
            //        if (Game.FireBalls.Count < 2)
            //        {
            //            Game.Add_NewFireball();
            //            sourceRectangle = new Rectangle(16 * 16, 64, 16, 32);
            //            //state = State.ThrowFire;
            //        }
            //    }
            //}

            if (KB_curState.IsKeyDown(Keys.D))
            {
                if (xVelocity < (Constants.max_walk_velocity - 2))  //speed up if not reach max_v
                    xVelocity += xAcceleration;
            }
            else if (KB_curState.IsKeyDown(Keys.A))
            {
                if (xVelocity > (Constants.max_walk_velocity - 2) * -1) //speed up if not reach max_v
                    xVelocity -= xAcceleration;
            }

            if (!KB_curState.IsKeyDown(Keys.W))
            {
                state = State.Falling;
            }
            else
                yVelocity -= Constants.fall_velocity;
        }

        private void Falling()
        {
            if (yVelocity < Constants.max_y_velocity)
            {
                yVelocity += Constants.fall_velocity * 1.25f;
            }

            if (KB_curState.IsKeyDown(Keys.D))
            {
                if (xVelocity < (Constants.max_walk_velocity - 1))  //speed up if not reach max_v
                    xVelocity += xAcceleration;
            }
            else if (KB_curState.IsKeyDown(Keys.A))
            {
                if (xVelocity > (Constants.max_walk_velocity - 1) * -1) //speed up if not reach max_v
                    xVelocity -= xAcceleration;
            }

            //if (KB_curState.IsKeyDown(Keys.G))
            //{
            //    if (level == Level.Fire && !KB_preState.IsKeyDown(Keys.G))
            //    {
            //        if (Game.FireBalls.Count < 2)
            //        {
            //            Game.Add_NewFireball();
            //            sourceRectangle = new Rectangle(16 * 16, 64, 16, 32);
            //            //state = State.ThrowFire;
            //        }
            //    }
            //}
        }

        private void Dying()
        {
            isTransforming = true;
            positionRectangle.Y += (int)yVelocity;
            yVelocity += Constants.fall_velocity - 0.05f;
        }
        private void SlidingDown()
        {
            xVelocity = 0;
            yVelocity = 0;
            positionRectangle.Y += 1;
        }
        private void Slided() //when mario slided down to btm of the pole
        {
            //turn to the right side of the pole
            if (movingRight == true)
            {
                movingRight = false;
                positionRectangle.X += positionRectangle.Width + 3;
            }
            if (pole.slided) state = State.FallFormBase;
        }
        private void FallFromBase()
        {
            positionRectangle.Y += 1;
            yVelocity += 2;
        }
        private void WalkingtoCastle(ref float StateTimer) //
        {
            //
            movingRight = true;
            xVelocity = 1;
            if (level == Level.Small)
                sourceRectangle = smWalk.GetFrame(ref StateTimer);
            else
                sourceRectangle = bigWalk.GetFrame(ref StateTimer);
        }
        /// <summary>
        /// start mario's tranformation, 
        /// 1 = to big
        /// 2 = got hit by enemies
        /// 3 = invisible
        /// </summary>
        /// <param name="stt"></param>
        public void startTranformation(int stt = 0)
        {
            if (isTransforming) return;
            lastY = positionRectangle.Y;
            xVelocity = 0;
            yVelocity = 0;
            isTransforming = true;
            switch (stt)
            {
                case 0:
                    state = State.toBig;
                    break;
                case 1: //got hit
                    if (level != Level.Small)
                        state = State.toSmall;
                    else
                    {
                        state = State.Dying;
                        positionRectangle.Y -= 5; //jump up
                        yVelocity = 0; //falling velocity
                    }
                    break;
                case 2:
                    state = State.toFire;
                    break;
                case 3:
                    sourceRectangle.Y = lastYframe;
                    isTransforming = false;
                    isInvisible = true;
                    break;
                default:
                    break;
            }
        }

        //perform tranformation animation
        private void SmalltoBig(float ElapsedGameTime)
        {
            Transformation_Timer += ElapsedGameTime;
            if (Transformation_Timer < 120) //mid size
            {
                sourceRectangle = toBig[0];
                positionRectangle.Y = lastY - 16;
            }
            else if (Transformation_Timer < 240) //small
            {
                sourceRectangle = toBig[1];
                positionRectangle.Y = lastY;
            }
            else if (Transformation_Timer < 360) //mid size
            {
                sourceRectangle = toBig[0];
                positionRectangle.Y = lastY - 16;
            }
            else if (Transformation_Timer < 480) //small
            {
                sourceRectangle = toBig[1];
                positionRectangle.Y = lastY;
            }
            else if (Transformation_Timer < 620) //mid size
            {
                sourceRectangle = toBig[0];
                positionRectangle.Y = lastY - 16;
            }
            else if (Transformation_Timer < 760) //big size
                sourceRectangle = toBig[2];
            else if (Transformation_Timer < 880) //mid size
                sourceRectangle = toBig[0];
            else if (Transformation_Timer < 960) //small size
            {
                sourceRectangle = toBig[1];
                positionRectangle.Y = lastY;
            }
            else
            {
                sourceRectangle = toBig[0];
                positionRectangle.Y = lastY - 16;

                level = Level.Big;
                isTransforming = false;
                state = State.Standing;
                Transformation_Timer = 0;
            }
        }

        private void BigtoSmall(float ElapsedGameTime)
        {
            Transformation_Timer += ElapsedGameTime;
            if (Transformation_Timer < 120) //big size
            {
                sourceRectangle = toSmall[0];
            }
            else if (Transformation_Timer < 240) //small size
            {
                sourceRectangle = toSmall[1];
                positionRectangle.Y = lastY + 16;
            }
            else if (Transformation_Timer < 360) //big size
            {
                sourceRectangle = toSmall[0];
                positionRectangle.Y = lastY;
            }
            else if (Transformation_Timer < 480) //small size
            {
                sourceRectangle = toSmall[1];
                positionRectangle.Y = lastY + 16;
            }
            else if (Transformation_Timer < 620) //big size
            {
                sourceRectangle = toSmall[0];
                positionRectangle.Y = lastY;
            }
            else if (Transformation_Timer < 760) //small size
            {
                sourceRectangle = toSmall[1];
                positionRectangle.Y = lastY + 16;
            }
            else if (Transformation_Timer < 880) //big size
            {
                sourceRectangle = toSmall[0];
                positionRectangle.Y = lastY;
            }
            else //small size
            {
                sourceRectangle = toSmall[1];
                positionRectangle.Y = lastY + 16;

                level = Level.Small;
                isTransforming = false;
                HurtInvisible = true;
                state = State.Standing;
                Transformation_Timer = 0;
            }
        }

        private void BigtoFire(float ElapsedGameTime)
        {
            Transformation_Timer += ElapsedGameTime;
            if (Transformation_Timer < 140) // Fire, row 2
            {
                sourceRectangle.Y = 32 * 2;
            }
            else if (Transformation_Timer < 240) //Red, row 3
            {
                sourceRectangle.Y = 32 * 3;
            }
            else if (Transformation_Timer < 380) //Black, row 4
            {
                sourceRectangle.Y = 32 * 4;
            }
            else if (Transformation_Timer < 480) //Green, row 5
            {
                sourceRectangle.Y = 32 * 5;
            }
            else if (Transformation_Timer < 600) //Red, row 3
            {
                sourceRectangle.Y = 32 * 3;
            }
            else if (Transformation_Timer < 700) //Black, row 4
            {
                sourceRectangle.Y = 32 * 4;
            }
            else if (Transformation_Timer < 800) //Green, row 5
            {
                sourceRectangle.Y = 32 * 5;
            }
            else if (Transformation_Timer < 890) //Red, row 3
            {
                sourceRectangle.Y = 32 * 3;
            }
            else // Fire, row 2
            {
                sourceRectangle.Y = 32 * 2;

                level = Level.Fire;
                state = State.Falling;
                isTransforming = false;
                Transformation_Timer = 0;
            }
        }

        //mario blink
        private int id = 0;
        private void Invisible(float ElapsedGameTime, int i)
        {
            Transformation_Timer += ElapsedGameTime;
            if (Transformation_Timer < InvisibleTime)
            {
                InvisibleTimer += ElapsedGameTime;
                if (InvisibleTimer > 100)
                {
                    lastYframe = YframeInvis[i][id++];
                    sourceRectangle.Y = lastYframe;
                    InvisibleTimer = 0;
                }
                if (id >= 3)
                    id = 0;
            }
            else
            {
                Transformation_Timer = 0;
                InvisibleTimer = 0;
                isInvisible = false;
            }
        }

        private void GetHit(float ElapsedGameTime, int i)
        {
            Transformation_Timer += ElapsedGameTime;
            if (Transformation_Timer < GotHit_Invisible)
            {
                InvisibleTimer += ElapsedGameTime;
                if (InvisibleTimer > 100)
                {
                    sourceRectangle.Y = YframeInvis[i][id++];
                    InvisibleTimer = 0;
                }
                if (id >= 3)
                    id = 0;
            }
            else
            {
                Transformation_Timer = 0;
                InvisibleTimer = 0;
                HurtInvisible = false;
            }
        }

        public override void update(GameTime gameTime, List<Sprite> sprites)
        {

            KB_curState = Keyboard.GetState();
            GetFrametoDraw((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            Sprite s = null;
            if (!isTransforming)
            {
                positionRectangle.X += (int)xVelocity;
                s = checkCollision(sprites);
                if (s != null && state!=State.SlidingDown) xCollision(s);
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
                        if (s == null && state != State.Jumping&& state!=State.SlidingDown &&state!=State.Slided && state!= State.FallFormBase) state = State.Falling;
                        positionRectangle.Y -= 1;
                    }
                    positionRectangle.X += (int)xVelocity;
                }
            }

            KB_preState = KB_curState;
            #region test
            //test, use these function to change mario level
            if (Keyboard.GetState().IsKeyDown(Keys.T) && level == Level.Small)
            {
                startTranformation(); //tobig
                //level = Level.Big;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F) && level != Level.Small && level != Level.Fire)
            {
                startTranformation(2); //tofire
                //level = Level.Fire;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Y) && level != Level.Small)
            {
                startTranformation(1); //tosmall, gothit invisible
                //level = Level.Small;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.R))
                startTranformation(3); //star invisible
            if (Keyboard.GetState().IsKeyDown(Keys.E))
                startTranformation(1); //die when mario is small
            #endregion
        }
        public override void xCollision(Sprite s)
        {
            switch (s.name)
            {
                case "gps":
                case "brick":
                case "itemBlock":
                    {
                        if (positionRectangle.Left < s.positionRectangle.Left)
                        {
                            positionRectangle.X = s.positionRectangle.Left - positionRectangle.Width;
                            xVelocity = 0;
                            
                        }
                        else if (positionRectangle.Right > s.positionRectangle.Right)
                        {
                            positionRectangle.X = s.positionRectangle.Right;
                            xVelocity = 0;
                        }
                        break;
                    }
                case "bigMushoom":
                    {
                        if (level == Level.Small) startTranformation();
                        s.canRemove = true;
                        break;
                    }
                case "flower":
                    {
                        if (level == Level.Big) startTranformation(2);
                        if (level == Level.Small) startTranformation();
                        s.canRemove = true;
                        break;
                    }
                case "star":
                    {
                        startTranformation(3);
                        s.canRemove = true;
                        break;
                    }
                case "goomba":
                    {
                        Goomba g = s as Goomba;
                        if (isInvisible) g.die(true);
                        else
                            if (!HurtInvisible) startTranformation(1);
                        break;
                    }
                case "turtle":
                    {
                        Turtle t = s as Turtle;
                        if(t.state==Turtle.State.alive)
                        {
                            if (isInvisible) t.die();
                            else
                                if (!HurtInvisible) startTranformation(1);
                            break;
                        }
                        else
                        {
                            if(t.stoping)
                            {
                                if (positionRectangle.Left < s.positionRectangle.Left)
                                {
                                    positionRectangle.X = s.positionRectangle.X - positionRectangle.Width - 1;
                                    t.kick(true);
                                }
                                else
                                {
                                    positionRectangle.X = s.positionRectangle.X + s.positionRectangle.Width + 1;
                                    t.kick(false);
                                }

                            }
                            else
                            {
                                if (isInvisible) t.die();
                                else
                                    if (!HurtInvisible) startTranformation(1);
                                break;
                            }
                        }
                        break;
                    }
                case "pole":
                    {
                        Pole p = s as Pole;
                        p.slide();
                        pole = p;
                        state = State.SlidingDown;
                        break;
                    }
            }
        }
        public override void yCollision(Sprite s)
        {
            switch (s.name)
            {
                case "gps" :
                    {
                        if (positionRectangle.Bottom > s.positionRectangle.Top)
                        {
                            positionRectangle.Y = s.positionRectangle.Top - positionRectangle.Height;
                            yVelocity = 0;
                            if (state == State.SlidingDown)
                                state = State.Slided;
                            else if (state == State.FallFormBase)
                                state = State.WalkingtoCastle;
                            else
                                state = State.Walking;
                        }
                        break;
                    }
                case "brick":
                    {
                        if (positionRectangle.Top < s.positionRectangle.Top)
                        {
                            positionRectangle.Y = s.positionRectangle.Top - positionRectangle.Height;
                            yVelocity = 0;
                            state = State.Walking;
                        }
                        else
                        {
                            s.yCollision(this);
                            positionRectangle.Y = s.positionRectangle.Bottom;
                            state = State.Falling;
                            yVelocity = 1;
                        }
                        break;
                    }
                case "itemBlock":
                    {
                        if (positionRectangle.Top < s.positionRectangle.Top)
                        {
                            positionRectangle.Y = s.positionRectangle.Top - positionRectangle.Height;
                            yVelocity = 0;
                            state = State.Walking;
                        }
                        else
                        {
                            s.yCollision(this);
                            positionRectangle.Y = s.positionRectangle.Bottom;
                            state = State.Falling;
                            yVelocity = 1;
                        }
                        break;
                    }
                case "bigMushoom":
                    {

                        if (level == Level.Small) startTranformation();
                        s.canRemove = true;
                        break;
                    }
                case "flower":
                    {
                        if (level == Level.Big) startTranformation(2);
                        if (level == Level.Small) startTranformation();
                        s.canRemove = true;
                        break;
                    }
                case "star":
                    {
                        startTranformation(3);
                        s.canRemove = true;
                        break;
                    }
                case "goomba":
                    {
                        Goomba g = s as Goomba;
                        if (isInvisible) 
                        {
                            g.die(true);
                        }
                        if (positionRectangle.Top < s.positionRectangle.Top)
                        {
                                positionRectangle.Y = s.positionRectangle.Top - positionRectangle.Height;
                                yVelocity = -1;
                                g.die();
                                state = State.Walking;
                        }
                        else
                        {
                            if (!HurtInvisible) startTranformation(1);
                            break;
                        }
                        break;
                    }
                case "turtle":
                    {
                        Turtle t = s as Turtle;
                        if (isInvisible)
                        {
                            t.die();
                            break;
                        }
                        if (t.state == Turtle.State.alive)
                        {
                            if (positionRectangle.Top < s.positionRectangle.Top)
                            {
                                positionRectangle.Y = s.positionRectangle.Top - positionRectangle.Height;
                                yVelocity = -1;
                                t.toShell();
                                break;
                            }
                            else
                            {
                                if (!HurtInvisible) startTranformation(1);
                                break;
                            }
                        }
                        else
                        {
                            if (positionRectangle.Top < s.positionRectangle.Top)
                            {
                                t.stop();
                            }
                        }
                        break;
                    }
            }
        }
        public Mario(int x,int y)
        {
            name = "mario";
            positionRectangle = new Rectangle(x, y, 11, 16);
            init();
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            if (movingRight)
                spriteBatch.Draw(texture, positionRectangle, sourceRectangle, Color.White);
            else
                spriteBatch.Draw(texture, positionRectangle, sourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
        }
    }
}
