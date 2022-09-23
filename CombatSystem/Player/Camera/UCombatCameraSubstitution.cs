using System;
using System.Collections;
using UnityEngine;

namespace CombatSystem.Player
{
    public class UCombatCameraSubstitution : MonoBehaviour
    {
        [SerializeField] private Camera combatCamera;


        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            CombatCameraHandler.DoMainCameraSubstitution(combatCamera);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            CombatCameraHandler.ResetMainCameraState();
        }
    }
}
