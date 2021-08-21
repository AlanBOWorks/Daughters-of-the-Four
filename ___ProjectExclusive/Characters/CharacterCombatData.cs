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
        [ShowInInspector]
        public CombatPassivesHolder PassivesHolder { get; private set; }


        [ShowInInspector, NonSerialized] 
        public CharacterCombatAreasData AreasDataTracker;

        [ShowInInspector, NonSerialized] 
        public readonly CombatCharacterEvents Events;
        [ShowInInspector]
        public HarmonyBuffInvoker HarmonyBuffInvoker { get; private set; }

        [ShowInInspector] 
        public CharacterCriticalActionHandler CharacterCriticalBuff { get; private set; }

        public IEquipSkill<CombatSkill> SharedSkills => CombatSkills.SharedSkills;
        public ISkillPositions<List<CombatSkill>> UniqueSkills => CombatSkills.UniqueSkills;

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
        public void Injection(CombatPassivesHolder passivesHolder) => 
            PassivesHolder = passivesHolder;
        public void Injection(HarmonyBuffInvoker harmonyBuffInvoker) => 
            HarmonyBuffInvoker = harmonyBuffInvoker;
        public void Injection(SDelayBuffPreset criticalBuff) =>
            CharacterCriticalBuff.CriticalBuff = criticalBuff;

    }



    public class CharacterCombatData : ICharacterFullStatsData
    {

        [Title("Combat Stats")]
        [ShowInInspector, HorizontalGroup("Base Stats"),PropertyOrder(-2), GUIColor(.4f,.8f,.6f)]
        public CharacterCombatStatsFull BaseStats { get; protected set; }
        /// <summary>
        /// This remains active for the whole fight
        /// </summary>
        [ShowInInspector, HorizontalGroup("Buff Stats"), GUIColor(.4f, .6f, .8f)]
        public CharacterCombatStatsFull BuffStats { get; protected set; }

        [ShowInInspector, HorizontalGroup("Buff Stats"), GUIColor(.2f, .3f, .6f)]
        public CharacterCombatStatsFull BurstStats { get; protected set; }

        [ShowInInspector, HorizontalGroup("Base Stats"), PropertyOrder(-1)]
        private ICharacterBasicStatsData TeamStats => TeamData.GetCurrentStats();

        public CombatingTeam TeamData;


        [TitleGroup("Local stats"), PropertyOrder(10)]
        public int ActionsLefts;
        [TitleGroup("Local stats")] 
        public float AccumulatedStaticDamage;

        public bool IsAlive() => MortalityPoints > 0;
        public bool HasActionLeft() => ActionsLefts > 0;


        public void RefillInitiativeActions()
        {
            UtilsCombatStats.AddActionAmount(this, ActionsPerInitiative);
        }

        public void Revive()
        {
            UtilsCombatStats.HealToMax(this);
        }


        public CharacterCombatData(ICharacterFullStats presetStats)
        {
            BaseStats = new CharacterCombatStatsFull(presetStats);
            BuffStats = new CharacterCombatStatsFull();
            BurstStats = new CharacterCombatStatsFull();

            BaseStats.HealthPoints = BaseStats.MaxHealth;
            BaseStats.MortalityPoints = BaseStats.MaxMortalityPoints;
        }

        public float CalculateBaseAttackPower()
        {
            return BaseStats.AttackPower + TeamStats.GetAttackPower();
        }
        public float AttackPower
        {
            get => UtilsStats.StatsFormula(
                    CalculateBaseAttackPower(),
                    BuffStats.AttackPower,
                    BurstStats.AttackPower);
            set => BuffStats.AttackPower = value;
        }
        public float DeBuffPower
        {
            get => UtilsStats.StatsFormula(
                BaseStats.DeBuffPower + TeamStats.GetDeBuffPower(),
                BuffStats.DeBuffPower,
                BurstStats.DeBuffPower);
            set => BuffStats.DeBuffPower = value;
        }

        public float CalculateBaseStaticDamagePower()
        {
            return BaseStats.StaticDamagePower + TeamStats.GetStaticDamagePower();
        }
        public float StaticDamagePower
        {
            get => UtilsStats.StatsFormula(
                CalculateBaseStaticDamagePower(),
                BuffStats.StaticDamagePower,
                BurstStats.StaticDamagePower);
            set => BuffStats.StaticDamagePower = value;
        }

        public float HealPower
        {
            get => UtilsStats.StatsFormula(
                BaseStats.HealPower + TeamStats.GetHealPower(),
                BuffStats.HealPower,
                BurstStats.HealPower);
            set => BuffStats.HealPower = value;
        }
        public float BuffPower
        {
            get => UtilsStats.StatsFormula(
                BaseStats.BuffPower + TeamStats.GetBuffPower(),
                BuffStats.BuffPower,
                BurstStats.BuffPower);
            set => BuffStats.BuffPower = value;
        }
        public float BuffReceivePower
        {
            get => UtilsStats.StatsFormula(
                BaseStats.BuffReceivePower + TeamStats.GetBuffReceivePower(),
                BuffStats.BuffReceivePower,
                BurstStats.BuffReceivePower);
            set => BuffStats.BuffReceivePower = value;
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

        public float Enlightenment
        {
            get => UtilsStats.StatsFormula(
                BaseStats.Enlightenment + TeamStats.GetEnlightenment(),
                BuffStats.Enlightenment,
                BurstStats.Enlightenment);
            set => BuffStats.Enlightenment = value;
        }
        public float CriticalChance
        {
            get => UtilsStats.StatsFormula(
                BaseStats.CriticalChance + TeamStats.GetCriticalChance(),
                BuffStats.CriticalChance,
                BurstStats.CriticalChance);
            set => BuffStats.CriticalChance = value;
        }

        public float SpeedAmount
        {
            get => UtilsStats.StatsFormula(
                BaseStats.SpeedAmount + TeamStats.GetSpeedAmount(),
                BuffStats.SpeedAmount,
                BurstStats.SpeedAmount);
            set => BuffStats.SpeedAmount = value;
        }

        public float MaxHealth
        {
            get => BaseStats.MaxHealth + TeamStats.GetMaxHealth();
            set => BuffStats.MaxHealth = value;
        }
        public float MaxMortalityPoints
        {
            get => BaseStats.MaxMortalityPoints + TeamStats.GetMaxMortalityPoints();
            set => BuffStats.MaxMortalityPoints = value;
        }

        public float CalculateDamageReduction()
        {
            return BaseStats.DamageReduction + TeamStats.GetDamageReduction();
        }
        public float DamageReduction
        {
            get => UtilsStats.StatsFormula(
                CalculateDamageReduction(),
                BuffStats.DamageReduction,
                BurstStats.DamageReduction);
            set => BuffStats.DamageReduction = value;
        }
        public float DeBuffReduction
        {
            get => UtilsStats.StatsFormula(
                BaseStats.DeBuffReduction + TeamStats.GetDeBuffReduction(),
                BuffStats.DeBuffReduction,
                BurstStats.DeBuffReduction);
            set => BuffStats.DeBuffReduction = value;
        }

        public void SetInitiativePercentage(float value)
        {
            BaseStats.InitiativePercentage = value;
        }

        public void SetActionsPerInitiative(int value)
        {
            BaseStats.ActionsPerInitiative = value;

        }

        public void SetHarmonyAmount(float value)
        {
            BaseStats.HarmonyAmount = value;
        }

        public float GetAttackPower() => AttackPower;

        public float GetDeBuffPower() => DeBuffPower;

        public float GetStaticDamagePower() => StaticDamagePower;

        public float GetHealPower() => HealPower;

        public float GetBuffPower() => BuffPower;

        public float GetBuffReceivePower() => BuffReceivePower;

        public float GetMaxHealth() => MaxHealth;

        public float GetMaxMortalityPoints() => MaxMortalityPoints;

        public float GetDamageReduction() => DamageReduction;

        public float GetDeBuffReduction() => DeBuffReduction;

        public float GetEnlightenment() => Enlightenment;

        public float GetCriticalChance() => CriticalChance;

        public float GetSpeedAmount() => SpeedAmount;

        public float GetInitiativePercentage() => InitiativePercentage;

        public int GetActionsPerInitiative() => ActionsPerInitiative;

        public float GetHarmonyAmount() => HarmonyAmount;
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
                currentUpgrades.Enlightenment);
            CriticalChance = UtilsStats.GrowFormula(
                initialStats.GetCriticalChance(), growStats.GetCriticalChance(),
                currentUpgrades.Enlightenment);
            SpeedAmount = UtilsStats.GrowFormula(
                initialStats.GetSpeedAmount(), growStats.GetSpeedAmount(),
                currentUpgrades.Enlightenment);


            HealthPoints = initialStats.HealthPoints + growStats.HealthPoints;
            ShieldAmount = initialStats.ShieldAmount + growStats.ShieldAmount;
            MortalityPoints = initialStats.MortalityPoints + growStats.MortalityPoints;
            HarmonyAmount = initialStats.GetHarmonyAmount() + growStats.GetHarmonyAmount();
            InitiativePercentage = initialStats.GetInitiativePercentage() + growStats.GetInitiativePercentage();

            ActionsPerInitiative = initialStats.GetActionsPerInitiative() +
                                   (int)(growStats.GetActionsPerInitiative() * currentUpgrades.Enlightenment * GrowActionsModifier);
        }

        private const float GrowActionsModifier = .2f; //Each 5 upgrades
    }


}
