using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UCombatSkillEntitySwitcher : MonoBehaviour, ICombatStatesListener,
        ITempoTeamStatesListener, ITempoDedicatedEntityStatesListener
    {
        [SerializeField]
        private TeamReferences references = new TeamReferences();

        [SerializeField] private UCombatSkillButtonsHolder skillsHolder;
        [SerializeField] private UCombatSkillButtonsHolder flexRoleSkills;

        public TeamBasicGroupStructure<UCombatSkillEntitySwitchButton> GetButtons() => references;

        private void Awake()
        {
            references.InstantiateElements();
            references.activeCount = references.Members.Length;
            references.HidePrefab();
            OnInstantiation();

            _buttonsDictionary = new Dictionary<CombatEntity, UCombatSkillEntitySwitchButton>();

            var playerCombatEvents = PlayerCombatSingleton.PlayerCombatEvents;
            playerCombatEvents.ManualSubscribe(this as ICombatStatesListener);
            playerCombatEvents.DiscriminationEventsHolder.Subscribe(this);
        }

        private const float IterationHeight = 70 + 6;
        private void OnInstantiation()
        {
            var enumerable = UtilsTeam.GetEnumerable(references);
            int i = references.activeCount;
            foreach (var button in enumerable)
            {
                OnInstantiateButton(in button);
                i--;
            }

            void OnInstantiateButton(in UCombatSkillEntitySwitchButton button)
            {
                var buttonTransform = button.transform;
                var position =
                    buttonTransform.localPosition;
                position.y = i * IterationHeight;
                buttonTransform.localPosition = position;
            }
        }


        private Dictionary<CombatEntity, UCombatSkillEntitySwitchButton> _buttonsDictionary;
        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            _buttonsDictionary.Clear();

            var enumerable = UtilsTeam.GetEnumerable(playerTeam.GetMainMembers(), references);
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

        public void OnCombatFinish(bool isPlayerWin)
        {
            _buttonsDictionary.Clear();
        }

        public void OnCombatQuit()
        {
            _buttonsDictionary.Clear();
        }

        public void ShowAll()
        {
            gameObject.SetActive(true);
        }

        public void HideAll()
        {
            gameObject.SetActive(false);
        }

        public void OnTempoStartControl(in CombatTeamControllerBase controller,in CombatEntity firstEntity)
        {
           ShowAll();
        }

       public void OnTempoFinishControl(in CombatTeamControllerBase controller)
       {
           HideAll();
       }

       public void OnTrinityEntityRequestSequence(CombatEntity entity, bool canAct)
       {
           _buttonsDictionary[entity].DoEnable(canAct);
       }

       public void OnOffEntityRequestSequence(CombatEntity entity, bool canAct)
       {
           
       }

       public void OnTrinityEntityFinishSequence(CombatEntity entity)
       {
           if (!_buttonsDictionary.ContainsKey(entity)) return;

           _buttonsDictionary[entity].DoEnable(false);
        }

       public void OnOffEntityFinishSequence(CombatEntity entity)
       {
       }


        public void DoSwitchEntity(in CombatEntity entity)
        {
            skillsHolder.SwitchControllingEntity(in entity);
            flexRoleSkills.ResetStateIfSelectedIsActive();
        }


       [Serializable]
       private sealed class TeamReferences : TeamBasicStructureInstantiateHandler<UCombatSkillEntitySwitchButton>
       {

       }

    }
}
