using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMario
{
    class Enemy:MovableSprite
    {
        protected static Texture2D texture;
        public static void loadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("sprites/enemies.png");
        }
        public Enemy()
        {
            //hiden = true;
        }
    }
}
