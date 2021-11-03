using CombatSystem.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Enemy
{
    public sealed class EnemyCombatSingleton
    {
        private static readonly EnemyCombatSingleton Instance = new EnemyCombatSingleton();

        static EnemyCombatSingleton()
        {
            EventsHolder = new EnemyEvents();
        }

        private EnemyCombatSingleton()
        {

        }

        public static EnemyCombatSingleton GetInstance() => Instance;

        [ShowInInspector] 
        public static readonly EnemyEvents EventsHolder;
        [ShowInInspector]
        public static readonly IEntitySkillRequestHandler EntitySkillRequestHandler;
    }
}
