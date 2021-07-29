using System.Collections.Generic;
using _Team;
using Characters;
using CombatEffects;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;

namespace _CombatSystem
{
    public class CombatCharactersHolder : ICharacterFaction<CombatingTeam>, ICombatPreparationListener
    {
        [ShowInInspector,DisableInEditorMode]
        public CombatingTeam PlayerFaction { get; private set; }
        [ShowInInspector,DisableInEditorMode]
        public CombatingTeam EnemyFaction { get; private set; }
        public CharacterArchetypesList<CombatingEntity> AllEntities { get; private set; }

        public bool IsEntityControllable(CombatingEntity entity)
        {
            return PlayerFaction.Contains(entity);
        }


       
        public void OnBeforeStart(CombatingTeam playerEntities, CombatingTeam enemyEntities,
            CharacterArchetypesList<CombatingEntity> allEntities)
        {
            PlayerFaction = playerEntities;
            EnemyFaction = enemyEntities;
            DoInjection(PlayerFaction, EnemyFaction);
            AllEntities = allEntities;
        }

        private static void DoInjection(
            CombatingTeam playerEntities,
            CombatingTeam enemyEntities)
        {
            InjectFaction(playerEntities, enemyEntities);
            InjectFaction(enemyEntities, playerEntities);

            void InjectFaction(
                CombatingTeam entities,
                CombatingTeam rivalEntities)
            {
                for (var i = 0; i < entities.Count; i++)
                {
                    CombatingEntity entity = entities[i];
                    CharacterSelfGroup characterGroup
                        = new CharacterSelfGroup(entity, entities, rivalEntities);
                    entity.CharacterGroup = characterGroup;
                }
            }
        }

    }


    public class CharacterSelfGroup
    {
        //There's this many amount of list because the references will be used for the Effects (like a LOT)
        /// <summary>
        /// Used mainly for [<seealso cref="SEffectBase.DoEffect"/>];<br></br>
        /// Instead of creating lists for each one of the possible
        /// combinations of targets and effects these Lists can be used instead.<br></br>
        /// Since the possible Lists to chose from
        /// are always the same, these can be used for the referencing the targets for the effects.
        /// <br></br><br></br>
        /// Keep in mind: <br></br>
        /// [<seealso cref="SEffectBase.DoEffect"/>]: Will be used by the DoSkill
        /// in a loop based in the Skill effects and targeting. <br></br>
        /// [<seealso cref="PerformSkillHandler.DoSkill"/>]: Won't use this Lists directly because just uses
        /// one target only.
        /// </summary>
        [HideInEditorMode,HideInPlayMode]
        public readonly List<CombatingEntity> Self;
        /// <summary>
        /// <inheritdoc cref="Self"/>
        /// </summary>
        public readonly List<CombatingEntity> TeamNotSelf;
        /// <inheritdoc cref="Self"/>
        public readonly CombatingTeam Team;
        /// <inheritdoc cref="Self"/>
        public readonly CombatingTeam Enemies;

        public CharacterSelfGroup(CombatingEntity self,
            CombatingTeam team,
            CombatingTeam enemies)
        {
            Self = new List<CombatingEntity>
            {
                self
            };
            Team = team;
            Enemies = enemies;

            TeamNotSelf = new List<CombatingEntity>(team);
            TeamNotSelf.Remove(self);
        }
    }


}
