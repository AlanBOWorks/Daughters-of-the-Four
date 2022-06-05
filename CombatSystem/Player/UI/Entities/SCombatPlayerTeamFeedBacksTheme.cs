using System;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    [CreateAssetMenu(fileName = "N [CombatTeamFeedBacksThemes]",
        menuName = "Combat/UI/FeedBacks [Themes]")]
    public sealed class SCombatPlayerTeamFeedBacksTheme : ScriptableObject, IOppositionTeamStructureRead<CombatPlayerTeamFeedBack>
    {
        [SerializeField,HorizontalGroup()] private CombatPlayerTeamFeedBack playerTeamType;
        [SerializeField,HorizontalGroup()] private CombatPlayerTeamFeedBack enemyTeamType;

        public CombatPlayerTeamFeedBack PlayerTeamType => playerTeamType;
        public CombatPlayerTeamFeedBack EnemyTeamType => enemyTeamType;
    }

    [Serializable]
    public sealed class CombatPlayerTeamFeedBack : ClassTeamRolesStructure<CombatPlayerRoleFeedBack>,
        ITeamFlexStructureRead<string>, ITeamFlexStructureRead<Sprite>, ITeamFlexStructureRead<Color>
    {
        

        string ITeamTrinityStructureRead<string>.VanguardType => VanguardType.GetName();
        Sprite ITeamTrinityStructureRead<Sprite>.VanguardType => VanguardType.GetIcon();
        Color ITeamTrinityStructureRead<Color>.VanguardType => VanguardType.GetColor();

        string ITeamTrinityStructureRead<string>.AttackerType => AttackerType.GetName();
        Sprite ITeamTrinityStructureRead<Sprite>.AttackerType => AttackerType.GetIcon();
        Color ITeamTrinityStructureRead<Color>.AttackerType => AttackerType.GetColor();

        string ITeamTrinityStructureRead<string>.SupportType => SupportType.GetName();
        Sprite ITeamTrinityStructureRead<Sprite>.SupportType => SupportType.GetIcon();
        Color ITeamTrinityStructureRead<Color>.SupportType => SupportType.GetColor();


        string ITeamFlexStructureRead<string>.FlexType => FlexType.GetName();
        Sprite ITeamFlexStructureRead<Sprite>.FlexType => FlexType.GetIcon();
        Color ITeamFlexStructureRead<Color>.FlexType => FlexType.GetColor();


    }

   

    [Serializable]
    public sealed class CombatPlayerRoleFeedBack
    {
        [SerializeField] private string roleName;
        [SerializeField, PreviewField, GUIColor(.3f,.3f,.3f)] private Sprite roleIcon;
        [SerializeField] private Color roleColor;

        public string GetName() => roleName;
        public Sprite GetIcon() => roleIcon;
        public Color GetColor() => roleColor;
    }
}
