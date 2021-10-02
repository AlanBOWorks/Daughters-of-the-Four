using System.Collections.Generic;
using CombatTeam;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSkills
{
    [CreateAssetMenu(fileName = "N Skill Set[Preset]",
        menuName = "Combat/Skills/Shared Skills Set")]
    public class SSharedSkillSet : ScriptableObject, ITeamStanceStructureRead<ICollection<SSkill>>
    {
        [Title("Attacking")]
        [SerializeField] private SSkill[] sharedAttackingSkills;
        [Title("Neutral")]
        [SerializeField] private SSkill[] sharedNeutralSkills;
        [Title("Defending")]
        [SerializeField] private SSkill[] sharedDefenseSkills;

        public ICollection<SSkill> OnAttackStance => sharedAttackingSkills;
        public ICollection<SSkill> OnNeutralStance => sharedNeutralSkills;
        public ICollection<SSkill> OnDefenseStance => sharedDefenseSkills;
    }
}
