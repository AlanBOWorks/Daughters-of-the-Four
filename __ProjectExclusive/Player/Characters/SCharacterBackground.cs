using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace __ProjectExclusive.Player
{
    [CreateAssetMenu(fileName = "N [Character BACKGROUND]",
        menuName = "Player/Characters/Background")]
    public class SCharacterBackground : ScriptableObject
    {
        [SerializeField]
        private CharacterBackground data = new CharacterBackground();

        public CharacterBackground GetData() => data;
        public string GetCharacterName() => data.characterName;


        [Button]
        private void UpdateAssetName()
        {
            string assetName = GetCharacterName();
            assetName += " [Character BACKGROUND]";
            UtilsAssets.UpdateAssetName(this,assetName);
        }
    }

    [Serializable]
    public sealed class CharacterBackground
    {
        public string characterName = "NULL";
    }
}
