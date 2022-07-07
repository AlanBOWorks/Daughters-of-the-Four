using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CombatSystem.Player.UI
{
    public class UCombatEntitySwitcherHandler : MonoBehaviour, 
        ICombatStartListener,
        ITempoControlStatesListener,
        ITempoEntityActionStatesListener,
        IPlayerCombatEventListener
    {
       

        [Title("Behaviour")]
        [SerializeField]
        private TeamReferences references = new TeamReferences();
        [SerializeField] 
        private UPerformerSelectionFeedback selectionFeedback;

        private Dictionary<CombatEntity, UCombatEntitySwitchButton> _buttonsDictionary;

        public IReadOnlyDictionary<CombatEntity, UCombatEntitySwitchButton> GetDictionary() => _buttonsDictionary;

        private void Start()
        {
            var shortcutCommands = CombatShortcutsSingleton.InputActions;

            InstantiateReferences();
            OnInstantiation();
            DoNextShortcutsInitialization();
            DoPreviousShortcutsInitialization();

            _buttonsDictionary = new Dictionary<CombatEntity, UCombatEntitySwitchButton>();

            var playerCombatEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerCombatEvents.ManualSubscribe(this as ICombatStartListener);
            playerCombatEvents.ManualSubscribe(this as IPlayerCombatEventListener);
            playerCombatEvents.DiscriminationEventsHolder.Subscribe(this);


            void DoNextShortcutsInitialization()
            {
                var shortCutInputAction = shortcutCommands.SwitchNextEntityShortCutElement;

                shortCutInputAction.action.performed += DoPerformNextSwitchShortcut;
            }
            void DoPreviousShortcutsInitialization()
            {
                var shortCutInputAction = shortcutCommands.SwitchPreviousEntityShortCutElement;

                shortCutInputAction.action.performed += DoPerformPreviousSwitchShortcut;
            }
        }

        private void InstantiateReferences()
        {
            references.InstantiateElements();
            references.activeCount = references.Members.Length;
            references.HidePrefab();
        }

        

        private void OnDestroy()
        {
            var shortcutCommands = CombatShortcutsSingleton.InputActions;

            var playerCombatEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerCombatEvents.UnSubscribe(this);
            playerCombatEvents.DiscriminationEventsHolder.UnSubscribe(this);

            var nextShortCutAction = shortcutCommands.SwitchNextEntityShortCutElement;
            nextShortCutAction.action.performed -= DoPerformNextSwitchShortcut;
            var previousShortCutAction = shortcutCommands.SwitchPreviousEntityShortCutElement;
            previousShortCutAction.action.performed -= DoPerformPreviousSwitchShortcut;
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


        public void ShowAll()
        {
            gameObject.SetActive(true);
        }

        public void HideAll()
        {
            gameObject.SetActive(false);
        }

        private void EnableButton(CombatEntity entity)
        {
            if(!_buttonsDictionary.ContainsKey(entity)) return;
            _buttonsDictionary[entity].DoEnable(true);
        }
        private void DisableButton(CombatEntity entity)
        {
            if (!_buttonsDictionary.ContainsKey(entity)) return;

            _buttonsDictionary[entity].DoEnable(false);
        }

        private CombatEntity _currentPerformer;
        public void DoSwitchEntity(CombatEntity entity)
        {
            if (entity == _currentPerformer || entity == null) return;

            var performerSwitcher = PlayerCombatSingleton.PerformerSwitcher;
            performerSwitcher.DoPerformEntityWithIndex(entity);
        }

        public void OnHoverEnter(Image button)
        {
            selectionFeedback.OnSwitchButtonHover(button);
        }
        public void OnHoverExit()
        {
            selectionFeedback.OnSwitchButtonExit();
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





        public void OnTempoStartControl(CombatTeamControllerBase controller, CombatEntity firstControl)
        {
            ShowAll();
            var actors = controller.ControllingTeam.GetControllingMembers();
            EnableButtons(actors);

            void EnableButtons(IEnumerable<CombatEntity> actingEntities)
            {
                foreach (var member in actingEntities)
                {
                    EnableButton(member);
                }
            }
        }

        public void OnAllActorsNoActions(CombatEntity lastActor)
        {
           DisableButton(lastActor);
        }

        public void OnTempoFinishControl(CombatTeamControllerBase controller)
        {
           _currentPerformer = null;
        }

        public void OnPerformerSwitch(CombatEntity performer)
        {
            _currentPerformer = performer;
        }

        public void OnTeamStancePreviewSwitch(EnumTeam.StanceFull targetStance)
        {
            
        }

        private void DoPerformNextSwitchShortcut(InputAction.CallbackContext context)
        {
            if(!enabled) return;
            if(PlayerCombatSingleton.IsInPauseMenu) return;
            PlayerCombatSingleton.PerformerSwitcher.DoPerformNextEntity();
        }

        private void DoPerformPreviousSwitchShortcut(InputAction.CallbackContext context)
        {
            if (!enabled) return;
            if (PlayerCombatSingleton.IsInPauseMenu) return;
            PlayerCombatSingleton.PerformerSwitcher.DoPerformPreviousEntity();
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
           DisableButton(entity);
       }


       [Serializable]
       private sealed class TeamReferences : TeamMainStructureInstantiateHandler<UCombatEntitySwitchButton>
       {

       }

    }
}
