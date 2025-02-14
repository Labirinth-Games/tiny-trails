using TinyTrails.Managers;
using TMPro;
using UnityEngine;

namespace TinyTrails.UI
{
    public class ActionPointUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;

        private void SetActionPoint(int value) => label.text = value.ToString();

        void Start()
        {
            SetActionPoint(3);
            GameManager.Instance.EventManager.Subscriber<int>(Types.EventChannelType.OnUIRemainActionPointsChange, SetActionPoint);
        }
    }
}