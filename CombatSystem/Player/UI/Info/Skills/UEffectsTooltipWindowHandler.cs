using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Localization;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;
using CombatSystem.Stats;
using MEC;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Utils;
using Utils_Project;

namespace CombatSystem.Player.UI
{
    public class UEffectsTooltipWindowHandler : UEffectsTooltipWindowBase
    {
        [Title("Mono References")]
        [SerializeField]
        private RectTransform resizeWindow;
        [SerializeField] private float verticalPadding;

        [Title("Pool References")]
        [SerializeField, PropertyOrder(10)] private ToolTipWindowPool pool = new ToolTipWindowPool();

        [Serializable]
        private sealed class ToolTipWindowPool : TrackedMonoObjectPool<UEffectTooltipHolder>
        {}

        private void Awake()
        {
            pool.Awake();
            gameObject.SetActive(false);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            pool.ReturnElementsToPool();
        }

        public void Show()
        {
            if (gameObject.activeSelf) return;
            gameObject.SetActive(true);

            //todo animation?
        }



        public void HandleEffects(ICombatSkill skill, IEnumerable<PerformEffectValues> effects, CombatEntity performer)
        {
            var handleParams = new HandlerParams(resizeWindow, verticalPadding, pool);
            var skillParams = new SkillParams(skill, effects, performer);
            HandleEffects(in handleParams, in skillParams);
        }

    }

    public abstract class UEffectsTooltipWindowBase : MonoBehaviour
    {

        protected void HandleEffects(
            in HandlerParams handlerParams,
            in SkillParams skillParams)
        {
            float accumulatedHeight = 0;
            CombatStats stats = skillParams.Performer?.Stats;
            var skill = skillParams.Skill;
            foreach (var value in skillParams.Effects)
            {
                var holder = handlerParams.Pool.PopElementSafe();
                holder.HandleText(in value, stats, skill);
                holder.HandleIcon(value.Effect);

                holder.HandleTextHeight(ref accumulatedHeight);
            }

            var windowTransform = handlerParams.WindowTransform;
            var verticalSeparation = handlerParams.VerticalSeparation;
            HeightResize(windowTransform, accumulatedHeight, verticalSeparation);
        }
        
        protected static void HeightResize(RectTransform resizeWindow, float accumulatedHeight, float verticalPadding = 0)
        {
            accumulatedHeight += verticalPadding;
            resizeWindow.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, accumulatedHeight);
        }

        protected readonly struct HandlerParams
        {
            public readonly RectTransform WindowTransform;
            public readonly float VerticalSeparation;
            public readonly MonoObjectPool<UEffectTooltipHolder> Pool;

            public HandlerParams(RectTransform windowTransform, float verticalSeparation, MonoObjectPool<UEffectTooltipHolder> pool)
            {
                WindowTransform = windowTransform;
                VerticalSeparation = verticalSeparation;
                Pool = pool;
            }
        }
        protected readonly struct SkillParams
        {
            public readonly ICombatSkill Skill;
            public readonly IEnumerable<PerformEffectValues> Effects;
            public readonly CombatEntity Performer;

            public SkillParams(ICombatSkill skill, IEnumerable<PerformEffectValues> effects, CombatEntity performer)
            {
                Skill = skill;
                Effects = effects;
                Performer = performer;
            }
        }
    }
}
