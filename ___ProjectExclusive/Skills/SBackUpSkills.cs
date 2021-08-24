using System;
using _CombatSystem;
using _Team;
using Characters;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "N - BackUpSkills [Preset]",
        menuName = "Combat/Skill/[Global] BackUp Skill Set", order = 100)]
    public class SBackUpSkills : ScriptableObject, 
        ISkillBackUp<SkillPreset[]>
    {
        [SerializeField]
        private OnDeathSkills onDeathSkills;
        public ICharacterArchetypesData<SkillPreset[]> OnDeathSkill()
            => onDeathSkills;




        [Serializable]
        private class OnDeathSkills : ICharacterArchetypesData<SkillPreset[]>
        {
            [SerializeField] private SkillPreset[] frontLiner;
            [SerializeField] private SkillPreset[] midLiner;
            [SerializeField] private SkillPreset[] backLiner;

            public SkillPreset[] Vanguard => frontLiner;
            public SkillPreset[] Attacker => midLiner;
            public SkillPreset[] Support => backLiner;
        }
    }

    public class OnDeathSkillInjector : IHealthZeroListener
    {
        public void OnHealthZero(CombatingEntity entity)
        {}

        public void OnMortalityZero(CombatingEntity entity)
        {
            // Add the role backup skills on death so the necessary skills (such attack, stance, etc)
            // can be used by other roles as well when the specialized Role's [CombatEntity] is not 
            // avaliable
            var backUpSkills = (CombatSystemSingleton.ParamsVariable.BackUpSkills);

            var roleIndex = (int) entity.Role;
            var injectionSkill = CharacterArchetypes.GetElement(
                 backUpSkills.OnDeathSkill(), roleIndex);
            if(injectionSkill.Length <= 0) return;

            var team = entity.CharacterGroup.TeamNotSelf;
            foreach (SkillPreset preset in injectionSkill)
            {
                foreach (CombatingEntity ally in team)
                {
                    ally.CombatSkills.AddNotRepeat(preset);
                }
            }
        }

        public void OnRevive(CombatingEntity entity)
        {}

        public void OnTeamHealthZero(CombatingTeam losingTeam)
        {}
    }
}
