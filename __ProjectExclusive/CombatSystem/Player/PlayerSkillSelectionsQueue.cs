using System.Collections.Generic;
using System.IO;
using CombatEntity;
using CombatSkills;
using CombatSystem;
using CombatSystem.Events;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace __ProjectExclusive.Player
{
    public class PlayerSkillSelectionsQueue : ITempoListener<CombatingEntity>, IEntitySkillRequestHandler
    {
#if UNITY_EDITOR
        [ShowInInspector, TextArea] 
        private const string BehaviourDescription = "Keeps track of the player desired actions. Will check and then " +
                                                    "submit the deQueued skills to the ActionRequester when this is " +
                                                    "invoked by it.";
#endif

        public PlayerSkillSelectionsQueue()
        {
            _skillsQueue = new Queue<SkillUsageValues>();
        }

        private CombatingEntity _currentUser;
        [ShowInInspector]
        private readonly Queue<SkillUsageValues> _skillsQueue;

        public void SubmitSkill(CombatingSkill skill, CombatingEntity target)
        {
            var queueElement = new SkillUsageValues(skill,target);
            _skillsQueue.Enqueue(queueElement);
        }

        public void OnInitiativeTrigger(CombatingEntity element)
        {
            _currentUser = element;
            _skillsQueue.Clear();
        }

        public void OnDoMoreActions(CombatingEntity element)
        {
        }

        public void OnFinishAllActions(CombatingEntity element)
        {
            _skillsQueue.Clear();
        }

        public void OnSkipActions(CombatingEntity element)
        {
        }

        //TODO make the check if is correct the parameters (eg: the target is dead, skill has a problem or something)
        /// <summary>
        /// This check is different from [<seealso cref="SkillValuesHolders.IsValid"/>] (that checks for nulls).
        /// This checks for more specific conditions that the player might chose incorrectly (such pre-moves
        /// an action towards an enemy and this dies)
        /// </summary>
        private bool IsValid(SkillUsageValues skill)
        {
            return true;
        }

        private bool QueueHasElements() => _skillsQueue.Count > 0;

        private bool _submitRequest;
        public IEnumerator<float> OnRequestAction(SkillValuesHolders skillValues)
        {
            _submitRequest = false;
            while (!_submitRequest)
            {
                yield return Timing.WaitUntilTrue(QueueHasElements);
                var element = _skillsQueue.Dequeue();
                if (!IsValid(element))
                {
                    throw new InvalidDataException("The selected element by the player is invalid");
                }

                var skillInjection = new SkillUsageValues(element.UsedSkill,element.Target);
                skillValues.Inject(skillInjection);
                _submitRequest = true;
            }

        }

    }
}
