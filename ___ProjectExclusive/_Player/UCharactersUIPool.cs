﻿using System;
using System.Collections.Generic;
using _CombatSystem;
using _Player;
using Characters;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Player
{
    public class UCharactersUIPool : MonoBehaviour, IPlayerCombatPool<UCharacterUIHolder>
    {
        [Title("References")] 
        [SerializeField] private UTeamsSpawner spawner = null;
        [SerializeField] private Camera canvasCamera = null;
        [SerializeField] private Transform instantiationParent = null;

        [TitleGroup("Pooling")] [SerializeField, HideInPlayMode]
        private UCharacterUIHolder holderPrefab = null;

        private Stack<UCharacterUIHolder> _holders;


        private void Awake()
        {
            int allocationAmount = CharacterUtils.PredictedAmountOfCharactersInBattle;
            _holders = new Stack<UCharacterUIHolder>(allocationAmount);
            DoPooling(allocationAmount, true);

            PlayerEntitySingleton.CombatElementsPools.CharacterUIPool = this;
        }

        public UCharacterUIHolder PoolDoInjection(CombatingEntity entity)
        {
            var holder = _holders.Pop();
            holder.Injection(entity);
            holder.gameObject.SetActive(true);
            return holder;
        }

        public void ReturnElement(UCharacterUIHolder holder)
        {
            _holders.Push(holder);
            holder.gameObject.SetActive(false);
        }

        private void DoPooling(int poolAmount, bool initialPosition = false)
        {
            for (int i = 0; i < poolAmount; i++)
            {
                UCharacterUIHolder holder =
                    Instantiate(holderPrefab, instantiationParent);
                _holders.Push(holder);
                holder.Injection(canvasCamera);



                if (!initialPosition) continue;



                const int indexLimit = CharacterArchetypes.AmountOfArchetypes;
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

        public virtual void Injection(CombatingEntity entity)
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



            Transform onTransform = holder.GetTooltipTransform();
            RePosition(onTransform.position);
        }

        public void RePosition(Vector3 worldPosition)
        {
            transform.position = canvasCamera.WorldToScreenPoint(worldPosition);
        }

        public void OnFinish(CombatingTeam removeEnemies)
        {
            gameObject.SetActive(false);
            currentEntity = null;
        }
        public void OnStart()
        {
            gameObject.SetActive(true);
        }
    }
}
