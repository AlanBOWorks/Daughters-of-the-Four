using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    [RequireComponent(typeof(UUIHoverEntitiesHandler))]
    public sealed class UHoverSkillTargetingHandler : MonoBehaviour, 
        ITargetPointerListener,
        IPlayerCombatEventListener, ISkillSelectionListener
    {
        private IReadOnlyDictionary<CombatEntity, UUIHoverEntityHolder> _dictionary;
        private HashSet<CombatEntity> _activeMembers;

        private void Awake()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.SubscribeAsPlayerEvent(this);

            _dictionary = GetComponent<UUIHoverEntitiesHandler>().GetDictionary();
            _activeMembers = new HashSet<CombatEntity>();
        }

        private void OnDestroy()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.UnSubscribe(this);
        }

        public void OnHoverTargetInteraction(CombatEntity target)
        {
            ToggleHoverElement(target, true);
        }

        public void OnHoverTargetExit()
        {
            const bool active = false;
            foreach (var pair in _dictionary)
            {
                var entity = pair.Key;
                ToggleHoverElement(entity, active);
            }
        }

        private ISkill _currentSkill;
        private CombatEntity _currentPerformer;

        private void ToggleHoverElement(CombatEntity entity, bool active)
        {
            if (!_dictionary.ContainsKey(entity)) return;
            _dictionary[entity].GetHoverFeedbackHolder().SetActive(active);
        }

        public void OnTargetButtonHover(CombatEntity target)
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;

            _activeMembers.Clear();
            var effects = _currentSkill.GetEffects();
            foreach (PerformEffectValues effect in effects)
            {
                var effectTargets = UtilsTarget.GetEffectTargets(
                    effect.TargetType, _currentPerformer, target);
                VoidHandleEffectsTarget(effectTargets,in effect);
            }

            void VoidHandleEffectsTarget(IEnumerable<CombatEntity> effectTargets, in PerformEffectValues effectValue)
            {
                foreach (var effectTarget in effectTargets)
                {
                        InvokeTarget(effectTarget, _currentSkill);
                }
            }
            void InvokeTarget(CombatEntity possibleTarget, ISkill skill)
            {
                if (!_activeMembers.Add(possibleTarget)) return;

                OnHoverTargetInteraction(possibleTarget);
                playerEvents.OnHoverTargetInteraction(possibleTarget, skill);
            }
        }

        public void OnTargetButtonExit(CombatEntity target)
        {
            OnHoverTargetExit();
            PlayerCombatSingleton.PlayerCombatEvents.OnHoverTargetExit();
            _activeMembers.Clear();
        }

        public void OnPerformerSwitch(CombatEntity performer)
        {
            _currentPerformer = performer;
            _activeMembers.Clear();
        }

        public void OnTeamStancePreviewSwitch(EnumTeam.StanceFull targetStance)
        {
            
        }

        public void OnSkillSelect(IFullSkill skill)
        {
        }

        public void OnSkillSelectFromNull(IFullSkill skill)
        {
        }

        public void OnSkillSwitch(IFullSkill skill, IFullSkill previousSelection)
        {
            _currentSkill = skill;
            _activeMembers.Clear();
        }

        public void OnSkillDeselect(IFullSkill skill)
        {
        }

        public void OnSkillCancel(CombatSkill skill)
        {
        }

        public void OnSkillSubmit(IFullSkill skill)
        {
        }
    }

    
    /// <summary>
    /// Dedicated event for hovering Effects
    /// </summary>
    public interface IHoverInteractionEffectTargetsListener : ICombatEventListener
    {
        void OnHoverTargetInteraction(CombatEntity target, ISkill skill);
        void OnHoverTargetExit();
    }
}
