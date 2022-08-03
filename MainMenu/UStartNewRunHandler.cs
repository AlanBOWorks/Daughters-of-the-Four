using UnityEngine;
using UnityEngine.EventSystems;
using Utils_Project;

namespace MainMenu
{
    public class UStartNewRunHandler : MonoBehaviour, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            UtilsScene.LoadMainCharacterSelectionScene(true);
        }
    }
}
