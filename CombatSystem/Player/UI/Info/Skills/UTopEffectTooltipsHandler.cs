using System;
using System.Collections;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Stats;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace CombatSystem.Player.UI
{
    public class UTopEffectTooltipsHandler : MonoBehaviour
    {
        [Title("Params")] 
        [SerializeField, SuffixLabel("px")] 
        private float topPadding = 60;
        [Title("References")] 
        [SerializeField] 
        private ScrollRect effectsWindow;
        [SerializeReference] 
        private EffectTooltipPools pools = null;
        [ShowInInspector,HideInEditorMode]
        private DictionaryTargetingPool _targetingGroupsTracker;

        [SerializeField, DisableInPlayMode,
         Tooltip("Overrides the effect's tooltip info if it exits in the specific targeting group.\n" +
                                 "- OnFalse: pools without caring of repetitions")] 
        private bool overrideEffectRepetition = false;
        [ShowInInspector,HideInEditorMode, ShowIf("overrideEffectRepetition")]
        private DictionaryEffectTooltips _dictionaryEffectTooltips;

        [ShowInInspector,HideInEditorMode, HideIf("overrideEffectRepetition")]
        private HashSet<UEffectTooltipHolder> _hashSetEffectTooltips;


        private float _effectsWindowWidth;
        private void Awake()
        {
            _effectsWindowWidth = effectsWindow.content.rect.width;

            _targetingGroupsTracker = new DictionaryTargetingPool();

            if(overrideEffectRepetition)
                _dictionaryEffectTooltips = new DictionaryEffectTooltips();
            else
                _hashSetEffectTooltips = new HashSet<UEffectTooltipHolder>();


            pools.Awake();
        }

        private void OnDisable()
        {
            Timing.KillCoroutines(_windowHandle);
        }

        private CoroutineHandle _windowHandle;
        public void HandleEffects(
            IEnumerable<PerformEffectValues> effects,
            ISkill skill = null, 
            CombatEntity performer = null)
        {
            Timing.KillCoroutines(_windowHandle);
            _windowHandle = Timing.RunCoroutine(_ResizeCoroutine());
            IEnumerator<float> _ResizeCoroutine()
            {
                CombatStats stats = performer?.Stats;

                foreach (var value in effects)
                {
                    var effect = value.Effect;
                    var targetType = value.TargetType;
                    var targetingGroup = GetTargetingGroup(targetType);
                    UEffectTooltipHolder effectHolder;

                    if (overrideEffectRepetition)
                    {
                        if (_dictionaryEffectTooltips.ContainsKey(effect))
                        {
                            effectHolder = _dictionaryEffectTooltips[effect];
                            effectHolder.UpdateEffectDigitText(value.EffectValue);
                        }
                        else
                        {
                            PopHolder();
                            _dictionaryEffectTooltips.Add(effect,effectHolder);
                        }
                    }
                    else
                    {
                        PopHolder();
                        _hashSetEffectTooltips.Add(effectHolder);
                    }

                    void PopHolder()
                    {
                        effectHolder = pools.Pop(targetingGroup);
                        effectHolder.HandleText(in value, stats, skill);
                        effectHolder.HandleIcon(value.Effect);
                    }
                }

                yield return Timing.WaitForOneFrame;
                HeightResize();
            }
        }

        private UEffectsTargetingGroupHolder GetTargetingGroup(EnumsEffect.TargetType targetType)
        {
            var groupHolder = _targetingGroupsTracker[targetType];
            if (groupHolder) return groupHolder;

            var scrollContent = effectsWindow.content;

            groupHolder = pools.Pop();
            _targetingGroupsTracker[targetType] = groupHolder;

            var groupRectTransform = groupHolder.GetRectTransform();
            groupRectTransform.SetParent(scrollContent);


            var position = groupRectTransform.anchoredPosition;
            position.x = 0;
            groupRectTransform.anchoredPosition = position;

            groupHolder.SetWidth(_effectsWindowWidth);

            groupHolder.HandleInjection(targetType);
            return groupHolder;
        }

        private void HeightResize()
        {
            float accumulatedHeight = 0;
            foreach (var group in _targetingGroupsTracker.GetEnumerable())
            {
                if(!group) continue;

                var groupWindow = group.GetRectTransform();
                float groupHeight = group.ResizeHeight();
                
                var position = groupWindow.anchoredPosition;
                position.y = -accumulatedHeight;
                position.x = 0;

                groupWindow.anchoredPosition = position;
                accumulatedHeight += groupHeight;
            }

            accumulatedHeight += topPadding;
            var tooltipWindow = (RectTransform)transform;
            tooltipWindow.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, accumulatedHeight);
        }

        public void Clear()
        {
            ClearEffectTooltips();
            ClearTargetingGroups();
        }

        private void ClearEffectTooltips()
        {
            if (overrideEffectRepetition)
            {
                ClearDictionary();
            }
            else
            {
                ClearHashSet();
            }
           

            void ClearDictionary()
            {
                foreach (var element in _dictionaryEffectTooltips)
                {
                    var elementValue = element.Value;
                    pools.Release(elementValue);
                }
                _dictionaryEffectTooltips.Clear();
            }

            void ClearHashSet()
            {
                foreach (var element in _hashSetEffectTooltips)
                {
                    pools.Release(element);
                }
                _hashSetEffectTooltips.Clear();
            }
        }
        private void ClearTargetingGroups()
        {
            foreach (var targetingHolder in _targetingGroupsTracker.ClearAndGetEnumerable())
            {
                if(targetingHolder)
                {
                    pools.Release(targetingHolder);
                }
            }
        }



        private sealed class DictionaryTargetingPool : MonoTargetingStructure<UEffectsTargetingGroupHolder>
        { }

        private sealed class DictionaryEffectTooltips : Dictionary<IEffect, UEffectTooltipHolder>
        { }
    }
}
