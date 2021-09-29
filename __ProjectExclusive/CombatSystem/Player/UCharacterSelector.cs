using System;
using CombatEntity;
using CombatSystem;
using Stats;
using CombatTeam;
using UnityEngine;

namespace __ProjectExclusive.Player
{
    public class UCharacterSelector : MonoBehaviour, ITeamStructureRead<SCombatEntityUpgradeablePreset>
    {
        [SerializeField] private SCombatEntityUpgradeablePreset vanguard;
        [SerializeField] private SCombatEntityUpgradeablePreset attacker;
        [SerializeField] private SCombatEntityUpgradeablePreset support;

#if UNITY_EDITOR
        [SerializeField] private bool injectInAwake = true;
        private void Awake()
        {
            if(injectInAwake)
                PlayerCombatSingleton.CharactersHolder.InjectMembers(this);
        }
#endif

        public SCombatEntityUpgradeablePreset Vanguard => vanguard;
        public SCombatEntityUpgradeablePreset Attacker => attacker;
        public SCombatEntityUpgradeablePreset Support => support;
    }
}
