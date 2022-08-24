using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
using UnityEngine;

namespace CombatSystem.Passives
{
    public interface ICombatPassive
    {
        EnumsEffect.ConcreteType GetEffectType();
        string GetPassiveEffectText();
    }

    public interface ICombatPassiveListener : ICombatEventListener
    {
        void OnPassiveTrigged(CombatEntity entity, ICombatPassive passive, ref float value);
    }
}
