using TinyTrails.Managers;
using TinyTrails.Types;
using TMPro;
using UnityEngine;

namespace TinyTrails.UI
{
    public class FocusMeditorUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;

        private void SetFocus(int value) => label.text = value.ToString();

        void Start()
        {
            GameManager.Instance.EventManager.Subscriber<int>(EventChannelType.OnUIFocusChange, SetFocus);
        }
    }
}