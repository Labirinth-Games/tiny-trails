using TinyTrails.Managers;
using TinyTrails.Types;
using TMPro;
using UnityEngine;

namespace TinyTrails.UI
{
    public class DefenseMeditorUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;

        private void SetDefense(int value) => label.text = value.ToString();

        public void Init()
        {
            GameManager.Instance.EventManager.Subscriber<int>(EventChannelType.OnUIDefenseChange, SetDefense);
        }
    }
}