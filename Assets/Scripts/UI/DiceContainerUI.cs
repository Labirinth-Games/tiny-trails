using System.Threading.Tasks;
using TinyTrails.Managers;
using TinyTrails.Types;
using TMPro;
using UnityEngine;

namespace TinyTrails.UI
{
    public class DiceContainerUI : MonoBehaviour
    {
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private GameObject containerPrefab;

        public void OnNotification(int value)
        {
            var instance = Instantiate(itemPrefab);
            instance.transform.SetParent(containerPrefab.transform);
            instance.GetComponent<DiceItemUI>().SetValue(value);
        }

        public void Init()
        {
            GameManager.Instance.EventManager.Subscriber<int>(EventChannelType.OnUIRollDiceNotification, OnNotification);
        }
    }
}