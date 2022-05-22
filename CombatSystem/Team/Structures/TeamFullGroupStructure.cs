using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public class TeamFullGroupStructure<T> : TeamOffGroupStructure<T>,
        ITeamFullStructureRead<T>, ITeamFlexPositionStructureRead<T>,
        ITeamFlexStructureInject<T>
    {
        public TeamFullGroupStructure() : base(EnumTeam.TeamAlimentRolesLength)
        { }

        T ITeamTrinityStructureRead<T>.VanguardType => VanguardFrontLine[EnumTeam.MainRoleInFullTeamArrayIndex];
       
        T ITeamTrinityStructureRead<T>.AttackerType => AttackerMidLine[EnumTeam.MainRoleInFullTeamArrayIndex];
        T ITeamTrinityStructureRead<T>.SupportType => SupportBackLine[EnumTeam.MainRoleInFullTeamArrayIndex];
        T ITeamFlexStructureRead<T>.FlexType => FlexFlexLine[EnumTeam.MainRoleInFullTeamArrayIndex];

        T ITeamPositionStructureRead<T>.FrontLineType => VanguardFrontLine[EnumTeam.MainRoleInFullTeamArrayIndex];
        T ITeamPositionStructureRead<T>.MidLineType => AttackerMidLine[EnumTeam.MainRoleInFullTeamArrayIndex];
        T ITeamPositionStructureRead<T>.BackLineType => SupportBackLine[EnumTeam.MainRoleInFullTeamArrayIndex];
        T ITeamFlexPositionStructureRead<T>.FlexLineType => FlexFlexLine[EnumTeam.MainRoleInFullTeamArrayIndex];


        public new T VanguardType
        {
            get => VanguardFrontLine[EnumTeam.MainRoleInFullTeamArrayIndex];
            set => VanguardFrontLine[EnumTeam.MainRoleInFullTeamArrayIndex] = value;
        }

        public new T AttackerType
        {
            get => AttackerMidLine[EnumTeam.MainRoleInFullTeamArrayIndex];
            set => AttackerMidLine[EnumTeam.MainRoleInFullTeamArrayIndex] = value;
        }

        public new T SupportType
        {
            get => SupportBackLine[EnumTeam.MainRoleInFullTeamArrayIndex];
            set => SupportBackLine[EnumTeam.MainRoleInFullTeamArrayIndex] = value;
        }

        public new T FlexType
        {
            get => FlexFlexLine[EnumTeam.MainRoleInFullTeamArrayIndex];
            set => FlexFlexLine[EnumTeam.MainRoleInFullTeamArrayIndex] = value;
        }

        public new IEnumerator<T> GetEnumerator()
        {
            yield return VanguardType;
            yield return AttackerType;
            yield return SupportType;
            yield return FlexType;

            yield return SecondaryVanguardElement;
            yield return SecondaryAttackerElement;
            yield return SecondarySupportElement;
            yield return SecondaryFlexElement;

            yield return ThirdVanguardElement;
            yield return ThirdAttackerElement;
            yield return ThirdSupportElement;
            yield return ThirdFlexElement;
        }
    }


    [Serializable]
    public abstract class UTeamFullGroupStructure<T> : MonoBehaviour,
        ITeamFullStructureRead<T>,
        ITeamFlexPositionStructureRead<T[]>
    {
        [SerializeField, HorizontalGroup("FrontLiners")] private RoleAliment frontAliment = new RoleAliment();
        [SerializeField, HorizontalGroup("FrontLiners")] private RoleAliment midAliment = new RoleAliment();
        [SerializeField, HorizontalGroup("BackLiners")] private RoleAliment backAliment = new RoleAliment();
        [SerializeField, HorizontalGroup("BackLiners")] private RoleAliment flexAliment = new RoleAliment();

        private void Awake()
        {
            frontAliment.OnAfterDeserialize();
            midAliment.OnAfterDeserialize();
            backAliment.OnAfterDeserialize();
            flexAliment.OnAfterDeserialize();
        }

        public T VanguardType => frontAliment.MainRole;
        public T AttackerType => midAliment.MainRole;
        public T SupportType => backAliment.MainRole;
        public T FlexType => flexAliment.MainRole;

        public T SecondaryVanguardElement => frontAliment.SecondaryRole;
        public T SecondaryAttackerElement => midAliment.SecondaryRole;
        public T SecondarySupportElement => backAliment.SecondaryRole;
        public T SecondaryFlexElement => flexAliment.SecondaryRole;

        public T ThirdVanguardElement => frontAliment.ThirdRole;
        public T ThirdAttackerElement => midAliment.ThirdRole;
        public T ThirdSupportElement => backAliment.ThirdRole;
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

        public T[] FrontLineType => frontAliment.Members;
        public T[] MidLineType => midAliment.Members;
        public T[] BackLineType => backAliment.Members;
        public T[] FlexLineType => flexAliment.Members;
    }
}
