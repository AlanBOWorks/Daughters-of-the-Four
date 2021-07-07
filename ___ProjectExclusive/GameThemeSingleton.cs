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


}
