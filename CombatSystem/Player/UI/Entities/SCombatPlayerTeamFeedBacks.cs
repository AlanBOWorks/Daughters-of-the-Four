using System;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    [CreateAssetMenu(fileName = "N [CombatTeamFeedBacks]",
        menuName = "Combat/UI/FeedBacks")]
    public sealed class SCombatPlayerTeamFeedBacks : ScriptableObject, IOppositionTeamStructureRead<CombatPlayerTeamFeedBack>
    {
        [SerializeField,HorizontalGroup()] private CombatPlayerTeamFeedBack playerTeamType;
        [SerializeField,HorizontalGroup()] private CombatPlayerTeamFeedBack enemyTeamType;

        public CombatPlayerTeamFeedBack PlayerTeamType => playerTeamType;
        public CombatPlayerTeamFeedBack EnemyTeamType => enemyTeamType;
    }

    [Serializable]
    public sealed class CombatPlayerTeamFeedBack : FlexPositionMainGroupClass<CombatPlayerRoleFeedBack>,
        ITeamFlexRoleStructureRead<string>, ITeamFlexRoleStructureRead<Sprite>, ITeamFlexRoleStructureRead<Color>
    {
        

        string ITeamRoleStructureRead<string>.VanguardType => VanguardType.GetName();
        Sprite ITeamRoleStructureRead<Sprite>.VanguardType => VanguardType.GetIcon();
        Color ITeamRoleStructureRead<Color>.VanguardType => VanguardType.GetColor();

        string ITeamRoleStructureRead<string>.AttackerType => AttackerType.GetName();
        Sprite ITeamRoleStructureRead<Sprite>.AttackerType => AttackerType.GetIcon();
        Color ITeamRoleStructureRead<Color>.AttackerType => AttackerType.GetColor();

        string ITeamRoleStructureRead<string>.SupportType => SupportType.GetName();
        Sprite ITeamRoleStructureRead<Sprite>.SupportType => SupportType.GetIcon();
        Color ITeamRoleStructureRead<Color>.SupportType => SupportType.GetColor();


        string ITeamFlexRoleStructureRead<string>.FlexType => FlexType.GetName();
        Sprite ITeamFlexRoleStructureRead<Sprite>.FlexType => FlexType.GetIcon();
        Color ITeamFlexRoleStructureRead<Color>.FlexType => FlexType.GetColor();


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
