using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    /// <summary>
    /// Extends from [<seealso cref="FullPositionMainGroupStructureStructure{T}"/>]; also Instantiates prefabs object to a parent Transform
    /// on Awake
    /// </summary>
    [Serializable]
    public class TeamMainStructureInstantiateHandler<T> : FlexPositionMainGroupStructure<T> where T : MonoBehaviour
    {
        [SerializeField] private Transform instantiationParent;
        [SerializeField] private T instantiationPrefab;

        [ReadOnly]
        public int activeCount;

        public virtual void InstantiateElements()
        {
            if(!instantiationParent || !instantiationPrefab) return;

            VanguardType = OnInstantiation(0);
            AttackerType = OnInstantiation(1);
            SupportType = OnInstantiation(2);
            FlexType = OnInstantiation(3);

        }

        protected virtual T OnInstantiation(int index)
        {
            return UnityEngine.Object.Instantiate(instantiationPrefab, instantiationParent);
        }

        public void HidePrefab()
        {
            if(!instantiationPrefab) return;
            instantiationPrefab.gameObject.SetActive(false);
        }
    }

}
