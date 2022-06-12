using CombatSystem.Entity;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CombatSystem.Player.UI
{
    public class UTargetButton : MonoBehaviour, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler
    {
        private CombatEntity _user;
        private UFrontTargetButtonsHandler _holder;
        public void Inject(CombatEntity user)
        {
            _user = user;
        }
        public void Inject(UFrontTargetButtonsHandler holder)
        {
            _holder = holder;
        }

        public void ShowButton()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            // todo animate
            gameObject.SetActive(false);
        }

        public void HideInstantly()
        {
            gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _holder.DoTargetSelect(_user);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _holder.DoTargetButtonHover(_user);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _holder.DoTargetButtonExit(_user);
        }
    }
}
