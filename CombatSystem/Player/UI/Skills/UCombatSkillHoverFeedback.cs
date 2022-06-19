using System;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public sealed class UCombatSkillHoverFeedback : MonoBehaviour, ISkillButtonListener
    {
        [Title("References")] 
        [SerializeField] private UCombatSkillButtonsHolder skillButtonsHolder;

        [TitleGroup("Images")]
        [SerializeField] private RectTransform hoverHolder;
        [SerializeField] private RectTransform focusHolder;

        [Title("Animation Params")] 

        private Quaternion _hoverInitialRotation;
        private Quaternion _focusInitialRotation;
        private void Start()
        {
            PlayerCombatSingleton.PlayerCombatEvents.SubscribeAsPlayerEvent(this);

            _hoverInitialRotation = hoverHolder.localRotation;
            _focusInitialRotation = focusHolder.localRotation;

            hoverHolder.gameObject.SetActive(false);
            focusHolder.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }

        private const float AnimationDuration = .08f;
        private static void DoAnimate(RectTransform imageHolder, UCombatSkillButton onButton, Quaternion targetRotation)
        {
            imageHolder.gameObject.SetActive(true);
            var groupHolder = onButton.GetGroupHolder();
            imageHolder.SetParent(groupHolder);
            imageHolder.localPosition = Vector3.zero;

            DOTween.Kill(imageHolder);
            imageHolder.DOLocalRotateQuaternion(targetRotation, AnimationDuration);
            DOTween.Kill(groupHolder);
            groupHolder.localScale = Vector3.one;
            groupHolder.DOPunchScale(Vector3.one * -.2f, AnimationDuration);
        }

        private static void Hide(Component imageHolder)
        {
            imageHolder.gameObject.SetActive(false);
        }


        public void OnSkillButtonHover(ICombatSkill skill)
        {
            var targetButton = skillButtonsHolder.GetDictionary()[skill];

            hoverHolder.localRotation = Quaternion.AngleAxis(-10, Vector3.forward);
            DoAnimate(hoverHolder,targetButton, _hoverInitialRotation);
        }

        public void OnSkillButtonExit(ICombatSkill skill)
        {
            Hide(hoverHolder);
        }


        public void OnSkillSelect(CombatSkill skill)
        {

        }

        public void OnSkillSelectFromNull(CombatSkill skill)
        {
        }

        public void OnSkillSwitch(CombatSkill skill, CombatSkill previousSelection)
        {
            var targetButton = skillButtonsHolder.GetDictionary()[skill];

            focusHolder.localRotation = _hoverInitialRotation;
            DoAnimate(focusHolder,targetButton, _focusInitialRotation);
        }

        public void OnSkillDeselect(CombatSkill skill)
        {
            Hide(focusHolder);
        }

        public void OnSkillCancel(CombatSkill skill)
        {
            Hide(focusHolder);
        }

        public void OnSkillSubmit(CombatSkill skill)
        {
        }
    }
}
