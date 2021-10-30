using __ProjectExclusive.Enemy;
using CombatEntity;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatTeam
{
    [CreateAssetMenu(fileName = "TEAM - N [Preset]",
        menuName = "Combat/Entity/Enemy Team Preset", order = -100)]
    public class SEnemyTeamProviderPreset : ScriptableObject, ITeamProvider
    {
        [SerializeField] private string teamName = "NULL";

        [SerializeField] private SCombatEntityEnemyPreset vanguard;
        [SerializeField] private SCombatEntityEnemyPreset attacker;
        [SerializeField] private SCombatEntityEnemyPreset support;

        public ICombatEntityProvider Vanguard => vanguard;
        public ICombatEntityProvider Attacker => attacker;
        public ICombatEntityProvider Support => support;

        [Button]
        private void UpdateAssetName()
        {
            name = "TEAM - " +teamName + " [Preset] _ " + GetInstanceID();
            UtilsAssets.UpdateAssetName(this);
        }
    }
}
