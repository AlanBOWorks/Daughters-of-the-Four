using System;
using _CombatSystem;
using Sirenix.OdinInspector;
using Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Player
{
    public class USkillEffectFeedback : MonoBehaviour, IEffectUsageListener, ISkillUsageListener
    {
        private void Start()
        {
            CombatSystemSingleton.SkillUsagesEvent.Subscribe(this as IEffectUsageListener);
            CombatSystemSingleton.SkillUsagesEvent.Subscribe(this as ISkillUsageListener);
        }

        [TitleGroup("Skill")]
        [SerializeField] private TextMeshProUGUI skillNaming;
        [SerializeField] private Image skillUsedIcon;

        [TitleGroup("Effects")]
        [SerializeField] private EffectFeedbackGUI mainEffectGui;

        //[SerializeField] private EffectFeedbackGUI secondaryEffect; TODO



        public void OnFirstEffect(EffectResolution effectResolution)
        {
            mainEffectGui.ShowEffect(effectResolution);
        }

        public void OnSecondaryEffect(EffectResolution effectResolution)
        {
        }
        public void OnSkillDone(SkillFeedback skillFeedback)
        {
            var skill = skillFeedback.UsedSkill;
            skillNaming.text = skill.SkillName;
            skillUsedIcon.sprite = skill.Icon;
        }

        [Serializable]
        private class EffectFeedbackGUI
        {

            //TODo change to popUP
            //[SerializeField] private TextMeshProUGUI effectNaming;
            [SerializeField] private TextMeshProUGUI effectValue;


            public void ShowEffect(EffectResolution effectResolution)
            {

                float value = effectResolution.Value;
                effectValue.text = $"{value:###.0}";
            }

        }

    }
}
