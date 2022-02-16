using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UUIHoverEntitiesHolder : MonoBehaviour, ICombatPreparationListener, ICombatStatesListener,
        ICombatEntityExistenceListener
    {
        [SerializeField,HorizontalGroup("Teams")] 
        private HoverEntityHandler playerHoverHandler = new HoverEntityHandler();
        [SerializeField,HorizontalGroup("Teams")]
        private HoverEntityHandler enemyHoverHandler = new HoverEntityHandler();
        [ShowInInspector, ReadOnly]
        private IUIHoverListener[] uiHoverListeners;

        private void Awake()
        {
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
            uiHoverListeners = GetComponents<IUIHoverListener>();
        }



        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            Transform canvasTransform = transform;
            playerHoverHandler.GenerateHovers(in playerTeam, in canvasTransform, uiHoverListeners);
            enemyHoverHandler.GenerateHovers(in enemyTeam, in canvasTransform, uiHoverListeners);
        }
        public void OnCreateEntity(in CombatEntity entity, in bool isPlayers)
        {
            var handler = GetHandler(isPlayers);
            handler.GenerateHover(in entity, transform, isPlayers, uiHoverListeners);
        }

        public void OnDestroyEntity(in CombatEntity entity, in bool isPlayers)
        {
           var handler = GetHandler(isPlayers);
            handler.ReturnElement(in entity);
        }

        private HoverEntityHandler GetHandler(in bool isPlayer) => isPlayer
            ? playerHoverHandler
            : enemyHoverHandler;


        public void OnCombatStart()
        {
            playerHoverHandler.StartTicking();
            enemyHoverHandler.StartTicking();
        }
        public void OnCombatFinish()
        {
            playerHoverHandler.Clear();
            enemyHoverHandler.Clear();

            foreach (var listener in uiHoverListeners)
            {
                listener.ClearEntities();
            }
        }

        public void OnCombatQuit()
        {
            OnCombatFinish();
        }


        [Serializable]
        private sealed class HoverEntityHandler
        {
            [SerializeField] private UUIHoverEntityHolder hoverEntityPrefab;
            private readonly Stack<UUIHoverEntityHolder> _instantiatedPool;
            private readonly Dictionary<CombatEntity, UUIHoverEntityHolder> _dictionary;

            public HoverEntityHandler()
            {
                _instantiatedPool = new Stack<UUIHoverEntityHolder>();
                _dictionary = new Dictionary<CombatEntity, UUIHoverEntityHolder>();
            }

            public void StartTicking()
            {
                foreach (var hoverEntity in _dictionary)
                {
                    hoverEntity.Value.StartTicking();
                }
            }
            public void StopTicking()
            {
                foreach (var hoverEntity in _dictionary)
                {
                    hoverEntity.Value.StopTicking();
                }
            }


            private void InvokeListeners(in UUIHoverEntityHolder holder, in CombatEntity entity, in IUIHoverListener[] listeners)
            {
                if (listeners == null) return;

                foreach (var listener in listeners)
                {
                    listener.OnHoverEntityCreated(in holder, in entity);
                }
            }

            public void GenerateHovers(in CombatTeam team, in Transform parent, IUIHoverListener[] listeners)
            {
                foreach (var member in team)
                {
                    var holder = GenerateHover(in member, in parent);
                    InvokeListeners(in holder, in member, in listeners);
                }
            }



            public void GenerateHover(in CombatEntity entity,in Transform parent, bool isPlayerEntity, IUIHoverListener[] listeners)
            {
                var holder = GenerateHover(in entity, in parent);
                InvokeListeners(in holder, in entity, in listeners);
            }


            private UUIHoverEntityHolder GenerateHover(in CombatEntity entity, in Transform parent)
            {
                UUIHoverEntityHolder entityHolder;
                if (_instantiatedPool.Count > 0)
                    entityHolder = _instantiatedPool.Pop();
                else
                    entityHolder = Instantiate(hoverEntityPrefab,parent);


                entityHolder.Injection(entity);
                _dictionary.Add(entity, entityHolder);

                entityHolder.Show();

#if UNITY_EDITOR
                entityHolder.name = entity.GetEntityName() + " - UI Hover [Group] (Clone)";
#endif

                return entityHolder;
            }

            public void ReturnElement(in CombatEntity entity)
            {
                var hoverElement = _dictionary[entity];
                _instantiatedPool.Push(hoverElement);
                hoverElement.HideInstant();
            }

            public bool Contains(in CombatEntity entity) => _dictionary.ContainsKey(entity);

            public void Clear()
            {
                foreach (var hoverEntity in _dictionary)
                {
                    var hoverElement = hoverEntity.Value;
                    _instantiatedPool.Push(hoverElement);
                    hoverElement.StopTicking();
                }
                _dictionary.Clear();
            }


        }

    }


    public interface IUIHoverListener
    {
        void OnHoverEntityCreated(in UUIHoverEntityHolder hover, in CombatEntity entity);
        void ClearEntities();
    }
}
