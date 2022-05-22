using System;
using CombatSystem.Entity;
using CombatSystem.Player;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public class USandBox : UTeamElementSpawner<GameObject>
    {
        protected override void OnCreateElement(CombatEntity entity, GameObject element,
            int index)
        {
        }
    }
}
