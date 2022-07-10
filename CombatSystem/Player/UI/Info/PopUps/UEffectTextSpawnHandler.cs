using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Localization;
using CombatSystem.Skills;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Player.UI
{
    public class UEffectTextSpawnHandler : MonoBehaviour, IEffectUsageListener
    {
        [SerializeField, Min(0), SuffixLabel("s")] 
        private float popUpFrequency = .2f;
        [SerializeField]
        private EffectPopupHandler effectPopupHandler = new EffectPopupHandler();


        private Queue<KeyValuePair<Transform, SubmitEffectValues>> _popUpQueue;

        private void Awake()
        {
            _popUpQueue = new Queue<KeyValuePair<Transform, SubmitEffectValues>>(16);
            CombatSystemSingleton.EventsHolder.Subscribe(this);

            effectPopupHandler.Awake();
        }

        private Camera _combatCamera;
        private void OnEnable()
        {
            _combatCamera = PlayerCombatSingleton.CamerasHolder.GetMainCameraType;

        }

        private void Update()
        {
            effectPopupHandler.Tick();
        }

        private void OnDestroy()
        {
            CombatSystemSingleton.EventsHolder.UnSubscribe(this);
        }


        private void OnDisable()
        {
            _popUpQueue.Clear();
            Timing.KillCoroutines(_loopHandle);
        }


        public void OnCombatPrimaryEffectPerform(EntityPairInteraction entities, in SubmitEffectValues values)
        {
            OnCombatEffectPerform(entities,in values);
        }

        public void OnCombatSecondaryEffectPerform(EntityPairInteraction entities, in SubmitEffectValues values)
        {
            OnCombatEffectPerform(entities,in values);
        }

        public void OnCombatVanguardEffectPerform(EntityPairInteraction entities, in SubmitEffectValues values)
        {
            OnCombatEffectPerform(entities,in values);
        }


        public void OnCombatEffectPerform(EntityPairInteraction entities, in SubmitEffectValues values)
        {
            EnQueue(entities.Target, values);
            if (_loopHandle.IsRunning) return;
            _loopHandle = Timing.RunCoroutine(_StartSpawningPopUps(), Segment.RealtimeUpdate);
        }


        private CoroutineHandle _loopHandle;
        private IEnumerator<float> _StartSpawningPopUps()
        {
            while (_popUpQueue.Count > 0)
            {
                yield return Timing.WaitForSeconds(popUpFrequency);
                (Transform key, SubmitEffectValues value) = _popUpQueue.Dequeue();
                var popUpText = value;

                Spawn(key, in popUpText);
            }
        }

        private void EnQueue(CombatEntity target, in SubmitEffectValues values)
        {
            var pivot = target.Body.PivotRootType;

            var queueElement = new KeyValuePair<Transform, SubmitEffectValues>(pivot,values);
            _popUpQueue.Enqueue(queueElement);
        }

        private void Spawn(Transform targetTransform, in SubmitEffectValues queueValues)
        {
            var popUpText = LocalizeEffects.LocalizeEffectDigitValue(in queueValues);
            var icon = UtilsVisual.GetEffectSprite(queueValues.Effect);

            Vector3 targetPosition = _combatCamera.WorldToScreenPoint(targetTransform.position);

            var popUp = effectPopupHandler.DoEffect(popUpText, icon);
            popUp.transform.position = targetPosition;
            popUp.gameObject.SetActive(true);
        }

        [Serializable]
        private sealed class EffectPopupHandler : TrackedMonoObjectPool<UEffectTextPopUp>
        {
            [SerializeField,SuffixLabel("%")] 
            private AnimationCurve alphaCurve = new AnimationCurve(
                new Keyframe(0,0), new Keyframe(.5f,1));
            [SerializeField] private Vector2 velocity;
            [SerializeField] private float deltaSpeed = 4f;

            public void Tick()
            {
                TickAll(Time.deltaTime * deltaSpeed);
            }

            public UEffectTextPopUp DoEffect(string effectText, Sprite effectIcon)
            {
                var element = GetElementSafe();
                element.Injection(this);
                element.Injection(effectText);
                element.Injection(effectIcon);
                return element;
            }


            private void TickAll(float delta)
            {
                var deltaVelocity = delta * velocity;
                foreach (var element in GetActiveElements())
                {
                    float currentLerp = element.LerpAmount;
                    float targetLerp = currentLerp + delta;
                    element.LerpAmount = targetLerp;

                    element.Translation(deltaVelocity);
                    float targetAlpha = alphaCurve.Evaluate(targetLerp);
                    element.SetAlpha(targetAlpha);
                }
            }
        }
    }
}
