using System.Collections.Generic;
using CombatEntity;
using CombatSkills;
using CombatSystem.CombatSkills;
using CombatSystem.Events;
using UnityEngine;

namespace CombatEffects
{
    public sealed class ProvokeHandler : IOffensiveActionReceiverListener<CombatingEntity>
    {
        public CombatingEntity CurrentBait { get; private set; }


        public void SwitchBait(CombatingEntity bait) => CurrentBait = bait;

        public void CheckForReset(CombatingEntity initiativeEntity)
        {
            if (CurrentBait == initiativeEntity)
                CurrentBait = null;
        }
        public void OnReceiveOffensiveAction(CombatingEntity offensiveReceiver)
        {
            if(CurrentBait == null) return;
            if (CurrentBait != offensiveReceiver)
            {
                CurrentBait.ProvokeEffects.Invoke();
            }
        }
    }

    public sealed class ProvokeEffectsHolder : ISkillParameters
    {
        public ProvokeEffectsHolder(CombatingEntity user)
        {
            Performer = user;
            _effectQueue = new Queue<ProvokeParameters>();
        }

        public void Invoke()
        {
            //It targets it self while the EffectParameter.TargetingType will handle how receives the effect
            while (_effectQueue.Count > 0)
            {
                var effectParameter = _effectQueue.Dequeue();
                var effect = effectParameter.Effect;

                UsedSkill = effectParameter.UsedSkill;
                EffectTargets = UtilsTarget.GetPossibleTargets(UsedSkill.GetTargetType(), Performer);

                effect.DoDirectEffect(this);
            }
        }

        public void Enqueue(EffectParameter effect, CombatingSkill skill)
        => _effectQueue.Enqueue(new ProvokeParameters(effect, skill));

        private readonly Queue<ProvokeParameters> _effectQueue;
        public CombatingEntity Performer { get; }

        public CombatingEntity Target => Performer;
        public bool IsCritical { get; private set; }
        public CombatingSkill UsedSkill { get; private set; }
        public ICollection<CombatingEntity> EffectTargets { get; private set; }

        private struct ProvokeParameters
        {
            public readonly EffectParameter Effect;
            public readonly CombatingSkill UsedSkill;
            public ProvokeParameters(EffectParameter effect, CombatingSkill skill)
            {
                Effect = effect;
                UsedSkill = skill;
            }
        }
    }
}
