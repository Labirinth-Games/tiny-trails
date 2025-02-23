using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace TinyTrails.UI
{
    public class MenuInitialItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] GameObject background;
        [SerializeField] TextMeshProUGUI label;

        [SerializeField] bool isBlocked;

        void Start()
        {
            Button button = gameObject.GetComponent<Button>();

            if (button != null) gameObject.GetComponent<Button>().interactable = !isBlocked;

            label.color = isBlocked ? new Color(1, 1, 1, .4f) : Color.white;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            background.SetActive(false);
            label.color = isBlocked ? new Color(1, 1, 1, .4f) : Color.white;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            background.SetActive(true);
            label.color = isBlocked ? new Color(0, 0, 0, .4f) : Color.black;
        }
    }
}