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

    [Serializable]
    public class SerializableNestedTeamData<T> : ITeamsData<ICharacterArchetypesData<T>>
    {
        [SerializeField] private TeamElements playerData = new TeamElements();
        [SerializeField] private TeamElements enemyData = new TeamElements();
        
        public ICharacterArchetypesData<T> PlayerData => playerData;
        public ICharacterArchetypesData<T> EnemyData => enemyData;

        [Serializable]
        private class TeamElements : CharacterArchetypes<T>
        { }

        public void InjectInDictionary<TKey>(Dictionary<TKey, T> dictionary,
            ITeamsData<ICharacterArchetypesData<TKey>> keys)
            => UtilsTeam.InjectInDictionary(dictionary, keys, this);
    }

}
