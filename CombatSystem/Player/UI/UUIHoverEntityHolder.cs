using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using MEC;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UUIHoverEntityHolder : MonoBehaviour
    {
        private CombatEntity _entity;
        public void Injection(CombatEntity user)
        {
            _entity = user;
        }

        private CoroutineHandle _tickingHandle;
        public void StartTicking()
        {
            _tickingHandle = Timing.RunCoroutine(_RepositionOverEntity());
        }
        public void StopTicking()
        {
            Timing.KillCoroutines(_tickingHandle);
        }
        private void OnDestroy()
        {
            StopTicking();
        }
        private void OnEnable()
        {
            Timing.ResumeCoroutines(_tickingHandle);
        }
        private void OnDisable()
        {
            Timing.PauseCoroutines(_tickingHandle);
        }

        private IEnumerator<float> _RepositionOverEntity()
        {
            yield return Timing.WaitForOneFrame; //safe wait for instantiations

            Camera playerCamera = PlayerCombatSingleton.InterfaceCombatCamera;
            Transform followReference = _entity.InstantiationReference.transform;
            RectTransform rectTransform = (RectTransform)transform;

            while (followReference)
            {
                yield return Timing.WaitForOneFrame;
                var targetPoint = playerCamera.WorldToScreenPoint(followReference.position);
                rectTransform.position = targetPoint;
            }
        }


        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {

        }

        public void HideInstant()
        {
            gameObject.SetActive(false);
        }
    }
}
