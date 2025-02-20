using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
namespace TinyTrails.UI
{
    public class MenuInitialItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] GameObject background;
        [SerializeField] TextMeshProUGUI label;

        public void OnPointerExit(PointerEventData eventData)
        {
            background.SetActive(false);
            label.color = Color.white;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            background.SetActive(true);
            label.color = Color.black;
        }
    }
}