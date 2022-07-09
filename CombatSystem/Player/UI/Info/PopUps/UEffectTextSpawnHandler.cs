using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Localization;
using CombatSystem.Skills;
using DamageNumbersPro;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Player.UI
{
    public class UEffectTextSpawnHandler : MonoBehaviour, IEffectUsageListener
    {
        [SerializeField] private DamageNumber numberPrefab;
        [SerializeField, Min(0), SuffixLabel("s")] private float popUpFrequency = .2f;
        [SerializeField]
        private EffectPopupHandler effectPopupHandler = new EffectPopupHandler();


        private Queue<KeyValuePair<Transform, PerformEffectValues>> _popUpQueue;

        private void Awake()
        {
            _popUpQueue = new Queue<KeyValuePair<Transform, PerformEffectValues>>(16);
            CombatSystemSingleton.EventsHolder.Subscribe(this);

            effectPopupHandler.Awake();
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


        public void OnCombatPrimaryEffectPerform(EntityPairInteraction entities, in PerformEffectValues values)
        {
            OnCombatEffectPerform(entities,in values);
        }

        public void OnCombatSecondaryEffectPerform(EntityPairInteraction entities, in PerformEffectValues values)
        {
            OnCombatEffectPerform(entities,in values);
        }

        public void OnCombatVanguardEffectPerform(EntityPairInteraction entities, in PerformEffectValues values)
        {
            OnCombatEffectPerform(entities,in values);
        }


        public void OnCombatEffectPerform(EntityPairInteraction entities, in PerformEffectValues values)
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
                var queueElement = _popUpQueue.Dequeue();
                var targetTransform = queueElement.Key;
                var popUpText = queueElement.Value;

                Spawn(targetTransform, in popUpText);
            }
        }

        private void EnQueue(CombatEntity target, in PerformEffectValues values)
        {
            var pivot = target.Body.PivotRootType;

            var queueElement = new KeyValuePair<Transform, PerformEffectValues>(pivot,values);
            _popUpQueue.Enqueue(queueElement);
        }

        private void Spawn(Transform targetTransform, in PerformEffectValues queueValues)
        {
            return;
            var popUpText = LocalizeEffects.LocalizeEffectDigitValue(in queueValues);
            var popUpElement = numberPrefab.Spawn(Vector3.zero, popUpText);
            popUpElement.SetAnchoredPosition(targetTransform, Vector2.zero);
            var textHandler = popUpElement.GetComponent<UEffectTextHandler>();
            textHandler.HandleEffect(in queueValues);
        }

        [Serializable]
        private sealed class EffectPopupHandler : TrackedMonoObjectPool<UEffectTextPopUp>
        {
            [SerializeField,SuffixLabel("%")] 
            private AnimationCurve alphaCurve = new AnimationCurve(
                new Keyframe(0,0), new Keyframe(.5f,1));
            [SerializeField] private Vector2 velocity;


            public void Tick()
            {
                TickAll(Time.deltaTime);
            }

            public UEffectTextPopUp DoEffect()
            {
                var element = GetElementSafe();
                element.Injection(this);
                return element;
            }

            private void TickAll(float delta)
            {
                var deltaVelocity = delta * velocity;
                foreach (var element in GetActiveElements())
                {
                    float currentLerp = element.GetCurrentLerp();
                    float targetLerp = currentLerp + delta;

                    element.Translation(deltaVelocity);
                    float targetAlpha = alphaCurve.Evaluate(targetLerp);
                    element.SetAlpha(targetAlpha);
                }
            }
        }
    }
}
