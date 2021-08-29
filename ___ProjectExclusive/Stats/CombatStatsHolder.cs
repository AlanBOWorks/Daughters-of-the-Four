using System;
using _Team;
using Characters;
using CombatConditions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Stats
{

    public class CombatStatsHolder : IFullStatsData<float>, IStanceAllData<IBasicStatsData<float>>, ICombatHealthStats<float>,
        IStatsHolder<IBasicStats<float>>
    {
        public CombatStatsHolder(IFullStatsData<float> presetStats)
        {
            BaseStats = new CombatStatsFull(presetStats);
            BuffStats = new CombatStatsFull();
            BurstStats = new CombatStatsFull();

            _positionalStats = PositionalStats.GenerateProvisionalBasics();
            _multiplierStats = new MultiplierStats();

            _formulatedStats = new FormulatedStats(this);
            _formulatedStats.Inject(_positionalStats);
        }

        [ShowInInspector, HorizontalGroup("Base Stats"), PropertyOrder(-2), GUIColor(.4f, .8f, .6f)]
        public CombatStatsFull BaseStats { get; protected set; }
        /// <summary>
        /// This remains active for the whole fight
        /// </summary>
        [ShowInInspector, HorizontalGroup("Buff Stats"), GUIColor(.4f, .6f, .8f)]
        public CombatStatsFull BuffStats { get; protected set; }

        [ShowInInspector, HorizontalGroup("Buff Stats"), GUIColor(.2f, .3f, .6f)]
        public CombatStatsFull BurstStats { get; protected set; }


        // By design multiplier are consistent / Burst are temporary and specific;
        // making multipliers Burst type could be confusing.
        // Some buff could increase multiplier and/or a specific stat
        //
        // Eg: increasing the Heal power instead the Support)
        [ShowInInspector, TabGroup("Multipliers"), GUIColor(.7f,.6f,.4f)]
        private readonly MultiplierStats _multiplierStats;
        [ShowInInspector, TabGroup("Formulated"), GUIColor(.7f, .6f, .4f)]
        private readonly FormulatedStats _formulatedStats;
        [ShowInInspector, TabGroup("Positional"), GUIColor(.7f, .6f, .4f)]
        private readonly PositionalStats _positionalStats;

        [TitleGroup("Local stats"), PropertyOrder(-10)]
        public int ActionsLefts;

        public IBasicStats<float> GetStatsHolder(EnumStats.StatsType statsType)
        {
            return UtilsEnumStats.GetStatsHolder(this, statsType);
        }

        private IBasicStatsData<float> TeamStats => TeamData.GetCurrentStanceValue();
        public IMasterStats<float> GetMultiplierStats() => _multiplierStats;
        public IStanceData<IBasicStats<float>> GetPositionalStats() => _positionalStats;

        public CombatingTeam TeamData { get; set; }

        public void Injection(IStanceProvider positionStatsStanceProvider)
        {
            _positionalStats.Injection(positionStatsStanceProvider);
        }

        public void Initialization()
        {
            BaseStats.HealthPoints = BaseStats.MaxHealth;
            BaseStats.MortalityPoints = BaseStats.MaxMortalityPoints;
        }

        public bool IsAlive() => MortalityPoints > 0;
        public bool HasActionLeft() => ActionsLefts > 0;


        public void RefillInitiativeActions()
        {
            UtilsCombatStats.AddActionAmount(this, _formulatedStats.ActionsPerInitiative);
        }
        public void Revive()
        {
            UtilsCombatStats.HealToMax(this);
        }
        public void ResetBurst()
        {
            _formulatedStats.ResetBurst();
        }

        public void ResetInitiativePercentage()
        {
            _formulatedStats.ResetInitiative();
        }

        public float CalculateBaseAttackPower() => _formulatedStats.AttackPower;
        public float CalculateBaseStaticDamagePower() => _formulatedStats.StaticDamagePower;
        public float CalculateDamageReduction() => _formulatedStats.DamageReduction;


        public float HealthPoints
        {
            get => BaseStats.HealthPoints;
            set => BaseStats.HealthPoints = value;
        }
        public float ShieldAmount
        {
            get => BaseStats.ShieldAmount;
            set => BaseStats.ShieldAmount = value;
        }
        public float MortalityPoints
        {
            get => BaseStats.MortalityPoints;
            set => BaseStats.MortalityPoints = value;
        }

        public float AccumulatedStatic
        {
            get => BaseStats.AccumulatedStatic;
            set => BaseStats.AccumulatedStatic = value;
        }

        public float AttackPower => _multiplierStats.OffensivePower * _formulatedStats.AttackPower;
        public float DeBuffPower => _multiplierStats.OffensivePower * _formulatedStats.DeBuffPower;
        public float StaticDamagePower => _multiplierStats.OffensivePower * _formulatedStats.StaticDamagePower;

        public float HealPower => _multiplierStats.SupportPower * _formulatedStats.HealPower;
        public float BuffPower => _multiplierStats.SupportPower * _formulatedStats.BuffPower;
        public float BuffReceivePower => _multiplierStats.SupportPower * _formulatedStats.BuffReceivePower;

        public float MaxHealth => _multiplierStats.VitalityAmount * _formulatedStats.MaxHealth;
        public float MaxMortalityPoints => _multiplierStats.VitalityAmount * _formulatedStats.MaxMortalityPoints;
        public float DamageReduction => _multiplierStats.VitalityAmount * _formulatedStats.DamageReduction;
        public float DeBuffReduction => _multiplierStats.VitalityAmount * _formulatedStats.DeBuffReduction;

        public float Enlightenment => _multiplierStats.ConcentrationAmount * _formulatedStats.Enlightenment;
        public float CriticalChance => _multiplierStats.ConcentrationAmount * _formulatedStats.CriticalChance;
        public float SpeedAmount => _multiplierStats.ConcentrationAmount * _formulatedStats.SpeedAmount;


        public float HarmonyAmount => _formulatedStats.HarmonyAmount;
        public float InitiativePercentage => _formulatedStats.InitiativePercentage;
        public float ActionsPerInitiative => _formulatedStats.ActionsPerInitiative;

        public IBasicStatsData<float> AttackingStance => _positionalStats.AttackingStance;
        public IBasicStatsData<float> NeutralStance => _positionalStats.NeutralStance;
        public IBasicStatsData<float> DefendingStance => _positionalStats.DefendingStance;
        public IBasicStatsData<float> InAllStances => _formulatedStats;
        public IBasicStats<float> GetBuff() => BuffStats;
        public IBasicStats<float> GetBurst() => BurstStats;
        public IBasicStats<float> GetBase() => BaseStats;

        private class MultiplierStats : IMasterStats<float>
        {
            [ShowInInspector]
            public float OffensivePower { get; set; } = 1;
            [ShowInInspector]
            public float SupportPower { get; set; } = 1;
            [ShowInInspector]
            public float VitalityAmount { get; set; } = 1;
            [ShowInInspector]
            public float ConcentrationAmount { get; set; } = 1;
        }

        private class FormulatedStats : HashsetStatsHolder
        {
            [HorizontalGroup("Base Stats"), PropertyOrder(-2), GUIColor(.4f, .8f, .6f)]
            private CombatStatsFull BaseStats { get; }
            /// <summary>
            /// This remains active for the whole fight
            /// </summary>
            [HorizontalGroup("Buff Stats"), GUIColor(.4f, .6f, .8f)]
            private CombatStatsFull BuffStats { get; }

            [HorizontalGroup("Buff Stats"), GUIColor(.2f, .3f, .6f)]
            private CombatStatsFull BurstStats { get; }

            

            public FormulatedStats(CombatStatsHolder stats) :
                base(stats.BaseStats,stats.BuffStats,stats.BurstStats)
            {
                BaseStats = stats.BaseStats;
                BuffStats = stats.BuffStats;
                BurstStats = stats.BurstStats;
            }

            private IBasicStatsData<float> _positionalStats;
            public void Inject(PositionalStats positionalStats)
            {
                if (_positionalStats != null)
                    baseType.Remove(_positionalStats);

                _positionalStats = positionalStats;
                baseType.Add(_positionalStats);
            }

            public void ResetBurst()
            {
                buffType.Clear();
                buffType.Add(BurstStats);
            }

            public void ResetInitiative()
            {
                BaseStats.InitiativePercentage = 0;
                BurstStats.InitiativePercentage = 0;
            }
        }
    }



    /// <summary>
    /// It's the same than <see cref="CombatStatsFull"/> but its constructor
    /// allows to inject <seealso cref="IStatsUpgradable"/>
    /// </summary>
    [Serializable]
    public class PlayerCombatStats : CombatStatsFull
    {
        public PlayerCombatStats()
        { }

        public PlayerCombatStats(float overrideByDefault) : base(overrideByDefault)
        { }

        public PlayerCombatStats(PlayerCombatStats copyFrom)
        : base(copyFrom)
        { }

        public PlayerCombatStats(IPlayerCharacterStats playerCharacterStats)
        : this(playerCharacterStats.InitialStats, playerCharacterStats.GrowStats, playerCharacterStats.UpgradedStats)
        { }

        public PlayerCombatStats(
            IFullStatsData<float> initialStats,
            IFullStatsData<float> growStats, IStatsUpgradable currentUpgrades)
        {
            AttackPower = UtilsStats.GrowFormula(
                initialStats.AttackPower, growStats.AttackPower,
                currentUpgrades.OffensivePower);
            DeBuffPower = UtilsStats.GrowFormula(
                initialStats.DeBuffPower, growStats.DeBuffPower,
                currentUpgrades.OffensivePower);
            StaticDamagePower = UtilsStats.GrowFormula(
                initialStats.StaticDamagePower, growStats.StaticDamagePower,
                currentUpgrades.OffensivePower
            );


            HealPower = UtilsStats.GrowFormula(
                initialStats.HealPower, growStats.HealPower,
                currentUpgrades.SupportPower);
            BuffPower = UtilsStats.GrowFormula(
                initialStats.BuffPower, growStats.BuffPower,
                currentUpgrades.SupportPower);
            BuffReceivePower = UtilsStats.GrowFormula(
                initialStats.BuffReceivePower, growStats.BuffReceivePower,
                currentUpgrades.SupportPower);

            MaxHealth = UtilsStats.GrowFormula(
                initialStats.MaxHealth, growStats.MaxHealth,
                currentUpgrades.VitalityAmount);
            MaxMortalityPoints = UtilsStats.GrowFormula(
                initialStats.MaxMortalityPoints, growStats.MaxMortalityPoints,
                currentUpgrades.VitalityAmount);
            DamageReduction = UtilsStats.GrowFormula(
                initialStats.DamageReduction, growStats.DamageReduction,
                currentUpgrades.VitalityAmount);
            DeBuffReduction = UtilsStats.GrowFormula(
                initialStats.DeBuffReduction, growStats.DeBuffReduction,
                currentUpgrades.VitalityAmount);


            Enlightenment = UtilsStats.GrowFormula(
                initialStats.Enlightenment, growStats.Enlightenment,
                currentUpgrades.ConcentrationAmount);
            CriticalChance = UtilsStats.GrowFormula(
                initialStats.CriticalChance, growStats.CriticalChance,
                currentUpgrades.ConcentrationAmount);
            SpeedAmount = UtilsStats.GrowFormula(
                initialStats.SpeedAmount, growStats.SpeedAmount,
                currentUpgrades.ConcentrationAmount);


            HealthPoints = initialStats.HealthPoints + growStats.HealthPoints;
            ShieldAmount = initialStats.ShieldAmount + growStats.ShieldAmount;
            MortalityPoints = initialStats.MortalityPoints + growStats.MortalityPoints;
            HarmonyAmount = initialStats.HarmonyAmount + growStats.HarmonyAmount;
            InitiativePercentage = initialStats.InitiativePercentage + growStats.InitiativePercentage;

            ActionsPerInitiative = initialStats.ActionsPerInitiative +
                                   (int)(growStats.ActionsPerInitiative * currentUpgrades.ConcentrationAmount * GrowActionsModifier);
        }

        private const float GrowActionsModifier = .2f; //Each 5 upgrades
    }
}
