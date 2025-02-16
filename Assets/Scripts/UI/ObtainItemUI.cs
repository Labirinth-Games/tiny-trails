using TinyTrails.Managers;
using TinyTrails.SO;
using TinyTrails.Types;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TinyTrails.UI
{
    public class ObtainItemUI : MonoBehaviour
    {
        [SerializeField] GameObject display;
        [SerializeField] Image preview;
        [SerializeField] TextMeshProUGUI description;
        [SerializeField] TextMeshProUGUI title;

        public void Init()
        {
            GameManager.Instance.EventManager.Subscriber<ItemSO>(EventChannelType.OnChestObtainItem, OnObtainItem);
        }

        #region Events
        void OnObtainItem(ItemSO item)
        {
            display.SetActive(true);

            title.text = item.displayName;
            description.text = item.desctiption;
            preview.sprite = item.sprite;
        }
        #endregion

        #region Buttons
        public void CloseUIButton()
        {
            display.SetActive(false);
            GameManager.Instance.EventManager.Publisher(EventChannelType.OnChestObtainItemClose);
        }
        #endregion
    }
}