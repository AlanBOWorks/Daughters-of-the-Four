using CombatSystem.Entity;
using CombatSystem.Skills.Effects;
using UnityEngine;

namespace CombatSystem.Passives
{
    public interface ICombatPassive
    {
        EnumsEffect.ConcreteType GetEffectType();
    }

    public interface ICombatPassiveListener
    {
        void OnPassiveTrigged(CombatEntity entity, ICombatPassive passive, ref float value);
    }
}
