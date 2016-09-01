using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SuperMario
{
    class FrameSelector
    {
        //animation frame selector
        public List<Rectangle> Frames;
        private float Fps;
        public int curFrameID;
        private bool LoopBack;

        public FrameSelector(float fps, List<Rectangle> frames, bool loop = true)
        {
            this.Fps = fps;
            this.Frames = frames;

            this.curFrameID = 0;
            this.LoopBack = loop;
        }

        public Rectangle GetFrame(ref float dt)
        {
            if (dt > Fps)
            {
                dt = 0;

                //increase to next frame to be ready
                curFrameID++;
                if (curFrameID >= Frames.Count && LoopBack)
                    curFrameID = 0; //reset if exceed animation                
            }

            //create source rectangle to draw
            return Frames[curFrameID];
        }
    }
}
