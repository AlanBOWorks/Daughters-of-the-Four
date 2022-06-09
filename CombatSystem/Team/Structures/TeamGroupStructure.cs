using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public class TeamMainGroupStructure<T> : ITeamFlexPositionStructureRead<T>,
        ITeamFlexStructureInject<T>
    {
        [ShowInInspector, TitleGroup("Vanguard")]
        public T VanguardType { get; set; }
        [ShowInInspector, TitleGroup("Attacker")]
        public T AttackerType { get; set; }
        [ShowInInspector, TitleGroup("Support")]
        public T SupportType { get; set; }
        [ShowInInspector, TitleGroup("Flex")]
        public T FlexType { get; set; }


        public T FrontLineType
        {
            get => VanguardType;
            set => VanguardType = value;
        }
        public T MidLineType
        {
            get => AttackerType;
            set => AttackerType = value;
        }
        public T BackLineType
        {
            get => SupportType;
            set => SupportType = value;
        }
        public T FlexLineType
        {
            get => FlexType;
            set => FlexType = value;
        }
    }
    public class TeamOffGroupStructure<T> : ITeamOffStructureRead<T>
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


    public interface ITeamOffStructureRead<out T>
    {
        T SecondaryVanguardElement { get; }
        T SecondaryAttackerElement { get; }
        T SecondarySupportElement { get; }
        T SecondaryFlexElement { get; }

        T ThirdVanguardElement { get; }
        T ThirdAttackerElement { get; }
        T ThirdSupportElement { get; }
        T ThirdFlexElement { get; }
    }
}
