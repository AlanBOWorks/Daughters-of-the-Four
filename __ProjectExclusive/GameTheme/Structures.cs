using System;
using CombatSkills;
using CombatTeam;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace GameTheme
{
    public abstract class SGameThemeHolder<TMaster,TElement> : ScriptableObject,
        ISkillInteractionStructureRead<TElement>
    {
        [SerializeField]
        private MasterTypesThemeHolder masterTypesTheme = new MasterTypesThemeHolder();
        [SerializeField] 
        private DominionTypesThemeHolder dominionTypesTheme = new DominionTypesThemeHolder();

        [SerializeField, HorizontalGroup()]
        private RoleTypesThemeHolder roleTypesTheme = new RoleTypesThemeHolder();
        [SerializeField, HorizontalGroup()]
        private SkillTypesThemeHolder skillTypesTheme = new SkillTypesThemeHolder();

        public IMasterStats<TMaster> GetMasterTypes() => masterTypesTheme;
        public IBaseStats<TElement> GetBaseStatsTypes() => masterTypesTheme;
        public ITeamRoleStructure<TElement> GetRoleTypes() => roleTypesTheme;
        public ISkillTypes<TElement> GetSkillTypes() => skillTypesTheme;


        [Serializable, GUIColor(.3f, .3f, .3f)]
        private class MasterTypesThemeHolder : CondensedMasterStructure<TMaster, TElement>
        { }
        [Serializable, GUIColor(.3f, .3f, .3f)]
        private class DominionTypesThemeHolder : DominionWrapper<TMaster, TElement>
        { }

        [Serializable, GUIColor(.3f, .3f, .3f)]
        private class RoleTypesThemeHolder : RoleArchetypeStructure<TElement>
        { }

        [Serializable, GUIColor(.3f, .3f, .3f)]
        private class SkillTypesThemeHolder : SkillTypeStructure<TElement>
        { }

        public TElement Attack => masterTypesTheme.Attack;
        public TElement Persistent => masterTypesTheme.Persistent;
        public TElement Debuff => masterTypesTheme.Persistent;
        public TElement FollowUp => masterTypesTheme.FollowUp;

        public TElement Heal => masterTypesTheme.Heal;
        public TElement Shielding => masterTypesTheme.Shielding;
        public TElement Buff => masterTypesTheme.Buff;
        public TElement ReceiveBuff => masterTypesTheme.ReceiveBuff;

        public TElement Guard => dominionTypesTheme.guard;
        public TElement Control => dominionTypesTheme.control;
        public TElement Provoke => dominionTypesTheme.provoke;
        public TElement Stance => dominionTypesTheme.stance;
    }

}
