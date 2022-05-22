using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public abstract class UTeamColumnElementSpawner<T> : UTeamElementSpawner<T> where T: UnityEngine.MonoBehaviour
    {
        [TitleGroup("Params")] 
        [SerializeField]
        private float marginBottomOffset = 8;

        [SerializeField] 
        private float offRolesMarginOffset = 16;

        private void Start()
        {
            PlayerCombatSingleton.PlayerCombatEvents.Subscribe(this);
        }

        private void OnDestroy()
        {
            PlayerCombatSingleton.PlayerCombatEvents.UnSubscribe(this);
        }

        protected override void OnCreateElement(CombatEntity entity, T element,
            int index)
        {
            RepositionElementByIndex(in element, index,0);
        }

        public override void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            base.OnCombatPreStarts(playerTeam, enemyTeam);

            HandleOffRoles(_playerTeamReferences);
            HandleOffRoles(_enemyTeamReferences);
        }

        private void HandleOffRoles(TeamPrefabReferences references)
        {
            var dictionary = references.GetMainDictionary();
            RectTransform parent = (RectTransform)references.OffMembersPrefab.GetInstantiationParent();
            HandleOffRoles(dictionary, parent);
        }

        private void HandleOffRoles(IReadOnlyDictionary<CombatEntity, T> collection, RectTransform parent)
        {
            int amount = collection.Count + 1;
            RepositionElementByIndex(in parent, amount, -offRolesMarginOffset);
        }

        public void RepositionElementByIndex(in T element, int index, float offset)
        {
            var rectTransform = element.GetComponent<RectTransform>();
            RepositionElementByIndex(in rectTransform, index, offset);
        }
        public void RepositionElementByIndex(in RectTransform rectTransform, int index, float offset)
        {
            float transformHeight = marginBottomOffset + rectTransform.rect.height;

            Vector3 localPosition = rectTransform.localPosition;
            localPosition.y = -transformHeight * index + offset;
            rectTransform.localPosition = localPosition;
        }

    }
}
