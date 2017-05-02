namespace nseh.Utils
{
    public static partial class Constants
    {
        public static class Logs
        {
            public static class PlayerHealth
            {
                public const string INCREASE_HEALTH = "Health of {0} is: {1} and applying {2}% more has changed to: {3}";
                public const string DECREASE_HEALTH = "Health of {0} is: {1} and reducing {2}% has changed to: {3}";

                public const string INVULNERABILITY_MODE_ACTIVATED = "Character {0} is entering Invulnerability mode";
                public const string INVULNERABILITY_MODE_DEACTIVATED = "Character {0} is exiting Invulnerability mode";

                public const string BONIFICATION_DEFENSE_ACTIVATED = "Character {0} has received an extra bonus defense: {1}%";
                public const string BONIFICATION_DEFENSE_DEACTIVATED = "Character {0} has received an extra bonus defense: {1}%";
            }
        }
    }
}
