using TinyTrails.Managers;
using TinyTrails.Types;
using TMPro;
using UnityEngine;

namespace TinyTrails.UI
{
    public class HPMeditorUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;

        private void SetHP(int value) => label.text = value.ToString();

        public void Init()
        {
            GameManager.Instance.EventManager.Subscriber<int>(EventChannelType.OnUIHPChange, SetHP);
        }
    }
}