using System.Collections.Generic;

namespace CombatSystem.Player
{
    public static class UtilsCamera
    {
        public static IEnumerable<T> GetEnumerable<T>(ICombatBackFrontCamerasStructureRead<T> structure)
        {
            yield return structure.BackCameraType;
            yield return structure.FrontCameraType;
        }

        public static IEnumerable<T> GetEnumerable<T>(ICombatCharacterCamerasStructureRead<T> structure)
        {
            yield return structure.CharacterBackCameraType;
            yield return structure.CharacterFrontCameraType;
        }

        public static IEnumerable<T> GetEnumerable<T>(ICombatCharacterCamerasStructureRead<T> characterStructure,
            ICombatBackFrontCamerasStructureRead<T> backFrontStructure)
        {
            yield return characterStructure.CharacterBackCameraType;
            yield return characterStructure.CharacterFrontCameraType;
            yield return backFrontStructure.BackCameraType;
            yield return backFrontStructure.FrontCameraType;
        }

    }
}
