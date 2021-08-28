using System;
using _CombatSystem;
using Sirenix.OdinInspector;
using Skills;
using Stats;
using UnityEngine;
using UnityEngine.UI;

namespace ___ProjectExclusive
{
    public class USandboxDebug : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private Button testButton;

        [SerializeField] private TesterStats Stats = new TesterStats();
        private void Awake()
        {
            testButton.onClick.AddListener(TestCode);
        }

        [ShowInInspector]
        private bool _wasPaused;
        private void TestCode()
        {
            var tempoHandler = CombatSystemSingleton.TempoHandler;
            if (_wasPaused)
                tempoHandler.DoResume();
            else
            {
                tempoHandler.DoPause();
            }

            _wasPaused = !_wasPaused;
        }

        [Button]
        private void TestEditorAction(SSkillPreset testSkill, EnumSkills.StatDriven statsDriven)
        {
            EnumSkills.TargetingType type = testSkill.GetSkillType();
            EnumSkills.Archetype targetArchetype = (EnumSkills.Archetype) ((int) type | (int) statsDriven);

            Debug.Log($"Selected: {type} + {statsDriven} \n" +
                      $"{(int)type} + {(int)statsDriven} => " +
                      $"Result: {(int) targetArchetype} || {targetArchetype}");
        }

        [Button]
        void TestEnum(EnumSkills.Archetype archetype)
        {
            Debug.Log(archetype + " => " +(int)archetype);
        }

        [Button]
        private void TestStats()
        {
            var stat = Stats.OffensiveStats;
            Debug.Log($"Stats {stat.AttackPower}");
        }

        [Serializable] private class TesterStats : SerializableHolderStats<float>
        {
            
        }
#endif
    }
}
