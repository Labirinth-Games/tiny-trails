using TinyTrails.SO;
using TinyTrails.UI;
using TinyTrails.World;
using UnityEngine;

namespace TinyTrails.Render
{
    public class ElementRender : MonoBehaviour
    {
        public static GameObject PrefabRender(GameObject prefab, Vector2 pos)
        {
            if (prefab == null) return null;

            var instance = Instantiate(prefab);
            instance.transform.position = (Vector3)pos;

            return instance;
        }

        public static GameObject ItemRender(ItemSO item, Vector2 pos)
        {
            var prefab = Resources.Load<GameObject>("World/item_prefab");

            if (prefab == null) Debug.LogWarning("failt to get \"World/item_prefab\"");

            return prefab;
        }
    }
}