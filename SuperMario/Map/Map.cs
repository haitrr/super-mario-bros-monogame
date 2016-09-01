using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace SuperMario
{
    class Map
    {
        protected Texture2D background;
        protected Rectangle mapRectangle;
        protected List<Sprite> sprites;
        protected Mario mario;
        protected List<Enemy> enemies;
        protected int end;
        protected CastleFlag castleFlag;
        public virtual void loadContent(ContentManager content)
        {

        }
        public virtual void update(Camera camera,GameTime gameTime)
        {
            camera.updateLookPosition(mario.positionRectangle);
            if (mario.level == Mario.Level.Fire && Keyboard.GetState().IsKeyDown(Keys.G)) addFireBall(gameTime);
            foreach(Sprite s in sprites)
            {
                s.update(gameTime, sprites);
            }
            if (mario.positionRectangle.X < camera.leftRectrict) mario.positionRectangle.X = camera.leftRectrict;
            if (mario.positionRectangle.X > end)
            {
                if (sprites.Contains(mario))
                {
                    castleFlag.Rise();
                    mario.canRemove = true;
                }
            }
            removeNoLongerNeededSprite();
            activeEnemies(camera);
        }
        public virtual void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, mapRectangle, Color.White);
            foreach (Sprite s in sprites)
            {
                s.draw(spriteBatch);
            }
        }
        public void addFireBall(GameTime gameTime)
        {
            FireBall newFireBall ;
            if(mario.movingRight) newFireBall = new FireBall(mario.positionRectangle.Right + 10, mario.positionRectangle.Center.Y, gameTime,true);
            else newFireBall = new FireBall(mario.positionRectangle.Left - 10, mario.positionRectangle.Center.Y, gameTime,false);
            sprites.Add(newFireBall);
        }
        public void removeNoLongerNeededSprite()
        {
            List<Sprite> toRemove = new List<Sprite>();
            Rectangle allowRectangle = new Rectangle(mapRectangle.X, mapRectangle.Y - 100, mapRectangle.Width, mapRectangle.Height + 200);
            foreach (Sprite s in sprites)
            {
                if (s.canRemove) toRemove.Add(s);
                if (Rectangle.Intersect(s.positionRectangle, allowRectangle) == Rectangle.Empty)
                {
                    toRemove.Add(s);
                }
            }
            foreach (Sprite s in toRemove)
            {
                sprites.Remove(s);
                Enemy e = s as Enemy;
                if(e!=null) enemies.Remove(e);
            }
        }
        public void activeEnemies(Camera camera)
        {
            foreach(Enemy e in enemies)
            {
                if(!sprites.Contains(e))
                {
                    if (e.positionRectangle.X - mario.positionRectangle.X < Constants.viewWidth/camera.zoom * 1.1)
                        sprites.Add(e);
                }
            }
        }
    }
}
