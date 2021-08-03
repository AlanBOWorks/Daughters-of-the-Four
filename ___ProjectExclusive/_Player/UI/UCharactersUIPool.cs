using System;
using System.Collections.Generic;
using _CombatSystem;
using _Player;
using Characters;
using MEC;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Player
{
    public class UCharactersUIPool : MonoBehaviour, IPlayerCombatPool<UCharacterUIHolder>,
        ICameraSingletonListener
    {
        [Title("References")] 
        [SerializeField] private UCombatTeamsSpawner spawner = null;
        [SerializeField] private Transform instantiationParent = null;

        [TitleGroup("Pooling")] 
        [SerializeField, HideInPlayMode]
        private UCharacterUIHolder holderPrefab = null;

        private Stack<UCharacterUIHolder> _holders;

        private void Awake()
        {
            CameraSingleton.Subscribe(this);
        }

        

        public UCharacterUIHolder PoolDoInjection(CombatingEntity entity, bool isPlayer)
        {
            var holder = _holders.Pop();
            holder.Injection(entity, isPlayer);
            holder.gameObject.SetActive(true);
            return holder;
        }

        public void ReturnElement(UCharacterUIHolder holder)
        {
            _holders.Push(holder);
            holder.gameObject.SetActive(false);
        }

        private void DoPooling(Camera canvasCamera, int poolAmount, bool initialPosition = false)
        {
            for (int i = 0; i < poolAmount; i++)
            {
                UCharacterUIHolder holder =
                    Instantiate(holderPrefab, instantiationParent);
                _holders.Push(holder);
                holder.Injection(canvasCamera);



                if (!initialPosition) continue;



                const int indexLimit = CharacterArchetypes.AmountOfArchetypesAmount;
                int elementIndex = i;
                Transform spawnElement;
                if (i < indexLimit)
                {
                    spawnElement =
                        CharacterArchetypes.GetElement(spawner.PlayerFaction, elementIndex);
                }
                else
                {
                    elementIndex -= indexLimit;
                    spawnElement =
                        CharacterArchetypes.GetElement(spawner.EnemyFaction, elementIndex);
                }

                Vector3 spawnPosition = spawnElement.position;
                holder.RePosition(spawnPosition);
            }
        }

        public void OnCameraInitiation(Camera injection)
        {
            int allocationAmount = UtilsCharacter.PredictedAmountOfCharactersInBattle;
            _holders = new Stack<UCharacterUIHolder>(allocationAmount);
            DoPooling(injection, allocationAmount, true);

            PlayerEntitySingleton.CombatElementsPools.CharacterUIPool = this;
        }
        public void OnCameraChange(Camera injection)
        {
            foreach (UCharacterUIHolder holder in _holders)
            {
                holder.Injection(injection);
            }
        }
    }

    public abstract class UCharacterOverTooltipBase : MonoBehaviour,ICombatStartListener, ICombatFinishListener
    {
        [ShowInInspector, HideInEditorMode, DisableInPlayMode]
        protected CombatingEntity currentEntity;

        protected Camera canvasCamera;

        private void Awake()
        {
            CombatSystemSingleton.Invoker.SubscribeListener(this);
            gameObject.SetActive(false);
        }

        public virtual void Injection(CombatingEntity entity, bool isPlayer)
        {
            if (currentEntity != null) return;
            currentEntity = entity;
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
