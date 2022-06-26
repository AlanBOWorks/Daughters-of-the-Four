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
            var feedBacks = CombatThemeSingleton.RolesThemeHolder;
            var enumerable = UtilsTeam.GetEnumerable(references, feedBacks);
            int i = references.activeCount;
            foreach ((UCombatEntitySwitchButton button, CombatThemeHolder value) in enumerable)
            {
                var icon = value.GetThemeIcon();
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

            ITeamFlexPositionStructureRead<CombatEntity> members = playerTeam.GetMainPositions();
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

        public void OnTempoStartControl(CombatTeamControllerBase controller)
        {
            ShowAll();
        }

        public void OnAllActorsNoActions(CombatEntity lastActor)
        {
        }

        public void OnTempoFinishControl(CombatTeamControllerBase controller)
       {
           _currentPerformer = null;
        }



        public void OnEntityRequestSequence(CombatEntity entity, bool canControl)
       {
           if (!_buttonsDictionary.ContainsKey(entity)) return;

           _buttonsDictionary[entity].DoEnable(canControl);
       }

       public void OnEntityRequestAction(CombatEntity entity)
       {
       }

       public void OnEntityBeforeSkill(CombatEntity entity)
       {
           
       }

       public void OnEntityFinishAction(CombatEntity entity)
       {
       }

       public void OnEntityEmptyActions(CombatEntity entity)
       {
           DisableButton(in entity);
       }

        public void OnEntityFinishSequence(CombatEntity entity, bool isForcedByController)
       {
       }
       private void DisableButton(in CombatEntity entity)
       {
           if (!_buttonsDictionary.ContainsKey(entity)) return;

           _buttonsDictionary[entity].DoEnable(false);
       }

        private CombatEntity _currentPerformer;
        public void DoSwitchEntity(CombatEntity entity)
        {
            if(entity == _currentPerformer || entity == null) return;

            var playerEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerEvents.OnPerformerSwitch(entity);
        }
        public void OnPerformerSwitch(CombatEntity performer)
        {
            _currentPerformer = performer;
        }

        [Serializable]
       private sealed class TeamReferences : TeamMainStructureInstantiateHandler<UCombatEntitySwitchButton>
       {

       }

    }
}
