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


        private void HandleEntity(CombatEntity entity)
        {
            if(entity == null) return;

            var dictionary = GetDictionary();
            if (!dictionary.ContainsKey(entity)) return;

            var element = dictionary[entity];
            element.UpdateToCurrent();
        }
        

        protected override void OnCreateElement(in CreationValues creationValues)
        {
            var element = creationValues.Element;
            var entity = creationValues.Entity;
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



        public void OnAfterEntityRequestSequence(CombatEntity entity)
        {
            HandleEntity(entity);
        }

        public void OnAfterEntitySequenceFinish(CombatEntity entity)
        {
            HandleEntity(entity);
        }

        public void OnNoActionsForcedFinish(CombatEntity entity)
        {
            HandleEntity(entity);
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {
            HandleEntity(entity);
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
