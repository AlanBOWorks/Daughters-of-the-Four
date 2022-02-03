using CombatSystem.Entity;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    public abstract class SEffect : ScriptableObject
    {
        public abstract void DoEffect(in CombatEntity performer, in CombatEntity target, in float effectValue);
    }
}
