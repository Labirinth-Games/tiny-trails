using System.Collections.Generic;
using TinyTrails.SO;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.Managers
{
    public class InventaryManager : MonoBehaviour
    {
        [SerializeField] List<ItemSO> items;

        // quando o player clica no item menu ele chama esse cara
        public void Use(ItemSO item)
        {
            items.Remove(item);

            switch (item.itemType)
            {
                case ItemType.Health:
                    GameManager.Instance.Player.SetHP(item.value);
                    break;
                case ItemType.Focus:
                    GameManager.Instance.Player.Stats.SetFocus(item.value);
                    break;
                case ItemType.Defense:
                    GameManager.Instance.Player.Stats.SetDefense(item.value);
                    break;
            }

            GameManager.Instance.EventManager.Publisher(EventChannelType.OnActionFinish);
        }

        #region Gets/Sets
        public void Add(ItemSO item) => items.Add(item);
        public List<ItemSO> GetItems() => items;
        #endregion
    }
}