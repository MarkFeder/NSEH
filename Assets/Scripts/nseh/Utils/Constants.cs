namespace nseh.Utils
{
    public static class Constants
    {
        public static class Resources
        {
            private const string PREFABS = "Prefabs/";

            public const string PREFABS_ACTORS = PREFABS + "Actors/";
            public const string PREFABS_CHARACTERS = PREFABS_ACTORS + "Characters/";
            public const string PREFABS_NPCs = PREFABS_ACTORS + "NPCs/";

            public const string PREFABS_ENVIRONMENTS = PREFABS + "Environments/";
            public const string PREFABS_ITEMS = PREFABS + "Prefabs/";
        }

        public static class Colors
        {
            public const string TEAL = "#008080ff";
            public const string OLIVE = "#808000ff";
            public const string BROWN = "#a52a2aff";
            public const string FUCHSIA = "#ff00ffff";
        }

        public static class Tags
        {
            public const string PLAYER = "Player";
            public const string ENEMY = "Enemy";
            public const string WEAPON = "Weapon";
            public const string CHARACTER = "Character";
            public const string BOSS = "Boss";
            public const string ONE_WAY_PLATFORM = "OneWayPlatform";
        }

        public static class Scenes
        {
            public const string SCENE_01 = "Game";
            public const string SCENE_MAIN_MENU = "MainMenu";
            public const string PLATFORM = "OneWayPlatform";
        }

        public static class GameManager
        {
            public const string ON_START_GAME = "FIGHT TO DEAD!";
        }

        public static class LevelManager
        {
            public const float TIME_REMAINING = 45F;
        }

        public static class Events
        {
            public static class Tar_Event
            {
                public const float EVENT_START = 5.0f;
                public const float EVENT_DURATION_MIN = 10f;
                public const float EVENT_DURATION_INCREASE = 5.0f;
                public const float EVENT_DURATION_MAX = 45.0f;
            }

            public static class ItemSpawn_Event
            {
                public const float SPAWN_PERIOD = 3.0f;
                public const float SPAWNPOINT_INTERNALCD = 1.5f;
                public const string SPAWN_ALERT = "A WILD ITEM APPEARED!!";
            }
        }

        public static class Items
        {
            public static class StandardItems
            {
                public const string HEALTH = "HEAL ME PLS!";
                public const string DAMAGE = "Sharp Pointy Things";
                public const string JUMP = "Fly! Or try it at least…";
                public const string SPEED = "Run, Forest, run!";
                public const string DEFENSE = "Harder skin!";
            }

            public static class SpecialItems
            {
                public const string INVULNERABILITY = "INSERT STAR MUSIC HERE";
                public const string AUTOATTACKS = "Why do you punch yourself?";
                public const string CRITICAL = "This hurts you way more than it hurts me.";
                public const string ULTIMATE = "NEW TOY!";
            }

            public static class DisadvantageItems
            {
                public const string BOMBCHEST = "BOOOOOM!";
                public const string POISONCLOUD = "Why did it have to be snakes?!";
                public const string CONFUSION = "Dyslexia topion";
            }
        }

        public static class Layers
        {
            public const string PLATFORM = "Platform";
            public const string PLAYER = "Player";
            public const string JUMP_LAYER = "PassesPlatform";
        }

        public static class Input
        {
            // Movement
            public const string AXIS_HORIZONTAL_GAMEPAD = "Horizontal_";
            public const string AXIS_VERTICAL_GAMEPAD = "Vertical_";

            public const string AXIS_HORIZONTAL_KEYBOARD = "Horizontal";
            public const string AXIS_VERTICAL_KEYBOARD = "Vertical";

            public const string JUMP = "Jump_";

            // Attacks
            public const string A = "A_";
            public const string B = "B_";
            public const string HABILITY = "Hability_";
            public const string DEFINITIVE = "Definitive_";

            public const string BUTTON_RUN = "Run";

            // Defense
            public const string DEFENSE = "Defense_";
        }

        public static class Animations
        {
            public static class Combat
            {
                public const string CHARACTER_DEFENSE = "Defend";
                public const string CHARACTER_ATTACK_A = "Attack_A";
                public const string CHARACTER_ATTACK_B = "Attack_B";

                public const string CHARACTER_COMBO_AAA_01 = "Combo_AAA_01";
                public const string CHARACTER_COMBO_AAA_02 = "Combo_AAA_02";
                public const string CHARACTER_COMBO_AAA_03 = "Combo_AAA_03";

                public const string CHARACTER_COMBO_BB_01 = "Combo_BB_01";
                public const string CHARACTER_COMBO_BB_02 = "Combo_BB_02";

                public const string CHARACTER_COMBO_B_SHARP = "Combo_B#";

                public const string CHARACTER_HABILITY = "Hability";
                public const string CHARACTER_DEFINITIVE = "Definitive";

                public const string CHARACTER_USEITEM = "UseItem";
                public const string CHARACTER_IMPACT = "Impact";

                public const string CHARACTER_DEAD = "Dead";
            }

            public static class Movement
            {
                public const string GROUNDED = "Grounded";
                public const string H = "H";
                public const string SPEED = "Speed";
                public const string LOCOMOTION = "Locomotion";
                public const string IDLE = "Idle";

                public const string LOCOMOTION_JUMP = "LocomotionJump";
                public const string IDLE_JUMP = "IdleJump";
            }
        }
    }
}
