using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Player;
using CombatSystem.Skills;
using CombatSystem.Team;
using MPUIKIT;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Utils
{
    public class USandBox : MonoBehaviour
    {
        [ShowInInspector]
        private CheckNull _toNull;
        [ShowInInspector]
        private CheckNull _toCheck;

        public float someValue;
        public float secondValue;

        [Button]
        private void Prepare()
        {
            _toNull = new CheckNull();
            Handle(in _toNull);
            void Handle(in CheckNull reference)
            {
                _toCheck = reference;
            }

            someValue = Random.value;

            HandleFloat(in someValue);
            void HandleFloat(in float value)
            {
                secondValue = value;
            }
        }

        [Button]
        private void ToNull()
        {
            _toNull = null;

            someValue = -1;
        }


        private sealed class CheckNull
        {
            public CheckNull()
            {
                Value = Random.value;
            }
            public float Value;
        }
    }
}
