using Characters;
using UnityEngine;

namespace _CombatSystem
{
    public class SafetyBackupSkillsInjection : ICombatAfterPreparationListener
    {
        public void OnAfterPreparation(CombatingTeam playerEntities, CombatingTeam enemyEntities, CharacterArchetypesList<CombatingEntity> allEntities)
        {
            foreach (CombatingEntity entity in allEntities)
            {
                AddBackupIfNull(entity);
            }
            void AddBackupIfNull(CombatingEntity entity)
            {
                if(entity.SharedSkills != null) return;
                entity.DoBackupSharedSkillsInjection();
            }
        }
    }
}
