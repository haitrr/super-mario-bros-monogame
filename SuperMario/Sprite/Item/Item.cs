using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMario
{
    class Item:MovableSprite
    {
        protected static Texture2D texture;
        public Item()
        {
            hiden = true;
            canNotCollide = true;
        }
        public virtual void showItem()
        {

        }
        public static void loadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("sprites/items.png");
        }
    }
}
