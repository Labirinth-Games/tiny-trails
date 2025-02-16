using TinyTrails.Behaviours;
using TinyTrails.Characters;
using TinyTrails.Managers;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.World
{
    public class TrapTrigger : TileBehaviour
    {
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                GameManager.Instance.EventManager.Publisher(EventChannelType.OnTrapTriggerActive);
            }
        }

        void Start()
        {
            Tile.SetTileType(TileType.Trap);
            Tile.gameObject = this;
            GameManager.Instance.MapManager.Register(transform.position, Tile);
        }
    }
}