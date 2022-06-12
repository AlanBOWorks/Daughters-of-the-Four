using System;
using CombatSystem.Entity;
using CombatSystem.Player;
using CombatSystem.Team;
using MPUIKIT;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utils
{
    public class USandBox : MonoBehaviour
    {
        [SerializeField] private MPImage blackImage;

        private bool _shitHappen;
        private void Update()
        {
            if(_shitHappen)
                DoDisable();
            else
                DoEnable();
        }

        private void DoEnable()
        {
            if (Keyboard.current.spaceKey.wasReleasedThisFrame)
            {
                blackImage.gameObject.SetActive(true);
                _shitHappen = true;
            }
        }

        private void DoDisable()
        {
            if (Keyboard.current.spaceKey.wasReleasedThisFrame)
            {
                blackImage.gameObject.SetActive(false);
                _shitHappen = false;
            }
        }
    }
}
