using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Enemy
{
    public sealed class EnemyCombatSingleton
    {
        private static readonly EnemyCombatSingleton Instance = new EnemyCombatSingleton();

        static EnemyCombatSingleton()
        {
        }

        private EnemyCombatSingleton()
        {

        }

        public static EnemyCombatSingleton GetInstance() => Instance;


        [ShowInInspector]
        public static readonly IEntitySkillRequestHandler EntitySkillRequestHandler;
    }
}
