using TinyTrails.Managers;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.UI
{
    public class CancelHighlightUI : MonoBehaviour
    {
        [SerializeField] GameObject button;

        public void CancelHightlightButton()
        {
            GameManager.Instance.EventManager.Publisher<ActionPointType>(EventChannelType.OnActionFinish, ActionPointType.None);
            button.SetActive(false);
        }

        void Start()
        {
            GameManager.Instance.EventManager.Subscriber(EventChannelType.OnUIHighlightOpen, () => button.SetActive(true));

            GameManager.Instance.EventManager.Subscriber<ActionPointType>(EventChannelType.OnActionFinish, (actionPointType) => button.SetActive(false));
        }
    }
}