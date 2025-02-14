using System.Collections.Generic;
using TinyTrails.Characters;
using TinyTrails.DTO;
using TinyTrails.Managers;
using TinyTrails.Render;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.Actions
{
    public class AttackAction : MonoBehaviour
    {
        private List<GameObject> _instances = new List<GameObject>();
        Player _player;

        public void Attack()
        {
            _player = GameManager.Instance.Player;
            List<Vector2> positions = GameManager.Instance.MapManager.GetAttackAround(_player.transform.position, _player.Stats.DistanceAttack);

            foreach (var position in positions)
            {
                _instances.Add(UIRender.HighLightAttackRender(position, OnSelectTile));
            }
        }

        void OnSelectTile(Vector2 position)
        {
            var tileLayer = GameManager.Instance.MapManager.GetTile(position);
            Tile tile = tileLayer.GetTile(TileType.Enemy);

            if (tile == null)
            {
                GameManager.Instance.EventManager.Publisher<Vector2>(EventChannelType.OnActionAttack, position);
                DestroyHighlight();
                return;
            }

            // mudando o contexto do jogo para modo batalha
            GameManager.Instance.EventManager.Publisher<ContextGameType>(EventChannelType.OnContextGameChangeStatus, ContextGameType.Battle);

            tile.gameObject.Hit(_player.Stats.Damage);

            GameManager.Instance.EventManager.Publisher<Vector2>(EventChannelType.OnActionAttack, position);
            GameManager.Instance.EventManager.Publisher<int>(EventChannelType.OnFocusReduce, GameManager.Instance.Settings.costFocusToAttack);

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
                GameManager.Instance.EventManager.Publisher<ActionPointType>(EventChannelType.OnActionFinish, ActionPointType.Attack);
                DestroyHighlight();
            }
        }
    }
}