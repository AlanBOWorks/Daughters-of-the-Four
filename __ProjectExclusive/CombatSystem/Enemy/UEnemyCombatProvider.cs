using __ProjectExclusive.Player;
using CombatEntity;
using CombatSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace __ProjectExclusive.Enemy
{
    public class UEnemyCombatProvider : MonoBehaviour, IEnemyCombatProvider
    {
        [SerializeField] private SCombatEntityEnemyPreset vanguard;
        [SerializeField] private SCombatEntityEnemyPreset attacker;
        [SerializeField] private SCombatEntityEnemyPreset support;

        public ICombatEntityProvider Vanguard => vanguard;

        public ICombatEntityProvider Attacker => attacker;

        public ICombatEntityProvider Support => support;

        [Button]
        public void InvokeCombatWithThisTeam()
            => InvokeCombatFromProvider(this);

        internal static void InvokeCombatFromProvider(IEnemyCombatProvider provider)
        {
            PlayerCharactersHolder playerTeam = PlayerCombatSingleton.CharactersHolder;
            CombatSystemSingleton.CombatPreparationHandler.StartCombat(playerTeam,provider);
        }
    }
}
