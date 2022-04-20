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

        public void InstantiateElements()
        {
            if(!instantiationParent || !instantiationPrefab) return;

            VanguardType = Instantiate();
            AttackerType = Instantiate();
            SupportType = Instantiate();
            FlexType = Instantiate();

            T Instantiate() => UnityEngine.Object.Instantiate(instantiationPrefab, instantiationParent);
        }

        public void HidePrefab()
        {
            if(!instantiationPrefab) return;
            instantiationPrefab.gameObject.SetActive(false);
        }
    }

    [Serializable]
    public class TeamOffStructureInstantiateHandler<T> : TeamOffGroupStructure<T> where T : MonoBehaviour
    {
        [SerializeField] private Transform instantiationParent;
        [SerializeField] private T instantiationPrefab;

        [ReadOnly]
        public int activeCount;

        public void InstantiateElements()
        {
            if(!instantiationParent || !instantiationPrefab) return;

            //todo optimize this by pooling
            SecondaryVanguardElement = Instantiate();
            SecondaryAttackerElement = Instantiate();
            SecondarySupportElement = Instantiate();
            SecondaryFlexElement = Instantiate();

            ThirdVanguardElement = Instantiate();
            ThirdAttackerElement = Instantiate();
            ThirdSupportElement = Instantiate();
            ThirdFlexElement = Instantiate();

            T Instantiate() => UnityEngine.Object.Instantiate(instantiationPrefab, instantiationParent);
        }

        public void HidePrefab()
        {
            if(!instantiationPrefab) return;
            instantiationPrefab.gameObject.SetActive(false);
        }
    }

    [Serializable]
    public class TeamBasicStructureInstantiateHandler<T> : TeamBasicGroupStructure<T> where T : MonoBehaviour
    {
        [SerializeField] private Transform instantiationParent;
        [SerializeField] private T instantiationPrefab;

        [ReadOnly]
        public int activeCount;

        public void InstantiateElements()
        {
            if (!instantiationParent || !instantiationPrefab) return;

            VanguardType = Instantiate();
            AttackerType = Instantiate();
            SupportType = Instantiate();

            T Instantiate() => UnityEngine.Object.Instantiate(instantiationPrefab, instantiationParent);
        }

        public void HidePrefab()
        {
            if (!instantiationPrefab) return;
            instantiationPrefab.gameObject.SetActive(false);
        }
    }
}
