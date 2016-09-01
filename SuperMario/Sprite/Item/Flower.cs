using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMario
{
    class Flower:Item
    {
        private static FrameSelector frame;
        float stateTimer;
        public Flower(int x, int y)
        {
            name = "flower";
            positionRectangle = new Rectangle(x, y, 16, 16);
            init();
        }
        public void init()
        {
            List<Rectangle> frames = new List<Rectangle>(), listTemp = new List<Rectangle>();
            frames.Add(new Rectangle(0, 32, 16, 16));
            frames.Add(new Rectangle(16, 32, 16, 16));
            frames.Add(new Rectangle(32, 32, 16, 16));
            frames.Add(new Rectangle(48, 32, 16, 16));
            frame = new FrameSelector(0.3f, frames);
        }
        public Rectangle getFrame()
        {
            stateTimer += 0.01f;
            return frame.GetFrame(ref stateTimer);
        }
        public override void update(GameTime gameTime, List<Sprite> sprites)
        {
        }
        public override void showItem()
        {
            hiden = false;
            canNotCollide = false;
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            if(!hiden) spriteBatch.Draw(texture, positionRectangle, getFrame(), Color.White);
        }
    }
}
