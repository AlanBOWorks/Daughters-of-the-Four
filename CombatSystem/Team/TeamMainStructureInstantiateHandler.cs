using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    /// <summary>
    /// Should instantiates prefabs object to a parent Transform on Awake
    /// </summary>
    [Serializable]
    public class TeamMainStructureInstantiateHandler<T> : TeamMainStructureInstantiateHandlerBasic<T> where T : MonoBehaviour
    {
        public override void InstantiateElements()
        {
            base.InstantiateElements();

          
            ITeamMainStructureInstantiationListener<T>[] listeners;
            if (instantiationParent != null)
                listeners = instantiationParent.GetComponents<ITeamMainStructureInstantiationListener<T>>();
            else
                return;

            if (listeners == null) return;
            CallListeners(VanguardType, EnumTeam.Role.Vanguard);
            CallListeners(AttackerType, EnumTeam.Role.Attacker);
            CallListeners(SupportType, EnumTeam.Role.Support);
            CallListeners(FlexType, EnumTeam.Role.Flex);

            void CallListeners(T element, EnumTeam.Role role)
            {
                foreach (var listener in listeners)
                {
                    listener.OnInstantiateElement(element, role);
                }
            }
        }

    }

    [Serializable]
    public class TeamMainStructureInstantiateHandlerBasic<T> : FlexPositionMainGroupStructure<T> where T : MonoBehaviour
    {
        [InfoBox("Listeners will be called in this parent")]
        [SerializeField]
        protected Transform instantiationParent;
        [SerializeField] protected T instantiationPrefab;

        [ReadOnly]
        public int activeCount;

        public virtual void InstantiateElements()
        {
            if (!instantiationPrefab) return;

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
            if (!instantiationPrefab) return;
            instantiationPrefab.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Event listener called in [<see cref="TeamMainStructureInstantiateHandler{T}.InstantiateElements"/>];
    /// </summary>
    public interface ITeamMainStructureInstantiationListener<in T>
    {
        void OnInstantiateElement(T element, EnumTeam.Role role);
    }
}
