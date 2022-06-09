using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UTempoMainCountHandler : MonoBehaviour, ICombatStatesListener, ITempoEntityPercentListener
    {
        [SerializeField,HorizontalGroup("Main Roles")] 
        private MainRolesHolder playerMainRoles = new MainRolesHolder();
        [SerializeField,HorizontalGroup("Main Roles")]
        private MainRolesHolder enemyMainRoles = new MainRolesHolder();

        [SerializeField, HorizontalGroup("Off Roles")]
        private OffRolesHolder playerOffRoles = new OffRolesHolder();
        [SerializeField, HorizontalGroup("Off Roles")]
        private OffRolesHolder enemyOffRoles = new OffRolesHolder();

        [Title("Params")] 
        [SerializeField] private bool hidePrefabs = true;
        [SerializeField] private float verticalSpacing = 32;

        [Title("Trackers")]
        [ShowInInspector,DisableInEditorMode]
        private Dictionary<CombatEntity, UTempoTrackerHolder> _dictionary;

        


        private void Awake()
        {
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
            _dictionary = new Dictionary<CombatEntity, UTempoTrackerHolder>();

            playerMainRoles.GetPrefab().gameObject.SetActive(false); 
            enemyMainRoles.GetPrefab().gameObject.SetActive(false); 

            playerOffRoles.GetPrefab()?.gameObject.SetActive(false); 
            enemyOffRoles.GetPrefab()?.gameObject.SetActive(false); 

            playerMainRoles.Injection(this);
            enemyMainRoles.Injection(this);

            playerOffRoles.Injection(this);
            enemyOffRoles.Injection(this);
        }



        private static void HideActiveTracker(UTempoTrackerHolder holder)
        {
            holder.DisableElement();
        }
        public void OnCombatEnd()
        {
            Action<UTempoTrackerHolder> hideTracker = HideActiveTracker; 
            playerMainRoles.OnCombatFinish(hideTracker);
            enemyMainRoles.OnCombatFinish(hideTracker);

            playerOffRoles.OnCombatFinish(hideTracker);
            enemyOffRoles.OnCombatFinish(hideTracker);
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
        }

        public void OnCombatQuit()
        {
        }

        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            playerMainRoles.OnCombatPrepares(playerTeam, null);
            enemyMainRoles.OnCombatPrepares(enemyTeam, null);

            playerOffRoles.OnCombatPrepares(playerTeam, null);
            enemyOffRoles.OnCombatPrepares(enemyTeam, null);
        }

        public void OnCombatStart()
        {
        }


        private void HandleEntity(CombatEntity entity, UTempoTrackerHolder element)
        {
            _dictionary.Add(entity,element);

            var entityRole = entity.RoleType;
            var targetHeight = TargetHeight(entityRole);
            HandleHeight(element, targetHeight);
            element.EntityInjection(in entity);
            element.OnInstantiation();
        }

        private static void HandleHeight(Component element, float targetHeight)
        {
            var elementTransform = element.transform;
            var position = elementTransform.localPosition;
            position.y = targetHeight;
            elementTransform.localPosition = position;
        }

        private float TargetHeight(EnumTeam.Role role)
        {
            return role switch
            {
                EnumTeam.Role.Vanguard => (EnumTeam.VanguardInvertedIndex * verticalSpacing),
                EnumTeam.Role.Attacker => (EnumTeam.AttackerInvertedIndex * verticalSpacing),
                EnumTeam.Role.Support => (EnumTeam.SupportInvertedIndex * verticalSpacing),
                EnumTeam.Role.Flex => (EnumTeam.FlexInvertedIndex * verticalSpacing),
                _ => 0
            };
        }


        public void OnEntityTick(in CombatEntity entity, in float currentTick, in float percentInitiative)
        {
            if(!_dictionary.ContainsKey(entity)) return;

            _dictionary[entity].TickTempo(in currentTick, in percentInitiative);
        }


        [Serializable]
        private sealed class MainRolesHolder : TeamMainMembersSpawner<UTempoTrackerHolder>
        {
            private UTempoMainCountHandler _handler;
            public void Injection(UTempoMainCountHandler handler) => _handler = handler;

            protected override void OnInstantiationElement(UTempoTrackerHolder element, CombatEntity entity, int count)
            {
                _handler.HandleEntity(entity,element);
            }
        }
        [Serializable]
        private sealed class OffRolesHolder : TeamOffMembersSpawner<UTempoTrackerHolder>
        {

            private UTempoMainCountHandler _handler;
            public void Injection(UTempoMainCountHandler handler) => _handler = handler;
            protected override void OnInstantiationElement(UTempoTrackerHolder element, CombatEntity entity, int count)
            {
                _handler.HandleEntity(entity,element);
            }
        }

    }
}
