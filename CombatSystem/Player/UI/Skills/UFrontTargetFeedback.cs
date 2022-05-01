using System;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UFrontTargetFeedback : MonoBehaviour, ITargetPointerListener, ITargetSelectionListener
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

        public void OnTargetButtonHover(in CombatEntity target)
        {
            var button = targetButtonsHandler.GetDictionary()[target];
            transform.position = button.transform.position;
            Show();
        }

        public void OnTargetButtonExit(in CombatEntity target)
        {
            Hide();
        }

        public void OnTargetSelect(in CombatEntity target)
        {
            Hide();
        }

        public void OnTargetCancel(in CombatEntity target)
        {
            Hide();
        }

        public void OnTargetSubmit(in CombatEntity target)
        {
            Hide();
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
