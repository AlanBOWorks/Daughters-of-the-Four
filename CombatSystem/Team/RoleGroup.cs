using System.Collections;
using System.Collections.Generic;
using CombatSystem.Entity;
using UnityEngine;

namespace CombatSystem.Team
{
    public sealed class RoleGroup : List<CombatEntity>
    {
        public RoleGroup(EnumTeam.Role type)
        {
            GroupType = type;
        }
        
        public readonly EnumTeam.Role GroupType;


        public CombatEntity GetMainMember() => this[0];


        public void SwitchToMain(in CombatEntity targetMember, out CombatEntity previousMainMember)
        {
            previousMainMember = this[0];
            SwitchToMain(in targetMember);
        }

        public void SwitchToMain(in CombatEntity targetMember)
        {
            Remove(targetMember);
            Insert(0, targetMember);
        }

    }
}
