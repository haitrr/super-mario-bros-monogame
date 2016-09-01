using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMario
{
    public static class Constants
    {
        public static int viewWidth=512;
        public static int viewHeight=450;
        public static float max_walk_velocity = 2.5f;
        public static float max_run_velocity = 4;
        public static float jump_velocity = -3;
        public static float fall_velocity = 0.15f;
        public static float max_y_velocity = 5;
        public static int timeBetweenToFireBall = 200;
        public static int brickBreakingTime = 500;
        public static float walk_accelerate = 0.075f;
        public static float run_accelerate = 0.1f;
    }
}
