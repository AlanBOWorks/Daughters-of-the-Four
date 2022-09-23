using System;
using CombatSystem.Entity;
using CombatSystem.Passives;
using CombatSystem.Team;
using Michsky.UI.MTP;
using UnityEngine;
using Utils;

namespace CombatSystem.Player.UI.Info
{
    public class UPassivePopUpHandler : MonoBehaviour,
        ICombatPassiveListener,
        ICameraHolderListener
    {
        [SerializeField] private PopUpPool popUpPool = new PopUpPool();

        private Camera _combatCamera;
        private void OnEnable()
        {
            _combatCamera = CombatCameraHandler.MainCamera;
        }

        private void Awake()
        {
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
            PlayerCombatSingleton.CameraEvents.Subscribe(this);
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
            PlayerCombatSingleton.CameraEvents.UnSubscribe(this);
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
        public void OnPassiveTrigged(CombatEntity entity, ICombatPassive passive, ref float value)
        {
            popUpPool.OnPassiveTrigged(_combatCamera,entity,passive);
        }

        [Serializable]
        private sealed class PopUpPool : TrackedMonoObjectPool<UPassivePopUp>
        {
            public void OnPassiveTrigged(Camera camera,CombatEntity entity, ICombatPassive passive)
            {
                var pivot = entity.Body.PivotRootType;
                var targetSpawnPoint = camera.WorldToScreenPoint(pivot.position);
                var spawnElement = PopElementSafe(false);
                spawnElement.transform.position = targetSpawnPoint;
                

                HandleElementText(spawnElement, passive);
                HandleElementColor(spawnElement,entity);

                spawnElement.Injection(this);
                spawnElement.gameObject.SetActive(true);
            }

            private static void HandleElementText(UPassivePopUp popUp, ICombatPassive passive)
            {
                var passiveText = passive.GetPassiveEffectText();
                popUp.InjectEffectText(passiveText);
            }

            private static void HandleElementColor(UPassivePopUp popUp, ITeamAreaDataRead entity)
            {
                var entityTheme = UtilsTeam.GetElement(entity.RoleType, CombatThemeSingleton.RolesThemeHolder);
                var entityColor = entityTheme.GetThemeColor();
                HandleElementColor(popUp, entityColor);
            }
            private static void HandleElementColor(UPassivePopUp popUp, Color color)
            {
                popUp.ChangeBackgroundColor(color);
            }
        }

    }
}
