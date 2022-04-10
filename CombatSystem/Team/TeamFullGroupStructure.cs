using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public class TeamFullGroupStructure<T> : TeamOffGroupStructure<T>,
        ITeamFullRolesStructureRead<T>, ITeamFlexPositionStructureRead<T>
    {
        public TeamFullGroupStructure() : base(EnumTeam.TeamAlimentRolesLength)
        { }

        protected override int MoveNextThreshold() => EnumTeam.FullTeamIndexCount;

        T ITeamRoleStructureRead<T>.VanguardType => VanguardFrontLine[EnumTeam.MainRoleInFullTeamArrayIndex];
        T ITeamRoleStructureRead<T>.AttackerType => AttackerMidLine[EnumTeam.MainRoleInFullTeamArrayIndex];
        T ITeamRoleStructureRead<T>.SupportType => SupportBackLine[EnumTeam.MainRoleInFullTeamArrayIndex];
        T ITeamFlexRoleStructureRead<T>.FlexType => FlexFlexLine[EnumTeam.MainRoleInFullTeamArrayIndex];

        T ITeamPositionStructureRead<T>.FrontLineType => VanguardFrontLine[EnumTeam.MainRoleInFullTeamArrayIndex];
        T ITeamPositionStructureRead<T>.MidLineType => AttackerMidLine[EnumTeam.MainRoleInFullTeamArrayIndex];
        T ITeamPositionStructureRead<T>.BackLineType => SupportBackLine[EnumTeam.MainRoleInFullTeamArrayIndex];
        T ITeamFlexPositionStructureRead<T>.FlexLineType => FlexFlexLine[EnumTeam.MainRoleInFullTeamArrayIndex];
    }


    [Serializable]
    public abstract class UTeamFullGroupStructure<T> : MonoBehaviour,
        ITeamFullRolesStructureRead<T>,
        ITeamFlexPositionStructureRead<T[]>
        where T : UnityEngine.Object
    {
        [SerializeField, HorizontalGroup("FrontLiners")] private RoleAliment vanguardAliment = new RoleAliment();
        [SerializeField, HorizontalGroup("FrontLiners")] private RoleAliment attackerAliment = new RoleAliment();
        [SerializeField, HorizontalGroup("BackLiners")] private RoleAliment supportAliment = new RoleAliment();
        [SerializeField, HorizontalGroup("BackLiners")] private RoleAliment flexAliment = new RoleAliment();

        private void Awake()
        {
            vanguardAliment.OnAfterDeserialize();
            attackerAliment.OnAfterDeserialize();
            supportAliment.OnAfterDeserialize();
            flexAliment.OnAfterDeserialize();
        }

        public T VanguardType => vanguardAliment.MainRole;
        public T AttackerType => attackerAliment.MainRole;
        public T SupportType => supportAliment.MainRole;
        public T FlexType => flexAliment.MainRole;

        public T SecondaryVanguardElement => vanguardAliment.SecondaryRole;
        public T SecondaryAttackerElement => attackerAliment.SecondaryRole;
        public T SecondarySupportElement => supportAliment.SecondaryRole;
        public T SecondaryFlexElement => flexAliment.SecondaryRole;

        public T ThirdVanguardElement => vanguardAliment.ThirdRole;
        public T ThirdAttackerElement => attackerAliment.ThirdRole;
        public T ThirdSupportElement => supportAliment.ThirdRole;
        public T ThirdFlexElement => flexAliment.ThirdRole;

        [Serializable]
        private sealed class RoleAliment : IFullRoleAlimentRead<T>
        {
            [Title("Members")]
            [SerializeField] private T mainRole;
            [SerializeField] private T secondaryRole;
            [SerializeField] private T thirdRole;
            [Title("Collection")]
            [NonSerialized,ShowInInspector,HideInEditorMode,ReadOnly] public T[] Members;



            public T MainRole => mainRole;
            public T SecondaryRole => secondaryRole;
            public T ThirdRole => thirdRole;


            public void OnAfterDeserialize()
            {
                Members = new[]
                {
                    mainRole,
                    secondaryRole,
                    thirdRole
                };
            }
        }

        public T[] FrontLineType => vanguardAliment.Members;
        public T[] MidLineType => attackerAliment.Members;
        public T[] BackLineType => supportAliment.Members;
        public T[] FlexLineType => flexAliment.Members;
    }
}
