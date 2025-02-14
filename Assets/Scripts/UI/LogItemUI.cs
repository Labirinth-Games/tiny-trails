using TinyTrails.Managers;
using TMPro;
using UnityEngine;

namespace TinyTrails.UI
{
    public class LogItemUI : MonoBehaviour
    {
        public void SetMessage(string value)
        {
            GetComponent<TextMeshProUGUI>().text = value;

            Destroy(gameObject, 5);
        }
    }
}