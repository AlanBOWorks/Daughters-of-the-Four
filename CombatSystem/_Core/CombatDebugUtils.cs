using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace CombatSystem._Core
{
#if UNITY_EDITOR
    internal sealed class CombatDebugUtils
    {
        private bool IsCombatActive()
        {
            return CombatSystemSingleton.GetIsCombatActive();
        }
        [Button("Knock Out [Enemy]"), EnableIf("IsCombatActive")]
        private void KnockOutEnemy(EnumTeam.ActiveRole target)
        {
            var enemyTeam = CombatSystemSingleton.OppositionTeam;
            KnockOutEntity(enemyTeam, target);
        }
        [Button("Knock Out [Player]"), EnableIf("IsCombatActive")]
        private void KnockOutAlly(EnumTeam.ActiveRole target)
        {
            var playerTeam = CombatSystemSingleton.PlayerTeam;
            KnockOutEntity(playerTeam, target);
        }

        private static void KnockOutEntity(CombatTeam team, EnumTeam.ActiveRole target)
        {
            var member = UtilsTeam.GetMember(in team, in target);
            if(member == null) return;
            var targetStats = member.Stats;
            targetStats.CurrentHealth = 0;
            targetStats.CurrentMortality = 0;
            targetStats.CurrentShields = 0;

            CombatEntity performer = null;
            CombatSystemSingleton.EventsHolder.OnDamageDone(performer,in member, 99999);
            CombatSystemSingleton.EventsHolder.OnKnockOut(performer,in member);
        }

        private sealed class CombatDebugUtilsWindow : OdinEditorWindow
        {
            public CombatDebugUtils Utils;

            [MenuItem("Combat/Debug/Utils", priority = -100)]
            private static void ShowWindow()
            {
                CombatDebugUtilsWindow window = GetWindow<CombatDebugUtilsWindow>();
                
                if(window.Utils == null) window.Utils = new CombatDebugUtils();
                window.Show();
            }
        }
    } 


    
#endif
}
