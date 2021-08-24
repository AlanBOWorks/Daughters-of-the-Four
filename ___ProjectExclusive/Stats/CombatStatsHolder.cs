using System;
using _Team;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Stats
{

    public class CombatStatsHolder : IFullStatsData
    {
        public CombatStatsHolder(IFullStatsData presetStats)
        {
            BaseStats = new CombatStatsFull(presetStats);
            BuffStats = new CombatStatsFull();
            BurstStats = new CombatStatsFull();

            PositionalStats = PositionalStats.GenerateProvisionalBasics();
            _formulatedStats = new FormulatedStats(this);
            _multiplierStats = new MultiplierStats();
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

        public PositionalStats PositionalStats { get; protected set; }

        // By design multiplier are consistent / Burst are temporary and specific;
        // making multipliers Burst type could be confusing.
        // Some buff could increase multiplier and/or a specific stat
        //
        // Eg: increasing the Heal power instead the Support)
        private readonly MultiplierStats _multiplierStats;

        private readonly FormulatedStats _formulatedStats;
        [TitleGroup("Local stats"), PropertyOrder(-10)]
        public int ActionsLefts;


        private IBasicStatsData TeamStats => TeamData.GetCurrentStanceValue();
        public IStatsPrimordial GetMultiplierStats() => _multiplierStats;

        public CombatingTeam TeamData
        {
            get => _formulatedStats.TeamData;
            set => _formulatedStats.TeamData = value;
        }

        public void Injection(IStanceProvider positionStatsStanceProvider)
        {
            PositionalStats.Injection(positionStatsStanceProvider);
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
            UtilsCombatStats.AddActionAmount(this, _formulatedStats.GetActionsPerInitiative());
        }
        public void Revive()
        {
            UtilsCombatStats.HealToMax(this);
        }


        public float CalculateBaseAttackPower()
        {
            return BaseStats.AttackPower + TeamStats.GetAttackPower();
        }
        public float CalculateBaseStaticDamagePower()
        {
            return BaseStats.StaticDamagePower + TeamStats.GetStaticDamagePower();
        }
        public float CalculateDamageReduction()
        {
            return BaseStats.DamageReduction + TeamStats.GetDamageReduction();
        }

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

        public float HarmonyAmount
        {
            get => BaseStats.HarmonyAmount + TeamStats.GetHarmonyAmount() + BurstStats.HarmonyAmount;
            set => BaseStats.HarmonyAmount = value;
        }

        public float InitiativePercentage
        {
            get => BaseStats.InitiativePercentage + TeamStats.GetInitiativePercentage()
                                                  + BuffStats.InitiativePercentage
                                                  + BurstStats.InitiativePercentage;
            set => BaseStats.InitiativePercentage = value;
        }

        public int ActionsPerInitiative
        {
            get => BaseStats.ActionsPerInitiative + TeamStats.GetActionsPerInitiative()
                                                  + BuffStats.ActionsPerInitiative
                                                  + BurstStats.ActionsPerInitiative;
            set => BaseStats.ActionsPerInitiative = value;
        }



        public float GetAttackPower() => _multiplierStats.OffensivePower * _formulatedStats.GetAttackPower();
        public float GetDeBuffPower() => _multiplierStats.OffensivePower * _formulatedStats.GetDeBuffPower();
        public float GetStaticDamagePower() => _multiplierStats.OffensivePower * _formulatedStats.GetStaticDamagePower();
        public float GetHealPower() => _multiplierStats.SupportPower * _formulatedStats.GetHealPower();
        public float GetBuffPower() => _multiplierStats.SupportPower * _formulatedStats.GetBuffPower();
        public float GetBuffReceivePower() => _multiplierStats.SupportPower * _formulatedStats.GetBuffReceivePower();
        public float GetMaxHealth() => _multiplierStats.VitalityAmount * _formulatedStats.GetMaxHealth();
        public float GetMaxMortalityPoints() => _multiplierStats.VitalityAmount * _formulatedStats.GetMaxMortalityPoints();
        public float GetDamageReduction() => _multiplierStats.VitalityAmount * _formulatedStats.GetDamageReduction();
        public float GetDeBuffReduction() => _multiplierStats.VitalityAmount * _formulatedStats.GetDeBuffReduction();
        public float GetEnlightenment() => _multiplierStats.ConcentrationAmount * _formulatedStats.GetEnlightenment();
        public float GetCriticalChance() => _multiplierStats.ConcentrationAmount * _formulatedStats.GetCriticalChance();
        public float GetSpeedAmount() => _multiplierStats.ConcentrationAmount * _formulatedStats.GetSpeedAmount();
        public float GetInitiativePercentage() => InitiativePercentage;
        public int GetActionsPerInitiative() => ActionsPerInitiative;
        public float GetHarmonyAmount() => HarmonyAmount;

        private class MultiplierStats : IStatsPrimordial
        {
            public float OffensivePower { get; set; } = 1;
            public float SupportPower { get; set; } = 1;
            public float VitalityAmount { get; set; } = 1;
            public float ConcentrationAmount { get; set; } = 1;
        }

        private class FormulatedStats : IBasicStatsData
        {
            [ShowInInspector, HorizontalGroup("Base Stats"), PropertyOrder(-2), GUIColor(.4f, .8f, .6f)]
            public CombatStatsFull BaseStats { get; protected set; }
            /// <summary>
            /// This remains active for the whole fight
            /// </summary>
            [ShowInInspector, HorizontalGroup("Buff Stats"), GUIColor(.4f, .6f, .8f)]
            public CombatStatsFull BuffStats { get; protected set; }

            [ShowInInspector, HorizontalGroup("Buff Stats"), GUIColor(.2f, .3f, .6f)]
            public CombatStatsFull BurstStats { get; protected set; }

            [ShowInInspector, HorizontalGroup("Base Stats"), PropertyOrder(-1)]
            private IBasicStatsData TeamStats => TeamData.GetCurrentStanceValue();

            public CombatingTeam TeamData;

            public FormulatedStats(CombatStatsHolder stats)
            {
                BaseStats = stats.BaseStats;
                BuffStats = stats.BuffStats;
                BurstStats = stats.BurstStats;
            }

            public FormulatedStats(CombatStatsHolder stats, CombatingTeam teamData) : this(stats)
            {
                TeamData = teamData;
            }




            public float CalculateBaseAttackPower()
            {
                return BaseStats.AttackPower + TeamStats.GetAttackPower();
            }
            public float CalculateBaseStaticDamagePower()
            {
                return BaseStats.StaticDamagePower + TeamStats.GetStaticDamagePower();
            }

            public float GetAttackPower()
            {
                return UtilsStats.StatsFormula(
                        CalculateBaseAttackPower(),
                        BuffStats.AttackPower,
                        BurstStats.AttackPower);
            }
            public float GetDeBuffPower()
            {
                return UtilsStats.StatsFormula(
                    BaseStats.DeBuffPower + TeamStats.GetDeBuffPower(),
                    BuffStats.DeBuffPower,
                    BurstStats.DeBuffPower);
            }


            public float GetStaticDamagePower()
            {
                return UtilsStats.StatsFormula(
                    CalculateBaseStaticDamagePower(),
                    BuffStats.StaticDamagePower,
                    BurstStats.StaticDamagePower);
            }

            public float GetHealPower()
            {
                return UtilsStats.StatsFormula(
                    BaseStats.HealPower + TeamStats.GetHealPower(),
                    BuffStats.HealPower,
                    BurstStats.HealPower);
            }
            public float GetBuffPower()
            {
                return UtilsStats.StatsFormula(
                    BaseStats.BuffPower + TeamStats.GetBuffPower(),
                    BuffStats.BuffPower,
                    BurstStats.BuffPower);
            }
            public float GetBuffReceivePower()
            {
                return UtilsStats.StatsFormula(
                    BaseStats.BuffReceivePower + TeamStats.GetBuffReceivePower(),
                    BuffStats.BuffReceivePower,
                    BurstStats.BuffReceivePower);
            }
            public float GetHarmonyAmount()
            {
                return BaseStats.HarmonyAmount + TeamStats.GetHarmonyAmount() + BurstStats.HarmonyAmount;
            }

            public float GetInitiativePercentage()
            {
                return BaseStats.InitiativePercentage + TeamStats.GetInitiativePercentage()
                       + BuffStats.InitiativePercentage
                       + BurstStats.InitiativePercentage;
            }

            public int GetActionsPerInitiative()
            {
                return BaseStats.ActionsPerInitiative + TeamStats.GetActionsPerInitiative()
                       + BuffStats.ActionsPerInitiative
                       + BurstStats.ActionsPerInitiative;
            }

            public float GetEnlightenment()
            {
                return UtilsStats.StatsFormula(
                    BaseStats.Enlightenment + TeamStats.GetEnlightenment(),
                    BuffStats.Enlightenment,
                    BurstStats.Enlightenment);
            }
            public float GetCriticalChance()
            {
                return UtilsStats.StatsFormula(
                    BaseStats.CriticalChance + TeamStats.GetCriticalChance(),
                    BuffStats.CriticalChance,
                    BurstStats.CriticalChance);
            }

            public float GetSpeedAmount()
            {
                return UtilsStats.StatsFormula(
                    BaseStats.SpeedAmount + TeamStats.GetSpeedAmount(),
                    BuffStats.SpeedAmount,
                    BurstStats.SpeedAmount);
            }

            public float GetMaxHealth()
            {
                return BaseStats.MaxHealth + TeamStats.GetMaxHealth();
            }
            public float GetMaxMortalityPoints()
            {
                return BaseStats.MaxMortalityPoints + TeamStats.GetMaxMortalityPoints();
            }

            public float CalculateDamageReduction()
            {
                return BaseStats.DamageReduction + TeamStats.GetDamageReduction();
            }
            public float GetDamageReduction()
            {
                return UtilsStats.StatsFormula(
                    CalculateDamageReduction(),
                    BuffStats.DamageReduction,
                    BurstStats.DamageReduction);
            }
            public float GetDeBuffReduction()
            {
                return UtilsStats.StatsFormula(
                    BaseStats.DeBuffReduction + TeamStats.GetDeBuffReduction(),
                    BuffStats.DeBuffReduction,
                    BurstStats.DeBuffReduction);
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
            IFullStatsData initialStats,
            IFullStatsData growStats, IStatsUpgradable currentUpgrades)
        {
            AttackPower = UtilsStats.GrowFormula(
                initialStats.GetAttackPower(), growStats.GetAttackPower(),
                currentUpgrades.OffensivePower);
            DeBuffPower = UtilsStats.GrowFormula(
                initialStats.GetDeBuffPower(), growStats.GetDeBuffPower(),
                currentUpgrades.OffensivePower);
            StaticDamagePower = UtilsStats.GrowFormula(
                initialStats.GetStaticDamagePower(), growStats.GetStaticDamagePower(),
                currentUpgrades.OffensivePower
            );


            HealPower = UtilsStats.GrowFormula(
                initialStats.GetHealPower(), growStats.GetHealPower(),
                currentUpgrades.SupportPower);
            BuffPower = UtilsStats.GrowFormula(
                initialStats.GetBuffPower(), growStats.GetBuffPower(),
                currentUpgrades.SupportPower);
            BuffReceivePower = UtilsStats.GrowFormula(
                initialStats.GetBuffReceivePower(), growStats.GetBuffReceivePower(),
                currentUpgrades.SupportPower);

            MaxHealth = UtilsStats.GrowFormula(
                initialStats.GetMaxHealth(), growStats.GetMaxHealth(),
                currentUpgrades.VitalityAmount);
            MaxMortalityPoints = UtilsStats.GrowFormula(
                initialStats.GetMaxMortalityPoints(), growStats.GetMaxMortalityPoints(),
                currentUpgrades.VitalityAmount);
            DamageReduction = UtilsStats.GrowFormula(
                initialStats.GetDamageReduction(), growStats.GetDamageReduction(),
                currentUpgrades.VitalityAmount);
            DeBuffReduction = UtilsStats.GrowFormula(
                initialStats.GetDeBuffReduction(), growStats.GetDeBuffReduction(),
                currentUpgrades.VitalityAmount);


            Enlightenment = UtilsStats.GrowFormula(
                initialStats.GetEnlightenment(), growStats.GetEnlightenment(),
                currentUpgrades.ConcentrationAmount);
            CriticalChance = UtilsStats.GrowFormula(
                initialStats.GetCriticalChance(), growStats.GetCriticalChance(),
                currentUpgrades.ConcentrationAmount);
            SpeedAmount = UtilsStats.GrowFormula(
                initialStats.GetSpeedAmount(), growStats.GetSpeedAmount(),
                currentUpgrades.ConcentrationAmount);


            HealthPoints = initialStats.HealthPoints + growStats.HealthPoints;
            ShieldAmount = initialStats.ShieldAmount + growStats.ShieldAmount;
            MortalityPoints = initialStats.MortalityPoints + growStats.MortalityPoints;
            HarmonyAmount = initialStats.GetHarmonyAmount() + growStats.GetHarmonyAmount();
            InitiativePercentage = initialStats.GetInitiativePercentage() + growStats.GetInitiativePercentage();

            ActionsPerInitiative = initialStats.GetActionsPerInitiative() +
                                   (int)(growStats.GetActionsPerInitiative() * currentUpgrades.ConcentrationAmount * GrowActionsModifier);
        }

        private const float GrowActionsModifier = .2f; //Each 5 upgrades
    }
}
