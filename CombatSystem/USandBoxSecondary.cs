using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CombatSystem
{
    public class USandBoxSecondary : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private bool _logShit;
        public void OnPointerEnter(PointerEventData eventData)
        {
            _logShit = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _logShit = false;
        }

        private void Update()
        {
            if(!_logShit) return;

            Debug.Log("Well Shit");
        }
    }
}
