using TinyTrails.Managers;
using TinyTrails.Types;
using TMPro;
using UnityEngine;

namespace TinyTrails.UI
{
    public class LogUI : MonoBehaviour
    {
        [SerializeField] private GameObject textPrefab;

        private void SetMessage(string value) {
            var instance = Instantiate(textPrefab);
            instance.transform.SetParent(transform);

            instance.GetComponent<LogItemUI>().SetMessage(value);
        }

        public void Init()
        {
            GameManager.Instance.EventManager.Subscriber<string>(EventChannelType.OnUILog, SetMessage);
        }
    }
}