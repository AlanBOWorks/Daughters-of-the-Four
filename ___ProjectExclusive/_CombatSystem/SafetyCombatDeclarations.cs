using Characters;
using UnityEngine;

namespace _CombatSystem
{
    public class SafetyBackupSkillsInjection : ICombatAfterPreparationListener
    {
        public void OnAfterPreparation(CombatingTeam playerEntities, CombatingTeam enemyEntities, CharacterArchetypesList<CombatingEntity> allEntities)
        {
            //TODO any necessary backUP
        }
    }
}
