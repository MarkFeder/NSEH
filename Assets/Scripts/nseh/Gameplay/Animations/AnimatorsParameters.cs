using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nseh.Gameplay.Animations
{
    public static class AnimatorsParameters
    {

        public static class Ryu
        {
            public const string DEFEND = "DEFEND";
            public const string WALK = "WALK";
            public const string WALK_BACK = "WALK_BACK";
            public const string DUCK = "DUCK";

            public const string KICK = "KICK";
            public const string PUNCH = "PUNCH";
            public const string TAKE_HIT = "TAKE_HIT";
            public const string TAKE_HIT_DEFEND = "TAKE_HIT_DEFEND";
            public const string JUMP = "JUMP";
            public const string HADOKEN = "HADOKEN";
            public const string DEAD = "DEAD";
        }

        public static class Paladin
        {
            public const string IS_WALKING = "IsWalking";
            public const string IS_RUNNING = "IsRunning";
            public const string IS_IDLE = "IsIdle";

            public const string SPEED = "Speed";
            public const string JUMP_01 = "Jump03";
            public const string JUMP_02 = "Jump02";
        }
    }
}
