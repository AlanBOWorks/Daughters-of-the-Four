using System;
using UnityEngine;

namespace CombatTeam
{
    [Serializable]
    public class RoleArchetypeStructure<T> : ITeamRoleStructure<T>
    {
        [SerializeField]
        private T vanguard;

        [SerializeField]
        private T attacker;
        [SerializeField]
        private T support;

        public T Vanguard
        {
            get => vanguard;
            set => vanguard = value;
        }

        public T Attacker
        {
            get => attacker;
            set => attacker = value;
        }

        public T Support
        {
            get => support;
            set => support = value;
        }
    }

    [Serializable]
    public class RoleArchetypeObjectStructure<T> : ITeamRoleStructure<T> where T : new()
    {
        [SerializeField]
        private T vanguard = new T();
        [SerializeField]
        private T attacker = new T();
        [SerializeField]
        private T support = new T();

        public T Vanguard
        {
            get => vanguard;
            set => vanguard = value;
        }

        public T Attacker
        {
            get => attacker;
            set => attacker = value;
        }

        public T Support
        {
            get => support;
            set => support = value;
        }
    }
}
