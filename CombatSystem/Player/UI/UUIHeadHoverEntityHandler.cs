using CombatSystem.Entity;
using CombatSystem.Skills.Effects;
using CombatSystem.Stats;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UUIHeadHoverEntityHandler : UTeamElementSpawner<UUIHeadHoverEntityHolder>,
        IStatsChangeListener
    {
        private void Start()
        {
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
        }
        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }


        protected override void OnCreateElement(CombatEntity entity, UUIHeadHoverEntityHolder element, bool isPlayerElement)
        {
            element.Injection(entity);
            element.Show();
        }

        public void OnBuffDone(EntityPairInteraction entities, IBuffEffect buff, float effectValue)
        {
            HandleEntity(entities.Target);
        }

        public void OnDeBuffDone(EntityPairInteraction entities, IDeBuffEffect deBuff, float effectValue)
        {
            HandleEntity(entities.Target);
        }

        private void HandleEntity(CombatEntity entity)
        {
            var dictionary = GetDictionary();
            if(!dictionary.ContainsKey(entity)) return;

            var stats = entity.Stats;
            dictionary[entity].UpdateActions(stats);
        }
    }
}
