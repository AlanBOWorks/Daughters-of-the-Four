using CombatSystem.Entity;
using JetBrains.Annotations;
using UnityEngine;

namespace CombatSystem.Team
{

    public sealed class TeamStructureIterationValues
    {
        public int IterationIndex;
        public int NotNullIndex;
        public bool IsPlayerElement;
    }

    public interface IEntityElementInstantiationListener<T>
    {
        void OnCombatPreStarts();
        void OnFinishPreStarts();
        void OnIterationCall([NotNull] in T element, [CanBeNull] in CombatEntity entity, in TeamStructureIterationValues values);
    }
}
