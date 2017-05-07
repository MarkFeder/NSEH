namespace nseh.Utils
{
    public static partial class Constants
    {
        public static class Resources
        {
            // PREFABS

            private const string ROOT_PREFABS = "Prefabs/";

            public const string PREFABS_ACTORS = ROOT_PREFABS + "Actors/";
            public const string PREFABS_CHARACTERS = PREFABS_ACTORS + "Characters/";
            public const string PREFABS_NPCs = PREFABS_ACTORS + "NPCs/";

            public const string PREFABS_ENVIRONMENTS = ROOT_PREFABS + "Environments/";
            public const string PREFABS_ITEMS = ROOT_PREFABS + "Prefabs/";

            // CAMERAS

            private const string ROOT_CAMERAS = "Cameras/";

            public const string CAMERAS_LEVEL_CAMERA = ROOT_CAMERAS + "LevelCamera";
            public const string CAMERAS_CHARACTER_CAMERA = ROOT_CAMERAS + "CharacterCamera";
            public const string CAMERAS_TEXT_CAMERA = ROOT_CAMERAS + "TextCamera";

            // CHARACTERS

            private const string ROOT_CHARACTERS = "Characters/";

            public const string CHARACTERS_WRARR = ROOT_CHARACTERS + "Wrarr/";
        }
    }
}
