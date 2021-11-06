using System;
using CombatSkills;
using CombatTeam;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace GameTheme
{
    public abstract class SGameThemeHolder<TMaster,TElement> : ScriptableObject
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

    }

}
