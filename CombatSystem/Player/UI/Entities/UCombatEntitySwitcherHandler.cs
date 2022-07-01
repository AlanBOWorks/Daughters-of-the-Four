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
        ICombatStatesListener,
        ITempoControlStatesListener,
        ITempoEntityActionStatesListener,
        IPlayerEntityListener,
        ISwitchEntityShortcutCommandStructureRead<TextMeshProUGUI>
    {
        [Title("ShortCuts")]
        [SerializeField] 
        private UShortcutCommandsHolder shortcutCommands;
        [SerializeField] 
        private TextMeshProUGUI switchPerformerTextHolder;

        [Title("Behaviour")]
        [SerializeField]
        private TeamReferences references = new TeamReferences();
        [SerializeField] 
        private UPerformerSelectionFeedback selectionFeedback;

        private Dictionary<CombatEntity, UCombatEntitySwitchButton> _buttonsDictionary;

        public IReadOnlyDictionary<CombatEntity, UCombatEntitySwitchButton> GetDictionary() => _buttonsDictionary;

        private void Start()
        {
           InstantiateReferences();
            OnInstantiation();
            DoShortcutsInitialization();

            _buttonsDictionary = new Dictionary<CombatEntity, UCombatEntitySwitchButton>();

            var playerCombatEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerCombatEvents.ManualSubscribe(this as ICombatStatesListener);
            playerCombatEvents.ManualSubscribe(this as IPlayerEntityListener);
            playerCombatEvents.DiscriminationEventsHolder.Subscribe(this);
        }

        private void InstantiateReferences()
        {
            references.InstantiateElements();
            references.activeCount = references.Members.Length;
            references.HidePrefab();
        }

        private void DoShortcutsInitialization()
        {
            var shortcutName = UtilsShortCuts.DefaultNamesHolder.SwitchEntityShortCutElement;
            var shortCutInputAction = shortcutCommands.SwitchEntityShortCutElement;

            switchPerformerTextHolder.text = shortcutName;
            shortCutInputAction.action.performed += DoPerformSwitchShortcut;
        }


        private void OnDestroy()
        {
            var playerCombatEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerCombatEvents.UnSubscribe(this);
            playerCombatEvents.DiscriminationEventsHolder.UnSubscribe(this);

            var shortCutInputAction = shortcutCommands.SwitchEntityShortCutElement;
            shortCutInputAction.action.performed -= DoPerformSwitchShortcut;
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

        public TextMeshProUGUI SwitchEntityShortCutElement => switchPerformerTextHolder;

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

        private void DoPerformSwitchShortcut(InputAction.CallbackContext context)
        {
            if(!enabled) return;
            PlayerCombatSingleton.PerformerSwitcher.DoPerformNextEntity();
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
