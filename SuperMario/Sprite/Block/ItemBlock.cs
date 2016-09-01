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
    class ItemBlock:UnMovableSprite
    {
        private static Texture2D texture;
        public enum State { initial, hitItem, afterHit }
        public State state { get; set; }
        private FrameSelector initialF, hitItemF, afterHitF;
        private float stateTimer;
        public Item item { get; set; }
        public void init()
        {
            Rectangle temp;
            List<Rectangle> listInitalF = new List<Rectangle>(), listTemp = new List<Rectangle>();
            listInitalF.Add(new Rectangle(80, 112, 16, 16));
            listInitalF.Add(new Rectangle(96, 112, 16, 16));
            listInitalF.Add(new Rectangle(112, 112, 16, 16));
            initialF = new FrameSelector(0.3f, listInitalF);
            temp = new Rectangle(128, 112, 16, 16);
            listTemp.Add(temp);
            hitItemF = new FrameSelector(0.1f, listTemp);
            listTemp = new List<Rectangle>();
            temp = new Rectangle(144, 112, 16, 16);
            listTemp.Add(temp);
            afterHitF = new FrameSelector(0.1f, listTemp);
        }
        public Rectangle getFrame()
        {
            float notthing = 0;
            switch (state)
            {
                case State.initial:
                    stateTimer += 0.01f;
                    return initialF.GetFrame(ref stateTimer);
                case State.hitItem: return hitItemF.GetFrame(ref notthing);
                case State.afterHit: return afterHitF.GetFrame(ref notthing);
                default: return new Rectangle();
            }
        }
        public static void loadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("sprites/BrickAndItemBox.png");
        }
        public ItemBlock(int x,int y,Item i=null)
        {
            name = "itemBlock";
            positionRectangle = new Rectangle(x, y, 16, 16);
            state = State.initial;
            stateTimer = 0;
            init();
            item = i;
        }
        public override void yCollision(Sprite sprite)
        {
            switch (sprite.name)
            {
                case "mario":
                    {
                        if (sprite.positionRectangle.Top >= positionRectangle.Top)
                        {
                            if (item != null) item.showItem();
                            state = State.afterHit;
                        }
                        break;
                    }
                default: break;
            }
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, positionRectangle, getFrame(), Color.White);
        }
    }
}
