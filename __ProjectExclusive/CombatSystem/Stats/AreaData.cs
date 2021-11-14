using System;
using CombatEntity;
using CombatSystem;
using CombatTeam;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class AreaData
    {
        public AreaData()
        {}

        public AreaData(EnumStats.RangeType rangeType, EnumTeam.Role role)
        {
            this.rangeType = rangeType;
            this.role = role;
        }

        [SerializeField] protected EnumStats.RangeType rangeType;
        [SerializeField] protected EnumTeam.Role role;

        public EnumStats.RangeType GetRangeType() => rangeType;
        public EnumTeam.Role GetRole() => role;
        public EnumTeam.TeamPosition GetPositioning() => EnumTeam.ParseEnum(role);
    }

}
