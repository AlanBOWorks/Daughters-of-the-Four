using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Editor
{
    public class UCombatInstantiationForTesting : MonoBehaviour
    {
        [SerializeReference]
        private ICombatTeamProvider playerTeam = new PlayerTeam();
        [SerializeReference]
        private ICombatTeamProvider enemyTeam = new EnemyTeam();

        private bool CanInvoke()
        {
            return !CombatSystemSingleton.GetIsCombatActive();
        }

        [Button,EnableIf("CanInvoke"), DisableInEditorMode]
        private void Invoke()
        {
            CombatInitializationHandler.StartCombat(playerTeam,enemyTeam);
        }

        [Serializable]
        private sealed class ScriptableTeam : ICombatTeamProvider
        {
            [SerializeField]
            private SPreparationEntity[] members = new SPreparationEntity[0];

            public IEnumerable<ICombatEntityProvider> GetSelectedCharacters() => members;

            public int MembersCount => members.Length;
        }
        [Serializable]
        private sealed class EnemyPredefinedScriptableTeam : ICombatTeamProvider
        {
            [SerializeField] private SEnemyPredefinedTeam enemyTeam;

            public IEnumerable<ICombatEntityProvider> GetSelectedCharacters() => enemyTeam.GetSelectedCharacters();

            public int MembersCount => enemyTeam.MembersCount;
        }



        [Serializable]
        private sealed class PlayerTeam : ICombatTeamProvider
        {
            public PlayerTeam()
            {
                vanguard = new DebugEntity(EnumTeam.Role.Vanguard);
                attacker = new DebugEntity(EnumTeam.Role.Attacker);
                support = new DebugEntity(EnumTeam.Role.Support);
                flex = new DebugEntity(EnumTeam.Role.Flex);
            }

            [SerializeReference] private DebugEntity vanguard;
            [SerializeReference] private DebugEntity attacker;
            [SerializeReference] private DebugEntity support;
            [SerializeReference] private DebugEntity flex;

            public IEnumerable<ICombatEntityProvider> GetSelectedCharacters()
            {
                yield return vanguard;
                yield return attacker;
                yield return support;
                yield return flex;
            }

            public int MembersCount
            {
                get
                {
                    int i = 0;
                    foreach (var provider in GetSelectedCharacters())
                    {
                        if(provider != null) i++;
                    }

                    return i;
                }
            }
        }

        [Serializable]
        private sealed class EnemyTeam : ICombatTeamProvider
        {
            private const string ProvisionalSuffix = "Enemy";
            [SerializeReference] private DebugEntity[] enemies = new[]
            {
                new DebugEntity(ProvisionalSuffix,EnumTeam.Role.Vanguard),
                new DebugEntity(ProvisionalSuffix,EnumTeam.Role.Attacker),
                new DebugEntity(ProvisionalSuffix,EnumTeam.Role.Support),
            };


            public IEnumerable<ICombatEntityProvider> GetSelectedCharacters() => enemies;

            public int MembersCount => enemies.Length;
        }

        [Serializable]
        private sealed class DebugEntity : ICombatEntityProvider
        {
            public DebugEntity(string name,TeamAreaData areaData)
            {
                this.name = name;
                this.areaData = areaData;
            }

            public DebugEntity(string name, EnumTeam.Role role) : this(name, new TeamAreaData(role))
            { }

            public DebugEntity() : this("", new TeamAreaData())
            { }

            public DebugEntity(EnumTeam.Role role) : this("", role)
            { }

            [SerializeField] private string name;
            [SerializeField] 
            private TeamAreaData areaData;
            [SerializeField] 
            private GameObject visualPrefab;

            [Title("Stats")]
            [SerializeField]
            private BaseStats baseStats = new BaseStats();
            [Title("Skills")]
            [SerializeReference]
            private ISkillHolder skills = new SameSkillsHolder();

            public IBasicStatsRead<float> GetBaseStats() => baseStats;

            public TeamAreaData GetAreaData() => areaData;

            public string GetProviderEntityName() => "[Provisional] " + name + $" - {areaData.RoleType}";

            public GameObject GetVisualPrefab() => visualPrefab;

            public IStanceStructureRead<IReadOnlyCollection<IFullSkill>> GetPresetSkills() => skills;

            private interface ISkillHolder : IStanceStructureRead<IReadOnlyCollection<IFullSkill>>
            { }

            [Serializable]
            private sealed class SameSkillsHolder : ISkillHolder
            {
                [SerializeField] private SSkillPresetBase[] skills = new SSkillPresetBase[0];
                public IReadOnlyCollection<IFullSkill> AttackingStance => skills;
                public IReadOnlyCollection<IFullSkill> SupportingStance => skills;
                public IReadOnlyCollection<IFullSkill> DefendingStance => skills;
            }
            [Serializable]
            private sealed class StanceSkillsHolder : ISkillHolder
            {
                [SerializeField] private SSkillPresetBase[] attackingSkills = new SSkillPresetBase[0];
                [SerializeField] private SSkillPresetBase[] supportSkills = new SSkillPresetBase[0];
                [SerializeField] private SSkillPresetBase[] defendingSkills = new SSkillPresetBase[0];
                public IReadOnlyCollection<IFullSkill> AttackingStance => attackingSkills;
                public IReadOnlyCollection<IFullSkill> SupportingStance => supportSkills;
                public IReadOnlyCollection<IFullSkill> DefendingStance => defendingSkills;
            }
        }
    }
}
