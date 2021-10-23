using UnityEngine;

namespace CombatCamera
{
    public class UCombatUICameraInjector : MonoBehaviour
    {
        [SerializeField] private Camera combatUICamera;
        private void Awake()
        {
            CombatCameraSingleton.SwitchCombatUICamera(combatUICamera);
        }
    }
}
