using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace CombatSystem.Player.UI
{
    [Serializable]
    public abstract class ShortCutStanceElementsSpawner<T> : ISwitchStanceShortcutCommandStructureRead<T>
        where T : Component
    {
        [Title("Instantiation")]
        [SerializeField] private Transform skillSpawnOnParent;
        [SerializeField] private T stancePrefab;
        [SerializeField] private bool enableOnInstantiation;

        private T[] _stanceElements;

        public void DoInstantiations(Action<T, int> onInstantiationCallback = null)
        {
            InstantiateSkills(onInstantiationCallback);
        }

        public void DisablePrefab()
        {
            stancePrefab.gameObject.SetActive(false);
        }

        private void InstantiateSkills(Action<T, int> onInstantiation)
        {
            if (!stancePrefab) return;

            const int count = EnumShortCuts.StanceShortcutsCount;
            _stanceElements = new T[count];
            for (int i = 0; i < count; i++)
            {
                var element =
                    _stanceElements[i] = Object.Instantiate(stancePrefab, skillSpawnOnParent);
                element.gameObject.SetActive(enableOnInstantiation);

                onInstantiation?.Invoke(element, i);
            }
        }

        public T GetSkillPrefab() => stancePrefab;
        public T[] GetElements() => _stanceElements;

        public T SupportStanceShortCutElement => _stanceElements[EnumShortCuts.SupportStanceIndex];
        public T AttackStanceShortCutElement => _stanceElements[EnumShortCuts.AttackerStanceIndex];
        public T DefendStanceShortCutElement => _stanceElements[EnumShortCuts.DefendStanceIndex];
    }

    [Serializable]
    public abstract class ShortCutSkillElementsSpawner<T> : ISkillShortcutCommandStructureRead<T> where T : Component
    {
        [Title("Instantiation")]
        [SerializeField] private Transform skillSpawnOnParent;
        [SerializeField] private T skillPrefab;
        [SerializeField] private bool enableOnInstantiation;

        private T[] _skillElements;

        public void DoInstantiations(Action<T,int> onInstantiationCallback = null)
        {
            InstantiateSkills(onInstantiationCallback);
        }

        public void DisablePrefab()
        {
            skillPrefab.gameObject.SetActive(false);
        }

        private void InstantiateSkills(Action<T,int> onInstantiation)
        {
            if (!skillPrefab) return;

            const int count = EnumShortCuts.SkillShortcutsCount;
            _skillElements = new T[count];
            for (int i = 0; i < count; i++)
            {
                var element =
                _skillElements[i] = Object.Instantiate(skillPrefab, skillSpawnOnParent);
                element.gameObject.SetActive(enableOnInstantiation);

                onInstantiation?.Invoke(element,i);
            }
        }

        public T GetSkillPrefab() => skillPrefab;
        public T[] GetElements() => _skillElements;
        public IReadOnlyList<T> SkillShortCuts => _skillElements;
    }
}
