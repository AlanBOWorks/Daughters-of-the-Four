using System.Collections.Generic;
using CombatSystem;
using UnityEngine;

namespace CombatSkills
{
    public class SkillUsageTracker
    {
        public SkillUsageTracker()
        {
            _fateSkills = new Queue<SkillUsageValues>();
            _lastSequenceUsedSkills = new Queue<SkillUsageValues>();
        }

        public int MaxFateAmount = 1;

        private readonly Queue<SkillUsageValues> _fateSkills;
        private readonly Queue<SkillUsageValues> _lastSequenceUsedSkills;

        public void AddSkill(SkillUsageValues values)
        {
            _lastSequenceUsedSkills.Enqueue(values);
         
            if (_fateSkills.Count < MaxFateAmount)
                _fateSkills.Enqueue(values);
        }

        public SkillUsageValues DeQueueFateSkill() => _fateSkills.Dequeue();

        public bool HadUsedSkill(CombatingSkill skill)
        {
            foreach (var usedSkill in _lastSequenceUsedSkills)
                if (usedSkill.UsedSkill == skill)
                    return true;

            return false;
        }

        public bool HadUsedSkill(SkillUsageValues skillUsage)
        {
            var target = skillUsage.Target;
            var skill = skillUsage.UsedSkill;
            foreach (var usedSkill in _lastSequenceUsedSkills)
                if (usedSkill.UsedSkill == skill && usedSkill.Target == target)
                    return true;

            return false;
        }

        public void ResetOnStartSequence()
        {
            _lastSequenceUsedSkills.Clear();
        }
    }
}
