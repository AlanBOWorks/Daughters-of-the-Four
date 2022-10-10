using System;
using System.Collections;
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

        public T this[EnumsEffect.TargetType key]
        {
            get => UtilsEffect.GetElement(key, this);
            set => SetElement(key, value);
        }


        public void SetElement(EnumsEffect.TargetType type, T element)
        {
            switch (type)
            {
                case EnumsEffect.TargetType.Target:
                    targetSingle = element;
                    break;
                case EnumsEffect.TargetType.TargetLine:
                    targetLine = element;
                    break;
                case EnumsEffect.TargetType.TargetTeam:
                    targetTeam = element;
                    break;
                case EnumsEffect.TargetType.Performer:
                    performerSingle = element;
                    break;
                case EnumsEffect.TargetType.PerformerLine:
                    performerLine = element;
                    break;
                case EnumsEffect.TargetType.PerformerTeam:
                    performerTeam= element;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
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

        public IEnumerable<EnumsEffect.TargetType> GetTargetingEnumerable()
        {
            yield return EnumsEffect.TargetType.Target;
            yield return EnumsEffect.TargetType.TargetLine;
            yield return EnumsEffect.TargetType.TargetTeam;
            yield return EnumsEffect.TargetType.Performer;
            yield return EnumsEffect.TargetType.PerformerLine;
            yield return EnumsEffect.TargetType.PerformerTeam;
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
        public IEnumerator<KeyValuePair<EnumsEffect.TargetType, T>> GetEnumeratorWithType()
        {
            yield return new KeyValuePair<EnumsEffect.TargetType, T>(EnumsEffect.TargetType.Target, targetSingle);
            yield return new KeyValuePair<EnumsEffect.TargetType, T>(EnumsEffect.TargetType.TargetLine, targetLine);
            yield return new KeyValuePair<EnumsEffect.TargetType, T>(EnumsEffect.TargetType.TargetTeam, targetTeam);
            yield return new KeyValuePair<EnumsEffect.TargetType, T>(EnumsEffect.TargetType.Performer, performerSingle);
            yield return new KeyValuePair<EnumsEffect.TargetType, T>(EnumsEffect.TargetType.PerformerLine, performerLine);
            yield return new KeyValuePair<EnumsEffect.TargetType, T>(EnumsEffect.TargetType.PerformerTeam, performerTeam);
        }
    }


    public class SkillTargetingStructureClass<T> : SkillTargetingStructure<T> where T : new()
    {
        public SkillTargetingStructureClass(bool instantiateElements)
        {
            if(!instantiateElements) return;

            targetSingle = new T();
            targetLine = new T();
            targetTeam = new T();
            performerSingle = new T();
            performerLine = new T();
            performerTeam = new T();
        }

    }

    public class MonoTargetingStructure<T> : SkillTargetingStructure<T> where T : Component
    {

        public IEnumerable<T> ClearAndGetEnumerable()
        {
            yield return targetSingle;
            yield return targetLine;
            yield return targetTeam;
            yield return performerSingle;
            yield return performerLine;
            yield return performerTeam;

            targetSingle = null;
            targetLine = null;
            targetTeam = null;
            performerSingle = null;
            performerLine = null;
            performerTeam = null;
        }
    }

}
