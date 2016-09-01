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
    class Brick:UnMovableSprite
    {
        Item item;
        public enum State { initial, hitSmall, breaking, hitItem, afterHit }
        public State state { get; set; }
        private FrameSelector initialF, hitSmallF, breakingF, hitItemF, afterHitF;
        public static Texture2D texture;
        private DateTime breakingTime;
        public static void loadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("sprites/BrickAndItemBox.png");
        }
        public Brick(int x,int y,Item i=null)
        {
            name = "brick";
            positionRectangle = new Rectangle(x, y, 16, 16);
            item = i;
            init();
        }
        public Rectangle getFrame()
        {
            float notthing = 0;
            switch (state)
            {
                case State.initial: return initialF.GetFrame(ref notthing);
                case State.hitSmall: return hitSmallF.GetFrame(ref notthing);
                case State.hitItem: return hitItemF.GetFrame(ref notthing);
                case State.breaking: return breakingF.GetFrame(ref notthing);
                case State.afterHit: return afterHitF.GetFrame(ref notthing);
                default: return new Rectangle();
            }
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, positionRectangle, getFrame(), Color.White);
        }
        public override void update(GameTime gameTime, List<Sprite> sprites)
        {
            //base.update(gameTime, sprites);
            if(state==State.breaking)
            {
                if ((DateTime.Now - breakingTime).TotalMilliseconds > Constants.brickBreakingTime) canRemove = true;
            }
        }
        public override void yCollision(Sprite sprite)
        {
            switch (sprite.name)
            {
                case "mario":
                    {
                        if (item == null)
                        {
                            if (sprite.positionRectangle.Top >= positionRectangle.Top)
                            {
                                state = State.breaking;
                                breakingTime = DateTime.Now;
                            }
                        }
                        else
                        {
                            if (sprite.positionRectangle.Top >= positionRectangle.Top)
                            {
                                state = State.afterHit;
                                item.showItem();
                            }
                        }
                        break;
                    }
                default: break;
            }
        }
        public void init()
        {
            Rectangle temp;
            List<Rectangle> ltemp;
            temp = new Rectangle(272, 112, 16, 16);
            ltemp = new List<Rectangle>();
            ltemp.Add(temp);
            initialF = new FrameSelector(0.1f, ltemp);
            temp = new Rectangle(288, 112, 16, 16);
            ltemp = new List<Rectangle>();
            ltemp.Add(temp);
            hitSmallF = new FrameSelector(0.1f, ltemp);
            temp = new Rectangle(304, 112, 16, 16);
            ltemp = new List<Rectangle>();
            ltemp.Add(temp);
            breakingF = new FrameSelector(0.1f, ltemp);
            temp = new Rectangle(320, 112, 16, 16);
            ltemp = new List<Rectangle>();
            ltemp.Add(temp);
            hitItemF = new FrameSelector(0.1f, ltemp);
            temp = new Rectangle(336, 112, 16, 16);
            ltemp = new List<Rectangle>();
            ltemp.Add(temp);
            afterHitF = new FrameSelector(0.1f, ltemp);

        }
    }
}
