using CombatSystem.Entity;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    public abstract class SEffect : ScriptableObject, IEffect
    {
        public abstract void DoEffect(in CombatEntity performer, in CombatEntity target, in float effectValue);
        public abstract string EffectTag { get; }

        public const string EffectPrefix = "Effect";
    }
}
