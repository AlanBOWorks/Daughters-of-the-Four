using System.Collections.Generic;
using _CombatSystem;
using _Team;
using Characters;
using MEC;
using Sirenix.OdinInspector;
using Stats;

namespace Skills
{
    /// <summary>
    /// Perform the skill with all event calls in a sequence that syncs with Animations and such
    /// </summary>
    public class PerformSkillHandler : ICombatAfterPreparationListener, ITempoListener, ISkippedTempoListener
    {
        [ShowInInspector]
        private CombatingEntity _currentUser;
        [ShowInInspector]
        private readonly SkillTargets _currentSkillTargets;


        public PerformSkillHandler()
        {
            int sizeAllocation = UtilsCharacter.PredictedAmountOfCharactersInBattle;
            _currentSkillTargets = new SkillTargets(sizeAllocation); // it could be a whole targets
        }

        public void OnAfterPreparation(
            CombatingTeam playerEntities, 
            CombatingTeam enemyEntities, 
            CharacterArchetypesList<CombatingEntity> allEntities)
        {
            
        }


        public void OnInitiativeTrigger(CombatingEntity entity)
        {
            _currentUser = entity;
            _currentSkillTargets.UsingSkill = null;
            _currentSkillTargets.Clear();
        }

        public void OnDoMoreActions(CombatingEntity entity)
        {
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
            _currentUser = null;
            _currentSkillTargets.UsingSkill = null;
            _currentSkillTargets.Clear();
        }

        public void OnSkippedEntity(CombatingEntity entity)
        {
            OnFinisAllActions(entity);
        }

        private CoroutineHandle _doSkillHandle;
        public void DoSkill(CombatingEntity target)
        {
            _doSkillHandle =
                Timing.RunCoroutineSingleton(_DoSkill(target), _doSkillHandle,SingletonBehavior.Wait);
        }
        public static void SendDoSkill(CombatingEntity target)
        {
            CombatSystemSingleton.PerformSkillHandler.DoSkill(target);
        }

        private IEnumerator<float> _DoSkill(CombatingEntity target)
        {
            var skill = _currentSkillTargets.UsingSkill;
            var skillPreset = skill.Preset;
            var effectTargets = skillPreset.GetMainEffectTargets(_currentUser, target);

            // TODO make a waitUntil(Animation call for Skill)
            yield return Timing.WaitUntilDone(_currentUser.CombatAnimator.DoAnimation(
                        _currentUser, effectTargets,
                        _currentSkillTargets.UsingSkill));
            CombatSystemSingleton.StatsInteractionHandler.DoSkill(skill, _currentUser, target);

            //>>>>>>>>>>>>>>>>>>> Finish Do SKILL
            skill.OnSkillUsage();
            CombatSystemSingleton.TempoHandler.DoSkillCheckFinish(_currentUser);
            CombatSystemSingleton.CharacterEventsTracker.Invoke();
        }

        public List<CombatingEntity> HandlePossibleTargets(CombatSkill skill)
        {
            UtilsTargets.InjectPossibleTargets(skill, _currentUser, _currentSkillTargets);
            return _currentSkillTargets;
        }

        public static List<CombatingEntity> SendHandlePossibleTargets(CombatSkill skill)
        {
            return CombatSystemSingleton.PerformSkillHandler.HandlePossibleTargets(skill);
        }


    }

    public class SkillTargets : List<CombatingEntity>
    {
        public CombatSkill UsingSkill;
        public SkillTargets(int memoryAlloc) : base(memoryAlloc)
        {}
    }

}
