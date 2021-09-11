using System;
using System.Collections.Generic;
using _CombatSystem;
using _Player;
using _Team;
using Characters;
using MEC;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Player
{
    public class UTeamsOverUIHolder : MonoBehaviour, ITeamsData<ICharacterArchetypesData<UCharacterUIHolder>>
    {
        [SerializeField] private TeamOverUIHolder playerHolders = new TeamOverUIHolder();
        [SerializeField] private TeamOverUIHolder enemyHolders = new TeamOverUIHolder();

        private void Awake()
        {
            CameraSingleton.Subscribe(playerHolders);
            CameraSingleton.Subscribe(enemyHolders);
        
            var playerElements = PlayerEntitySingleton.CombatUiElements;
            UtilsTeam.DoInjection(playerElements,this,DoInjection);
            void DoInjection(PlayerCombatUIElement element, UCharacterUIHolder holder)
            {
                element.UIHolder = holder;
            }
        }


        public ICharacterArchetypesData<UCharacterUIHolder> PlayerData => playerHolders;
        public ICharacterArchetypesData<UCharacterUIHolder> EnemyData => enemyHolders;


        [Serializable]
        private class TeamOverUIHolder : ICharacterArchetypesData<UCharacterUIHolder>, ICameraSingletonListener
        {
            [SerializeField] private UCharacterUIHolder vanguard;
            [SerializeField] private UCharacterUIHolder attacker;
            [SerializeField] private UCharacterUIHolder support;

            public UCharacterUIHolder Vanguard => vanguard;
            public UCharacterUIHolder Attacker => attacker;
            public UCharacterUIHolder Support => support;

            public void OnCameraInitiation(Camera injection)
                => OnCameraChange(injection);

            public void OnCameraChange(Camera injection)
            {
                UtilsCharacter.DoAction(this,DoInjection);

                void DoInjection(UCharacterUIHolder holder)
                    => holder.Injection(injection);
            }
        }
    }

    public abstract class UCharacterOverTooltipBase : MonoBehaviour,ICombatStartListener, ICombatFinishListener,
        IPersistentEntitySwitchListener, IPersistentElementInjector
    {
        [ShowInInspector, HideInEditorMode, DisableInPlayMode]
        protected CombatingEntity currentEntity;

        protected Camera canvasCamera;

        private void Awake()
        {
            CombatSystemSingleton.Invoker.SubscribeListener(this);
            gameObject.SetActive(false);
        }

        public virtual void OnEntitySwitch(CombatingEntity entity)
        {
            currentEntity = entity;
        }
        public virtual void DoInjection(EntityPersistentElements persistentElements)
        {
            persistentElements.SubscribeListener(this);
        }

        
        public void Injection(Camera projectionCamera)
        {
            this.canvasCamera = projectionCamera;
        }
       
        public void LateUpdate()
        {
            if(currentEntity == null) return;

            UCharacterHolder holder = currentEntity.Holder;
            if (holder == null || canvasCamera == null) return;

            RePosition(GetUIPosition(holder));
        }

        protected abstract Vector3 GetUIPosition(UCharacterHolder holder);

        public void RePosition(Vector3 worldPosition)
        {
            transform.position = canvasCamera.WorldToScreenPoint(worldPosition);
        }

        public void OnCombatStart()
        {
            gameObject.SetActive(true);
        }

        public void OnCombatFinish(CombatingEntity lastEntity, bool isPlayerWin)
        {
            gameObject.SetActive(false);
            currentEntity = null;
        }

    }
}
