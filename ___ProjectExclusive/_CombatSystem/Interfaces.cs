using Characters;
using NUnit.Framework;
using Skills;
using UnityEngine;

namespace _CombatSystem
{
    public interface ICharacterFaction<out T>
    {
        T PlayerFaction { get; }
        T EnemyFaction { get; }
    }
}
