using TinyTrails.Behaviours;
using TinyTrails.Characters;
using TinyTrails.DTO;
using TinyTrails.Managers;
using TinyTrails.SO;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.World
{
    public class Item : TileBehaviour
    {
        private ItemSO _stats;

        // public override ContextActionPointDTO Action(ContextActionPointDTO context = null)
        // {
        //     switch (_stats.type)
        //     {
        //         case Types.ItemType.Health:
        //             GameManager.Instance.Player.SetHP(_stats.value);
        //             break;
        //         case Types.ItemType.Strength:
        //             GameManager.Instance.Player.SetAdditionalStrength(_stats.value);
        //             break;
        //     }

        //     Destroy(gameObject, 1);
        //     return new ContextActionPointDTO() { isFinished = true, type = Types.ActionPointType.UseItem };
        // }

        public void Init(ItemSO stats)
        {
            _stats = Instantiate(stats);
        }

        void Start()
        {
            Tile.SetTileType(TileType.Item);
            Tile.gameObject = this;
            GameManager.Instance.MapManager.Register(transform.position, Tile);
        }
    }
}