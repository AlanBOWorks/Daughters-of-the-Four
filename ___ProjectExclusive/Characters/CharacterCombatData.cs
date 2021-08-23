using System;
using System.Collections;
using System.Collections.Generic;
using _CombatSystem;
using _Team;
using Passives;
using Sirenix.OdinInspector;
using Skills;
using Stats;
using UnityEngine;

namespace Characters
{
    /// <summary>
    /// Used in combat only: In every fight objects of this type should be cleaned
    /// and re-instantiated (since player's one, or others, could being modified).<br></br>
    /// Conceptually it's different from a [<seealso cref="CharacterCombatData"/>] since a character is
    /// something that is permanent in the game while a <see cref="CombatingEntity"/>
    /// exits as long there's a 'Combat'. In other words, 'Character' is a general concept
    /// while <see cref="CombatingEntity"/> is hyper specific to the Combat and their existence
    /// are bond to the Combat.<br></br>
    /// _____ <br></br>
    /// TL;DR:<br></br>
    /// [<see cref="CharacterCombatData"/>]:
    /// persist as an entity of the Game.<br></br>
    /// [<see cref="CombatingEntity"/>]:
    /// exists while there's a Combat and contains the core data for the combating part
    /// </summary>
    public class CombatingEntity
    { public CombatingEntity(string characterName, GameObject prefab)
        {
            CharacterName = characterName;
            InstantiationPrefab = prefab;
            ReceivedStats = new SerializedTrackedStats(UtilsStats.ZeroValuesFull);
            DelayBuffHandler = new DelayBuffHandler(this);
            Events = new CombatCharacterEvents(this);
            CharacterCriticalBuff = new CharacterCriticalActionHandler(this);
            Guarding = new CharacterGuarding(this);

            Events.Subscribe(Guarding);
        }
        

        [ShowInInspector,GUIColor(.3f,.5f,1)]
        public readonly string CharacterName;

        public readonly GameObject InstantiationPrefab;
        [ShowInInspector, NonSerialized] 
        public UCharacterHolder Holder;

        [ShowInInspector, GUIColor(.4f,.8f,.7f)]
        public CharacterCombatData CombatStats { get; private set; }

        /// <summary>
        /// Used to track the damage received, heals, etc.
        /// </summary>
        public readonly SerializedTrackedStats ReceivedStats;

        /// <summary>
        /// <inheritdoc cref="DelayBuffHandler"/>
        /// </summary>
        [ShowInInspector]
        public readonly DelayBuffHandler DelayBuffHandler;

        [ShowInInspector, NonSerialized] 
        public CharacterCombatAreasData AreasDataTracker;

        [ShowInInspector, NonSerialized] 
        public readonly CombatCharacterEvents Events;

        [ShowInInspector] 
        public CharacterCriticalActionHandler CharacterCriticalBuff { get; private set; }

        [ShowInInspector]
        public CombatSkills CombatSkills { get; private set; }

        public ICharacterCombatAnimator CombatAnimator;

        /// <summary>
        /// Keeps track of the entity's allies and enemies
        /// </summary>
        [ShowInInspector] 
        public CharacterSelfGroup CharacterGroup;

        [ShowInInspector] 
        public readonly CharacterGuarding Guarding;


        /// <summary>
        /// If is Conscious, has actions left and at least can use any skill
        /// </summary>
        public bool CanAct()
        {
            // In order of [false] possibilities 
            return IsConscious() &&  HasActions() && CanUseSkills();
        }

        public bool IsInDanger() => CharacterGroup.Team.IsInDangerState();

        public bool IsAlive() => CombatStats.IsAlive();

        public bool IsConscious()
        {
            if (IsInDanger())
                return IsAlive();

            return CombatStats.HealthPoints > 0;
        }

        public bool HasActions() => CombatStats.HasActionLeft();
        public bool CanUseSkills() => UtilsSkill.CanUseSkills(this);
        public CharacterArchetypes.RoleArchetype Role => AreasDataTracker.Role;


        public void Injection(CharacterCombatData combatStats)
        {
            if(CombatStats != null)
                throw new ArgumentException("Can't inject stats when the Entity already has its Stats");
            CombatStats = combatStats;
        } 
        

        public void Injection(CombatSkills combatSkills) => 
            CombatSkills = combatSkills;
        public void Injection(CombatingTeam team)
        {
            CombatStats.TeamData = team;
            AreasDataTracker.Injection(team.State);
        }
        public void Injection(SDelayBuffPreset criticalBuff) =>
            CharacterCriticalBuff.CriticalBuff = criticalBuff;

    }



    public class CharacterCombatData : ICharacterFullStatsData
    {
        public CharacterCombatData(ICharacterFullStats presetStats)
        {
            BaseStats = new CharacterCombatStatsFull(presetStats);
            BuffStats = new CharacterCombatStatsFull();
            BurstStats = new CharacterCombatStatsFull();

            _formulatedStats = new FormulatedStats(this);
            _multiplierStats = new MultiplierStats();
        }

        [ShowInInspector, HorizontalGroup("Base Stats"),PropertyOrder(-2), GUIColor(.4f,.8f,.6f)]
        public CharacterCombatStatsFull BaseStats { get; protected set; }
        /// <summary>
        /// This remains active for the whole fight
        /// </summary>
        [ShowInInspector, HorizontalGroup("Buff Stats"), GUIColor(.4f, .6f, .8f)]
        public CharacterCombatStatsFull BuffStats { get; protected set; }

        [ShowInInspector, HorizontalGroup("Buff Stats"), GUIColor(.2f, .3f, .6f)]
        public CharacterCombatStatsFull BurstStats { get; protected set; }

        // By design multiplier are consistent / Burst are temporary and specific;
        // making multipliers Burst type could be confusing.
        // Some buff could increase multiplier and/or a specific stat
        //
        // Eg: increasing the Heal power instead the Support)
        private readonly MultiplierStats _multiplierStats;

        private ICharacterBasicStatsData TeamStats => TeamData.GetCurrentStanceValue();
        public IStatsPrimordial GetMultiplierStats() => _multiplierStats;

        public CombatingTeam TeamData
        {
            get => _formulatedStats.TeamData;
            set => _formulatedStats.TeamData = value;
        }

        private readonly FormulatedStats _formulatedStats;
        [TitleGroup("Local stats"), PropertyOrder(-10)]
        public int ActionsLefts;


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

        private class FormulatedStats : ICharacterBasicStatsData
        {
            [ShowInInspector, HorizontalGroup("Base Stats"), PropertyOrder(-2), GUIColor(.4f, .8f, .6f)]
            public CharacterCombatStatsFull BaseStats { get; protected set; }
            /// <summary>
            /// This remains active for the whole fight
            /// </summary>
            [ShowInInspector, HorizontalGroup("Buff Stats"), GUIColor(.4f, .6f, .8f)]
            public CharacterCombatStatsFull BuffStats { get; protected set; }

            [ShowInInspector, HorizontalGroup("Buff Stats"), GUIColor(.2f, .3f, .6f)]
            public CharacterCombatStatsFull BurstStats { get; protected set; }

            [ShowInInspector, HorizontalGroup("Base Stats"), PropertyOrder(-1)]
            private ICharacterBasicStatsData TeamStats => TeamData.GetCurrentStanceValue();

            public CombatingTeam TeamData;

            public FormulatedStats(CharacterCombatData data)
            {
                BaseStats = data.BaseStats;
                BuffStats = data.BuffStats;
                BurstStats = data.BurstStats;
            }

            public FormulatedStats(CharacterCombatData data, CombatingTeam teamData) : this(data)
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
    /// It's the same than <see cref="CharacterCombatStatsFull"/> but its constructor
    /// allows to inject <seealso cref="IStatsUpgradable"/>
    /// </summary>
    [Serializable]
    public class PlayerCharacterCombatStats : CharacterCombatStatsFull
    {
        public PlayerCharacterCombatStats()
        {}

        public PlayerCharacterCombatStats(float overrideByDefault) : base(overrideByDefault)
        {}

        public PlayerCharacterCombatStats(PlayerCharacterCombatStats copyFrom)
        : base(copyFrom)
        {}

        public PlayerCharacterCombatStats(IPlayerCharacterStats playerCharacterStats)
        : this(playerCharacterStats.InitialStats,playerCharacterStats.GrowStats,playerCharacterStats.UpgradedStats)
        { }

        public PlayerCharacterCombatStats(
            ICharacterFullStatsData initialStats, 
            ICharacterFullStatsData growStats, IStatsUpgradable currentUpgrades)
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
