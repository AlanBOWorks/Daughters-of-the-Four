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
        private IEnumerable<RandomNumber> _enumerable;
        [ShowInInspector] 
        private RandomNumber[] _numbers;

        [ShowInInspector] private RandomNumber _manualNumber1;
        [ShowInInspector] private RandomNumber _manualNumber2;
        [ShowInInspector] private RandomNumber _manualNumber3;

        [Button]
        private void Randomise(int amount = 4)
        {
            if (amount > 10) amount = 10;
            _numbers = new RandomNumber[amount];
            for (int i = 0; i < amount; i++)
            {
                _numbers[i] = new RandomNumber();
            }

            _manualNumber1 = new RandomNumber();
            _manualNumber2 = new RandomNumber();
            _manualNumber3 = new RandomNumber();
        }

        [Button]
        private void ByManual()
        {
            _enumerable = GenerateManualEnumerable();
        }

        [Button]
        private void ByArray()
        {
            _enumerable = GenerateEnumerable();
        }


        private IEnumerable<RandomNumber> GenerateEnumerable()
        {
            foreach (var randomNumber in _numbers)
            {
                yield return randomNumber;
            }
        }

        private IEnumerable<RandomNumber> GenerateManualEnumerable()
        {
            yield return _manualNumber1;
            yield return _manualNumber2;
            yield return _manualNumber3;
        }

        [Button]
        private void PrintNumbers()
        {
            int i = 0;
            foreach (RandomNumber number in _enumerable)
            {
                if (number == null)
                {
                    Debug.Log($"NULL {i}");
                    continue;
                }
                Debug.Log(number.RandomValue);
                i++;
            }
        }

        private sealed class RandomNumber
        {
            public RandomNumber()
            {
                RandomValue = Random.Range(0, 10);
            }
            public int RandomValue;
        }
    }
}
