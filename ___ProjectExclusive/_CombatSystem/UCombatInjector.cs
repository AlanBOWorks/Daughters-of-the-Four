using System;
using UnityEngine;

namespace _CombatSystem
{
    public class UCombatInjector : MonoBehaviour
    {
        [SerializeField] private SCombatParams combatParams = null;

        private void Awake()
        {
            CombatSystemSingleton.ParamsVariable = combatParams;
        }

        private void Start()
        {
            Destroy(this);
        }
    }
}
