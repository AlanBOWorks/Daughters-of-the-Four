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
    /// Conceptually it's different from a [<seealso cref="CombatStatsHolder"/>] since a character is
    /// something that is permanent in the game while a <see cref="CombatingEntity"/>
    /// exits as long there's a 'Combat'. In other words, 'Character' is a general concept
    /// while <see cref="CombatingEntity"/> is hyper specific to the Combat and their existence
    /// are bond to the Combat.<br></br>
    /// _____ <br></br>
    /// TL;DR:<br></br>
    /// [<see cref="CombatStatsHolder"/>]:
    /// persist as an entity of the Game.<br></br>
    /// [<see cref="CombatingEntity"/>]:
    /// exists while there's a Combat and contains the core data for the combating part
    /// </summary>
    public class CombatingEntity
    { 
        public CombatingEntity(string characterName, GameObject prefab, CombatStatsHolder combatStats)
        {
            CharacterName = characterName;
            InstantiationPrefab = prefab;
            CombatStats = combatStats;

            ReceivedStats = new TrackingStats();
            Events = new CombatCharacterEvents(this);

            DelayBuffHandler = new DelayBuffHandler(this);
            CharacterCriticalBuff = new CharacterCriticalActionHandler(this); //TODO add to events
            Guarding = new CharacterGuarding(this);
            PassivesHolder = new CombatPassivesHolder(this);

            Events.Subscribe(Guarding);
            Events.CheckAndSubscribe(DelayBuffHandler);
        }
        

        [ShowInInspector,GUIColor(.3f,.5f,1)]
        public readonly string CharacterName;

        public readonly GameObject InstantiationPrefab;
        [ShowInInspector, NonSerialized] 
        public UCharacterHolder Holder;

        [ShowInInspector, GUIColor(.4f, .8f, .7f)]
        public readonly CombatStatsHolder CombatStats;

       
        /// <summary>
        /// Used to track the damage received, heals, etc.
        /// </summary>
        public readonly TrackingStats ReceivedStats;

        [ShowInInspector] 
        public readonly CombatPassivesHolder PassivesHolder;

        /// <summary>
        /// <inheritdoc cref="DelayBuffHandler"/>
        /// </summary>
        [ShowInInspector]
        public readonly DelayBuffHandler DelayBuffHandler;
        [ShowInInspector]
        public CharacterCriticalActionHandler CharacterCriticalBuff { get; private set; }

        [ShowInInspector] 
        public CharacterCombatAreasData AreasDataTracker { get; private set; }

        [ShowInInspector, NonSerialized] 
        public readonly CombatCharacterEvents Events;

       

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
        public EnumCharacter.RoleArchetype Role => AreasDataTracker.Role;

        public void Injection(CharacterCombatAreasData areasDataTracker)
        {
            AreasDataTracker = areasDataTracker;
            CombatStats.Injection(areasDataTracker);
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

        [Button]
        public void Injection(IPassiveInjector injector)
        {
            injector.InjectPassive(this);
        }
    }
}
