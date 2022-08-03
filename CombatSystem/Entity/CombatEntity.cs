using System;
using System.Collections;
using System.Collections.Generic;
using CombatSystem.Luck;
using CombatSystem.Skills;
using CombatSystem.Stats;
using CombatSystem.Team;
using Localization.Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Entity
{
    public sealed class CombatEntity : ITeamAreaDataRead, ICombatEntityInfoHolder
    {
        public CombatEntity(ICombatEntityProvider preparationData)
        {

            Provider = preparationData;

            var baseStats = preparationData.GetBaseStats();
            var areaData = preparationData.GetAreaData();
            var skills = preparationData.GetPresetSkills();

            Stats = new CombatStats(baseStats);
            _skillsHolder = new SkillsHolder(this,skills);
            DiceValuesHolder = new DiceValuesHolder(Stats);

            RoleType = areaData.RoleType;
            PositioningType = areaData.PositioningType;

            DamageDoneTracker = new CombatEntityVitalityTracker();
            DamageReceiveTracker = new CombatEntityVitalityTracker();
            ProtectionDoneTracker = new CombatEntityVitalityTracker();
            ProtectionReceiveTracker = new CombatEntityVitalityTracker();


            // PLAYER's essentials
            CombatCharacterName = 
                LocalizationPlayerCharacters.LocalizeCharactersName(Provider.GetProviderEntityName());
            CombatCharacterFullName = 
                LocalizationPlayerCharacters.LocalizeCharactersName(Provider.GetProviderEntityFullName());
            CombatCharacterShorterName =
                LocalizationPlayerCharacters.LocalizeCharactersName(Provider.GetProviderShorterName());

            // COMBAT's others
            DiceValuesHolder.RollDice();
        }
        public CombatEntity(ICombatEntityProvider preparationData, CombatTeam team) :
            this(preparationData)
        {
            InjectTeam(team);
        }

        
        public void Injection(EnumTeam.RolePriorityType rolePriorityType)
        {
            RolePriorityType = rolePriorityType;
            ActiveRole = UtilsTeam.GetActiveRole(RoleType, rolePriorityType);
        }

        /// <summary>
        /// Localized name
        /// </summary>
        public string CombatCharacterName;
        public string CombatCharacterFullName;
        public string CombatCharacterShorterName;

        [ShowInInspector, InlineEditor()] 
        public readonly ICombatEntityProvider Provider;
        public GameObject InstantiationReference;
        [ShowInInspector]
        public ICombatEntityBody Body { get; internal set; }

        [ShowInInspector]
        public readonly CombatStats Stats;

        [ShowInInspector]
        private readonly SkillsHolder _skillsHolder;

        [ShowInInspector] 
        public readonly DiceValuesHolder DiceValuesHolder;



        [ShowInInspector]
        public EnumTeam.Role RoleType { get; }

        [ShowInInspector]
        public EnumTeam.ActiveRole ActiveRole { get; internal set; }

        [ShowInInspector]
        public EnumTeam.Positioning PositioningType { get; private set; }
        [ShowInInspector]
        public EnumTeam.RolePriorityType RolePriorityType { get; private set; }


        [ShowInInspector]
        public CombatTeam Team { get; private set; }

        public bool IsActive() => Team.IsActive(this);

        [ShowInInspector,HorizontalGroup("Damage Tracker")]
        public readonly CombatEntityVitalityTracker DamageDoneTracker;
        [ShowInInspector,HorizontalGroup("Damage Tracker")]
        public readonly CombatEntityVitalityTracker DamageReceiveTracker;

        [ShowInInspector, HorizontalGroup("Heal Tracker")]
        public readonly CombatEntityVitalityTracker ProtectionDoneTracker;
        [ShowInInspector, HorizontalGroup("Heal Tracker")]
        public readonly CombatEntityVitalityTracker ProtectionReceiveTracker;

        /// <summary>
        /// The provider's name; this is for Dev purposes.<br></br>
        /// For Player's info, use [<seealso cref="CombatCharacterName"/>] instead
        /// </summary>
        /// <returns></returns>
        public string GetProviderEntityName() => Provider.GetProviderEntityName();

        public string GetProviderEntityFullName()
        {
            throw new NotImplementedException();
        }

        public string GetProviderShorterName()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<CombatSkill> AllSkills => _skillsHolder;

        public bool CanBeTarget()
        {
            return Stats.IsAlive();
        }

        public EnumTeam.StanceFull GetCurrentStance() => Team.DataValues.CurrentStance;

        //Todo make forced disruptionStance
        public bool IsDisrupted;

        private void InjectTeam(CombatTeam team)
        {
            Team = team;
        }


        /// <summary>
        /// ReadOnly StanceSkills in the current [<seealso cref="EnumTeam.StanceFull"/>];<br></br>
        /// To modify the collection check [<seealso cref="SkillsHolder.GetCurrentSkills"/>]
        /// </summary>
        public IReadOnlyList<CombatSkill> GetCurrentSkills() 
            => _skillsHolder.GetCurrentSkills();

        public IReadOnlyList<CombatSkill> GetStanceSkills(EnumTeam.StanceFull stance)
            => UtilsTeam.GetElement(stance, _skillsHolder);

        // ----- Pseudo - EVENTS
        public void OnActionStart()
        {
            DamageReceiveTracker.ResetOnActionValues(); //Reset last OnActionValues
            DamageDoneTracker.ResetOnActionValues();

            DiceValuesHolder.RollDice();
        }
        public void OnActionFinish()
        {
            
        }

        public void OnSequenceStart()
        {
            Stats.OnSequenceStart();
            DamageReceiveTracker.ResetOnSequenceValues();
            DamageDoneTracker.ResetOnSequenceValues();

            UtilsCombatStats.ResetTempoStats(Stats);
        }
        public void OnSequenceFinish()
        {
            DiceValuesHolder.RollDice();

            DamageReceiveTracker.ResetOnActionValues(); //Because on OnActionStart triggers no more
            DamageDoneTracker.ResetOnActionValues();

            Stats.OnSequenceFinish();
        }

        // ----- CLASSES
        private sealed class SkillsHolder : IFullStanceStructureRead<List<CombatSkill>>, 
            IReadOnlyCollection<CombatSkill>
        {
            public SkillsHolder(CombatEntity user,
                IStanceStructureRead<IReadOnlyCollection<IFullSkill>> skills)
            {
                _user = user;
                DisruptionStance = new List<CombatSkill>();
                AttackingStance = GenerateSkills(skills.AttackingStance);
                SupportingStance = GenerateSkills(skills.SupportingStance);
                DefendingStance = GenerateSkills(skills.DefendingStance);

                _allSkills = new HashSet<CombatSkill>(AttackingStance);
                AddSkills(SupportingStance);
                AddSkills(DefendingStance);

                void AddSkills(IEnumerable<CombatSkill> stanceSkills)
                {
                    foreach (CombatSkill skill in stanceSkills)
                    {
                        _allSkills.Add(skill);
                    }
                }

                List<CombatSkill> GenerateSkills(IReadOnlyCollection<IFullSkill> presetSkills)
                {
                    List<CombatSkill> generatedSkills;
                    if (presetSkills == null)
                    {
                        generatedSkills = new List<CombatSkill>();
                    }
                    else
                    {
                        generatedSkills = new List<CombatSkill>(presetSkills.Count);
                        foreach (var preset in presetSkills)
                        {
                            if (preset == null)
                            {
                                Debug.LogWarning("Preset skill was [NULL]\n"
                                                 + $"Check Entity Skills: {user.Provider}");
                                continue;
                            }

                            CombatSkill skill = new CombatSkill(preset);
                            generatedSkills.Add(skill);
                        }
                    }
                    

                    return generatedSkills;
                }
            }


            private readonly HashSet<CombatSkill> _allSkills;
            private readonly CombatEntity _user;

            [ShowInInspector]
            public List<CombatSkill> AttackingStance { get; }
            [ShowInInspector]
            public List<CombatSkill> SupportingStance { get; }
            [ShowInInspector]
            public List<CombatSkill> DefendingStance { get; }
            [ShowInInspector]
            public List<CombatSkill> DisruptionStance { get; }

            public List<CombatSkill> GetCurrentSkills()
            {
                return UtilsTeam.GetElement(_user.GetCurrentStance(), this);
            }



            public IEnumerator<CombatSkill> GetEnumerator() => _allSkills.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            public int Count => _allSkills.Count;
        }

    }

    public readonly struct EntityPairInteraction
    {
        public readonly CombatEntity Performer;
        public readonly CombatEntity Target;
        public readonly bool AreAllies;


        public EntityPairInteraction(CombatEntity performer)
        {
            Performer = performer;
            Target = performer;
            AreAllies = true;
        }
        public EntityPairInteraction(CombatEntity performer, CombatEntity target)
        {
            Performer = performer;
            Target = target;
            AreAllies = performer.Team.Contains(target);
        }

        public void Extract(out CombatEntity performer, out CombatEntity target)
        {
            performer = Performer;
            target = Target;
        }

    }

    public interface ICombatEntityProvider : ICombatEntityPreparation, ICombatEntityInfoHolder
    {
        GameObject GetVisualPrefab();
        IStanceStructureRead<IReadOnlyCollection<IFullSkill>> GetPresetSkills();
    }

    public interface ICombatEntityPreparation
    {
        IBasicStatsRead<float> GetBaseStats();
        TeamAreaData GetAreaData();
    }

    public interface ICombatEntityProviderHolder
    {
        ICombatEntityProvider GetEntityProvider();
    }


    public interface ICombatEntityInfoHolder
    {
        string GetProviderEntityName();
        string GetProviderEntityFullName();
        string GetProviderShorterName();
    }
}
