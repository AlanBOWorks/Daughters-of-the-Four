using System;
using CombatSkills;
using UnityEngine;

namespace CombatTeam
{
    [Serializable]
    public class RoleArchetypeStructure<T> : ITeamRoleStructure<T>, ISkillTypes<T>, ITeamStanceStructure<T>
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


        public T SelfType
        {
            get => vanguard;
            set => vanguard = value;
        }
        public T OffensiveType
        {
            get => attacker;
            set => attacker = value;
        }
        public T SupportType
        {
            get => support;
            set => support = value;
        }


        public T OnAttackStance
        {
            get => vanguard;
            set => vanguard = value;
        }
        public T OnNeutralStance
        {
            get => attacker;
            set => attacker = value;
        }
        public T OnDefenseStance
        {
            get => support;
            set => support = value;
        }
    }

    [Serializable]
    public class SerializableRoleArchetypeStructure<T> : ITeamRoleStructure<T> where T : new()
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
