using CombatSystem.Entity;
using JetBrains.Annotations;
using UnityEngine;

namespace CombatSystem.Team
{

    public sealed class TeamStructureIterationValues
    {
        public int IterationIndex { get; private set; }
        public int NotNullIndex { get; private set; }
        public bool IsPlayerElement { get; private set; }

        public void ResetState(bool isPlayer)
        {
            IsPlayerElement = isPlayer; 
            IterationIndex = 0;
            NotNullIndex = 0;
        }

        public void IncrementAsNull()
        {
            IterationIndex++;
            NotNullIndex++;
        }

        public void IncrementAsNotNull()
        {
            IterationIndex++;
        }
    }

    public interface IEntityElementInstantiationListener<T>
    {
        void OnCombatPreStarts();
        void OnFinishPreStarts();
        void OnIterationCall([NotNull] in T element, [CanBeNull] in CombatEntity entity, in TeamStructureIterationValues values);
    }
}
