using System.Collections.Generic;
using TinyTrails.Managers;
using TinyTrails.SO;
using UnityEngine;

namespace TinyTrails.UI
{
    public class InventaryContainerUI : MonoBehaviour
    {
        [SerializeField] GameObject display;
        [SerializeField] GameObject containerItems;
        [SerializeField] GameObject itemPrefab;

        List<GameObject> _instances = new();

        #region Button
        public void OpenButton()
        {
            display.SetActive(true);

            foreach (ItemSO item in GameManager.Instance.InventaryManager.GetItems())
            {
                GameObject instance = Instantiate(itemPrefab, containerItems.transform);
                instance.GetComponent<InventaryContainerItemUI>().Init(item, OnSelectItem);

                _instances.Add(instance);
            }
        }

        public void CloseButton()
        {
            _instances.ForEach(f => Destroy(f));
            _instances.Clear();

            display.SetActive(false);
        }
        #endregion

        #region Events
        void OnSelectItem(ItemSO item)
        {
            GameManager.Instance.InventaryManager.Use(item);
            GameManager.Instance.ActionPointController.ItemActionButton();

            _instances.ForEach(f => Destroy(f));
            _instances.Clear();

            display.SetActive(false);
        }
        #endregion
    }

}