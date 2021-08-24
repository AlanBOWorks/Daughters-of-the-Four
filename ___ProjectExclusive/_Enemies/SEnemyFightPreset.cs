using _CombatSystem;
using _Enemies;
using _Team;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ___ProjectExclusive._Enemies
{
    [CreateAssetMenu(fileName = "_ Fight Preset _ N [Combat Preset]",
        menuName = "Combat/Fight Preset")]
    public class SEnemyFightPreset : ScriptableObject, 
        ICharacterArchetypesData<SEnemyCharacterEntityVariable>,
        ICombatConditionHolder
    {
        [TitleGroup("Enemies")] 
        [SerializeField]
        private STeamControlStatsPreset statsPreset;
        public STeamControlStatsPreset StatsPreset => statsPreset;


        [TitleGroup("Enemies")]
        [SerializeField] private SEnemyCharacterEntityVariable frontLiner;
        [SerializeField] private SEnemyCharacterEntityVariable midLiner;
        [SerializeField] private SEnemyCharacterEntityVariable backLiner;

        [TitleGroup("Controller")] 
        [SerializeField]
        private SCombatEnemyController combatController = null;

        [TitleGroup("Conditionals")] 
        [SerializeField] private SCombatConditions winCondition;
        [SerializeField] private SCombatConditions loseCondition;



        public SEnemyCharacterEntityVariable Vanguard => frontLiner;
        public SEnemyCharacterEntityVariable Attacker => midLiner;
        public SEnemyCharacterEntityVariable Support => backLiner;


        public bool GetWinCondition() => winCondition.GetTriggerCondition();
        public bool GetLoseCondition() => loseCondition.GetTriggerCondition();

        public bool IsWinConditionValid() => winCondition != null;
        public bool IsLoseConditionValid() => loseCondition != null;

        public ICombatEnemyController GetCombatController() => combatController;
    }
}
