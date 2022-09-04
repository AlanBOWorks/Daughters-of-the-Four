using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CombatSystem.Team
{
    public class TeamFullGroupStructure<T> : TeamRolesStructure<T>, ITeamFullStructureRead<T>
    {
        [ShowInInspector, TitleGroup("Vanguard")]
        public T SecondaryVanguardElement { get; set; }
        [ShowInInspector, TitleGroup("Attacker")]
        public T SecondaryAttackerElement { get; set; }
        [ShowInInspector, TitleGroup("Support")]
        public T SecondarySupportElement { get; set; }
        [ShowInInspector, TitleGroup("Flex")]
        public T SecondaryFlexElement { get; set; }

        [ShowInInspector, TitleGroup("Vanguard")]
        public T ThirdVanguardElement { get; set; }
        [ShowInInspector, TitleGroup("Attacker")]
        public T ThirdAttackerElement { get; set; }
        [ShowInInspector, TitleGroup("Support")]
        public T ThirdSupportElement { get; set; }
        [ShowInInspector, TitleGroup("Flex")]
        public T ThirdFlexElement { get; set; }
    }

    [Serializable]
    public class TeamFullGroupArrayStructure<T> : ITeamFullStructureRead<T>
    {
        [SerializeField]
        private T[] vanguards = new T[2];
        [SerializeField]
        private T[] attackers = new T[2];
        [SerializeField]
        private T[] supports = new T[2];
        [SerializeField]
        private T[] flexers = new T[2];

        public T VanguardType { get => vanguards[0]; set => vanguards[0] = value; }
        public T AttackerType { get => attackers[0]; set => attackers[0] = value; }
        public T SupportType { get => supports[0]; set => supports[0] = value; }
        public T FlexType { get => flexers[0]; set => flexers[0] = value; }

        public T SecondaryVanguardElement { get => vanguards[1]; set => vanguards[1] = value; }
        public T SecondaryAttackerElement { get => attackers[1]; set => attackers[1] = value; }
        public T SecondarySupportElement { get => supports[1]; set => supports[1] = value; }
        public T SecondaryFlexElement { get => flexers[1]; set => flexers[1] = value; }

        public T ThirdVanguardElement { get => vanguards[2]; set => vanguards[2] = value; }
        public T ThirdAttackerElement { get => attackers[2]; set => attackers[2] = value; }
        public T ThirdSupportElement { get => supports[2]; set => supports[2] = value; }
        public T ThirdFlexElement { get => flexers[2]; set => flexers[2] = value; }

        public static void Clear<TType>(TeamFullGroupStructure<TType> structure) where TType : Object
        {
            structure.VanguardType = null;
            structure.AttackerType = null;
            structure.SupportType = null;
            structure.FlexType = null;

            structure.SecondaryVanguardElement = null;
            structure.SecondaryAttackerElement = null;
            structure.SecondarySupportElement = null;
            structure.SecondaryFlexElement = null;

            structure.ThirdVanguardElement = null;
            structure.ThirdAttackerElement = null;
            structure.ThirdSupportElement = null;
            structure.ThirdFlexElement = null;
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

        public T SecondaryVanguardElement => frontAliment.SecondaryRole ?? frontAliment.MainRole;
        public T SecondaryAttackerElement => midAliment.SecondaryRole ?? midAliment.MainRole;
        public T SecondarySupportElement => backAliment.SecondaryRole ?? backAliment.MainRole;
        public T SecondaryFlexElement => flexAliment.SecondaryRole ?? flexAliment.MainRole;

        public T ThirdVanguardElement => frontAliment.ThirdRole ?? SecondaryVanguardElement;
        public T ThirdAttackerElement => midAliment.ThirdRole ?? SecondaryAttackerElement;
        public T ThirdSupportElement => backAliment.ThirdRole ?? SecondarySupportElement;
        public T ThirdFlexElement => flexAliment.ThirdRole ?? SecondaryFlexElement;

        [Serializable]
        private sealed class RoleAliment : ITeamAlimentStructureRead<T>
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

    [Serializable]
    public abstract class UTeamStructure<T> : MonoBehaviour, ITeamFlexPositionStructureRead<T>, ITeamFlexStructureRead<T>
    {
        [SerializeField] private T frontLineType;
        [SerializeField] private T midLineType;
        [SerializeField] private T backLineType;
        [SerializeField] private T flexLineType;


        public T FrontLineType => frontLineType;
        public T MidLineType => midLineType;
        public T BackLineType => backLineType;
        public T FlexLineType => flexLineType;

        public T VanguardType => frontLineType;
        public T AttackerType => midLineType;
        public T SupportType => backLineType;
        public T FlexType => flexLineType;

        public T GetElement(EnumTeam.Role role)
        {
            return UtilsTeam.GetElement(role, this);
        }

        public IEnumerable<T> GetEnumerable()
        {
            yield return frontLineType;
            yield return midLineType;
            yield return backLineType;
            yield return flexLineType;
        }

    } 
}
