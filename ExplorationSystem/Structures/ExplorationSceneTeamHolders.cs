using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils.Maths;
using Random = UnityEngine.Random;

namespace ExplorationSystem
{
    public interface IExplorationSceneEntitiesHolder
    {
        IEnumerable<ICombatEntityProvider> GetBasicEntities();
        IEnumerable<ICombatEntityProvider> GetEliteEntities();
    }


    public interface ISceneEntitiesHolder
    {
        FlexTeamStruct<ICombatEntityProvider> GetTeam();
    }

    public interface ISceneAdditionEntitiesHolder
    {
        IEnumerable<ICombatEntityProvider> GetAdditionalMembers();
    }

    [Serializable]
    public class BasicPreMadeSceneEntitiesHolder : ISceneEntitiesHolder, ISceneAdditionEntitiesHolder
    {
        [SerializeField] private TeamComposition[] possibleTeams;

        private TeamComposition GetRandomTeam() => possibleTeams[Random.Range(0, possibleTeams.Length)];
      
        public FlexTeamStruct<ICombatEntityProvider> GetTeam()
        {
            return GetRandomTeam().GetTeam();
        }
        public IEnumerable<ICombatEntityProvider> GetAdditionalMembers()
        {
            return GetRandomTeam().GetAdditionalMembers();
        }

        [Serializable]
        private struct TeamComposition : ITeamTrinityStructureRead<SEnemyPreparationEntity>, ISceneEntitiesHolder, 
            ISceneAdditionEntitiesHolder
        {
            [SerializeField, HorizontalGroup(),LabelWidth(10)]
            private SEnemyPreparationEntity vanguardType;
            [SerializeField, HorizontalGroup(), LabelWidth(10)]
            private SEnemyPreparationEntity attackerType;
            [SerializeField, HorizontalGroup(), LabelWidth(10)]
            private SEnemyPreparationEntity supportType;

            public SEnemyPreparationEntity VanguardType => vanguardType;
            public SEnemyPreparationEntity AttackerType => attackerType;
            public SEnemyPreparationEntity SupportType => supportType;


            public readonly FlexTeamStruct<ICombatEntityProvider> GetTeam()
            {
                return new FlexTeamStruct<ICombatEntityProvider>(vanguardType,attackerType,supportType,null);
            }

            public IEnumerable<ICombatEntityProvider> GetAdditionalMembers()
            {
                yield return vanguardType;
                yield return attackerType;
                yield return supportType;
            }
        }

    }

    [Serializable]
    public class RandomPicksSceneEntitiesHolder : TeamRolesStructure<SEnemyPreparationEntity[]>,ISceneEntitiesHolder,
        ISceneOffEntitiesHolder, ISceneAdditionEntitiesHolder
    {
        [SerializeField, Range(1, 4)] private int maxEntitiesAmount = 3;



        private FlexTeamStruct<ICombatEntityProvider> GetRandomTeam(bool getVanguard = true, bool getAttacker = true,
            bool getSupport = true, bool getFlex = true)
        {
            int count = 0;
            var attacker = GetRandomPick(attackerType, getAttacker);
            var support = GetRandomPick(supportType, getSupport);
            // Vanguard has less priority than attacker & Support
            var vanguard = GetRandomPick(vanguardType, getVanguard);             
            var flex = GetRandomPick(flexType, getFlex);

            return new FlexTeamStruct<ICombatEntityProvider>(vanguard,attacker,support,flex);

            ICombatEntityProvider GetRandomPick(SEnemyPreparationEntity[] collection, bool canGet)
            {
                if (!canGet || collection.Length == 0 || count >= maxEntitiesAmount) return null;
                count++;
                return collection[Random.Range(0, collection.Length)];
            }
        }
        public FlexTeamStruct<ICombatEntityProvider> GetTeam()
        {
            return GetRandomTeam();
        }
        public FlexTeamStruct<ICombatEntityProvider> GetOffMembers(in FlexTeamStruct<ICombatEntityProvider> mainMembers)
        {
            bool getVanguard = mainMembers.VanguardType == null;
            bool getAttacker = mainMembers.AttackerType == null;
            bool getSupport = mainMembers.SupportType == null;
            bool getFlex = mainMembers.FlexType == null;
            return GetRandomTeam(getVanguard, getAttacker, getSupport, getFlex);
        }

        public IEnumerable<ICombatEntityProvider> GetAdditionalMembers()
        {
            return GetRandomTeam().GetEnumerable();
        }
    }

    [Serializable]
    public sealed class HybridSceneEntitiesHolder : ISceneEntitiesHolder
    {
        [SerializeField, Range(0,1), SuffixLabel("00%")] 
        private float randomPickChance = .5f;
        [SerializeReference] 
        private BasicPreMadeSceneEntitiesHolder preMadeSceneEntities = new BasicPreMadeSceneEntitiesHolder();
        [SerializeReference]
        private RandomPicksSceneEntitiesHolder randomPicks = new RandomPicksSceneEntitiesHolder();
        

        public FlexTeamStruct<ICombatEntityProvider> GetTeam()
        {
            return randomPickChance > Random.value
                ? randomPicks.GetTeam()
                : preMadeSceneEntities.GetTeam();
        }
    }

    public interface ISceneOffEntitiesHolder
    {
        FlexTeamStruct<ICombatEntityProvider> GetOffMembers(in FlexTeamStruct<ICombatEntityProvider> mainMembers);
    }
}
