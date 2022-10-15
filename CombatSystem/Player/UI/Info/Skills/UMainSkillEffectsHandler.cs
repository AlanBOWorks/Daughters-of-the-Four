using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Skills;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Player.UI
{
    public class UMainSkillEffectsHandler : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private UAllEffectTooltipsHandler mainEffectsHandler;
        [SerializeField] private UAllEffectTooltipsHandler counterEffectsHandler;
        [SerializeField] private UAllEffectTooltipsHandler punishEffectsHandler;
       

        public void HandleMainEffects(IEnumerable<PerformEffectValues> effects, ISkill skill, CombatEntity performer)
        {
            mainEffectsHandler.HandleEffects(effects,skill,performer);
        }
        public void HandleCounterEffects(IEnumerable<PerformEffectValues> effects, ISkill skill, CombatEntity performer)
        {
            counterEffectsHandler.HandleEffects(effects, skill, performer);
        }
        public void HandlePunishEffects(IEnumerable<PerformEffectValues> effects, ISkill skill, CombatEntity performer)
        {
            punishEffectsHandler.HandleEffects(effects, skill, performer);
        }

        public void Show(bool showCounterEffects, bool showPunishEffects)
        {
            counterEffectsHandler.gameObject.SetActive(showCounterEffects);
            punishEffectsHandler.gameObject.SetActive(showPunishEffects);
        }

        public void Hide()
        {
            mainEffectsHandler.Clear();
            counterEffectsHandler.Clear();
            punishEffectsHandler.Clear();
        }

    }


    [Serializable]
    internal sealed class EffectTooltipPools
    {
        [Tooltip("To which parent will the elements be store after returning to the pool.")]
        [SerializeField] 
        private Transform onReleaseParent;

        [SerializeField]
        private TargetingPool targetingPool = new TargetingPool();
        [SerializeField]
        private EffectTooltipPool effectTooltipPool = new EffectTooltipPool();


        [Serializable]
        private sealed class TargetingPool : MonoObjectPoolBasic<UEffectsTargetingGroupHolder>
        { }
        [Serializable]
        private sealed class EffectTooltipPool : MonoObjectPoolBasic<UEffectTooltipHolder>
        { }

        public void Awake()
        {
            var targetingCloneable = targetingPool.GetCloneableElement();
            var effectsCloneable = effectTooltipPool.GetCloneableElement();

            targetingCloneable.transform.SetParent(onReleaseParent);
            effectsCloneable.transform.SetParent(onReleaseParent);
            onReleaseParent.gameObject.SetActive(false);
        }

        public UEffectsTargetingGroupHolder Pop()
        {
            return targetingPool.Pop();
        }

        public UEffectTooltipHolder Pop(UEffectsTargetingGroupHolder onParent)
        {
            var element = effectTooltipPool.Pop();
            onParent.HandleElement(element);
            return element;
        }

        public void Release(UEffectsTargetingGroupHolder groupHolder)
        {
            groupHolder.transform.SetParent(onReleaseParent);
            targetingPool.RawRelease(groupHolder);
        }

        public void Release(UEffectTooltipHolder tooltipHolder)
        {
            tooltipHolder.transform.SetParent(onReleaseParent);
            effectTooltipPool.RawRelease(tooltipHolder);
        }

    }

}
