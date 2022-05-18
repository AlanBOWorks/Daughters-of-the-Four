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
    public class UEffectTextSpawnHandler : MonoBehaviour, ISkillUsageListener
    {
        [SerializeField] private DamageNumber numberPrefab;
        [SerializeField, Min(0), SuffixLabel("s")] private float popUpFrequency = .2f;

        private IReadOnlyDictionary<CombatEntity, UUIHoverEntity> _hoverDictionary;
        private Queue<KeyValuePair<RectTransform, string>> _popUpQueue;

        private void Awake()
        {
            _popUpQueue = new Queue<KeyValuePair<RectTransform, string>>(16);
            CombatSystemSingleton.EventsHolder.Subscribe(this);
        }

        private void Start()
        {
            _hoverDictionary = PlayerCombatSingleton.HoverEntityElements;
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


        public void OnCombatSkillSubmit(in CombatEntity performer, in CombatSkill usedSkill,in CombatEntity target)
        {
        }

        public void OnCombatSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
        }

        public void OnCombatEffectPerform(in CombatEntity performer, in CombatEntity target, in PerformEffectValues values)
        {
            EnQueue(in target, values);
            if (_loopHandle.IsRunning) return;
            _loopHandle = Timing.RunCoroutine(_StartSpawningPopUps(), Segment.RealtimeUpdate);
        }

        public void OnCombatSkillFinish(in CombatEntity performer)
        {
            
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
            if (!_hoverDictionary.ContainsKey(target)) return;

            var hoverElement = _hoverDictionary[target];
            RectTransform targetTransform = (RectTransform) hoverElement.transform;
            string popUpText = LocalizeSkills.LocalizeEffect(in values);

            KeyValuePair<RectTransform,string> queueElement = new KeyValuePair<RectTransform, string>(targetTransform,popUpText);
            _popUpQueue.Enqueue(queueElement);
        }

        private void Spawn(in RectTransform targetTransform, in string popUpText)
        {
            var popUpElement = numberPrefab.Spawn(Vector3.zero, popUpText);
            popUpElement.SetAnchoredPosition(targetTransform, Vector2.zero);
        }
    }
}
