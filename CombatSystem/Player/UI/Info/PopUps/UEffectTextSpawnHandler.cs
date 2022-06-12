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

namespace CombatSystem.Player.UI
{
    public class UEffectTextSpawnHandler : MonoBehaviour, IEffectUsageListener
    {
        [SerializeField] private DamageNumber numberPrefab;
        [SerializeField, Min(0), SuffixLabel("s")] private float popUpFrequency = .2f;

        private Queue<KeyValuePair<RectTransform, PerformEffectValues>> _popUpQueue;

        private void Awake()
        {
            _popUpQueue = new Queue<KeyValuePair<RectTransform, PerformEffectValues>>(16);
            CombatSystemSingleton.EventsHolder.Subscribe(this);
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


        public void OnCombatPrimaryEffectPerform(CombatEntity performer, CombatEntity target, in PerformEffectValues values)
        {
            OnCombatEffectPerform(performer,target,in values);
        }

        public void OnCombatSecondaryEffectPerform(CombatEntity performer, CombatEntity target, in PerformEffectValues values)
        {
            OnCombatEffectPerform(performer,target,in values);
        }


        public void OnCombatEffectPerform(CombatEntity performer, CombatEntity target, in PerformEffectValues values)
        {
            EnQueue(in target, values);
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

                Spawn(in targetTransform, in popUpText);
            }
        }

        private void EnQueue(in CombatEntity target, in PerformEffectValues values)
        {
            var hoverDictionary = PlayerCombatSingleton.HoverEntitiesHandler.GetDictionary();
            if (!hoverDictionary.ContainsKey(target)) return;

            var hoverElement = hoverDictionary[target];
            RectTransform targetTransform = (RectTransform) hoverElement.transform;

            var queueElement = new KeyValuePair<RectTransform, PerformEffectValues>(targetTransform,values);
            _popUpQueue.Enqueue(queueElement);
        }

        private void Spawn(in RectTransform targetTransform, in PerformEffectValues queueValues)
        {
            var popUpText = LocalizeEffects.LocalizeEffectDigitValue(in queueValues);
            var popUpElement = numberPrefab.Spawn(Vector3.zero, popUpText);
            popUpElement.SetAnchoredPosition(targetTransform, Vector2.zero);
            var textHandler = popUpElement.GetComponent<UEffectTextHandler>();
            textHandler.HandleEffect(in queueValues);
        }
    }
}
