using System;
using CombatSystem.Entity;
using CombatSystem.Player;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public class USandBox : MonoBehaviour
    {
        [SerializeField] private GameObject destroyTest;

        [Button,DisableInEditorMode]
        public void DestroyThing()
        {
            Destroy(destroyTest);
        }
    }
}
