using TinyTrails.Managers;
using TinyTrails.SO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TinyTrails.UI
{
    public class InventaryContainerItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField] Image preview;
        [SerializeField] GameObject descriptionContainer;
        [SerializeField] TextMeshProUGUI descriptionText;

        ItemSO _item;
        System.Action<ItemSO> OnSelectItem;

        public void Init(ItemSO item, System.Action<ItemSO> OnSelectItem)
        {
            _item = item;
            this.OnSelectItem = OnSelectItem;

            preview.sprite = item.sprite;
            descriptionText.text = item.desctiption;

            descriptionContainer.SetActive(false);
        }


        #region Events Mouse
        public void OnPointerEnter(PointerEventData eventData)
        {
            descriptionContainer.SetActive(true);
            descriptionContainer.transform.SetParent(transform.parent.parent);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            descriptionContainer.SetActive(false);
            descriptionContainer.transform.SetParent(transform);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnSelectItem?.Invoke(_item);
        }
        #endregion

        void OnDestroy()
        {
            Destroy(descriptionContainer);
        }
    }

}