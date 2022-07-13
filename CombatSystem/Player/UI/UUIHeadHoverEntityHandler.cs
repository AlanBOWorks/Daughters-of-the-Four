using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills.Effects;
using CombatSystem.Stats;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UUIHeadHoverEntityHandler : UTeamElementSpawner<UUIHeadHoverEntityHolder>,
        IStatsChangeListener,
        ITempoEntityStatesExtraListener,
        ITempoEntityActionStatesListener
    {
        private void Start()
        {
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
        }
        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }


        private void HandleAction(CombatEntity entity)
        {
            var dictionary = GetDictionary();
            if (!dictionary.ContainsKey(entity)) return;

            var stats = entity.Stats;
            var element = dictionary[entity];
            element.UpdateActions(stats);
        }

        protected override void OnCreateElement(CombatEntity entity, UUIHeadHoverEntityHolder element, bool isPlayerElement)
        {
            element.Injection(entity);
            element.UpdateLuck(entity);
            element.Show();
        }

        public void OnBuffDone(EntityPairInteraction entities, IBuffEffect buff, float effectValue)
        {
            HandleAction(entities.Target);
        }

        public void OnDeBuffDone(EntityPairInteraction entities, IDeBuffEffect deBuff, float effectValue)
        {
            HandleAction(entities.Target);
        }


        private void HandleLuck(CombatEntity entity)
        {
            var dictionary = GetDictionary();
            if (!dictionary.ContainsKey(entity)) return;

            dictionary[entity].UpdateLuck(entity);
        }

        public void OnAfterEntityRequestSequence(CombatEntity entity)
        {
            HandleLuck(entity);
        }

        public void OnAfterEntitySequenceFinish(CombatEntity entity)
        {
            HandleLuck(entity);
        }

        public void OnNoActionsForcedFinish(CombatEntity entity)
        {
            HandleLuck(entity);
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {
            HandleLuck(entity);
        }

        public void OnEntityBeforeSkill(CombatEntity entity)
        {
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
        }

        public void OnEntityEmptyActions(CombatEntity entity)
        {
        }
    }
}
