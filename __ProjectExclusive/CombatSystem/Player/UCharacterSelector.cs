using System;
using CombatEntity;
using CombatSystem;
using Stats;
using CombatTeam;
using UnityEngine;

namespace __ProjectExclusive.Player
{
    public class UCharacterSelector : MonoBehaviour
    {
        [SerializeField] private PlayerCharactersSelector selector = new PlayerCharactersSelector();

#if UNITY_EDITOR
        [SerializeField] private bool injectInAwake = true;
        private void Awake()
        {
            if(injectInAwake)
                selector.InjectInSingleton();
        }
#endif

    }

    [Serializable]
    public class PlayerCharactersSelector : ITeamRoleStructureRead<SCombatEntityUpgradeablePreset>
    {
        [SerializeField] private SCombatEntityUpgradeablePreset vanguard;
        [SerializeField] private SCombatEntityUpgradeablePreset attacker;
        [SerializeField] private SCombatEntityUpgradeablePreset support;


        public SCombatEntityUpgradeablePreset Vanguard => vanguard;
        public SCombatEntityUpgradeablePreset Attacker => attacker;
        public SCombatEntityUpgradeablePreset Support => support;

        public void InjectInSingleton()
        {
            PlayerCombatSingleton.CharactersHolder.InjectMembers(this);
        }
    }
}
