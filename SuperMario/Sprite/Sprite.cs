using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SuperMario
{
    class Sprite
    {
        public bool hiden { get; set; }
        public bool canNotCollide { get; set; }
        public string name;
        public bool canRemove;
        public Rectangle positionRectangle;
        public Sprite()
        {

        }
        public virtual void update(GameTime gameTime,List<Sprite> sprite)
        {

        }
        public Sprite(Rectangle postion)
        {
            positionRectangle = postion;
            name = "gps";
        }

        public virtual void draw(SpriteBatch spriteBatch)
        {
        }
        public virtual Sprite checkCollision(List<Sprite> sprites)
        {
            foreach (Sprite s in sprites)
            {
                if (this == s) continue;
                if (s.canNotCollide || canNotCollide) continue;
                if (positionRectangle.Intersects(s.positionRectangle))
                {
                    return s;
                }
            }
            return null;
        }
        public virtual void xCollision(Sprite s)
        {
        }
        public virtual void yCollision(Sprite s)
        {

        }
    }
}
