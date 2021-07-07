using System;
using System.Collections.Generic;
using ___ProjectExclusive._Enemies;
using Characters;
using _Player;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CombatSystem
{
    public class SystemInvoker
    {
        [ShowInInspector, DisableInEditorMode, DisableInPlayMode]
        private readonly Queue<ICombatPreparationListener> _preparationListeners;
        [ShowInInspector, DisableInEditorMode, DisableInPlayMode]
        private readonly Queue<ICombatAfterPreparationListener> _afterPreparationListeners;


        [ShowInInspector, DisableInEditorMode, DisableInPlayMode]
        private readonly Queue<ICombatStartListener> _onStartListeners;

        [ShowInInspector, DisableInEditorMode, DisableInPlayMode]
        private readonly Queue<ICombatFinishListener> _onFinishListeners;


        [ShowInInspector, DisableInEditorMode, DisableInPlayMode]
        private readonly Queue<ICombatPauseListener> _onPauseListeners;


        public SystemInvoker(int lengthAllocation = 0)
        {
            _preparationListeners = new Queue<ICombatPreparationListener>(lengthAllocation);
            _afterPreparationListeners = new Queue<ICombatAfterPreparationListener>(lengthAllocation);
            _onStartListeners = new Queue<ICombatStartListener>(lengthAllocation);
            _onFinishListeners = new Queue<ICombatFinishListener>(lengthAllocation);
            _onPauseListeners = new Queue<ICombatPauseListener>(lengthAllocation);
        }

        public void SubscribeListener(IInvokerListenerBase listener)
        {
            //Don't convert to switch because it can be multiples types at once
            if(listener is ICombatPreparationListener combatPreparation)
                _preparationListeners.Enqueue(combatPreparation);
            if(listener is ICombatAfterPreparationListener afterPreparationListener)
                _afterPreparationListeners.Enqueue(afterPreparationListener);
            if(listener is ICombatStartListener combatStart)
                _onStartListeners.Enqueue(combatStart);
            if(listener is ICombatFinishListener finishListener)
                _onFinishListeners.Enqueue(finishListener);
            if(listener is ICombatPauseListener pauseListener)
                _onPauseListeners.Enqueue(pauseListener);

        }

        [NonSerialized]
        public CoroutineHandle CombatHandle;
        [Button,DisableInEditorMode, GUIColor(.5f,.9f,.8f)]
        public void InvokeCombat(SEnemyFightPreset enemyFightPreset)
        {
            var playerSelections 
                = PlayerEntitySingleton.SelectedCharacters;

            CombatingTeam playerEntities = new CombatingTeam();
            CombatingTeam enemyEntities = new CombatingTeam();

            CombatHandle = Timing.RunCoroutine(_DoCombat());

            IEnumerator<float> _DoCombat()
            {
                playerEntities.InjectParse(playerSelections, GenerateEntity);
                enemyEntities.InjectParse(enemyFightPreset, GenerateEntity);
                CombatingTeam allEntities; 
                GenerateAllEntities();
                // This phase could be loaded asynchronously while the player is preparing the characters
                Debug.Log("x--- Preparing Combat");
                PreparationPhase();

                // Small wait for secure some loads
                yield return Timing.WaitForSeconds(1f);
                Debug.Log("x--- Starting Combat");
                StartPhase();
                Debug.Log("x--- In Combat");



                void PreparationPhase()
                {
                    foreach (ICombatPreparationListener listener in _preparationListeners)
                    {
                        listener.OnBeforeStart(playerEntities, enemyEntities, allEntities);
                    }

                    foreach (ICombatAfterPreparationListener listener in _afterPreparationListeners)
                    {
                        listener.OnAfterPreparation(playerEntities,enemyEntities,allEntities);
                    }
                }

                void StartPhase()
                {
                    foreach (ICombatStartListener listener in _onStartListeners)
                    {
                        listener.OnStart();
                    }
                }


                void GenerateAllEntities()
                {
                    int length = playerEntities.Count + enemyEntities.Count;
                    allEntities = new CombatingTeam(length);
                    allEntities.AddRange(playerEntities);
                    allEntities.AddRange(enemyEntities);
                }
            }
        }

        public void FinishCombat()
        {
            Timing.KillCoroutines(CombatHandle);
        }

        [Button(ButtonSizes.Large),HideInEditorMode, GUIColor(.9f,.8f,.8f)]
        public void PauseCombat()
        {
            foreach (ICombatPauseListener listener in _onPauseListeners)
            {
                listener.OnPause();
            }
        }

        [Button(ButtonSizes.Large), HideInEditorMode, GUIColor(.9f,.8f,.8f)]
        public void ResumeCombat()
        {
            foreach (ICombatPauseListener listener in _onPauseListeners)
            {
                listener.OnResume();
            }
        }


        private static CombatingEntity GenerateEntity(SCharacterEntityVariable variable)
        {
            CombatingEntity entity = new CombatingEntity(variable.characterName,
                variable.CharacterPrefab);

            entity.Injection(variable.GenerateData());
            entity.Injection(variable.skillsPreset);
            return entity;
        }
    }


    public interface IInvokerListenerBase
    {}

    public interface ICombatFullListener : ICombatPreparationListener, ICombatStartListener,
        ICombatFinishListener, ICombatPauseListener
    {}

    /// <summary>
    /// During loads/preparation of the Combat
    /// </summary>
    public interface ICombatPreparationListener : IInvokerListenerBase
    {
        // It uses the CombatingEntity only (and doesn't uses ICharacterStats for example)
        // because on Add/Remove (by a summon for example) the entity will handle the changes 
        void OnBeforeStart(
            CombatingTeam playerEntities,
            CombatingTeam enemyEntities,
            CharacterArchetypesList<CombatingEntity> allEntities);
    }
    /// <summary>
    /// After [<see cref="ICombatPreparationListener"/>]s but before any 'formal' Combat
    /// phase is invoked (such animations, text, sounds). This is used for those
    /// elements that needs dependencies being prepared before such elements are invoked.
    /// </summary>
    public interface ICombatAfterPreparationListener : IInvokerListenerBase
    {
        void OnAfterPreparation(
            CombatingTeam playerEntities,
            CombatingTeam enemyEntities,
            CharacterArchetypesList<CombatingEntity> allEntities);
    }

    public interface ICombatStartListener : IInvokerListenerBase
    {
        void OnStart();
    }

    public interface ICombatPauseListener : IInvokerListenerBase
    {
        void OnPause();
        void OnResume();
    }

    public interface ICombatFinishListener : IInvokerListenerBase
    {
        void OnFinish(CombatingTeam removeEnemies);
    }
}