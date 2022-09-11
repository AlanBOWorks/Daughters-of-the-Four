namespace Utils_Project
{
    public static class AssetPaths
    { 
        // ROOT
        public const string ScriptableObjectsFolderRootPath =
            "Assets/ScriptableObjects/";

        // COMBAT
        public const string ScriptablesCombatFolderPath = ScriptableObjectsFolderRootPath + "Combat/";

        public const string ScriptablesPlayerTeams = ScriptablesCombatFolderPath + "_Player/Teams/";

        // LEVELS
        public const string ScenesScriptableObjectFolderPath = ScriptableObjectsFolderRootPath +
                                                               "Scenes Data/";
        public const string ExplorationScenesScriptablesFolderPath = ScenesScriptableObjectFolderPath + "Levels/";

        // SCENES
        public const string InGameScenesFolder = "Assets/Scenes/InGame/";
        public const string InGameScenesLevelsFolder = InGameScenesFolder + "Levels/";
        public const string InGameScenesCombatFolder = InGameScenesLevelsFolder + "Combat/";
    }
}
