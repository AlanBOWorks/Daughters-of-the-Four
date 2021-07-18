using System;
using System.Collections;
using System.Collections.Generic;
using _CombatSystem;
using _Player;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;
using Random = UnityEngine.Random;

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
    { public CombatingEntity(string characterName, GameObject prefab,
            ICharacterCombatAnimator combatAnimator)
        {
            CharacterName = characterName;
            InstantiationPrefab = prefab;
            ReceivedStats = new SerializedCombatStatsFull(UtilsStats.ZeroValuesFull);
            SpecialBuffHolders = new CharacterBuffHolders(this);
            Events = new CombatCharacterEvents(this);

            CombatAnimator = combatAnimator;
            AllSkills = new CombatCharacterSkills();
        }
        /// <summary>
        /// Initialize this object with a provisional [<see cref="ICharacterCombatAnimator"/>] of type
        /// [<seealso cref="ProvisionalCharacterAnimator"/>]
        /// </summary>
        public CombatingEntity(string characterName, GameObject prefab)
            : this(characterName, prefab, ProvisionalCharacterAnimator.ProvisionalAnimator)
        { }

        [ShowInInspector,GUIColor(.3f,.5f,1)]
        public readonly string CharacterName;

        public GameObject InstantiationPrefab { get; }
        [ShowInInspector, NonSerialized] 
        public UCharacterHolder Holder;

        [ShowInInspector]
        public CharacterCombatData CombatStats { get; private set; }
        /// <summary>
        /// Used to track the damage received, heals, etc.
        /// </summary>
        public SerializedCombatStatsFull ReceivedStats { get; private set; }

        [ShowInInspector]
        public readonly CharacterBuffHolders SpecialBuffHolders;

        [ShowInInspector, NonSerialized] 
        public CombatAreasData AreasDataTracker;

        [ShowInInspector, NonSerialized] 
        public CombatCharacterEvents Events;

        [ShowInInspector]
        public ISkillShared<CombatSkill> SharedSkills { get; private set; }
        [ShowInInspector]
        public ISkillPositions<List<CombatSkill>> UniqueSkills { get; private set; }

        [ShowInInspector]
        public CombatCharacterSkills AllSkills { get; private set; }

        public ICharacterCombatAnimator CombatAnimator;

        /// <summary>
        /// Keeps track of the entity's allies and enemies
        /// </summary>
        [ShowInInspector]
        public CharacterSelfGroup CharacterGroup { get; set; }

        public bool CanAct()
        {
            return IsAlive() && CombatStats.ActionsLefts > 0;
        }
        public bool IsAlive() => CombatStats.MortalityPoints > 0;
        public bool IsConscious() => IsAlive(); //TODO change it to HP or Mortality based in combat state


        public void Injection(CharacterCombatData combatStats)
        {
            if(CombatStats != null)
                throw new ArgumentException("Can't inject stats when the Entity already has its Stats");
            CombatStats = combatStats;
        } 
        public void Injection(ISkillPositions<List<Skill>> injectedSkills)
        {
            if(injectedSkills == null) return;
            this.UniqueSkills = new PositionCombatSkills(injectedSkills);
            AllSkills.Add(UniqueSkills);
        }

        public void Injection(ISkillShared<Skill> injectedSkills)
        {
            if(injectedSkills == null) return;
            SharedSkills = new SharedCombatSkills(injectedSkills);
            AllSkills.Add(SharedSkills);
        }
        public void Injection(CombatingTeam team)
        {
            AreasDataTracker.Injection(team.Data);
        }
       

        public void DoBackupSharedSkillsInjection()
        {
            var archetype = this.AreasDataTracker.PositionInTeam;
            var backUp
                = CombatSystemSingleton.ParamsVariable.ArchetypesBackupSkills;
            var skills = CharacterArchetypes.GetElement(backUp, archetype);
            Injection(new SharedCombatSkills(skills));
        }

    }

    public class CombatCharacterSkills : List<CombatSkill>
    {
        public ISkillShared<CombatSkill> SharedSkills { get; private set; }
        public ISkillPositions<List<CombatSkill>> UniqueSkills { get; private set; }

        public CombatCharacterSkills()
        {}

        public CombatCharacterSkills(ISkillShared<CombatSkill> shared)
        {
            Add(shared);
        }

        public CombatCharacterSkills(ISkillPositions<List<CombatSkill>> uniqueSkills)
        {
            Add(uniqueSkills);
        }

        public CombatCharacterSkills(
            ISkillShared<CombatSkill> shared, 
            ISkillPositions<List<CombatSkill>> uniqueSkills) :
            this(shared)
        {
            Add(uniqueSkills);
        }

        

        public void Add([NotNull]ISkillShared<CombatSkill> shared)
        {
            if(SharedSkills != null) return;
            SharedSkills = shared;
            Add(shared.UltimateSkill);
            Add(shared.CommonSkillFirst);
            Add(shared.CommonSkillSecondary);
        }
        public void Add([NotNull] ISkillPositions<List<CombatSkill>> uniqueSkills)
        {
            if(UniqueSkills !=null) return;
            UniqueSkills = uniqueSkills;
            AddRange(uniqueSkills.AttackingSkills);
            AddRange(uniqueSkills.NeutralSkills);
            AddRange(uniqueSkills.DefendingSkills);
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
        public CharacterCombatStatsBasic BuffStats { get; protected set; }

        [ShowInInspector]
        public SerializedCombatStatsFull BurstStats { get; protected set; }

        public int ActionsLefts = 0;

        public void RefillInitiativeActions()
        {
            UtilsCombatStats.AddActionAmount(this, ActionsPerInitiative);
        }

        public CharacterCombatData(ICharacterFullStats presetStats)
        {
            BaseStats = new CharacterCombatStatsFull(presetStats as ICharacterFullStats);
            BuffStats = new CharacterCombatStatsBasic(0);
            BurstStats = new SerializedCombatStatsFull(UtilsStats.ZeroValuesFull);

            BaseStats.HealthPoints = BaseStats.MaxHealth;
            BaseStats.MortalityPoints = BaseStats.MaxMortalityPoints;
        }

        public float AttackPower
        {
            get => UtilsStats.StatsFormula(
                BaseStats.AttackPower,
                BuffStats.AttackPower,
                BurstStats.AttackPower);
            set => BuffStats.AttackPower = value;
        }
        public float DeBuffPower
        {
            get => UtilsStats.StatsFormula(
                BaseStats.DeBuffPower,
                BuffStats.DeBuffPower,
                BurstStats.DeBuffPower);
            set => BuffStats.DeBuffPower = value;
        }
        public float HealPower
        {
            get => UtilsStats.StatsFormula(
                BaseStats.HealPower,
                BuffStats.HealPower,
                BurstStats.HealPower);
            set => BuffStats.HealPower = value;
        }
        public float BuffPower
        {
            get => UtilsStats.StatsFormula(
                BaseStats.BuffPower,
                BuffStats.BuffPower,
                BurstStats.BuffPower);
            set => BuffStats.BuffPower = value;
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
            get => BaseStats.HarmonyAmount;
            set => BaseStats.HarmonyAmount = value;
        }

        public float InitiativePercentage
        {
            get => BaseStats.InitiativePercentage;
            set => BaseStats.InitiativePercentage = value;
        }

        public int ActionsPerInitiative
        {
            get => BaseStats.ActionsPerInitiative + BurstStats.ActionsPerInitiative;
            set => BaseStats.ActionsPerInitiative = value;
        }

        public float Enlightenment
        {
            get => UtilsStats.StatsFormula(
                BaseStats.Enlightenment,
                BuffStats.Enlightenment,
                BurstStats.Enlightenment);
            set => BuffStats.Enlightenment = value;
        }
        public float CriticalChance
        {
            get => UtilsStats.StatsFormula(
                BaseStats.CriticalChance,
                BuffStats.CriticalChance,
                BurstStats.CriticalChance);
            set => BuffStats.CriticalChance = value;
        }

        public float SpeedAmount
        {
            get => UtilsStats.StatsFormula(
                BaseStats.SpeedAmount,
                BuffStats.SpeedAmount,
                BurstStats.SpeedAmount);
            set => BuffStats.SpeedAmount = value;
        }

        public float MaxHealth
        {
            get => BaseStats.MaxHealth;
            set => BuffStats.MaxHealth = value;
        }
        public float MaxMortalityPoints
        {
            get => BaseStats.MaxMortalityPoints;
            set => BuffStats.MaxMortalityPoints = value;
        }
        public float DamageReduction
        {
            get => UtilsStats.StatsFormula(
                BaseStats.DamageReduction,
                BuffStats.DamageReduction,
                BurstStats.DamageReduction);
            set => BuffStats.DamageReduction = value;
        }
        public float DeBuffReduction
        {
            get => UtilsStats.StatsFormula(
                BaseStats.DeBuffReduction,
                BuffStats.DeBuffReduction,
                BurstStats.DeBuffReduction);
            set => BuffStats.DeBuffReduction = value;
        }
    }

    public class PlayerCharacterCombatData : CharacterCombatData
    {
        public PlayerCharacterCombatData(
            CharacterCombatStatsFull initialStats,
            CharacterCombatStatsFull growStats, 
            IStatsUpgradable upgrades) 
            : base(new PlayerCharacterCombatStats(initialStats,growStats, upgrades))
        {
        }
    }

    public class SerializedCombatStatsFull : SerializedCombatStatsBasic, ICharacterFullStats
    {
        public SerializedCombatStatsFull(ICharacterFullStats serializeThis) : base(FullStatsLength)
        {
            InjectValues(serializeThis);
            Add(serializeThis.HealthPoints);
            Add(serializeThis.ShieldAmount);
            Add(serializeThis.MortalityPoints);
            Add(serializeThis.HarmonyAmount);
            Add(serializeThis.InitiativePercentage);
            ActionsPerInitiative = serializeThis.ActionsPerInitiative;
        }

        public float HealthPoints
        {
            get => this[HealthPointsIndex];
            set => this[HealthPointsIndex] = value;
        }
        public float ShieldAmount
        {
            get => this[ShieldAmountIndex];
            set => this[ShieldAmountIndex] = value;
        }
        public float MortalityPoints
        {
            get => this[MortalityPointsIndex];
            set => this[MortalityPointsIndex] = value;
        }
        public float HarmonyAmount
        {
            get => this[HarmonyAmountIndex];
            set => this[HarmonyAmountIndex] = value;
        }
        public float InitiativePercentage
        {
            get => this[InitiativePercentageIndex];
            set => this[InitiativePercentageIndex] = value;
        }
        public int ActionsPerInitiative { get; set; }

        public const int HealthPointsIndex = BasicStatsLength+1;
        public const int ShieldAmountIndex = HealthPointsIndex+1;
        public const int MortalityPointsIndex = ShieldAmountIndex+1;
        public const int HarmonyAmountIndex = MortalityPointsIndex+1;
        public const int InitiativePercentageIndex = HarmonyAmountIndex+1;
        public const int FullStatsLength = InitiativePercentageIndex + 1;

        public override void ResetToZero()
        {
            base.ResetToZero();
            ActionsPerInitiative = 0;
        }
    }

    public class SerializedCombatStatsBasic : List<float> ,ICharacterBasicStats
    {
        public SerializedCombatStatsBasic(int memoryAllocation) : base(memoryAllocation)
        {}

        public SerializedCombatStatsBasic(ICharacterBasicStats serializeThis) : base(BasicStatsLength)
        {
            InjectValues(serializeThis);
        }

        protected void InjectValues(ICharacterBasicStats serializeThis)
        {
            Add(serializeThis.AttackPower);
            Add(serializeThis.DeBuffPower);
            Add(serializeThis.HealPower);
            Add(serializeThis.BuffPower);
            Add(serializeThis.MaxHealth);
            Add(serializeThis.MaxMortalityPoints);
            Add(serializeThis.DamageReduction);
            Add(serializeThis.DeBuffReduction);
            Add(serializeThis.Enlightenment);
            Add(serializeThis.CriticalChance);
            Add(serializeThis.SpeedAmount);
        }

        public float AttackPower
        {
            get => this[AttackPowerIndex];
            set => this[AttackPowerIndex] = value;
        }
        public float DeBuffPower
        {
            get => this[DeBuffPowerIndex];
            set => this[DeBuffPowerIndex] = value;
        }
        public float HealPower
        {
            get => this[HealPowerIndex];
            set => this[HealPowerIndex] = value;
        }
        public float BuffPower
        {
            get => this[BuffPowerIndex];
            set => this[BuffPowerIndex] = value;
        }
        public float MaxHealth
        {
            get => this[MaxHealthIndex];
            set => this[MaxHealthIndex] = value;
        }
        public float MaxMortalityPoints
        {
            get => this[MaxMortalityPointsIndex];
            set => this[MaxMortalityPointsIndex] = value;
        }
        public float DamageReduction
        {
            get => this[DamageReductionIndex];
            set => this[DamageReductionIndex] = value;
        }
        public float DeBuffReduction
        {
            get => this[DeBuffReductionIndex];
            set => this[DeBuffReductionIndex] = value;
        }
        public float Enlightenment
        {
            get => this[EnlightenmentIndex];
            set => this[EnlightenmentIndex] = value;
        }
        public float CriticalChance
        {
            get => this[CriticalChanceIndex];
            set => this[CriticalChanceIndex] = value;
        }
        public float SpeedAmount
        {
            get => this[SpeedAmountIndex];
            set => this[SpeedAmountIndex] = value;
        }

        public const int AttackPowerIndex = 0;
        public const int DeBuffPowerIndex = AttackPowerIndex + 1;
        public const int HealPowerIndex = DeBuffPowerIndex + 1;
        public const int BuffPowerIndex = HealPowerIndex + 1;
        public const int MaxHealthIndex = BuffPowerIndex + 1;
        public const int MaxMortalityPointsIndex = MaxHealthIndex + 1;
        public const int DamageReductionIndex = MaxMortalityPointsIndex + 1;
        public const int DeBuffReductionIndex = DamageReductionIndex + 1;
        public const int EnlightenmentIndex = DeBuffReductionIndex + 1;
        public const int CriticalChanceIndex = EnlightenmentIndex + 1;
        public const int SpeedAmountIndex = CriticalChanceIndex + 1;
        public const int BasicStatsLength = SpeedAmountIndex + 1;

        public virtual void ResetToZero()
        {
            for (int i = 0; i < Count; i++)
            {
                this[i] = 0;
            }
        }
    }

    [Serializable]
    public class CharacterCombatStatsBasic : ICharacterBasicStats
    {
        [Title("Offensive")]
        [SerializeField ,SuffixLabel("Units")] 
        private float attackPower = 10;
        [SerializeField, SuffixLabel("00%")] 
        private float deBuffPower = 0.1f;
        [Title("Support")]
        [SerializeField, SuffixLabel("Units")] 
        private float healPower = 10;
        [SerializeField, SuffixLabel("00%")] 
        private float buffPower = 1;
        [Title("Vitality")]
        [SerializeField, SuffixLabel("Units")] 
        private float maxHealth = 100;
        [SerializeField, SuffixLabel("Units")] 
        private float maxMortalityPoints = 1000;

        [SerializeField, SuffixLabel("00%")] 
        private float damageReduction = 0;
        [SerializeField, SuffixLabel("00%"), Tooltip("Counters DeBuffPower")] 
        private float deBuffReduction = 0;

        
        [Title("Enlightenment")]
        [SerializeField, SuffixLabel("00%"), Tooltip("Affects Harmony gain")] 
        private float enlightenment = 1; // before fight this could be modified
        [SerializeField, SuffixLabel("00%")] 
        private float criticalChance = 0;
        [SerializeField, SuffixLabel("Units")] 
        private float speedAmount = 100;

        public float AttackPower
        {
            get => attackPower;
            set => attackPower = value;
        }

        public float DeBuffPower
        {
            get => deBuffPower;
            set => deBuffPower = value;
        }

        public float HealPower
        {
            get => healPower;
            set => healPower = value;
        }

        public float BuffPower
        {
            get => buffPower;
            set => buffPower = value;
        }

        public float MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }

        public float MaxMortalityPoints
        {
            get => maxMortalityPoints;
            set => maxMortalityPoints = value;
        }

        public float DamageReduction
        {
            get => damageReduction;
            set => damageReduction = value;
        }

        public float DeBuffReduction
        {
            get => deBuffReduction;
            set => deBuffReduction = value;
        }

        public float Enlightenment
        {
            get => enlightenment;
            set => enlightenment = value;
        }

        public float CriticalChance
        {
            get => criticalChance;
            set => criticalChance = value;
        }

        public float SpeedAmount
        {
            get => speedAmount;
            set => speedAmount = value;
        }

        public CharacterCombatStatsBasic()
        { }

        public CharacterCombatStatsBasic(int overrideByDefault)
        {
            float value = overrideByDefault;
            AttackPower = value;
            DeBuffPower = value;

            HealPower = value;
            BuffPower = value;

            MaxHealth = value;
            MaxMortalityPoints = value;
            DamageReduction = value;

            Enlightenment = value;
            CriticalChance = value;
            SpeedAmount = value;
        }

        public CharacterCombatStatsBasic(ICharacterBasicStats copyFrom)
        {
            UtilsStats.CopyStats(this,copyFrom);
        }

    }

    [Serializable]
    public class CharacterCombatStatsFull : CharacterCombatStatsBasic, ICharacterFullStats
    {
        
        [Title("Temporal Stats")]
        [SerializeField, SuffixLabel("units")] 
        private float healthPoints; // before fight this could be reduced
        [SerializeField, SuffixLabel("units")] 
        private float shieldAmount; // before fight this could be increased
        [SerializeField, SuffixLabel("units")] 
        private float mortalityPoints; // after fight this could be reduced
        [SerializeField, SuffixLabel("units")] 
        private float harmonyAmount; // before fight this could be modified
        [SerializeField, SuffixLabel("00%"),Tooltip("Guaranteed amount of initiative on [Fight Starts]")] 
        private float initiativePercentage;
        [SerializeField, SuffixLabel("units")] 
        private int actionsPerInitiative = 0;


        public float HealthPoints
        {
            get => healthPoints;
            set => healthPoints = value;
        }

        public float ShieldAmount
        {
            get => shieldAmount;
            set => shieldAmount = value;
        }

        public float MortalityPoints
        {
            get => mortalityPoints;
            set => mortalityPoints = value;
        }

        public float HarmonyAmount
        {
            get => harmonyAmount;
            set => harmonyAmount = value;
        }

        public float InitiativePercentage
        {
            get => initiativePercentage;
            set => initiativePercentage = value;
        }

        public int ActionsPerInitiative
        {
            get => actionsPerInitiative;
            set => actionsPerInitiative = value;
        }


        public CharacterCombatStatsFull() : base()
        {}

        public CharacterCombatStatsFull(int overrideByDefault) : base(overrideByDefault)
        {
            float value = overrideByDefault;
            HealthPoints = value;
            ShieldAmount = value;
            MortalityPoints = value;
            HarmonyAmount = value;
            InitiativePercentage = value;
            ActionsPerInitiative = (int) value;
        }

        public CharacterCombatStatsFull(ICharacterFullStats copyFrom) 
        {
            UtilsStats.CopyStats(this, copyFrom);
        }

        private const float MaxInitialInitiative = .8f;
        private const float DefaultInitialInitiative = .6f;
        public void AddInitialInitiative(float randomMax = DefaultInitialInitiative)
        {
            float initiativeAddition = Random.value * randomMax;
            InitiativePercentage += initiativeAddition;
            if (InitiativePercentage > MaxInitialInitiative)
                InitiativePercentage = MaxInitialInitiative;
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


    [Serializable]
    public class CharacterUpgradeStats : IStatsUpgradable
    {
        [SerializeField] private float offensivePower;
        [SerializeField] private float supportPower;
        [SerializeField] private float vitalityAmount;
        [SerializeField] private float enlightenment;

        public float OffensivePower
        {
            get => offensivePower;
            set => offensivePower = value;
        }

        public float SupportPower
        {
            get => supportPower;
            set => supportPower = value;
        }

        public float VitalityAmount
        {
            get => vitalityAmount;
            set => vitalityAmount = value;
        }

        public float Enlightenment
        {
            get => enlightenment;
            set => enlightenment = value;
        }
    }
}
