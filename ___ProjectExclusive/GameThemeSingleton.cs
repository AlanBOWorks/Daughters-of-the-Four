using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ___ProjectExclusive
{
    public sealed class GameThemeSingleton
    {
        static GameThemeSingleton() { }

        private GameThemeSingleton()
        {
            _entity = new GameThemeEntity();
        }
        public static GameThemeSingleton Instance { get; } = new GameThemeSingleton();

        private readonly GameThemeEntity _entity;
        public GameThemeEntity Entity => _entity;

        public void Injection(SGameThemeSingleton variable)
        {
            _entity.Variable = variable;
        }

    }

    [Serializable]
    public class GameThemeEntity
    {
        [SerializeReference] 
        public SGameThemeSingleton Variable = null;
    }




    [Serializable]
    public struct CharacterColors
    {
        public Color MainColor;
        public Color Neutral;
        public Color Text;
    }

    public static class UtilsGameTheme
    {
        public const float KThousand = 1000;
        public const float KDivisor = 1 / KThousand;
        public static string GetNumericalPrint(float value)
        {
            string generated;

            if (value < 1000)
            {
                generated = $"{value:0000}";
            }
            else
            {
                generated = $"{value * KDivisor:000}K";
            }

            return generated;
        }
    }
}
