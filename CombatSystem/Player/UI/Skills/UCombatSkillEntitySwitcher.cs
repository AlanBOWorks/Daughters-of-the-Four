using System;
using CombatSystem.Entity;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UCombatSkillEntitySwitcher : MonoBehaviour
    {
        [SerializeField]
        private TeamReferences references = new TeamReferences();
        private void Awake()
        {
            references.InstantiateElements();
            references.activeCount = references.Members.Length;
            references.HidePrefab();
        }


        [Serializable]
       private sealed class TeamReferences : TeamBasicStructureInstantiateHandler<UCombatSkillEntitySwitchButton>
       {
           
       }
    }
}
