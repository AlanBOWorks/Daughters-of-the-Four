using UnityEngine;

namespace Common
{

    public interface IThemeHolder
    {
        string GetThemeName();
        Sprite GetThemeIcon();
        Color GetThemeColor();
    }
}
