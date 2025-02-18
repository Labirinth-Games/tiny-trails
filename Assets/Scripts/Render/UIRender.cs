using System.Collections.Generic;
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

        public static List<GameObject> HighLightAttackEnemyRender(List<Vector2> positions, Vector2 targetPosition)
        {
            var prefab = Resources.Load<GameObject>("UI/highlight_attack_enemy_prefab");
            List<GameObject> instances = new();

            foreach (var position in positions)
            {
                if (prefab == null) return null;

                var instance = Instantiate(prefab);

                HighLightUI highLightUI = instance.GetComponent<HighLightUI>();
                highLightUI.Init(position, null);

                if (position == targetPosition) highLightUI.GetOverlay().SetActive(true);

                instances.Add(instance);
            }

            return instances;
        }

        public static GameObject HitPushLabelUIRender(int damage, Vector2 position)
        {
            var prefab = Resources.Load<GameObject>("UI/hit_push_label_ui_prefab");

            if (prefab == null) return null;

            var instance = Instantiate(prefab);

            PushLabelUI hitUI = instance.GetComponent<PushLabelUI>();
            hitUI.Init(damage.ToString(), position);

            return instance;
        }

        public static GameObject FocusPushLabelUIRender(Vector2 position)
        {
            var prefab = Resources.Load<GameObject>("UI/focus_push_label_ui_prefab");

            if (prefab == null) return null;

            var instance = Instantiate(prefab);

            PushLabelUI hitUI = instance.GetComponent<PushLabelUI>();
            hitUI.Init("+", position);

            return instance;
        }

        public static GameObject HealthPushLabelUIRender(Vector2 position)
        {
            var prefab = Resources.Load<GameObject>("UI/heath_push_label_ui_prefab");

            if (prefab == null) return null;

            var instance = Instantiate(prefab);

            PushLabelUI hitUI = instance.GetComponent<PushLabelUI>();
            hitUI.Init("+", position);

            return instance;
        }

        public static GameObject DefensePushLabelUIRender(Vector2 position)
        {
            var prefab = Resources.Load<GameObject>("UI/defense_push_label_ui_prefab");

            if (prefab == null) return null;

            var instance = Instantiate(prefab);

            PushLabelUI hitUI = instance.GetComponent<PushLabelUI>();
            hitUI.Init("+", position);

            return instance;
        }
    }
}