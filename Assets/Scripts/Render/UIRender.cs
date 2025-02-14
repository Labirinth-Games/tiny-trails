using TinyTrails.UI;
using UnityEngine;

namespace TinyTrails.Render
{
    public class UIRender : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="OnSelect">callback quando é clicado a zone</param>
        /// <returns></returns>
        public static GameObject HighLightMoveRender(Vector2 pos, System.Action<Vector2> OnSelect)
        {
            var prefab = Resources.Load<GameObject>("UI/highlight_move_prefab");

            if (prefab == null) return null;

            var instance = Instantiate(prefab);
            HighLightUI highLightUI = instance.GetComponent<HighLightUI>();
            highLightUI.Init(pos, OnSelect);

            return instance;
        }

        public static GameObject HighLightAttackRender(Vector2 pos, System.Action<Vector2> OnSelect)
        {
            var prefab = Resources.Load<GameObject>("UI/highlight_attack_prefab");

            if (prefab == null) return null;

            var instance = Instantiate(prefab);

            HighLightUI highLightUI = instance.GetComponent<HighLightUI>();
            highLightUI.Init(pos, OnSelect);

            return instance;
        }

        public static GameObject HitUIRender(int damage, Vector2 position)
        {
            var prefab = Resources.Load<GameObject>("UI/hit_ui_prefab");

            if (prefab == null) return null;

            var instance = Instantiate(prefab);

            HitUI hitUI = instance.GetComponent<HitUI>();
            hitUI.Init(damage, position);

            return instance;
        }
    }
}