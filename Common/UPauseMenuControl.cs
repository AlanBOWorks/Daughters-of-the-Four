using System;
using UnityEngine;

namespace Common
{
    public class UPauseMenuControl : MonoBehaviour
    {
        [SerializeField] private GameObject menuRoot;

        private PlayerCommonControlValues _playerCommonControl;

        private void Awake()
        {
            _playerCommonControl = PlayerCommonControlValuesSingleton.Values;
            _playerCommonControl.GamePauseReferenceObject = menuRoot;
        }

        public bool IsPauseActive() => menuRoot.activeSelf;

        public void ShowMenu()
        {
            if(IsPauseActive()) return;
            
            menuRoot.SetActive(true);
            _playerCommonControl.IsGamePaused = true;
        }

        public void HideMenu()
        {
            if(!IsPauseActive()) return;

            menuRoot.SetActive(false);
            _playerCommonControl.IsGamePaused = false;
        }
    }
}
