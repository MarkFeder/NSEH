﻿namespace nseh.Utils
{
    public static partial class Constants
    {
        public static class Events
        {
            public static class Tar_Event
            {
                public const float EVENT_START = 5.0f;
                public const float EVENT_DURATION_MIN = 10f;
                public const float EVENT_DURATION_INCREASE = 5.0f;
                public const float EVENT_DURATION_MAX = 45.0f;
                public const float TAR_DAMAGE = 5.0f;
                public const float TAR_SLOWDOWN = 30.0f;
                public const float TAR_TICKDAMAGE = 1.0f;
            }

            public static class ItemSpawn_Event
            {
                public const float SPAWN_PERIOD = 3.0f;
                public const float SPAWNPOINT_INTERNALCD = 1.5f;
                public const string SPAWN_ALERT = "A WILD ITEM APPEARED!!";
            }
        }
    }
}
