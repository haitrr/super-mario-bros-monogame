using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuperMario
{
    class MovableSprite:UnMovableSprite
    {
        protected float xVelocity;
        protected float yVelocity;
        protected float xAcceleration;
        protected float yAcceleration;
        public MovableSprite()
        {

        }
    }
}
