using DG.Tweening;
using TinyTrails.DTO;
using TinyTrails.Managers;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.Behaviours
{
    public class TileBehaviour : MonoBehaviour
    {
        public Tile Tile { get; set; } = new();

        public void SetTileType(TileType tileType) => Tile.SetTileType(tileType);

        #region Actions
        public virtual void MoveTo(Vector2 pos)
        {
            bool canMove = GameManager.Instance.MapManager.MoveTile(transform.position, pos, Tile);

            if (!canMove) return;

            GetComponent<SpriteRenderer>().flipX = (pos.x - transform.position.x) < 0;
            transform.DOMove(pos, .15f);
        }
        public virtual void Hit(int damage)
        {
            GameManager.Instance.AudioManager.ActionAudio.Hit();
        }
        #endregion

        void OnDestroy()
        {
            GameManager.Instance.MapManager.Unregister(gameObject.transform.position, Tile);
        }
    }
}