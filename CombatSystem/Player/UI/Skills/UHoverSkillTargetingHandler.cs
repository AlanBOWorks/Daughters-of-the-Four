using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    [RequireComponent(typeof(UUIHoverEntitiesHandler))]
    public sealed class UHoverSkillTargetingHandler : MonoBehaviour, 
        IHoverInteractionTargetsListener
    {
        private HoverSkillTargetingHandler _hoverTargetingHelper;
        private IReadOnlyDictionary<CombatEntity, UUIHoverEntityHolder> _dictionary;

        private void Awake()
        {
            _hoverTargetingHelper = new HoverSkillTargetingHandler();
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.SubscribeAsPlayerEvent(_hoverTargetingHelper);
            playerEvents.SubscribeAsPlayerEvent(this);

            _dictionary = GetComponent<UUIHoverEntitiesHandler>().GetDictionary();
        }

        private void OnDestroy()
        {
            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.UnSubscribe(_hoverTargetingHelper);
        }

        public void OnHoverTargetInteraction(in CombatEntity target)
        {
            ToggleHoverElement(in target, true);
        }

        public void OnHoverTargetExit()
        {
            bool active = false;
            foreach (var pair in _dictionary)
            {
                var entity = pair.Key;
                ToggleHoverElement(in entity, in active);
            }
        }

        private void ToggleHoverElement(in CombatEntity entity, in bool active)
        {
            if (!_dictionary.ContainsKey(entity)) return;
            _dictionary[entity].GetHoverFeedbackHolder().SetActive(active);
        }



        private sealed class HoverSkillTargetingHandler : SkillTargetingHandler,
            IPlayerEntityListener,
            ITargetPointerListener, ISkillSelectionListener
        {
            private CombatEntity _performer;
            private CombatSkill _selectedSkill;

            public void OnPerformerSwitch(in CombatEntity performer)
            {
                _performer = performer;
            }

            public void OnTargetButtonHover(in CombatEntity target)
            {
                HandleSkill(in _performer, in _selectedSkill, in target);
            }

            public void OnTargetButtonExit(in CombatEntity target)
            {
            }



            public void OnSkillSelect(in CombatSkill skill)
            {
            }

            public void OnSkillSwitch(in CombatSkill skill, in CombatSkill previousSelection)
            {
                _selectedSkill = skill;
            }

            public void OnSkillDeselect(in CombatSkill skill)
            {
            }

            public void OnSkillCancel(in CombatSkill skill)
            {
            }

            public void OnSkillSubmit(in CombatSkill skill)
            {
            }

            protected override void AddInteractionEntity(in CombatEntity entity)
            {
                if (InteractionsEntities.Contains(entity)) return;
                InteractionsEntities.Add(entity);

                PlayerCombatSingleton.PlayerCombatEvents.OnHoverTargetInteraction(in entity);
            }
        }
    }

    

    public interface IHoverInteractionTargetsListener : ICombatEventListener
    {
        void OnHoverTargetInteraction(in CombatEntity target);
        void OnHoverTargetExit();
    }
}
