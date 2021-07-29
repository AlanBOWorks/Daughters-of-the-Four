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
            ReceivedStats = new SerializedCombatStatsFull(UtilsStats.ZeroValuesFull);
            DelayBuffHandler = new DelayBuffHandler(this);
            Events = new CombatCharacterEvents(this);
        }
        

        [ShowInInspector,GUIColor(.3f,.5f,1)]
        public readonly string CharacterName;

        public readonly GameObject InstantiationPrefab;
        [ShowInInspector, NonSerialized] 
        public UCharacterHolder Holder;

        [ShowInInspector]
        public CharacterCombatData CombatStats { get; private set; }

        /// <summary>
        /// Used to track the damage received, heals, etc.
        /// </summary>
        public readonly SerializedCombatStatsFull ReceivedStats;

        /// <summary>
        /// <inheritdoc cref="DelayBuffHandler"/>
        /// </summary>
        [ShowInInspector]
        public readonly DelayBuffHandler DelayBuffHandler;
        [ShowInInspector]
        public CombatPassivesHolder PassivesHolder { get; private set; }


        [ShowInInspector, NonSerialized] 
        public CombatAreasData AreasDataTracker;

        [ShowInInspector, NonSerialized] 
        public readonly CombatCharacterEvents Events;
        [ShowInInspector]
        public HarmonyBuffInvoker HarmonyBuffInvoker { get; private set; }

        [ShowInInspector] 
        public SDelayBuffPreset CriticalBuff { get; private set; }

        public IEquipSkill<CombatSkill> SharedSkills => CombatSkills.SharedSkills;
        public ISkillPositions<List<CombatSkill>> UniqueSkills => CombatSkills.UniqueSkills;

        [ShowInInspector]
        public CombatSkills CombatSkills { get; private set; }

        public ICharacterCombatAnimator CombatAnimator;

        /// <summary>
        /// Keeps track of the entity's allies and enemies
        /// </summary>
        [ShowInInspector]
        public CharacterSelfGroup CharacterGroup { get; set; }
        

        /// <summary>
        /// If is Conscious, has actions left and at least can use any skill
        /// </summary>
        public bool CanAct()
        {
            // In order of [false] possibilities 
            return IsConscious() && CombatStats.ActionsLefts > 0 && CanUseSkills();
        }
        public bool IsAlive() => CombatStats.MortalityPoints > 0;
        public bool IsConscious() => IsAlive(); //TODO change it to HP or Mortality based in combat state
        public bool CanUseSkills() => UtilsSkill.CanUseSkills(this);


        public void Injection(CharacterCombatData combatStats)
        {
            if(CombatStats != null)
                throw new ArgumentException("Can't inject stats when the Entity already has its Stats");
            CombatStats = combatStats;
        } 
        

        public void Injection(CombatSkills combatSkills) => 
            CombatSkills = combatSkills;
        public void Injection(CombatingTeam team) =>
            AreasDataTracker.Injection(team.Data);
        public void Injection(CombatPassivesHolder passivesHolder) => 
            PassivesHolder = passivesHolder;
        public void Injection(HarmonyBuffInvoker harmonyBuffInvoker) => 
            HarmonyBuffInvoker = harmonyBuffInvoker;
        public void Injection(SDelayBuffPreset criticalBuff) =>
            CriticalBuff = criticalBuff;

        public ISkillShared<SkillPreset> GetBackUpSkillShared()
        {
            var archetype = this.AreasDataTracker.PositionInTeam;
            var backUp
                = CombatSystemSingleton.ParamsVariable.ArchetypesBackupSkills;
            return CharacterArchetypes.GetElement(backUp, archetype);
        }

    }



    public class CharacterCombatData : ICharacterFullStats
    {
        [ShowInInspector]
        public CharacterCombatStatsFull BaseStats { get; protected set; }
        /// <summary>
        /// This remains active for the whole fight
        /// </summary>
        [ShowInInspector]
        public CharacterCombatStatsFull BuffStats { get; protected set; }

        [ShowInInspector]
        public SerializedCombatStatsFull BurstStats { get; protected set; }

        [ShowInInspector, NonSerialized] 
        public ICharacterFullStats TeamStats;

        public int ActionsLefts = 0;

        public void RefillInitiativeActions()
        {
            UtilsCombatStats.AddActionAmount(this, ActionsPerInitiative);
        }

        public CharacterCombatData(ICharacterFullStats presetStats)
        {
            BaseStats = new CharacterCombatStatsFull(presetStats as ICharacterFullStats);
            BuffStats = new CharacterCombatStatsFull(0);
            BurstStats = new SerializedCombatStatsFull(UtilsStats.ZeroValuesFull);

            BaseStats.HealthPoints = BaseStats.MaxHealth;
            BaseStats.MortalityPoints = BaseStats.MaxMortalityPoints;

            TeamStats = UtilsStats.ZeroValuesFull;
        }
        public float AttackPower
        {
            get => UtilsStats.StatsFormula(
                BaseStats.AttackPower + TeamStats.AttackPower,
                BuffStats.AttackPower,
                BurstStats.AttackPower);
            set => BuffStats.AttackPower = value;
        }
        public float DeBuffPower
        {
            get => UtilsStats.StatsFormula(
                BaseStats.DeBuffPower + TeamStats.DeBuffPower,
                BuffStats.DeBuffPower,
                BurstStats.DeBuffPower);
            set => BuffStats.DeBuffPower = value;
        }
        public float HealPower
        {
            get => UtilsStats.StatsFormula(
                BaseStats.HealPower + TeamStats.HealPower,
                BuffStats.HealPower,
                BurstStats.HealPower);
            set => BuffStats.HealPower = value;
        }
        public float BuffPower
        {
            get => UtilsStats.StatsFormula(
                BaseStats.BuffPower + TeamStats.BuffPower,
                BuffStats.BuffPower,
                BurstStats.BuffPower);
            set => BuffStats.BuffPower = value;
        }
        public float HealthPoints
        {
            get => BaseStats.HealthPoints + TeamStats.HealthPoints;
            set => BaseStats.HealthPoints = value;
        }
        public float ShieldAmount
        {
            get => BaseStats.ShieldAmount + TeamStats.ShieldAmount;
            set => BaseStats.ShieldAmount = value;
        }
        public float MortalityPoints
        {
            get => BaseStats.MortalityPoints + TeamStats.MortalityPoints;
            set => BaseStats.MortalityPoints = value;
        }
        public float HarmonyAmount
        {
            get => BaseStats.HarmonyAmount + TeamStats.HarmonyAmount + BurstStats.HarmonyAmount;
            set => BaseStats.HarmonyAmount = value;
        }

        public float InitiativePercentage
        {
            get => BaseStats.InitiativePercentage + TeamStats.InitiativePercentage
                   + BuffStats.InitiativePercentage
                   + BurstStats.InitiativePercentage;
            set => BaseStats.InitiativePercentage = value;
        }

        public int ActionsPerInitiative
        {
            get => BaseStats.ActionsPerInitiative + TeamStats.ActionsPerInitiative
                   + BuffStats.ActionsPerInitiative
                   + BurstStats.ActionsPerInitiative;
            set => BaseStats.ActionsPerInitiative = value;
        }

        public float Enlightenment
        {
            get => UtilsStats.StatsFormula(
                BaseStats.Enlightenment + TeamStats.Enlightenment,
                BuffStats.Enlightenment,
                BurstStats.Enlightenment);
            set => BuffStats.Enlightenment = value;
        }
        public float CriticalChance
        {
            get => UtilsStats.StatsFormula(
                BaseStats.CriticalChance + TeamStats.CriticalChance,
                BuffStats.CriticalChance,
                BurstStats.CriticalChance);
            set => BuffStats.CriticalChance = value;
        }

        public float SpeedAmount
        {
            get => UtilsStats.StatsFormula(
                BaseStats.SpeedAmount + TeamStats.SpeedAmount,
                BuffStats.SpeedAmount,
                BurstStats.SpeedAmount);
            set => BuffStats.SpeedAmount = value;
        }

        public float MaxHealth
        {
            get => BaseStats.MaxHealth + TeamStats.MaxHealth;
            set => BuffStats.MaxHealth = value;
        }
        public float MaxMortalityPoints
        {
            get => BaseStats.MaxMortalityPoints + TeamStats.MaxMortalityPoints;
            set => BuffStats.MaxMortalityPoints = value;
        }
        public float DamageReduction
        {
            get => UtilsStats.StatsFormula(
                BaseStats.DamageReduction + TeamStats.DamageReduction,
                BuffStats.DamageReduction,
                BurstStats.DamageReduction);
            set => BuffStats.DamageReduction = value;
        }
        public float DeBuffReduction
        {
            get => UtilsStats.StatsFormula(
                BaseStats.DeBuffReduction + TeamStats.DeBuffReduction,
                BuffStats.DeBuffReduction,
                BurstStats.DeBuffReduction);
            set => BuffStats.DeBuffReduction = value;
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

        public PlayerCharacterCombatStats(int overrideByDefault) : base(overrideByDefault)
        {}

        public PlayerCharacterCombatStats(PlayerCharacterCombatStats copyFrom)
        {
            AttackPower = copyFrom.AttackPower;
            DeBuffPower = copyFrom.DeBuffPower;


            HealPower = copyFrom.HealPower;
            BuffPower = copyFrom.BuffPower;


            MaxHealth = copyFrom.MaxHealth;
            MaxMortalityPoints = copyFrom.MaxMortalityPoints;
            DamageReduction = copyFrom.DamageReduction;
            DeBuffReduction = copyFrom.DeBuffReduction;



            Enlightenment = copyFrom.Enlightenment;
            CriticalChance = copyFrom.CriticalChance;
            SpeedAmount = copyFrom.SpeedAmount;


            HealthPoints = copyFrom.HealthPoints;
            ShieldAmount = copyFrom.ShieldAmount;
            MortalityPoints = copyFrom.MortalityPoints;
            HarmonyAmount = copyFrom.HarmonyAmount;
            InitiativePercentage = copyFrom.InitiativePercentage;

            ActionsPerInitiative = copyFrom.ActionsPerInitiative;
        }

        public PlayerCharacterCombatStats(IPlayerCharacterStats playerCharacterStats)
        : this(playerCharacterStats.InitialStats,playerCharacterStats.GrowStats,playerCharacterStats.UpgradedStats)
        { }

        public PlayerCharacterCombatStats(ICharacterFullStats initialStats,ICharacterFullStats growStats, IStatsUpgradable currentUpgrades)
        {
            AttackPower = UtilsStats.GrowFormula(
                initialStats.AttackPower, growStats.AttackPower,
                currentUpgrades.OffensivePower);
            DeBuffPower = UtilsStats.GrowFormula(
                initialStats.DeBuffPower, growStats.DeBuffPower,
                currentUpgrades.OffensivePower);

            HealPower = UtilsStats.GrowFormula(
                initialStats.HealPower, growStats.HealPower,
                currentUpgrades.SupportPower);
            BuffPower = UtilsStats.GrowFormula(
                initialStats.BuffPower, growStats.BuffPower,
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
                currentUpgrades.Enlightenment);
            CriticalChance = UtilsStats.GrowFormula(
                initialStats.CriticalChance, growStats.CriticalChance,
                currentUpgrades.Enlightenment);
            SpeedAmount = UtilsStats.GrowFormula(
                initialStats.SpeedAmount, growStats.SpeedAmount,
                currentUpgrades.Enlightenment);


            HealthPoints = initialStats.HealthPoints + growStats.HealthPoints;
            ShieldAmount = initialStats.ShieldAmount + growStats.ShieldAmount;
            MortalityPoints = initialStats.MortalityPoints + growStats.MortalityPoints;
            HarmonyAmount = initialStats.HarmonyAmount + growStats.HarmonyAmount;
            InitiativePercentage = initialStats.InitiativePercentage + growStats.InitiativePercentage;

            ActionsPerInitiative = initialStats.ActionsPerInitiative +
                                   (int) (growStats.ActionsPerInitiative * currentUpgrades.Enlightenment * GrowActionsModifier);
        }

        private const float GrowActionsModifier = .2f; //Each 5 upgrades
    }


}
