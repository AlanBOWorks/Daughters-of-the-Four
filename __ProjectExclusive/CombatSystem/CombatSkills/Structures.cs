using System;
using CombatTeam;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSkills
{
    [Serializable]
    public class SkillTypeStructure<T> : ISkillTypes<T>, ITeamRoleStructure<T>, ITeamStanceStructure<T>
    {
        [SerializeField]
        private T selfType;
        [SerializeField]
        private T offensiveType;
        [SerializeField]
        private T supportType;

        public T SelfType
        {
            get => selfType;
            set => selfType = value;
        }
        public T OffensiveType
        {
            get => offensiveType;
            set => offensiveType = value;
        }
        public T SupportType
        {
            get => supportType;
            set => supportType = value;
        }


        public T Vanguard
        {
            get => selfType;
            set => selfType = value;
        }
        public T Attacker
        {
            get => offensiveType;
            set => offensiveType = value;
        }
        public T Support
        {
            get => supportType;
            set => supportType = value;
        }


        public T OnAttackStance
        {
            get => selfType;
            set => selfType = value;
        }
        public T OnNeutralStance
        {
            get => offensiveType;
            set => offensiveType = value;
        }
        public T OnDefenseStance
        {
            get => selfType;
            set => selfType = value;
        }
    }

    [Serializable]
    public class SerializableSkillTypeStructure<T> : ISkillTypesRead<T> where T : new()
    {
        [TabGroup("Self"),ShowInInspector]
        public T SelfType { get; } = new T();
        [TabGroup("Offensive"), ShowInInspector]
        public T OffensiveType { get; } = new T();
        [TabGroup("Support"), ShowInInspector]
        public T SupportType { get; } = new T();
    }
}
