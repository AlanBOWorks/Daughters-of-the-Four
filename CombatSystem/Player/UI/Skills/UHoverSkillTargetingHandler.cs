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
        private IReadOnlyDictionary<CombatEntity, UUIHoverEntity> _dictionary;

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

            public void OnPerformerSwitch(CombatEntity performer)
            {
                _performer = performer;
            }

            public void OnTargetButtonHover(CombatEntity target)
            {
                HandleSkill(_selectedSkill, _performer, target);
            }

            public void OnTargetButtonExit(CombatEntity target)
            {
            }



            public void OnSkillSelect(CombatSkill skill)
            {
            }

            public void OnSkillSelectFromNull(CombatSkill skill)
            {
            }

            public void OnSkillSwitch(CombatSkill skill, CombatSkill previousSelection)
            {
                _selectedSkill = skill;
            }

            public void OnSkillDeselect(CombatSkill skill)
            {
            }

            public void OnSkillCancel(CombatSkill skill)
            {
            }

            public void OnSkillSubmit(CombatSkill skill)
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
