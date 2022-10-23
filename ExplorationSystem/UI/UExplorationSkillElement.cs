using CombatSystem.Skills;
using UnityEngine;
using UnityEngine.UI;

namespace ExplorationSystem.UI
{
    public class UExplorationSkillElement : MonoBehaviour
    {
        [SerializeField] private Image iconHolder;

        public IFullSkill CurrentSkill { get; private set; }

        public void Injection(IFullSkill skill)
        {
            CurrentSkill = skill;
        }
    }
}
