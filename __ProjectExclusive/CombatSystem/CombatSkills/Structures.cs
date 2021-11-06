using System;
using CombatTeam;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatSkills
{
    [Serializable]
    public class DominionWrapper<TMaster, TElement> : ICondensedDominionStructure<TMaster, TElement>
    {
        [Title("Master")] 
        public TMaster dominion;

        [Title("SubElement")] 
        public TElement guard;
        public TElement control;
        public TElement provoke;
        public TElement stance;

        public TMaster Dominion
        {
            get => dominion;
            set => dominion = value;
        }
        public TElement Guard
        {
            get => guard;
            set => guard = value;
        }
        public TElement Control
        {
            get => control;
            set => control = value;
        }
        public TElement Provoke
        {
            get => provoke;
            set => provoke = value;
        }
        public TElement Stance
        {
            get => stance;
            set => stance = value;
        }
    }
    [Serializable]
    public class SerializableDominionWrapper<TMaster, TElement> : ICondensedDominionStructure<TMaster, TElement>
    where TMaster : new() where TElement : new()
    {
        [Title("Master")] 
        public TMaster dominion = new TMaster();

        [Title("SubElement")]
        public TElement guard = new TElement();
        public TElement control = new TElement();
        public TElement provoke = new TElement();
        public TElement stance = new TElement();

        public TMaster Dominion
        {
            get => dominion;
            set => dominion = value;
        }
        public TElement Guard
        {
            get => guard;
            set => guard = value;
        }
        public TElement Control
        {
            get => control;
            set => control = value;
        }
        public TElement Provoke
        {
            get => provoke;
            set => provoke = value;
        }
        public TElement Stance
        {
            get => stance;
            set => stance = value;
        }
    }

    [Serializable]
    public class SkillTypeStructure<T> : ISkillTypes<T>, ITeamRoleStructure<T>, ITeamStanceStructure<T>
    {
        [SerializeField]
        private T selfType;
        [SerializeField]
        private T offensiveType;
        [SerializeField]
        private T supportType;

        public T SelfType
        {
            get => selfType;
            set => selfType = value;
        }
        public T OffensiveType
        {
            get => offensiveType;
            set => offensiveType = value;
        }
        public T SupportType
        {
            get => supportType;
            set => supportType = value;
        }


        public T Vanguard
        {
            get => selfType;
            set => selfType = value;
        }
        public T Attacker
        {
            get => offensiveType;
            set => offensiveType = value;
        }
        public T Support
        {
            get => supportType;
            set => supportType = value;
        }


        public T OnAttackStance
        {
            get => selfType;
            set => selfType = value;
        }
        public T OnNeutralStance
        {
            get => offensiveType;
            set => offensiveType = value;
        }
        public T OnDefenseStance
        {
            get => selfType;
            set => selfType = value;
        }
    }

    [Serializable]
    public class SkillInteractionsStructure<TMaster,TElement> :
        ICondensedOffensiveStat<TMaster, TElement>,
        ICondensedSupportStat<TMaster, TElement>,
        ICondensedDominionStructure<TMaster,TElement>
    {
        [SerializeField, TabGroup("Offensive")]
        private OffensiveWrapper offensiveValues = new OffensiveWrapper();
        [SerializeField, TabGroup("Support")]
        private SupportWrapper supportValues = new SupportWrapper();
        [SerializeField, TabGroup("Dominion")]
        private DominionWrapper dominionValues = new DominionWrapper();

        public TMaster Offensive
        {
            get => offensiveValues.offensiveElement;
            set => offensiveValues.offensiveElement = value;
        }

        public TMaster Support
        {
            get => supportValues.supportElement;
            set => supportValues.supportElement = value;
        }


        // OFFENSIVE
        public TElement Attack
        {
            get => offensiveValues.attack;
            set => offensiveValues.attack = value;
        }
        public TElement Persistent
        {
            get => offensiveValues.persistent;
            set => offensiveValues.persistent = value;
        }
        public TElement Debuff
        {
            get => offensiveValues.debuff;
            set => offensiveValues.debuff = value;
        }
        public TElement FollowUp
        {
            get => offensiveValues.followUp;
            set => offensiveValues.followUp = value;
        }

        // SUPPORT
        public TElement Heal
        {
            get => supportValues.heal;
            set => supportValues.heal = value;
        }
        public TElement Buff
        {
            get => supportValues.buff;
            set => supportValues.buff = value;
        }
        public TElement ReceiveBuff
        {
            get => supportValues.receiveBuff;
            set => supportValues.receiveBuff = value;
        }
        public TElement Shielding
        {
            get => supportValues.shielding;
            set => supportValues.shielding = value;
        }

        // DOMINION
        public TMaster Dominion
        {
            get => dominionValues.Dominion;
            set => dominionValues.Dominion = value;
        }
        public TElement Guard
        {
            get => dominionValues.Guard;
            set => dominionValues.Guard = value;
        }
        public TElement Control
        {
            get => dominionValues.Control;
            set => dominionValues.Control = value;
        }
        public TElement Provoke
        {
            get => dominionValues.Provoke;
            set => dominionValues.Provoke = value;
        }
        public TElement Stance
        {
            get => dominionValues.Stance;
            set => dominionValues.Stance = value;
        }

        [Serializable]
        private class OffensiveWrapper : OffensiveWrapper<TMaster,TElement>
        { }
        [Serializable]
        private class SupportWrapper : SupportWrapper<TMaster,TElement>
        { }
        [Serializable]
        private class DominionWrapper : DominionWrapper<TMaster,TElement>
        { }
    }

    [Serializable]
    public class SerializableSkillInteractionsStructure<TMaster, TElement> :
        ICondensedOffensiveStat<TMaster, TElement>,
        ICondensedSupportStat<TMaster, TElement>,
        ICondensedDominionStructure<TMaster,TElement>
        where TMaster : new() where TElement : new()
    {
        [SerializeField, TabGroup("Offensive")]
        private OffensiveWrapper offensiveValues = new OffensiveWrapper();
        [SerializeField, TabGroup("Support")]
        private SupportWrapper supportValues = new SupportWrapper();
        [SerializeField, TabGroup("Dominion")]
        private DominionWrapper dominionValues = new DominionWrapper();


        public TMaster Offensive
        {
            get => offensiveValues.offensiveElement;
            set => offensiveValues.offensiveElement = value;
        }

        public TMaster Support
        {
            get => supportValues.supportElement;
            set => supportValues.supportElement = value;
        }


        // OFFENSIVE
        public TElement Attack
        {
            get => offensiveValues.attack;
            set => offensiveValues.attack = value;
        }
        public TElement Persistent
        {
            get => offensiveValues.persistent;
            set => offensiveValues.persistent = value;
        }
        public TElement Debuff
        {
            get => offensiveValues.debuff;
            set => offensiveValues.debuff = value;
        }
        public TElement FollowUp
        {
            get => offensiveValues.followUp;
            set => offensiveValues.followUp = value;
        }

        // SUPPORT
        public TElement Heal
        {
            get => supportValues.heal;
            set => supportValues.heal = value;
        }
        public TElement Buff
        {
            get => supportValues.buff;
            set => supportValues.buff = value;
        }
        public TElement ReceiveBuff
        {
            get => supportValues.receiveBuff;
            set => supportValues.receiveBuff = value;
        }
        public TElement Shielding
        {
            get => supportValues.shielding;
            set => supportValues.shielding = value;
        }

        // DOMINION
        public TMaster Dominion
        {
            get => dominionValues.Dominion;
            set => dominionValues.Dominion = value;
        }
        public TElement Guard
        {
            get => dominionValues.Guard;
            set => dominionValues.Guard = value;
        }
        public TElement Control
        {
            get => dominionValues.Control;
            set => dominionValues.Control = value;
        }
        public TElement Provoke
        {
            get => dominionValues.Provoke;
            set => dominionValues.Provoke = value;
        }
        public TElement Stance
        {
            get => dominionValues.Stance;
            set => dominionValues.Stance = value;
        }


        [Serializable]
        private class OffensiveWrapper : SerializableOffensiveWrapper<TMaster, TElement>
        { }
        [Serializable]
        private class SupportWrapper : SerializableSupportWrapper<TMaster, TElement>
        { }
        [Serializable]
        private class DominionWrapper : SerializableDominionWrapper<TMaster, TElement>
        { }
    }
}
