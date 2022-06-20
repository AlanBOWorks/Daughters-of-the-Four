using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;

namespace CombatSystem.Team.VanguardEffects
{
    public sealed class VanguardEventsHandler : ISkillUsageListener
    {
        public void OnCombatSkillSubmit(in SkillUsageValues values)
        {
        }

        public void OnCombatSkillPerform(in SkillUsageValues values)
        {
            var skill = values.UsedSkill;
            if (!UtilsSkill.IsOffensiveSkill(skill)) return;

            var attacker = values.Performer;
            var target = values.Target;
            HandleVanguardOffensive(attacker,target);
        }
        public static void HandleVanguardOffensive(CombatEntity attacker, CombatEntity onTarget)
        {
            var targetTeam = onTarget.Team;
            if (targetTeam.Contains(attacker)) return;

            targetTeam.VanguardEffectsHolder.OnOffensiveDone(attacker, onTarget);
        }


        public void OnCombatSkillFinish(CombatEntity performer)
        {
        }

        private static void InvokeTeamsVanguardEffects()
        {
            var playerTeam = CombatSystemSingleton.PlayerTeam;
            HandleTeam(playerTeam);
            var enemyTeam = CombatSystemSingleton.OppositionTeam;
            HandleTeam(enemyTeam);

            void HandleTeam(CombatTeam team)
            {
                var vanguardEffectsHolder = team.VanguardEffectsHolder;
                var effectsCollection 
                    = vanguardEffectsHolder.GetEffectsStructure();
                var recordsCollection 
                    = vanguardEffectsHolder.GetOffensiveRecordsStructure();


                if(vanguardEffectsHolder.HasDelayEffects())
                    DoInvokeVanguardEffects(effectsCollection.VanguardDelayImproveType,0);
                if (vanguardEffectsHolder.HasPunishEffects())
                {
                    DoInvokeVanguardEffects(
                        recordsCollection.VanguardPunishType,
                        effectsCollection.VanguardPunishType);
                }

                if (vanguardEffectsHolder.HasRevengeEffects())
                {
                    DoInvokeVanguardEffects(
                        recordsCollection.VanguardRevengeType,
                        effectsCollection.VanguardRevengeType);
                }

                vanguardEffectsHolder.Clear();
            }
        }

        private static void DoInvokeVanguardEffects(
            Dictionary<CombatEntity, int> offensiveRecords,
            Dictionary<IVanguardSkill, int> vanguardEffects)
        {
            DoInvokeAttackersRecord(offensiveRecords,out var allEntitiesInteractionCount);
            DoInvokeVanguardEffects(vanguardEffects, allEntitiesInteractionCount);
        }

        private static void DoInvokeAttackersRecord(Dictionary<CombatEntity, int> collection,
            out int allEntitiesInteractionCount)
        {
            allEntitiesInteractionCount = 0;
            foreach (KeyValuePair<CombatEntity, int> pair in collection)
            {
                // todo eventCall
                allEntitiesInteractionCount += pair.Value;
            }
        }

        private static void DoInvokeVanguardEffects(Dictionary<IVanguardSkill, int> collection, int addition)
        {
            foreach (var pair in collection)
            {
                int elementCountTotal = pair.Value + addition;
                //todo eventCall
            }
        }
    }
}
