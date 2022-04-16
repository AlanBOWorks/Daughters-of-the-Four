using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Entity
{
    public class UCombatEntityBody : MonoBehaviour, ICombatEntityBody
    {
        [InfoBox("This component needs to be in Root",InfoMessageType.Warning)]
        [SerializeField] private Transform targetHolderForUI;

        public Transform GetUIHoverHolder() => targetHolderForUI;
        public void InjectPositionReference(in Transform reference)
        {
            //todo
        }
    }


    public interface ICombatEntityBody
    {
        Transform GetUIHoverHolder();
        void InjectPositionReference(in Transform reference);
    }
}
