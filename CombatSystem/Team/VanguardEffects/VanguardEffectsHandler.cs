using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;
using MEC;

namespace CombatSystem.Team.VanguardEffects
{
    public sealed class VanguardEffectsHandler : ISkillUsageListener, ICombatTerminationListener
    {
        public VanguardEffectsHandler()
        {
            _queue = new Queue<VanguardEffectsHolder>();
        }

        private const float WaitForNextEffect = .2f;
        private const float WaitBetweenTeams = .6f;
        private readonly Queue<VanguardEffectsHolder> _queue;

        public bool IsActive() => _queueCoroutineHandle.IsRunning;


        public void EnqueueVanguardEffects(CombatTeam team)
        {
            var vanguardEffectsHolder = team.VanguardEffectsHolder;
            if (!vanguardEffectsHolder.CanPerform()) return;

            _queue.Enqueue(team.VanguardEffectsHolder);
            if(IsActive()) return;
            _queueCoroutineHandle = Timing.RunCoroutine(_DoDeQueue());
        }

        private CoroutineHandle _queueCoroutineHandle;
        private IEnumerator<float> _DoDeQueue()
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            var animator = CombatSystemSingleton.CombatControllerAnimationHandler;

            while (_queue.Count > 0)
            {
                yield return Timing.WaitForSeconds(WaitBetweenTeams);
                var vanguardEffectsHolder = _queue.Dequeue();
                var performer = vanguardEffectsHolder.GetMainEntity();


                var vanguardEffects
                    = vanguardEffectsHolder.GetEffectsStructure();
                var vanguardOffensiveRecords
                    = vanguardEffectsHolder.GetOffensiveRecordsStructure();
                

                // REVENGE
                if (vanguardEffectsHolder.HasRevengeEffects())
                {
                    //Do animation Once
                    animator.PerformActionAnimation(StaticSkillTypes.RevengeVanguardSkill, performer, performer);
                    int revengeTotalOffensiveCount = CalculateIterationCount(vanguardOffensiveRecords.VanguardRevengeType);
                    foreach ((IVanguardSkill vanguardSkill, var count) in vanguardEffects.VanguardRevengeType)
                    {
                        InvokeVanguardEffect(vanguardSkill, count + revengeTotalOffensiveCount);
                        yield return Timing.WaitForSeconds(WaitForNextEffect);
                    }
                }

                // PUNISH
                if (vanguardEffectsHolder.HasPunishEffects())
                {
                    //Do animation Once
                    animator.PerformActionAnimation(StaticSkillTypes.PunishVanguardSkill, performer, performer);
                    int punishTotalOffensiveCount = CalculateIterationCount(vanguardOffensiveRecords.VanguardPunishType);
                    foreach ((IVanguardSkill vanguardSkill, var count) in vanguardEffects.VanguardPunishType)
                    {
                        InvokeVanguardEffect(vanguardSkill, count + punishTotalOffensiveCount);
                        yield return Timing.WaitForSeconds(WaitForNextEffect);
                    }
                }
                

                yield return Timing.WaitForOneFrame;
                vanguardEffectsHolder.Clear();
                yield return Timing.WaitForOneFrame;

                void InvokeVanguardEffect(IVanguardSkill skill, int totalIteration)
                {
                    eventsHolder.OnVanguardEffectPerform(
                        new VanguardSkillUsageValues(
                            vanguardEffectsHolder, 
                            skill,
                        totalIteration));

                    UtilsCombatSkill.DoSkillOnTarget(skill,performer,performer);
                }
            }
        }


        public static void HandleVanguardOffensive(CombatEntity attacker, CombatEntity onTarget)
        {
            var targetTeam = onTarget.Team;
            if (targetTeam.Contains(attacker)) return;

            // Events are handled inside this
            targetTeam.VanguardEffectsHolder.OnOffensiveDone(attacker, onTarget);
        }



        private static int CalculateIterationCount(Dictionary<CombatEntity, int> collection)
        {
            int allEntitiesInteractionCount = 0;
            foreach (var pair in collection)
            {
                allEntitiesInteractionCount += pair.Value;
            }

            return allEntitiesInteractionCount;
        }


        public void OnCombatSkillSubmit(in SkillUsageValues values)
        {
        }

        public void OnCombatSkillPerform(in SkillUsageValues values)
        {
            var skill = values.UsedSkill;
            if (!UtilsSkill.IsOffensiveSkill(skill)) return;

            var attacker = values.Performer;
            var target = values.Target;
            HandleVanguardOffensive(attacker, target);
        }
        public void OnCombatSkillFinish(CombatEntity performer)
        {
        }


        public void OnCombatEnd()
        {
            _queue.Clear();
            Timing.KillCoroutines(_queueCoroutineHandle);
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
        }

        public void OnCombatQuit()
        {
        }
    }
}
