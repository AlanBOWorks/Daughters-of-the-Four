using System;
using CombatSystem.Entity;
using CombatSystem.Skills;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Editor
{
    public class UCombatAnimatorDebugger : MonoBehaviour
    {
        [SerializeField, DisableInPlayMode] private UCombatEntityAnimator performer;
        [SerializeField] private UCombatAnimatorDebugger synchronizeTo;

        private void Awake()
        {
            DoInitialAnimation();
        }

        [Button,HideInEditorMode]
        private void SwitchPerformer(UCombatEntityAnimator next)
        {
            performer = next;
            DoInitialAnimation();
        }

        [Button,DisableInEditorMode]
        private void DoInitialAnimation()
        {
            if(performer == null) return;

            performer.PerformInitialCombatAnimation();
        }

        private void DoAnimation(CombatSkill skill)
        {
            if (performer == null) return;

            performer.PerformActionAnimation(skill, null);

        }

        [ButtonGroup("PerformerAnimation"), DisableInEditorMode, GUIColor(.8f, .3f, .3f)]
        private void DoOffensiveAnimation()
        {
            var skill = StaticSkillTypes.OffensiveCombatSkill;
            DoAnimation(skill);

            if (synchronizeTo != null)
                synchronizeTo.DoOffensiveReceiveAnimation();
        }
        [ButtonGroup("PerformerAnimation"), DisableInEditorMode, GUIColor(.3f, .6f, .8f)]
        private void DoSupportAnimation()
        {
            var skill = StaticSkillTypes.SupportCombatSkill;
            DoAnimation(skill);

            if (synchronizeTo != null)
                synchronizeTo.DoSupportReceiveAnimation();
        }
        [ButtonGroup("PerformerAnimation"), DisableInEditorMode, GUIColor(.8f, .8f, .3f)]
        private void DoTeamAnimation()
        {
            var skill = StaticSkillTypes.TeamCombatSkill;
            DoAnimation(skill);
        }


        private void DoReceiveAnimation(CombatSkill skill)
        {
            if (performer == null) return;

            performer.ReceiveActionAnimation(skill, null);
        }

        [ButtonGroup("ReceiveAnimations"), DisableInEditorMode, GUIColor(.8f, .3f, .3f)]
        private void DoOffensiveReceiveAnimation()
        {
            var skill = StaticSkillTypes.OffensiveCombatSkill;
            DoReceiveAnimation(skill);
        }
        [ButtonGroup("ReceiveAnimations"), DisableInEditorMode, GUIColor(.3f, .6f, .8f)]
        private void DoSupportReceiveAnimation()
        {
            var skill = StaticSkillTypes.SupportCombatSkill;
            DoReceiveAnimation(skill);
        }
        [ButtonGroup("ReceiveAnimations"), DisableInEditorMode, GUIColor(.8f, .8f, .3f)]
        private void DoTeamReceiveAnimation()
        {
            var skill = StaticSkillTypes.TeamCombatSkill;
            DoReceiveAnimation(skill);
        }
    }
}
