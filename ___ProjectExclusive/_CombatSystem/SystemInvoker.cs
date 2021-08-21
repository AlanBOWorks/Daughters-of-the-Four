using System;
using System.Collections.Generic;
using ___ProjectExclusive._Enemies;
using Characters;
using _Player;
using _Team;
using MEC;
using Passives;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;

namespace _CombatSystem
{
    public class SystemInvoker : ICombatFinishListener
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

            CombatingTeam playerEntities = new CombatingTeam(PlayerEntitySingleton.TeamControlStats);
            CombatingTeam enemyEntities = new CombatingTeam(enemyFightPreset.StatsPreset);


            CombatHandle = Timing.RunCoroutine(_DoCombat());

            IEnumerator<float> _DoCombat()
            {
                #region <<< COMBAT REGION >>>>
                // This phase could be loaded asynchronously while the player is preparing the characters
                CharacterArchetypesList<CombatingEntity> allEntities;
                CharacterArchetypes.TeamPosition entityPosition;
                Action onStartAction = StartPhase;

                InitializationPhase();
                InitializeCombatConditions();
                Debug.Log("x--- Preparing Combat");
                PreparationPhase();

                // Small wait for secure some loads
                yield return Timing.WaitForSeconds(1f);
                Debug.Log("x--- Starting Combat");
                // TODO make an starting animation
                onStartAction.Invoke();
                Debug.Log("x--- In Combat");

                void InitializationPhase()
                {
                    entityPosition = CharacterArchetypes.TeamPosition.FrontLine;
                    playerEntities.InjectParse(playerSelections, GenerateEntity);

                    entityPosition = CharacterArchetypes.TeamPosition.FrontLine;
                    enemyEntities.InjectParse(enemyFightPreset, GenerateEntity);

                    DoInitializations(playerEntities);
                    DoInitializations(enemyEntities);

                    GenerateAllEntities();
                }
                void GenerateAllEntities()
                {
                    int length = playerEntities.Count + enemyEntities.Count;
                    allEntities = new CharacterArchetypesList<CombatingEntity>(length);
                    allEntities.AddRange(playerEntities);
                    allEntities.AddRange(enemyEntities);
                }
                void DoInitializations(CombatingTeam team)
                {
                    foreach (CombatingEntity entity in team)
                    {
                        entity.Injection(team);
                        entity.CombatStats.Initialization();
                    }
                }

                void InitializeCombatConditions()
                {
                    GenericCombatConditions genericCombatConditions = null;

                    IWinCombatCondition winCombatCondition;
                    if (enemyFightPreset.IsWinConditionValid())
                        winCombatCondition = enemyFightPreset;
                    else
                    {
                        genericCombatConditions = CreateGenerics();
                        winCombatCondition = genericCombatConditions;
                    }

                    ILoseCombatCondition loseCombatCondition;
                    if (enemyFightPreset.IsLoseConditionValid())
                    {
                        loseCombatCondition = enemyFightPreset;
                    }
                    else
                    {
                        loseCombatCondition = genericCombatConditions ?? CreateGenerics();
                    }

                    var conditionChecker = CombatSystemSingleton.CombatConditionChecker;
                    conditionChecker.WinCombatCondition = winCombatCondition;
                    conditionChecker.LoseCombatCondition = loseCombatCondition;

                    GenericCombatConditions CreateGenerics()
                    {
                        return new GenericCombatConditions(playerEntities, enemyEntities);
                    }
                }

                void PreparationPhase()
                {
                    foreach (ICombatPreparationListener listener in _preparationListeners)
                    {
                        listener.OnBeforeStart(playerEntities, enemyEntities, allEntities);
                    }

                    foreach (ICombatAfterPreparationListener listener in _afterPreparationListeners)
                    {
                        listener.OnAfterPreparation(playerEntities, enemyEntities, allEntities);
                    }

                    CombatSystemSingleton.TempoHandler.Inject(enemyFightPreset.GetCombatController());
                }

                void StartPhase()
                {
                    foreach (ICombatStartListener listener in _onStartListeners)
                    {
                        listener.OnCombatStart();
                    }
                }




                CombatingEntity GenerateEntity(ICharacterCombatProvider variable)
                {
                    // Instantiate
                    CombatingEntity entity = new CombatingEntity(variable.CharacterName,
                        variable.CharacterPrefab);


                    // x----- CombatData
                    var combatData = variable.GenerateCombatData();
                    entity.Injection(combatData);

                    // x----- Area
                    var characterAreaData = new CharacterCombatAreasData(entityPosition, variable.RangeType);
                    entity.AreasDataTracker = characterAreaData;
                    
                    // x----- Skills
                    variable.GenerateCombatSkills(entity);

                    // x----- Harmony Buffer
                    var harmonyVariable = variable.GetHarmonyPassive();
                    HarmonyBuffInvoker harmonyBuffer = null;
                    if (harmonyVariable != null)
                    {
                        //TODO make a backup HarmonyBuff for empty by type (Archetype based)
                        harmonyBuffer = new HarmonyBuffInvoker(entity, harmonyVariable);
                        entity.Injection(harmonyBuffer);
                    }

                    // x----- Passives
                    var passivesHolderVariable = variable.GetPassivesHolder();
                    var passiveHolder 
                        = new CombatPassivesHolder(entity,
                            passivesHolderVariable,variable.GetSharedFilterPassives(),
                            harmonyBuffer);
                    entity.Injection(passiveHolder);
                    
                    // X----- Critical Buff
                    var criticalBuff = variable.GetCriticalBuff();
                    if (criticalBuff == null)
                    {
                        var defaultCriticalBuffs 
                            = CombatSystemSingleton.ParamsVariable.ArchetypesBackupOnNullCriticalBuffs;
                        criticalBuff = CharacterArchetypes.GetElement(
                            defaultCriticalBuffs, entityPosition);
                    }
                    entity.Injection(criticalBuff);

                    onStartAction += OpeningPassivesInjection;

                    entityPosition++;
                    return entity;

                    void OpeningPassivesInjection()
                    {
                        var openingPassives = variable.GetOpeningPassives();
                        openingPassives?.DoOpeningPassives(entity);
                    }
                }
                #endregion


            }
        }

        [Button(ButtonSizes.Large),HideInEditorMode, GUIColor(.9f,.8f,.8f)]
        public void PauseCombat()
        {
            foreach (ICombatPauseListener listener in _onPauseListeners)
            {
                listener.OnCombatPause();
            }
        }

        [Button(ButtonSizes.Large), HideInEditorMode, GUIColor(.9f,.8f,.8f)]
        public void ResumeCombat()
        {
            foreach (ICombatPauseListener listener in _onPauseListeners)
            {
                listener.OnCombatResume();
            }
        }


        public void OnCombatFinish(CombatingEntity lastEntity, bool isPlayerWin)
        {
            Debug.Log("x--- Finish Combat ---X");
            foreach (ICombatFinishListener listener in _onFinishListeners)
            {
                listener.OnCombatFinish(lastEntity,isPlayerWin);
            }
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
        void OnCombatStart();
    }

    public interface ICombatPauseListener : IInvokerListenerBase
    {
        void OnCombatPause();
        void OnCombatResume();
    }

    public interface ICombatFinishListener : IInvokerListenerBase
    {
        void OnCombatFinish(CombatingEntity lastEntity,bool isPlayerWin);
        // TODO AfterFinishAnimation()
        // TODO AfterConfirmFinish()
    }


}