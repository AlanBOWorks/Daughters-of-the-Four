using System;
using __ProjectExclusive.Player;
using CombatEntity;
using CombatSystem;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace __ProjectExclusive.Enemy
{
    public class UEnemyCombatProvider : MonoBehaviour
    {
        [SerializeField]
        private EnemyCombatProvider enemyCombatProvider = new EnemyCombatProvider();
        public IEnemyCombatProvider Provider => enemyCombatProvider;

        [Button]
        public void InvokeCombatWithThisTeam()
            => enemyCombatProvider.InvokeCombatWithThisTeam();
    }


    [Serializable]
    public class EnemyCombatProvider : IEnemyCombatProvider
    {
        [SerializeField] private SCombatEntityEnemyPreset vanguard;
        [SerializeField] private SCombatEntityEnemyPreset attacker;
        [SerializeField] private SCombatEntityEnemyPreset support;
        [SerializeField] private bool isElite;
        [SerializeField] private SceneAsset combatScene;

        public ICombatEntityProvider Vanguard => vanguard;
        public ICombatEntityProvider Attacker => attacker;
        public ICombatEntityProvider Support => support;

        public string GetCombatScenePath() => AssetDatabase.GetAssetPath(combatScene);

        public bool IsEliteCombat() => isElite;

        public void InvokeCombatWithThisTeam()
            => InvokeCombatFromProvider(this);

        internal static void InvokeCombatFromProvider(IEnemyCombatProvider provider)
        {
            PlayerCharactersHolder playerTeam = PlayerCombatSingleton.CharactersHolder;
            CombatSystemSingleton.CombatPreparationHandler.StartCombat(provider.GetCombatScenePath(),
                playerTeam, provider, provider.IsEliteCombat());
        }
    }
}
