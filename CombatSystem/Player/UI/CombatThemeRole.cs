using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace CombatSystem.Player.UI
{

    [Serializable]
    public sealed class CombatThemeHolder
    {
        [FormerlySerializedAs("roleName")] 
        [SerializeField] private string themeName;
        [FormerlySerializedAs("roleIcon")] 
        [SerializeField, PreviewField, GUIColor(.3f, .3f, .3f)] private Sprite themeIcon;
        [FormerlySerializedAs("roleColor")] [SerializeField] private Color themeColor;

        public string GetThemeName() => themeName;
        public Sprite GetThemeIcon() => themeIcon;
        public Color GetThemeColor() => themeColor;
    }
}
