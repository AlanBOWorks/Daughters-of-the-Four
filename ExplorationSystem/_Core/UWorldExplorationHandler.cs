using System;
using UnityEngine;

namespace ExplorationSystem._Core
{
    public class UWorldExplorationHandler : MonoBehaviour
    {
        [SerializeField] private UWorldExplorationDataHolder dataHolder;

        private SExplorationWorldLevelsHolder.LevelGroupValues[] _world;
        private SExplorationSceneDataHolder[] _currentLevels;

        private int _currentLevelIndex;

        private void Awake()
        {
            var worldHolder = dataHolder.GetWorldDataHolder();
            _world = worldHolder.GetWorld();
        }

    }
}
