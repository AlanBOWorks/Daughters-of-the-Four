using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSkills
{
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
