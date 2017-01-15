using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nseh.Utils
{
    public static class Constants
    {
        public static class Tags
        {
            public const string PLAYER = "Player";
            public const string ENEMY = "Enemy";
        }

        public static class Scenes
        {
            public const string SCENE_01 = "Game";
        }

        public static class GameManager
        {
            public const string ON_START_GAME = "FIGHT TO DEAD!";
        }

        public static class Input
        {
            public const string AXIS_HORIZONTAL = "Horizontal";
            public const string AXIS_VERTICAL = "Vertical";

            public const string BUTTON_RUN = "Run";
        }

        public static class Animations
        {
            public static class Combat
            {
                public const string CHARACTER_DEFENSE = "Defend";
                public const string CHARACTER_ATTACK_A = "Attack_A";
                public const string CHARACTER_ATTACK_B = "Attack_B";
                public const string CHARACTER_DEFINITIVE = "Definitive";
                public const string CHARACTER_HABILITY = "Hability";
                public const string CHARACTER_USEITEM = "UseItem";
                public const string CHARACTER_COMBO_AAA = "Combo_AAA";

                public const string CHARACTER_COMBO_AAA_01 = "Combo_AAA_01";
                public const string CHARACTER_COMBO_AAA_02 = "Combo_AAA_02";
                public const string CHARACTER_COMBO_AAA_03 = "Combo_AAA_03";

                public const string CHARACTER_COMBO_BB_01 = "Combo_BB_01";
                public const string CHARACTER_COMBO_BB_02 = "Combo_BB_02";

                public const string CHARACTER_DEAD = "Dead";
            }

            public static class Movement
            {
                public const string GROUNDED = "Grounded";
                public const string H = "H";
                public const string SPEED = "Speed";
                public const string JUMP = "Jump";
            }
        }
    }
}
