using System;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UFrontTargetFeedback : MonoBehaviour, 
        ITargetPointerListener, ITargetSelectionListener,
        ISkillSelectionListener
    {
        [SerializeField] private UFrontTargetButtonsHandler targetButtonsHandler;
        [SerializeField, SuffixLabel("deltas")] private float animationDeltaSpeed = 4f;
        [SerializeField] private AnimationCurve animationCurve = new AnimationCurve(
            new Keyframe(0,0), new Keyframe(.5f,1), new Keyframe(1,0));

        private void Awake()
        {
            PlayerCombatSingleton.PlayerCombatEvents.SubscribeAsPlayerEvent(this);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }

        private float _scaleModifier = 1;
        private void Update()
        {
            float scaleIncrement = Time.deltaTime * animationDeltaSpeed;
            _scaleModifier += scaleIncrement;
            if (_scaleModifier > 1) _scaleModifier = 0;

            float finalScaleModifier = animationCurve.Evaluate(_scaleModifier);
            transform.localScale = Vector3.one * finalScaleModifier;
        }
        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }


        public void OnTargetButtonHover(CombatEntity target)
        {
            var button = targetButtonsHandler.GetDictionary()[target];
            transform.position = button.transform.position;
            Show();
        }

        public void OnTargetButtonExit(CombatEntity target)
        {
            Hide();
        }

        public void OnTargetSelect(CombatEntity target)
        {
        }

        public void OnTargetCancel(CombatEntity target)
        {
        }

        public void OnTargetSubmit(CombatEntity target)
        {
        }


        public void OnSkillSelect(CombatSkill skill)
        {
        }

        public void OnSkillSelectFromNull(CombatSkill skill)
        {
        }

        public void OnSkillSwitch(CombatSkill skill, CombatSkill previousSelection)
        {
        }

        public void OnSkillDeselect(CombatSkill skill)
        {
            Hide();
        }

        public void OnSkillCancel(CombatSkill skill)
        {
        }

        public void OnSkillSubmit(CombatSkill skill)
        {
        }
    }
}
