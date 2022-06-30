using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CombatSystem.Player.UI
{
    [Serializable]
    public abstract class ShortCutCommandElementsSpawner<T> : ShortCutSkillElementsSpawner<T>, 
        IShortcutCommandStructureRead<T> 
        where T : Component
    {
        [Title("References")]
        [SerializeField] private T switchEntityElement;

        public T SwitchEntityShortCutElement => switchEntityElement;
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

            const int count = UtilsShortCuts.SKillShortCutsCount;
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
