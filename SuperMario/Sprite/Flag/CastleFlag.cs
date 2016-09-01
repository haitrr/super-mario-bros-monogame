using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMario
{
    class CastleFlag:Item
    {
        public bool Rising;
        int maxY = 0;
        public CastleFlag(int x,int y)
        {
            sourceRectangle = new Rectangle(129, 2, 14, 14);
            Rising = false;
            hiden = true;
            positionRectangle = new Rectangle(x, y, sourceRectangle.Width, sourceRectangle.Height);
            maxY = y - 12;
        }
        public void Rise()
        {
            Rising = true;
            hiden = false;
        }
        public override void update(GameTime gameTime, List<Sprite> sprites)
        {
            if (Rising)
            {
                positionRectangle.Y -= 1;
                if (positionRectangle.Y <=  maxY)
                    Rising = false;
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            if(!hiden) spriteBatch.Draw(texture, positionRectangle, sourceRectangle, Color.White);
        }
    }
}
