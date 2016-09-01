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
    class BigMushoom:Item
    {
        public BigMushoom(int x, int y)
        {
            name = "bigMushoom";
            positionRectangle = new Rectangle(x, y, 16, 16);
            sourceRectangle = new Rectangle(0, 0, 16, 16);
            xVelocity=0;
            yVelocity=0;
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
            }
        }
        public override void xCollision(Sprite s)
        {
            switch (s.name)
            {
                case "gps":
                case "brick":
                case "itemBlock": xVelocity = -xVelocity; break;
            }
        }
        public override void yCollision(Sprite s)
        {
            switch (s.name)
            {
                case "gps":
                case "brick":
                case "itemBlock": positionRectangle.Y = s.positionRectangle.Top - positionRectangle.Height; yVelocity = 0; break;
            }
        }
        public override void showItem()
        {
            if (hiden)
            {
                hiden = false;
                canNotCollide = false;
                xVelocity = 1;
            }
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            if(!hiden) spriteBatch.Draw(texture, positionRectangle, sourceRectangle, Color.White);
        }
    }
}
