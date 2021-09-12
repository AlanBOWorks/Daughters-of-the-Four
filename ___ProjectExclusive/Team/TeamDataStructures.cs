using System;
using System.Collections.Generic;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Team
{
    public class TeamData<T> : ITeamsData<T>
    {
        [ShowInInspector]
        public T PlayerData { get; protected set; }
        [ShowInInspector]
        public T EnemyData { get; protected set; }
    }

    [Serializable]
    public class SerializableTeamData<T> : ITeamsData<T>
    {
        [SerializeField] private T playerData;
        [SerializeField] private T enemyData;

        public T PlayerData => playerData;
        public T EnemyData => enemyData;
    }

    public abstract class NestedTeamData<T> : ITeamDataFull<T>
    {
        protected NestedTeamData()
        {
            _playerData = new TeamElements();
            _enemyData = new TeamElements();
            UtilsTeam.DoGenerationInjection(this);
        }

        public abstract T GenerateElement();

        [ShowInInspector]
        private readonly TeamElements _playerData;
        [ShowInInspector]
        private readonly TeamElements _enemyData;

        public ICharacterArchetypes<T> PlayerData => _playerData;
        public ICharacterArchetypes<T> EnemyData => _enemyData;

        [Serializable]
        private class TeamElements : CharacterArchetypes<T>
        { }

        public void InjectInDictionary<TKey>(Dictionary<TKey, T> dictionary,
            ITeamsData<ICharacterArchetypesData<TKey>> keys)
            => UtilsTeam.InjectInDictionary(dictionary, keys, this);
    }
}
