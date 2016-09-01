#region Version History (1.0)
// 03.07.11 ~ Created
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuperMario
{
    public class Camera
    {
        #region Fields

        public float zoom { get; set; } //zoom ratio
        public Matrix transform { get; set; } //transform matrix
        protected Vector2 position;     //camera's position
        public int leftRectrict { get; private set; }   //restrict to go left
        #endregion

        #region Properties

        #endregion

        #region Constructor

        public Camera()
        {
            zoom = 2.0f;        //zoom 2
            position = Vector2.Zero; //begin at begin of map
            leftRectrict = 0;   //restrict left at 0
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        #endregion

        #region Methods
        public void updateLookPosition(Rectangle mario)
        {
            position.X = mario.Left- Constants.viewWidth/(2*zoom) ;     //make sure mario in the center
            position.Y = 0; //camera only move horizontal
            if (position.X <= leftRectrict) position.X = leftRectrict;  //prevent camera from go left
            leftRectrict = (int)position.X; //update restrict
        }
        public void Update()
        {
            transform = Matrix.CreateTranslation(-position.X, -position.Y, 0) *     //translation matrix
                            Matrix.CreateScale(new Vector3(zoom, zoom, 1)); //scale matrix
        }

        #endregion
    }
}
