using System.Collections.Generic;
using Characters;
using NUnit.Framework;
using Skills;
using UnityEngine;

namespace _CombatSystem
{
    public interface ICharacterFaction<out T>
    {
        T PlayerFaction { get; }
        T EnemyFaction { get; }
    }

    public interface ICharacterAnimatorHandler
    {
        IEnumerator<float> _DoSkillAnimation(CombatSkill skill, List<CombatingEntity> onTargets);
    }

    // Inherits from ICombatAfterPreparationListener because the handler is created 
    // in each CombatPreparation phase
    public interface ITeamVariationListener : ICombatAfterPreparationListener
    {
        
        /// <param name="controlPercentage">Goes (-1,1)</param>
        void OnPlayerControlVariation(float controlPercentage, TeamCombatData.Stance targetStance);
        void OnPlayerControlVariation(float controlPercentage);
    }
}
