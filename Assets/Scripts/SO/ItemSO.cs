using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.SO
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
    public class ItemSO : ScriptableObject
    {
        public string displayName;
        public Sprite sprite;
        [TextArea]
        public string desctiption;
        public ItemType itemType;
        public int value;

    }
}