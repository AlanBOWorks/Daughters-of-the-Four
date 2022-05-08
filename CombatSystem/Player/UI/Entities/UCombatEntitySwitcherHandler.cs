using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UCombatEntitySwitcherHandler : MonoBehaviour, ICombatStatesListener,
        ITempoTeamStatesListener, ITempoEntityStatesListener,
        IPlayerEntityListener
    {
        [SerializeField]
        private TeamReferences references = new TeamReferences();

        [SerializeField] private UCombatSkillButtonsHolder skillsHolder;
        private Dictionary<CombatEntity, UCombatEntitySwitchButton> _buttonsDictionary;

        public FlexPositionMainGroupStructure<UCombatEntitySwitchButton> GetButtons() => references;
        public IReadOnlyDictionary<CombatEntity, UCombatEntitySwitchButton> GetDictionary() => _buttonsDictionary;

        private void Start()
        {
            references.InstantiateElements();
            references.activeCount = references.Members.Length;
            references.HidePrefab();
            OnInstantiation();

            _buttonsDictionary = new Dictionary<CombatEntity, UCombatEntitySwitchButton>();

            var playerCombatEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerCombatEvents.ManualSubscribe(this as ICombatStatesListener);
            playerCombatEvents.ManualSubscribe(this as IPlayerEntityListener);
            playerCombatEvents.DiscriminationEventsHolder.Subscribe(this);
        }

        private void OnDestroy()
        {
            var playerCombatEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerCombatEvents.UnSubscribe(this);
            playerCombatEvents.DiscriminationEventsHolder.UnSubscribe(this);

        }

        private const float IterationHeight = 70 + 6;
        private void OnInstantiation()
        {
            var feedBacks = PlayerCombatUserInterfaceSingleton.CombatTeemFeedBacks.PlayerTeamType;
            var enumerable = UtilsTeam.GetEnumerable(references, feedBacks);
            int i = references.activeCount;
            foreach (var pair in enumerable)
            {
                var button = pair.Key;
                var icon = pair.Value.GetIcon();
                OnInstantiateButton(in button);
                button.Injection(in icon);
                i--;
            }

            void OnInstantiateButton(in UCombatEntitySwitchButton button)
            {
                var buttonTransform = button.transform;
                var position =
                    buttonTransform.localPosition;
                position.y = i * IterationHeight;
                buttonTransform.localPosition = position;
            }
        }


        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            _buttonsDictionary.Clear();

            ITeamFlexPositionStructureRead<CombatEntity> members = playerTeam.GetMainMembers();
            var enumerable = UtilsTeam.GetEnumerable(members, references);
            foreach (var pair in enumerable)
            {
                var entity = pair.Key;
                var button = pair.Value;
                if(entity == null)
                {
                    button.OnNullEntity();
                    continue;
                }
                button.DoEnable(false);
                button.Injection(in entity);

                _buttonsDictionary.Add(entity,button);
            }
        }

        public void OnCombatStart()
        {
        }

        public void OnCombatEnd()
        {
            _buttonsDictionary.Clear();
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
        }

        public void OnCombatQuit()
        {
        }

        public void ShowAll()
        {
            gameObject.SetActive(true);
        }

        public void HideAll()
        {
            gameObject.SetActive(false);
        }

        public void OnTempoStartControl(in CombatTeamControllerBase controller)
        {
           ShowAll();
        }

        public void OnControlFinishAllActors(in CombatEntity lastActor)
        {
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
       {
            _currentPerformer = null;
           HideAll();
       }

        public void OnTempoFinishLastCall(in CombatTeamControllerBase controller)
        {
        }


        public void OnEntityRequestSequence(CombatEntity entity, bool canControl)
       {
           if (!_buttonsDictionary.ContainsKey(entity)) return;

           _buttonsDictionary[entity].DoEnable(canControl);
       }

       public void OnEntityRequestAction(CombatEntity entity)
       {
       }

       public void OnEntityFinishAction(CombatEntity entity)
       {
       }

       public void OnEntityFinishSequence(CombatEntity entity, in bool isForcedByController)
       {
           if (!_buttonsDictionary.ContainsKey(entity)) return;

           _buttonsDictionary[entity].DoEnable(false);
       }

       private CombatEntity _currentPerformer;
        public void DoSwitchEntity(in CombatEntity entity)
        {
            if(entity == _currentPerformer) return;

            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.OnPerformerSwitch(in entity);
        }
        public void OnPerformerSwitch(in CombatEntity performer)
        {
            _currentPerformer = performer;
        }

        [Serializable]
       private sealed class TeamReferences : TeamMainStructureInstantiateHandler<UCombatEntitySwitchButton>
       {

       }

    }
}
