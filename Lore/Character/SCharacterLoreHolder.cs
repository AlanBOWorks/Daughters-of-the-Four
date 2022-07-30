using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Lore.Character
{
    [CreateAssetMenu(fileName = "N" + AssetPrefix,
        menuName = "Lore/Character/LoreHolder")]
    public class SCharacterLoreHolder : ScriptableObject, ICharacterNameStructureRead
    {
        private const string NullName = "NULL";
        private const string AssetPrefix = "[Lore]";
        [Title("Names")]
        [SerializeField] private string entityName = NullName;
        [SerializeField] private string entityFullName = NullName;
        [SerializeField] private string shorterName = NullName;

        [Title("Images")]
        [SerializeField]
        private PortraitHolder portraitHolder = new PortraitHolder();

        public string CharacterNameType => entityName;
        public string CharacterFullNameType => entityFullName;
        public string CharacterShorterNameType => shorterName;

        public ICharacterPortraitHolder GetPortraitHolder() => portraitHolder;

        [Button]
        private void UpdateAssetName()
        {
            string assetName = entityName + " " + AssetPrefix;
            UtilsAssets.UpdateAssetNameWithID(this, assetName);
        }


        [Serializable]
        private sealed class PortraitHolder : ICharacterPortraitHolder
        {
            [SerializeField, PreviewField] private Sprite portraitImage;
            [SerializeField] private Vector2 selectCharacterPivotPoint;
            [SerializeField] private Vector2 faceIconPivotPoint;

            public Sprite GetCharacterPortraitImage() => portraitImage;

            public Vector2 SelectCharacterPivotPosition
            {
                get => selectCharacterPivotPoint;
                set => selectCharacterPivotPoint = value;
            }

            public Vector2 FacePivotPosition
            {
                get => faceIconPivotPoint;
                set => faceIconPivotPoint = value;
            }
        }
    }

    public interface ICharacterPortraitHolder
    {
        Sprite GetCharacterPortraitImage();
        Vector2 SelectCharacterPivotPosition { get; set; }
        Vector2 FacePivotPosition { get; set; }
    }
}
