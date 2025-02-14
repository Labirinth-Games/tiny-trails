using System.Collections.Generic;
using TinyTrails.Characters;
using TinyTrails.DTO;
using TinyTrails.Helpers;
using TinyTrails.Managers;
using TinyTrails.Render;
using TinyTrails.Types;
using TinyTrails.UI;
using UnityEngine;

namespace TinyTrails.Actions
{

    public class MoveAction : MonoBehaviour
    {
        List<GameObject> _instances = new List<GameObject>();

        public void Move()
        {
            Player player = GameManager.Instance.Player;
            List<Vector2> positions = GameManager.Instance.MapManager.GetEmptyAround(player.transform.position, player.Stats.Movement);

            foreach (var position in positions)
            {
                _instances.Add(UIRender.HighLightMoveRender(position, OnSelectTile));
            }
        }

        void OnSelectTile(Vector2 position)
        {
            GameManager.Instance.EventManager.Publisher<Vector2>(EventChannelType.OnActionMove, position);
            GameManager.Instance.EventManager.Publisher<int>(EventChannelType.OnFocusReduce, GameManager.Instance.Settings.costFocusToMove);

            DestroyHighlight();
        }

        public void DestroyHighlight()
        {
            _instances.ForEach(f => Destroy(f));
            _instances.Clear();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.EventManager.Publisher<ActionPointType>(EventChannelType.OnActionFinish, ActionPointType.Move);
                DestroyHighlight();
            }
        }
    }
}