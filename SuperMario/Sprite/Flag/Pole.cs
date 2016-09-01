using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMario
{
    class Pole:Item
    {
        Flag flag;
        public bool slided { get; set; }
        public bool sliding { get; set; }
        public Pole(Flag f,int y)
        {
            name = "pole";
            positionRectangle = new Rectangle(0, y, 2, 150);
            positionRectangle.X = f.positionRectangle.Right;
            flag=f;
            canNotCollide=false;
        }
        public override void update(GameTime gameTime, List<Sprite> sprites)
        {
            if (sliding) slide();
        }
        public void slide()
        {
            sliding = true;
            if (flag.positionRectangle.Bottom < positionRectangle.Bottom) flag.slideDown();
            else
            {
                sliding = false;
                slided = true;
            }

        }
    }
}
