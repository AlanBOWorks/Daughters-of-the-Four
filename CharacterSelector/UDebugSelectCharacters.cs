using CombatSystem.Entity;
using CombatSystem.Player;
using CombatSystem.Team;
using ExplorationSystem;
using UnityEngine;

namespace CharacterSelector
{
    public class UDebugSelectCharacters : MonoBehaviour, ITeamFlexStructureRead<ICombatEntityProvider>
    {
        [SerializeField] private SPlayerPreparationEntity vanguardEntity;
        [SerializeField] private SPlayerPreparationEntity attackerEntity;
        [SerializeField] private SPlayerPreparationEntity supportEntity;
        [SerializeField] private SPlayerPreparationEntity flexEntity;


        // Start is called before the first frame update
        private void Start()
        {
            var singleton = PlayerExplorationSingleton.Instance;
            if (singleton.SelectedTeam != null) return;

            singleton.InjectTeam(this);
        }

        public ICombatEntityProvider VanguardType => vanguardEntity;
        public ICombatEntityProvider AttackerType => attackerEntity;
        public ICombatEntityProvider SupportType => supportEntity;
        public ICombatEntityProvider FlexType => flexEntity;
    }
}
