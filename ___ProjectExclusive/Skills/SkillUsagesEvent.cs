using System.Collections.Generic;
using _CombatSystem;
using Characters;
using CombatEffects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Skills
{
    public class SkillUsagesEvent : ITempoListener
    {
        public SkillUsagesEvent()
        {
            SkillFeedback = new SkillFeedback();
            _skillListeners = new List<ISkillUsageListener>();
            _effectListeners = new List<IEffectUsageListener>();
        }
        [ShowInInspector]
        public readonly SkillFeedback SkillFeedback;
        [ShowInInspector]
        private readonly List<ISkillUsageListener> _skillListeners;
        [ShowInInspector]
        private readonly List<IEffectUsageListener> _effectListeners;

        public void Subscribe(ISkillUsageListener listener) => _skillListeners.Add(listener);
        public void Subscribe(IEffectUsageListener listener) => _effectListeners.Add(listener);

        public void OnInitiativeTrigger(CombatingEntity entity)
        {
            //Cleaning
            SkillFeedback.Clear();
            SkillFeedback.User = entity;
        }

        public void OnDoMoreActions(CombatingEntity entity)
        {
            SkillFeedback.Clear();
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
        }

        public void DoEffect(CombatingEntity target, IEffectBase effect, float value)
            => DoEffect(new EffectResolution(target, effect, value));

        public void DoEffect(EffectResolution effectResolution)
        {
            var effectQueue = SkillFeedback.EffectResolutions;
            effectQueue.Enqueue(effectResolution);

            if(effectQueue.Count == 1)
                SendFirstEffect();
            else
                SendSecondaryEffect();            


            void SendFirstEffect()
            {
                foreach (IEffectUsageListener listener in _effectListeners)
                {
                    listener.OnFirstEffect(effectResolution);
                }
            }
            void SendSecondaryEffect()
            {
                foreach (IEffectUsageListener listener in _effectListeners)
                {
                    listener.OnSecondaryEffect(effectResolution);
                }
            }
        }

        public void InvokeSkill(CombatSkill skill)
        {
            SkillFeedback.UsedSkill = skill;
            foreach (ISkillUsageListener listener in _skillListeners)
            {
                listener.OnSkillDone(SkillFeedback);
            }
        }
    }

    public interface ISkillUsageListener
    {
        void OnSkillDone(SkillFeedback skillFeedback);
    }

    public interface IEffectUsageListener
    {
        /// <summary>
        /// This is the first effect done; Generally the main effect.
        /// Some behaviors only care about the first effect and this
        /// is for those events
        /// </summary>
        void OnFirstEffect(EffectResolution effectResolution);
        /// <summary>
        /// This is invoked on Skills that has more than one effect. Some
        /// behaviour only care about the first effect, the rest uses this
        /// </summary>
        void OnSecondaryEffect(EffectResolution effectResolution);
    }
}
