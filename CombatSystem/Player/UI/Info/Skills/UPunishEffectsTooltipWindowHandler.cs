using System;
using System.Collections.Generic;
using CombatSystem.Skills;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Player.UI
{
    public class UPunishEffectsTooltipWindowHandler : UEffectsTooltipWindowBase
    {
        [Title("Params")]
        [SerializeField] 
        private float verticalPadding;

        [Title("Windows")]
        [SerializeField]
        private WindowValues performerWindow = new WindowValues();
        [SerializeField] 
        private WindowValues targetWindow = new WindowValues();

        [Title("Pool")]
        [SerializeField]
        private Pool effectElementPool = new Pool();

        [SerializeField] 
        private Transform onReleaseOnPoolParent;


        private void Awake()
        {
            effectElementPool.Awake();
        }

        public void ReleaseElements()
        {
            performerWindow.ReturnElementsToPool(effectElementPool, onReleaseOnPoolParent);
            targetWindow.ReturnElementsToPool(effectElementPool, onReleaseOnPoolParent);

            performerWindow.ResetValues();
            targetWindow.ResetValues();
        }

        public void AddVanguardEffects(IVanguardSkill skill)
        {
            var effects = skill.GetPunishEffects();
            foreach (var effect in effects)
            {
                var effectType = effect.TargetType;
                var effectPreset = effect.Effect;
                var effectValue = effect.EffectValue;

                bool isTargetType = IsTargetType(effectType);
                var windowValues = GetEffectWindow(isTargetType);
                var dictionary = windowValues.GetDictionary();

                UPunishEffectTooltipHandler tooltipHandler;
                if (dictionary.ContainsKey(effectPreset))
                {
                    tooltipHandler = dictionary[effectPreset];
                    tooltipHandler.IncreaseEffectValue(effectValue);
                }
                else
                {
                    var element = effectElementPool.Pop(windowValues.window);
                    element.gameObject.SetActive(true);
                    dictionary.Add(effectPreset,element);

                    element.HandleVanguardEffect(effect, effectValue);

                    tooltipHandler = element;
                    tooltipHandler.HandleTextHeight(ref windowValues.AccumulatedHeight);
                }

            }
            HeightResize(performerWindow.window, performerWindow.AccumulatedHeight, verticalPadding);
            HeightResize(targetWindow.window, targetWindow.AccumulatedHeight, verticalPadding);
        }


        private WindowValues GetEffectWindow(bool isTargetType)
        {
            return isTargetType ? targetWindow : performerWindow;
        }


        private bool IsTargetType(EnumsEffect.TargetType type)
        {
            return type switch
            {
                EnumsEffect.TargetType.Target => true,
                EnumsEffect.TargetType.TargetLine => true,
                EnumsEffect.TargetType.TargetTeam => true,

                EnumsEffect.TargetType.Performer => false,
                EnumsEffect.TargetType.PerformerLine => false,
                EnumsEffect.TargetType.PerformerTeam => false,

                EnumsEffect.TargetType.MostDesired => throw new ArgumentOutOfRangeException(nameof(type), type, null),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        [Serializable]
        private sealed class Pool : MonoObjectPoolBasic<UPunishEffectTooltipHandler>
        { }


        

        [Serializable]
        private sealed class WindowValues
        {
            public RectTransform window;
            [NonSerialized, ShowInInspector, ReadOnly]
            public float AccumulatedHeight;

            [SerializeField]
            private EffectsDictionary dictionary = new EffectsDictionary();

            public Dictionary<IEffect, UPunishEffectTooltipHandler> GetDictionary() => dictionary;


            public void ResetValues()
            {
                AccumulatedHeight = 0;
            }

            public void ReturnElementsToPool(Pool effectElementPool, Transform onReleaseOnPoolParent)
            {
                foreach (var pair in dictionary)
                {
                    var element = pair.Value;
                    effectElementPool.Release(element);
                    element.transform.SetParent(onReleaseOnPoolParent);
                }
                dictionary.Clear();
            }

            private sealed class EffectsDictionary : Dictionary<IEffect, UPunishEffectTooltipHandler>
            { }
        }
    }
}
