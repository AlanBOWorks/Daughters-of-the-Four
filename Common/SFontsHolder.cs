using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Common
{
    [CreateAssetMenu(fileName = "N [Fonts Holder]",
        menuName = "Editor/Visuals/Fonts Holder")]
    public class SFontsHolder : ScriptableObject, IFontsHolder
    {
        [SerializeField] private TMP_Asset mainFont;
        [SerializeField] private TMP_Asset secondaryFont;
        [SerializeField] private TMP_Asset numericalFont;

        public TMP_Asset GetMainFont() => mainFont;
        public TMP_Asset GetSecondaryImpactFont() => secondaryFont;
        public TMP_Asset GetNumericalFont() => numericalFont;
    }

    public interface IFontsHolder
    {
        TMP_Asset GetMainFont();
        TMP_Asset GetSecondaryImpactFont();
        TMP_Asset GetNumericalFont();
    }
}
