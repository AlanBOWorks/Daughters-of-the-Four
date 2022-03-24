using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Entity
{
    public sealed class UCombatEntityBody : MonoBehaviour, ICombatEntityBody
    {
        [InfoBox("This component needs to be in Root",InfoMessageType.Warning)]
        [SerializeField] private Transform targetHolderForUI;

        public Transform GetUIHoverHolder() => targetHolderForUI;

    }


    public interface ICombatEntityBody
    {
        Transform GetUIHoverHolder();
    }
}
