using System;
using CombatSystem.Localization;
using CombatSystem.Team;
using Localization;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;


namespace CombatSystem
{
    public static class LocalizeTeam
    {
        private const string RoleTag = "role_";
        private const string TagRoleInvalid = RoleTag + "Invalid";
        private const string TagRoleVanguard = RoleTag + "Vanguard";
        private const string TagRoleAttacker = RoleTag + "Attacker";
        private const string TagRoleSupport = RoleTag + "Support";
        private const string TagRoleFlex = RoleTag + "Flex";
        public static string GetLocaleID(EnumTeam.Role role) =>
            role switch
            {
                EnumTeam.Role.Vanguard => TagRoleVanguard,
                EnumTeam.Role.Attacker => TagRoleAttacker,
                EnumTeam.Role.Support => TagRoleSupport,
                EnumTeam.Role.Flex => TagRoleFlex,
                _ => TagRoleInvalid
            };

        public static string Localize(EnumTeam.Role role)
        {
            var roleId = GetLocaleID(role);
            var language = ProjectLocalizationSingleton.CurrentLanguage;

            return role.ToString();
        }
    }
}
