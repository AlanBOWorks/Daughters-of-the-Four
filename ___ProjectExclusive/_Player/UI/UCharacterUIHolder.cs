using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Player
{
    public class UCharacterUIHolder : MonoBehaviour, IEntitySwitchListener
    {

        [TitleGroup("References")]
        [SerializeField] 
        private UCharacterOverTargetTooltip targetTooltip = null;
        public UCharacterOverTooltipBase GetTargetToolTip() => targetTooltip;
        [SerializeField] 
        private UCharacterOverFeetTooltip feetTooltip = null;
        public UCharacterOverTooltipBase GetFeetTooltip() => feetTooltip;

        [SerializeField] 
        private UTargetButton targetButton = null;
        public UTargetButton TargetButton => targetButton;   

        private Camera _canvasCamera;

        public void Injection(Camera projectionCamera)
        {
            _canvasCamera = projectionCamera;
            targetTooltip.Injection(projectionCamera);
            feetTooltip.Injection(projectionCamera);
        }
        
        public void RePosition(Vector3 worldPosition)
        {
            transform.position = _canvasCamera.WorldToScreenPoint(worldPosition);
        }

        public void OnEntitySwitch(CombatingEntity entity)
        {
            targetTooltip.OnEntitySwitch(entity);
            targetButton.OnEntitySwitch(entity);
            feetTooltip.OnEntitySwitch(entity);
        }
    }
}
