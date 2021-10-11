
using Sirenix.OdinInspector;

namespace Stats
{
    public class MathematicalStats : IBaseStatsRead<float>, IBehaviourStatsRead<IBaseStatsRead<float>>
    {
        public MathematicalStats(
            IBaseStatsRead<float> baseStats, IBaseStatsRead<float> buffStats, IBaseStatsRead<float> burstStats)
        {
            BaseStats = baseStats;
            BuffStats = buffStats;
            BurstStats = burstStats;
            MasterStats = new MasterStats(1);
        }

        public MathematicalStats(
            IMasterStatsRead<float> masterStatsInjection,
            IBaseStatsRead<float> baseStats, IBaseStatsRead<float> buffStats, IBaseStatsRead<float> burstStats)
        {
            BaseStats = baseStats;
            BuffStats = buffStats;
            BurstStats = burstStats;
            MasterStats = new MasterStats(masterStatsInjection);
        }

        public readonly IMasterStats<float> MasterStats;

        [ShowInInspector]
        public IBaseStatsRead<float> BaseStats { get; }
        [ShowInInspector]
        public IBaseStatsRead<float> BuffStats { get; }
        [ShowInInspector]
        public IBaseStatsRead<float> BurstStats { get; }

        public float Attack
            => MasterStats.Offensive 
            * UtilStats.StatFormulaStackingBuffs(BaseStats.Attack, BuffStats.Attack, BurstStats.Attack);
        public float Persistent
            => MasterStats.Offensive 
            * UtilStats.StatFormulaStackingBuffs(BaseStats.Persistent, BuffStats.Persistent, BurstStats.Persistent);
        public float Debuff
            => MasterStats.Offensive 
            * UtilStats.StatFormulaPercentageAddition(BaseStats.Debuff, BuffStats.Debuff, BurstStats.Debuff);

        public float Heal
            => MasterStats.Support 
            * UtilStats.StatFormulaStackingBuffs(BaseStats.Heal, BuffStats.Heal, BurstStats.Heal);
        public float Buff
            => MasterStats.Support 
            * UtilStats.StatFormulaPercentageAddition(BaseStats.Buff, BuffStats.Buff, BurstStats.Buff);
        public float ReceiveBuff
            => MasterStats.Support 
            * UtilStats.StatFormulaPercentageAddition(BaseStats.ReceiveBuff, BuffStats.ReceiveBuff, BurstStats.ReceiveBuff);


        // Mortality is the exception of all rules since it determines when a character is defeated (that's a special case)
        public float MaxMortality => BaseStats.MaxMortality;
        public float MaxHealth
            => MasterStats.Vitality 
            * UtilStats.StatFormulaStackingBuffs(BaseStats.MaxHealth, BuffStats.MaxHealth, BurstStats.MaxHealth);
        public float DebuffResistance
            => MasterStats.Vitality
            * UtilStats.StatFormulaPercentageAddition(BaseStats.DebuffResistance, BuffStats.DebuffResistance, BurstStats.DebuffResistance);
        public float DamageResistance
            => MasterStats.Vitality
            * UtilStats.StatFormulaStackingBuffs(BaseStats.DamageResistance, BuffStats.DamageResistance, BurstStats.DamageResistance);

        public float InitiativeSpeed
            => MasterStats.Concentration
            * UtilStats.StatFormulaStackingBuffs(BaseStats.InitiativeSpeed, BuffStats.InitiativeSpeed, BurstStats.InitiativeSpeed);
        public float Critical
            => MasterStats.Concentration
            * UtilStats.StatFormulaPercentageAddition(BaseStats.Critical, BuffStats.Critical, BurstStats.Critical);
        public float InitialInitiative
            => MasterStats.Concentration
            * UtilStats.StatFormulaPercentageAddition(BaseStats.InitialInitiative, BuffStats.InitialInitiative, BurstStats.InitialInitiative);
        public float ActionsPerSequence
            => MasterStats.Concentration
            * UtilStats.StatFormulaPercentageAddition(BaseStats.ActionsPerSequence, BuffStats.ActionsPerSequence, BurstStats.ActionsPerSequence);
    }

}
