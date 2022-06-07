using System;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    [CreateAssetMenu(fileName = "N [Roles Themes]",
        menuName = "Combat/Holders/Roles Type/RoleFeedBacks [Themes]")]
    public sealed class SCombatRolesThemesHolder : ScriptableObject
    {
        [SerializeField, HorizontalGroup()] private CombatPlayerTeamFeedBack holder;

        public CombatPlayerTeamFeedBack GetHolder() => holder;
    }

    [Serializable]
    public sealed class CombatPlayerTeamFeedBack : ClassTeamRolesStructure<CombatThemeHolder>,
        ITeamFlexStructureRead<string>, ITeamFlexStructureRead<Sprite>, ITeamFlexStructureRead<Color>
    {
        

        string ITeamTrinityStructureRead<string>.VanguardType => VanguardType.GetThemeName();
        Sprite ITeamTrinityStructureRead<Sprite>.VanguardType => VanguardType.GetThemeIcon();
        Color ITeamTrinityStructureRead<Color>.VanguardType => VanguardType.GetThemeColor();

        string ITeamTrinityStructureRead<string>.AttackerType => AttackerType.GetThemeName();
        Sprite ITeamTrinityStructureRead<Sprite>.AttackerType => AttackerType.GetThemeIcon();
        Color ITeamTrinityStructureRead<Color>.AttackerType => AttackerType.GetThemeColor();

        string ITeamTrinityStructureRead<string>.SupportType => SupportSkillType.GetThemeName();
        Sprite ITeamTrinityStructureRead<Sprite>.SupportType => SupportSkillType.GetThemeIcon();
        Color ITeamTrinityStructureRead<Color>.SupportType => SupportSkillType.GetThemeColor();


        string ITeamFlexStructureRead<string>.FlexType => FlexType.GetThemeName();
        Sprite ITeamFlexStructureRead<Sprite>.FlexType => FlexType.GetThemeIcon();
        Color ITeamFlexStructureRead<Color>.FlexType => FlexType.GetThemeColor();


    }

   

}
