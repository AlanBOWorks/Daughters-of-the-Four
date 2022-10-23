using System;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using CombatSystem.Team;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace CombatSystem.Player.UI
{
    public sealed class UCombatSkillHoverFeedback : MonoBehaviour, ISkillButtonListener,
        ITempoControlStatesListener
    {
        [Title("References")] 
        [SerializeField] private UCombatSkillButtonsHolder skillButtonsHolder;

        [TitleGroup("Images")]
        [SerializeField] private Image hoverHolder;
        [SerializeField] private Image focusHolder;
        private Color _hoverInitialColor;

        [Title("Animation Params")] 

        private void Start()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.SubscribeAsPlayerEvent(this);
            playerEvents.SubscribeForTeamControl(this);

            _hoverInitialColor = hoverHolder.color;
            ToggleHoverElements(false);
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }

        private void OnDisable()
        {
            ToggleHoverElements(false);
        }


        private const float ButtonAnimationDuration = .08f;
        private const float HoverImageAnimationDuration = .16f;
        private const float PunchScaleModifier = -.2f;
        private static void DoAnimate(Transform imageHolder, float animationDuration)
        {
            DOTween.Kill(imageHolder);
            imageHolder.localScale = Vector3.one;
            Vector3 targetScale = new Vector3(PunchScaleModifier,PunchScaleModifier, 0);
            imageHolder.DOPunchScale(targetScale, animationDuration);
        }
        private static void DoAnimate(Transform imageHolder, Vector3 onPosition, float animationDuration)
        {
            imageHolder.position = onPosition;
            DoAnimate(imageHolder, animationDuration);
        }


        private static void Hide(Component imageHolder)
        {
            imageHolder.gameObject.SetActive(false);
        }

        private void ToggleHoverElements(bool active)
        {
            hoverHolder.gameObject.SetActive(active);
            focusHolder.gameObject.SetActive(active);

            var targetColor = new Color(_hoverInitialColor.r,_hoverInitialColor.g,_hoverInitialColor.b, .4f);
            ToggleHoverColors(targetColor);
        }

        private void ToggleHoverColors(Color targetColor)
        {
            hoverHolder.color = targetColor;
            focusHolder.color = targetColor;
        }

        public void OnSkillButtonHover(IFullSkill skill)
        {
            var targetButton = skillButtonsHolder.GetDictionary()[skill];

            var buttonTransform = targetButton.GetGroupHolder();
            hoverHolder.gameObject.SetActive(true);

            DoAnimate(buttonTransform, ButtonAnimationDuration);
            DoAnimate(hoverHolder.transform,buttonTransform.position, HoverImageAnimationDuration);

        }

        public void OnSkillButtonExit(IFullSkill skill)
        {
            Hide(hoverHolder);
        }


        public void OnSkillSelect(IFullSkill skill)
        {

        }

        public void OnSkillSelectFromNull(IFullSkill skill)
        {
        }

        public void OnSkillSwitch(IFullSkill skill, IFullSkill previousSelection)
        {
            var targetButton = skillButtonsHolder.GetDictionary()[skill];

            var buttonTransform = targetButton.GetGroupHolder();
            focusHolder.gameObject.SetActive(true);

            DoAnimate(buttonTransform, ButtonAnimationDuration);
            DoAnimate(focusHolder.transform, buttonTransform.position, HoverImageAnimationDuration);
        }

        public void OnSkillDeselect(IFullSkill skill)
        {
            Hide(focusHolder);
        }

        public void OnSkillCancel(CombatSkill skill)
        {
            Hide(focusHolder);
        }

        public void OnSkillSubmit(IFullSkill skill)
        {
        }

        public void OnTempoStartControl(CombatTeamControllerBase controller, CombatEntity firstControl)
        {
            ToggleHoverColors(_hoverInitialColor);
        }

        public void OnAllActorsNoActions(CombatEntity lastActor)
        {
            ToggleHoverElements(false);
        }

        public void OnTempoFinishControl(CombatTeamControllerBase controller)
        {
            ToggleHoverElements(false);
        }
    }
}
