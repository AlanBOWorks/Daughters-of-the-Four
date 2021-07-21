using System;
using _CombatSystem;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;
using UnityEngine.UI;

namespace ___ProjectExclusive
{
    public class USandboxDebug : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private Button testButton;

        private void Awake()
        {
            testButton.onClick.AddListener(TestCode);
        }

        [ShowInInspector]
        private bool _wasPaused;
        private void TestCode()
        {
            var tempoHandler = CombatSystemSingleton.TempoHandler;
            if (_wasPaused)
                tempoHandler.DoResume();
            else
            {
                tempoHandler.DoPause();
            }

            _wasPaused = !_wasPaused;
        }
#endif
    }
}
