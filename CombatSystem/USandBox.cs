using System;
using CombatSystem.Luck;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public class USandBox : UTeamFullGroupStructure<Transform>
    {
        [Button]
        private void DebugEnum()
        {
            var structure = new TeamFullGroupStructure<float>();
            var enumerable = UtilsTeam.GetEnumerable(structure, this);
            foreach (var keyValuePair in enumerable)
            {
                string log = (keyValuePair.Value) ? keyValuePair.Value.name : "NULL";
                Debug.Log(keyValuePair.Key + " / " + log);
            }
        }
    }

}
