using TinyTrails.Behaviours;
using TinyTrails.DTO;
using TinyTrails.i18n;
using TinyTrails.Managers;
using TinyTrails.Types;

namespace TinyTrails.World
{
    public class Chest : TileBehaviour
    {
        // public override ContextActionPointDTO Action(ContextActionPointDTO context = null)
        // {
        //     GameManager.Instance.EventManager.Publisher<string>(EventChannelType.OnUILog, MessageGame.ITEM_OPEN_CHEST);

        //     // self.AddTile(TileType.Item);
        //     // var oldPrefab = self.gameObject;
        //     // var items = GameManager.Instance.WorldManager.GetItems();

        //     // var itemSo = items[Random.Range(0, items.Count)];
        //     // var prefab = ElementRender.ItemRender(itemSo, self.position);

        //     // var instance = self.CreateGameObject(prefab);
        //     // instance.GetComponent<SpriteRenderer>().sprite = itemSo.sprite;
        //     // instance.GetComponent<Item>().Init(itemSo);

        //     // Destroy(oldPrefab);

        //     return new ContextActionPointDTO() { isFinished = true, type = ActionPointType.Open };
        // }

        void Start()
        {
            Tile.SetTileType(TileType.Chest);
            Tile.gameObject = this;
            GameManager.Instance.MapManager.Register(transform.position, Tile);
        }
    }
}