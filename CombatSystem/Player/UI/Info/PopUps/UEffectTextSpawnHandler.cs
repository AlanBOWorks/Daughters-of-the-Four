using System;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Skills.VanguardEffects;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;
using Utils_Project;
using Random = UnityEngine.Random;

namespace CombatSystem.Player.UI
{
    public class UEffectTextSpawnHandler : MonoBehaviour, ICameraHolderListener, 
        IEffectUsageListener 
    {
        [SerializeField]
        private EffectPopupHandler effectPopupHandler = new EffectPopupHandler();

        [ShowInInspector]
        private Queue<KeyValuePair<Transform, SubmitEffectValues>> _popUpQueue;
        private Queue<bool> _popUpPlayerCheck;

        private void Awake()
        {
            _popUpQueue = new Queue<KeyValuePair<Transform, SubmitEffectValues>>(16);
            _popUpPlayerCheck = new Queue<bool>();
            CombatSystemSingleton.EventsHolder.Subscribe(this);
            PlayerCombatSingleton.CameraEvents.Subscribe(this);

            effectPopupHandler.Awake();
        }

        private Camera _combatCamera;
        private void OnEnable()
        {
            _combatCamera = CombatCameraHandler.MainCamera;
        }

        private void Update()
        {
            effectPopupHandler.Tick();
        }

        private void OnDestroy()
        {
            CombatSystemSingleton.EventsHolder.UnSubscribe(this);
            PlayerCombatSingleton.CameraEvents.UnSubscribe(this);
        }


        private void OnDisable()
        {
            _popUpQueue.Clear();
            _popUpPlayerCheck.Clear();
            Timing.KillCoroutines(_loopHandle);
        }

        public void OnSwitchMainCamera(Camera combatCamera)
        {
            _combatCamera = combatCamera;
        }

        public void OnSwitchBackCamera(Camera combatBackCamera)
        {
        }

        public void OnSwitchFrontCamera(Camera combatFrontCamera)
        {
        }

        public void OnCombatPrimaryEffectPerform(EntityPairInteraction entities, in SubmitEffectValues values)
        {
            OnCombatEffectPerform(entities,in values);
        }

        public void OnCombatSecondaryEffectPerform(EntityPairInteraction entities, in SubmitEffectValues values)
        {
            OnCombatEffectPerform(entities,in values);
        }

        public void OnCombatVanguardEffectPerform(EntityPairInteraction entities, in SubmitEffectValues values)
        {
            OnCombatEffectPerform(entities,in values);
        }


        public void OnCombatEffectPerform(EntityPairInteraction entities, in SubmitEffectValues values)
        {
            _popUpPlayerCheck.Enqueue(entities.Target.Team.IsPlayerTeam);
            EnQueue(entities.Target, values);
            if (_loopHandle.IsRunning) return;
            _loopHandle = Timing.RunCoroutine(_StartSpawningPopUps(), Segment.RealtimeUpdate);
        }


        private CoroutineHandle _loopHandle;
        private const float PopUpFrequency = .4f;
        private IEnumerator<float> _StartSpawningPopUps()
        {
            while (_popUpQueue.Count > 0)
            {
                yield return Timing.WaitForSeconds(PopUpFrequency);
                (Transform key, SubmitEffectValues value) = _popUpQueue.Dequeue();
                var popUpText = value;
                var isPlayerEntity = _popUpPlayerCheck.Dequeue();

                Spawn(key, in popUpText, isPlayerEntity);
            }
        }

        private void EnQueue(CombatEntity target, in SubmitEffectValues values)
        {
            var pivot = target.Body.PivotRootType;

            var queueElement = new KeyValuePair<Transform, SubmitEffectValues>(pivot,values);
            _popUpQueue.Enqueue(queueElement);
        }

        
        private void Spawn(Transform targetTransform, in SubmitEffectValues queueValues, bool isPlayerEntity)
        {
            var effect = queueValues.Effect;
            var effectValue = queueValues.EffectValue;
            var popUpText = LocalizeMath.LocalizeMathfValue(effectValue,effect.IsPercentSuffix());
            if (effect is IBuffEffect buffEffect)
            {
                HandleStatVariationEffect(buffEffect, false);
            }
            else if(effect is IDeBuffEffect deBuffEffect)
            {
                HandleStatVariationEffect(deBuffEffect, true);
            }

            var icon = UtilsVisual.GetEffectSprite(queueValues.Effect);

            CalculatePopUpsVector(effect, isPlayerEntity, 
                out Vector3 popUpMovement, 
                out Vector3 offsetPoint);
            Vector3 spawnPoint = _combatCamera.WorldToScreenPoint(targetTransform.position);
            spawnPoint += offsetPoint;

            var popUp = effectPopupHandler.GetPopUp();
            DoEffect();

            void DoEffect()
            {
                popUp.transform.position = spawnPoint;
                popUp.Injection(popUpText);
                popUp.Injection(icon);
                popUp.SetDestination(popUpMovement);
                popUp.gameObject.SetActive(true);
            }

            void HandleStatVariationEffect(IStatVariationEffect statVariationEffect, bool negateValue)
            {
                string variationText = " " + statVariationEffect.GetStatVariationEffectText();
                popUpText += variationText;
                if (negateValue)
                    popUpText = "-" + popUpText;
            }
        }

        private const float PopUpVectorMagnitude = 72;
        private const float PopUpPositionOffset = 10;
        private static void CalculatePopUpsVector(
            IEffectBasicInfo effect, bool isPlayer, 
            out Vector3 popUpMovement,
            out Vector3 popUpOffsetPoint)
        {
            var effectType = effect.EffectType;
            switch (effectType)
            {
                case EnumsEffect.ConcreteType.DefaultOffensive:
                case EnumsEffect.ConcreteType.DoT:
                case EnumsEffect.ConcreteType.DamageType:
                case EnumsEffect.ConcreteType.DeBuff:
                case EnumsEffect.ConcreteType.DeBurst:
                    if (!isPlayer)
                    {
                        popUpOffsetPoint = CalculateRandomPointInVerticalPlane(PopUpPositionOffset);
                        popUpMovement = DoRightMovement();
                    }
                    else
                    {
                        popUpOffsetPoint = CalculateRandomPointInVerticalPlane(-PopUpPositionOffset);
                        popUpMovement = DoLeftMovement();
                    }
                    break;
                default:
                    popUpMovement = DoUpMovement();
                    popUpOffsetPoint = CalculateRandomPointInHorizontalPlane(PopUpPositionOffset);
                    break;
            }
        }

        static Vector3 DoUpMovement() => new Vector3(0, PopUpVectorMagnitude);
        static Vector3 DoRightMovement() => new Vector3(PopUpVectorMagnitude, 0);
        static Vector3 DoDownMovement() => new Vector3(0, -PopUpVectorMagnitude);
        static Vector3 DoLeftMovement() => new Vector3(-PopUpVectorMagnitude, 0);


        private const float PopUpVariationMagnitude = 100;
        private const float LerpStep = .2f;

        private static float _horizontalLerp = LerpStep * 3;
        private static Vector3 CalculateRandomPointInHorizontalPlane(float verticalOffset)
        {
            float horizontalValue = Mathf.Lerp(-PopUpVariationMagnitude, PopUpVariationMagnitude, _horizontalLerp);
            _horizontalLerp += LerpStep;
            if (_horizontalLerp > 1) _horizontalLerp = 0;
            return new Vector3(horizontalValue,verticalOffset, 0);
        }

        private static float _verticalLerp = LerpStep * 3;
        private static Vector3 CalculateRandomPointInVerticalPlane(float horizontalOffset)
        {
            float verticalValue = Mathf.Lerp(-PopUpVariationMagnitude, PopUpVariationMagnitude, _verticalLerp);
            _verticalLerp += LerpStep;
            if (_verticalLerp > 1) _verticalLerp = 0;
            return new Vector3(horizontalOffset,verticalValue,  0);
        }

        [Serializable]
        private sealed class EffectPopupHandler : TrackedMonoObjectPool<UEffectTextPopUp>
        {
            [SerializeField,SuffixLabel("%")] 
            private AnimationCurve alphaCurve = new AnimationCurve(
                new Keyframe(0,0), new Keyframe(.5f,1));
            [SerializeField,SuffixLabel("%")] 
            private AnimationCurve movementCurve = new AnimationCurve(
                new Keyframe(0, 0), new Keyframe(.5f, 1));

            [SerializeField, SuffixLabel("deltas")] private float deltaSpeed = .4f;


            public void Tick()
            {
                TickAll(Time.deltaTime * deltaSpeed);
            }

            public UEffectTextPopUp GetPopUp()
            {
                var element = PopElementSafe();
                element.Injection(this);
                return element;
            }


            private void TickAll(float delta)
            {
                foreach (var element in GetActiveElements())
                {
                    float currentLerp = element.LerpAmount;
                    float targetLerp = currentLerp + delta;
                    element.LerpAmount = targetLerp;

                    float targetAlpha = alphaCurve.Evaluate(targetLerp);
                    float targetPosition = movementCurve.Evaluate(targetLerp);
                    element.SetAlpha(targetAlpha);
                    element.Move(targetPosition);
                }
            }
        }

    }
}
