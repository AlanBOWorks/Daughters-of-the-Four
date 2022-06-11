using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UTempoMainTickHandler : MonoBehaviour, ICombatStatesListener, ITempoEntityPercentListener
    {
        [SerializeField, HorizontalGroup("Roles")]
        private RolesHolder playerHolder = new RolesHolder();
        [SerializeField, HorizontalGroup("Roles")]
        private RolesHolder enemyHolder = new RolesHolder();

        [Title("Params")] 
        [SerializeField] private float verticalSpacing = 32;

        [Title("Trackers")]
        [ShowInInspector,DisableInEditorMode]
        private Dictionary<CombatEntity, UTempoTrackerHolder> _dictionary;

        public IReadOnlyDictionary<CombatEntity, UTempoTrackerHolder> GetDictionary() => _dictionary;


        private void Awake()
        {
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
            _dictionary = new Dictionary<CombatEntity, UTempoTrackerHolder>();

            playerHolder.DoInjection(this);
            enemyHolder.DoInjection(this);
        }



        private static void HideActiveTracker(UTempoTrackerHolder holder)
        {
            holder.DisableElement();
        }
        public void OnCombatEnd()
        {
            Action<UTempoTrackerHolder> hideTracker = HideActiveTracker;

            playerHolder.OnFinish(hideTracker);
            enemyHolder.OnFinish(hideTracker);

            _dictionary.Clear();
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
        }

        public void OnCombatQuit()
        {
        }

        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            playerHolder.DoInjection(playerTeam);
            enemyHolder.DoInjection(enemyTeam);
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
            return EnumTeam.GetRoleInvertIndex(role) * verticalSpacing;
        }


        public void OnEntityTick(in CombatEntity entity, in float currentTick, in float percentInitiative)
        {
            if(!_dictionary.ContainsKey(entity)) return;

            _dictionary[entity].TickTempo(in currentTick, in percentInitiative);
        }


        [Serializable]
        private sealed class RolesHolder : ITeamAlimentStructureRead<TeamMembersTypeSpawner<UTempoTrackerHolder>>
        {
            [SerializeField]
            private RolesSpawner mainRoles = new RolesSpawner();
            [SerializeField]
            private RolesSpawner secondaryRoles = new RolesSpawner();
            [SerializeField]
            private RolesSpawner thirdRoles = new RolesSpawner();

            public void DoInjection(UTempoMainTickHandler handler)
            {
                foreach (var spawner in GetEnumerable())
                {
                    if(!spawner.IsValid()) return;

                    spawner.Injection(handler);
                    spawner.GetPrefab().gameObject.SetActive(false);
                }
            }

            public void DoInjection(CombatTeam team)
            {
                mainRoles.OnCombatPrepares(team.GetMainRoles(),null);
                secondaryRoles.OnCombatPrepares(team.GetSecondaryRoles(),null);
                thirdRoles.OnCombatPrepares(team.GetThirdRoles(),null);
            }

            public void OnFinish(Action<UTempoTrackerHolder> hideTracker)
            {
                foreach (var spawner in GetEnumerable())
                {
                    spawner.OnCombatFinish(hideTracker);
                }
            }

            private IEnumerable<RolesSpawner> GetEnumerable()
            {
                yield return mainRoles;
                yield return secondaryRoles;
                yield return thirdRoles;
            }

            public TeamMembersTypeSpawner<UTempoTrackerHolder> MainRole => mainRoles;
            public TeamMembersTypeSpawner<UTempoTrackerHolder> SecondaryRole => secondaryRoles;
            public TeamMembersTypeSpawner<UTempoTrackerHolder> ThirdRole => thirdRoles;
        }


        [Serializable]
        private sealed class RolesSpawner : TeamMembersTypeSpawner<UTempoTrackerHolder>
        {
            private UTempoMainTickHandler _handler;
            public void Injection(UTempoMainTickHandler handler) => _handler = handler;

            protected override void OnInstantiationElement(UTempoTrackerHolder element, CombatEntity entity, int count)
            {
                _handler.HandleEntity(entity,element);
            }
        }
    }
}
