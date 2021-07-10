using System;
using _Player;
using UnityEngine;

namespace _CombatSystem
{
    public class UCombatInjector : MonoBehaviour
    {
        [SerializeField] private SCombatParams combatParams = null;

        private void Awake()
        {
            CombatSystemSingleton.ParamsVariable = combatParams;
            PlayerEntitySingleton.DoSubscriptionsToCombatSystem();
        }

        private void Start()
        {
            Destroy(this);
        }
    }
}
