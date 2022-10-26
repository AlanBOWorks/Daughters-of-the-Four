using CombatSystem.Entity;
using CombatSystem.Player.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ExplorationSystem.UI
{
    public class UExplorationTeamMemberElement : MonoBehaviour, 
        IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private UHealthInfo healthInfo;

        private UExplorationTeamWindowHandler _mainHandler;
        private PlayerRunTimeEntity _entity;
        public void Injection(UExplorationTeamWindowHandler handler)
        {
            _mainHandler = handler;
        }

        public void Injection(PlayerRunTimeEntity entityProvider)
        {
            _entity = entityProvider;
        }

        public void UpdateHealth()
        {
            healthInfo.UpdateHealth(_entity,_entity);
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            _mainHandler.ShowSkillList(_entity);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }
    }
}
