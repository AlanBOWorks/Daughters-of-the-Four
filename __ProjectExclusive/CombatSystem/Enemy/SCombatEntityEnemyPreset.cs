using System.Collections.Generic;
using CombatEntity;
using CombatSkills;
using CombatTeam;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace __ProjectExclusive.Enemy
{
    [CreateAssetMenu(fileName = "Role - N [Enemy Preset]", 
        menuName = "Combat/Entity/Enemy Preset")]
    public class SCombatEntityEnemyPreset : SCombatEntityPreset, ICombatEntityInfo, ICombatEntityProvider
    {

        [Button, GUIColor(.7f,.8f,1)]
        private void UpdateAssetName()
        {
            UpdateAssetName(entityName);
        }

        public CombatStatsHolder GenerateStatsHolder() => new CombatStatsHolder(baseStats);
        public AreaData GenerateAreaData() => areaData;


        public ITeamStanceStructureRead<ICollection<SkillProviderParams>> ProvideStanceSkills()
            => this;
    }
}
