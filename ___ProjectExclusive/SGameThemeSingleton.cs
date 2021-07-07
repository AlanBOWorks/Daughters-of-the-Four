﻿using Sirenix.OdinInspector;
using UnityEngine;

namespace ___ProjectExclusive
{
    [CreateAssetMenu(fileName = "Game Theme [Singleton]",
        menuName = "Singleton/Game Theme")]
    public class SGameThemeSingleton : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField] private GameThemeEntity _singleton = GameThemeSingleton.Instance.Entity;
#endif


        [Button]
        public void InjectInSingleton()
        {
            GameThemeSingleton.Instance.Injection(this);
        }
    }
}
