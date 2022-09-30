using System;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem.Skills
{
    [Serializable]
    public class SkillTargetingStructure<T> : ISkillTargetingStructureRead<T>
    {
        [SerializeField] protected T targetSingle;
        [SerializeField] protected T targetLine;
        [SerializeField] protected T targetTeam;

        public T TargetSingleType => targetSingle;
        public T TargetLineType => targetLine;
        public T TargetTeamType => targetTeam;

        [SerializeField] protected T performerSingle;
        [SerializeField] protected T performerLine;
        [SerializeField] protected T performerTeam;

        public T PerformerSingleType => performerSingle;
        public T PerformerLineType => performerLine;
        public T PerformerTeamType => performerTeam;

        public T GetElement(EnumsEffect.TargetType type)
        {
            return UtilsEffect.GetElement(type, this);
        }

        public IEnumerable<T> GetEnumerable()
        {
            yield return targetSingle;
            yield return targetLine;
            yield return targetTeam;
            yield return performerSingle;
            yield return performerLine;
            yield return performerTeam;
        }

        public IEnumerable<KeyValuePair<EnumsEffect.TargetType, T>> GetEnumerableWithType()
        {
            yield return new KeyValuePair<EnumsEffect.TargetType, T>(EnumsEffect.TargetType.Target,targetSingle);
            yield return new KeyValuePair<EnumsEffect.TargetType, T>(EnumsEffect.TargetType.TargetLine, targetLine);
            yield return new KeyValuePair<EnumsEffect.TargetType, T>(EnumsEffect.TargetType.TargetTeam, targetTeam);
            yield return new KeyValuePair<EnumsEffect.TargetType, T>(EnumsEffect.TargetType.Performer, performerSingle);
            yield return new KeyValuePair<EnumsEffect.TargetType, T>(EnumsEffect.TargetType.PerformerLine, performerLine);
            yield return new KeyValuePair<EnumsEffect.TargetType, T>(EnumsEffect.TargetType.PerformerTeam, performerTeam);
        }

    }

    public class SkillTargetingStructureClass<T> : SkillTargetingStructure<T> where T : new()
    {
        public SkillTargetingStructureClass()
        {
            targetSingle = new T();
            targetLine = new T();
            targetTeam = new T();
            performerSingle = new T();
            performerLine = new T();
            performerTeam = new T();
        }
    }


}
