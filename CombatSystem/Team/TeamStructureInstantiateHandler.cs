using System;
using UnityEngine;

namespace CombatSystem.Team
{
    /// <summary>
    /// Extends from [<seealso cref="FullPositionGroup{T}"/>]; also Instantiates prefabs object to a parent Transform
    /// on Awake
    /// </summary>
    [Serializable]
    internal class TeamStructureInstantiateHandler<T> : FullPositionGroup<T> where T : MonoBehaviour
    {
        [SerializeField] private Transform instantiationParent;
        [SerializeField] private T instantiationPrefab;

        [SerializeField] private bool hidePrefabGameObject = true;

        public void InstantiateElements()
        {
            VanguardType = UnityEngine.Object.Instantiate(instantiationPrefab, instantiationParent);
            AttackerType = UnityEngine.Object.Instantiate(instantiationPrefab, instantiationParent);
            SupportType = UnityEngine.Object.Instantiate(instantiationPrefab, instantiationParent);
            FlexType = UnityEngine.Object.Instantiate(instantiationPrefab, instantiationParent);

            if(hidePrefabGameObject)
                instantiationPrefab.gameObject.SetActive(false);

        }
    }
}
