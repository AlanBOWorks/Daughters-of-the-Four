using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Team;
using CombatSystem.Team.VanguardEffects;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Player.UI
{
    public class UVanguardEffectsTooltipWindowHandler : MonoBehaviour, 
        IVanguardEffectUsageListener,
        ITempoTeamStatesListener,
        ICombatTerminationListener
    {
        [Title("Pool References")] 
        [SerializeField]
        private UEffectTooltipHolder effectPrefab;
        [SerializeField] 
        private RectTransform onReturnParent;

        [Title("Pools")]
        [SerializeField]
        private ObjectPool punishPool;
        [SerializeField] 
        private ObjectPool revengePool;

        private IReadOnlyDictionary<IVanguardSkill, ICollection<UEffectTooltipHolder>> _dictionary;
        public IReadOnlyDictionary<IVanguardSkill, ICollection<UEffectTooltipHolder>> GetDictionary() => _dictionary;

        private void Awake()
        {
            var inactiveQueue = new Queue<UEffectTooltipHolder>();
            var activeDictionary = new Dictionary<IVanguardSkill, ICollection<UEffectTooltipHolder>>();
            _dictionary = activeDictionary;

            var injectionValues = new InjectionValues(onReturnParent,effectPrefab,inactiveQueue, activeDictionary);

            punishPool.Awake(injectionValues);
            revengePool.Awake(injectionValues);

            onReturnParent.gameObject.SetActive(false);
            effectPrefab.gameObject.SetActive(false);
        }


        public void OnVanguardEffectSubscribe(in VanguardSkillAccumulation values)
        {
            var type = values.Type;
            ObjectPool pool = (type == EnumsVanguardEffects.VanguardEffectType.Revenge)
                ? revengePool
                : punishPool;

            var skill = values.Skill;
            if(_dictionary.ContainsKey(skill))
                MultiplyEffects();
            else
                pool.SpawnSkillEffects(values);

            void MultiplyEffects()
            {
                var elements = _dictionary[skill];
                float modifier = skill.VanguardEffectCount;
                foreach (var holder in elements)
                {
                    UtilsEffectTooltip.MultiplyEffectText(holder, modifier);
                }
            }
        }

        private void ReturnElementsToPool()
        {
            punishPool.ReturnToElementsToPool();
            revengePool.ReturnToElementsToPool();
        }

        public void OnVanguardEffectIncrement(EnumsVanguardEffects.VanguardEffectType type, CombatEntity attacker)
        {

        }

        public void OnVanguardEffectPerform(VanguardSkillUsageValues values)
        {
            punishPool.ReturnToElementsToPool();
            revengePool.ReturnToElementsToPool();
        }

        public void OnCombatEnd()
        {
            ReturnElementsToPool();
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
        }

        public void OnCombatQuit()
        {
        }

        public void OnTempoPreStartControl(CombatTeamControllerBase controller)
        {
            if(controller.ControllingTeam.VanguardEffectsHolder.IsMainEntityTurn())
                ReturnElementsToPool();
        }

        public void OnAllActorsNoActions(CombatEntity lastActor)
        {
        }

        public void OnControlFinishAllActors(CombatEntity lastActor)
        {
        }

        public void OnTempoFinishControl(CombatTeamControllerBase controller)
        {
        }

        public void OnTempoFinishLastCall(CombatTeamControllerBase controller)
        {
        }

        private readonly struct InjectionValues
        {
            public readonly RectTransform OnReturnParent;
            public readonly UEffectTooltipHolder ClonablePrefab;
            public readonly Queue<UEffectTooltipHolder> InactiveHolder;
            public readonly Dictionary<IVanguardSkill, ICollection<UEffectTooltipHolder>> ActiveElements;

            public InjectionValues(
                RectTransform onReturnParent, 
                UEffectTooltipHolder clonablePrefab, 
                Queue<UEffectTooltipHolder> inactiveHolder, 
                Dictionary<IVanguardSkill, ICollection<UEffectTooltipHolder>> activeElements)
            {
                OnReturnParent = onReturnParent;
                ClonablePrefab = clonablePrefab;
                InactiveHolder = inactiveHolder;
                ActiveElements = activeElements;
            }
        }

        [Serializable]
        private sealed class ObjectPool
        {
            [SerializeField]
            private RectTransform onPopParent;

            private RectTransform _onReturnParent;
            private UEffectTooltipHolder clonablePrefab;
            [ShowInInspector,HideInEditorMode]
            private Queue<UEffectTooltipHolder> _inactiveHolder;
            [ShowInInspector,HideInEditorMode]
            private Dictionary<IVanguardSkill, ICollection<UEffectTooltipHolder>> _activeElements;
            private float _accumulatedHeight;

            public void Awake(InjectionValues values)
            {
                clonablePrefab = values.ClonablePrefab;
                _inactiveHolder = values.InactiveHolder;
                _activeElements = values.ActiveElements;
                _onReturnParent = values.OnReturnParent;

            }

            public void SpawnSkillEffects(VanguardSkillAccumulation values)
            {
                var skill = values.Skill;
                int accumulatedAmount = values.AccumulatedAmount;
                HandleAsNewCollection();


                void HandleAsNewCollection()
                {
                    int targetCount = skill.VanguardEffectCount;
                    UEffectTooltipHolder[] holders = new UEffectTooltipHolder[targetCount];

                    int i = 0;
                    foreach (var effect in skill.GetPerformVanguardEffects())
                    {
                        var element = PopElement();
                        element.gameObject.SetActive(true);
                        holders[i] = element;
                        HandleElement(element,in effect);
                        i++;
                    }
                    

                    _activeElements.Add(skill, holders);
                }


                void HandleElement(UEffectTooltipHolder holder, in PerformEffectValues effect)
                {
                    PerformEffectValues accumulatedValue = new PerformEffectValues(
                        effect.Effect,
                        effect.EffectValue * accumulatedAmount,
                        effect.TargetType);
                    UtilsEffectTooltip.HandleText(holder, in accumulatedValue);
                    UtilsEffectTooltip.HandleIcon(holder, effect.Effect);
                    UtilsEffectTooltip.HandleTextHeight(holder.GetTextHolder(),ref _accumulatedHeight);
                    holder.EffectValues = effect;
                }
            }

            private UEffectTooltipHolder PopElement()
            {
                return UtilsPool.PoolElement(_inactiveHolder, clonablePrefab, onPopParent);
            }

            public void Clear()
            {
                foreach (var activeCollection in _activeElements)
                {
                    DestroyCollection(activeCollection.Value);
                }
                DestroyCollection(_inactiveHolder);

                _activeElements.Clear();
                _inactiveHolder.Clear();

                void DestroyCollection(IEnumerable<UEffectTooltipHolder> collection)
                {
                    foreach (var activeElement in collection)
                    {
                        Destroy(activeElement);
                    }
                }
                _accumulatedHeight = 0;
            }

            public void ReturnToElementsToPool()
            {
                foreach (var activeCollection in _activeElements)
                {
                    foreach (var activeElement in activeCollection.Value)
                    {
                        _inactiveHolder.Enqueue(activeElement);
                        activeElement.transform.SetParent(_onReturnParent);
                    }
                }
                _activeElements.Clear();
                _accumulatedHeight = 0;
            }
        }
    }
}
