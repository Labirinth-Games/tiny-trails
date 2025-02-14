using System.Collections.Generic;
using System.Threading.Tasks;
using TinyTrails.DTO;
using TinyTrails.Enemies;
using TinyTrails.Generators;
using TinyTrails.Helpers;
using TinyTrails.i18n;
using TinyTrails.IA;
using TinyTrails.Render;
using TinyTrails.SO;
using TinyTrails.Types;
using TinyTrails.World;
using UnityEngine;

namespace TinyTrails.Managers
{
    public class WorldManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> enemies;
        [SerializeField] private List<GameObject> bosses;
        [SerializeField] private List<ItemSO> items;
        [SerializeField] private GameObject chestPrefab;
        [SerializeField] private WorldIA worldIA;

        private ExploreElementType[] _exploreElementsTypes;
        private int _limitExploreMap;

        #region Enemies
        public void EnemyRemove(Enemy enemy) => worldIA.EnemyRemove(enemy);
        public List<GameObject> GetEnemies() => enemies;
        public void SetEnemyIA(Tile enemy) => worldIA.SetEnemy(enemy);
        #endregion

        public List<ItemSO> GetItems() => items;

        public GameObject GetChest() => chestPrefab;

        public async void ExploreAround(Vector2 position)
        {
            _limitExploreMap--;

            if (_limitExploreMap <= 0)
            {
                GameManager.Instance.EventManager.Publisher<string>(EventChannelType.OnUILog, MessageGame.EXPLORE_NOT_CAN_MORE_SEARCHING);
                return;
            }

            GameManager.Instance.EventManager.Publisher<string>(EventChannelType.OnUILog, MessageGame.EXPLORE_SEARCHING(_limitExploreMap));
            await Task.Delay(500);

            // roll type element
            var elementType = _exploreElementsTypes[DiceHelper.Roll() - 1];

            // roll position x and y
            TileLayer tileEmpty;

            while (true)
            {
                var x = DiceHelper.Roll(DiceType.D4, false) + position.x - 2;
                var y = DiceHelper.Roll(DiceType.D4, false) + position.y - 2;

                tileEmpty = GameManager.Instance.MapManager.GetTile(new Vector2(x, y));

                if (tileEmpty.IsEmpty()) break;
            }

            if (elementType == ExploreElementType.Enemy)
            {
                // var tile = new Tile(TileType.Enemy, tileEmpty.position);
                // tile.CreateGameObject(enemies[Random.Range(0, enemies.Count)]);

                // GameManager.Instance.EventManager.Publisher<string>(EventChannelType.OnLogUI, MessageGame.EXPLORE_ENEMY_APPEARED);

                return;
            }

            if (elementType == ExploreElementType.Items)
            {
                // var tile = new Tile(TileType.Item, tileEmpty.position);
                // var itemSo = items[Random.Range(0, items.Count)];
                // var prefab = ElementRender.ItemRender(itemSo, tileEmpty.position);

                // var instance = tile.CreateGameObject(prefab);
                // instance.GetComponent<SpriteRenderer>().sprite = itemSo.sprite;
                // instance.GetComponent<Item>().Init(itemSo);

                // GameManager.Instance.EventManager.Publisher<string>(EventChannelType.OnLogUI, MessageGame.EXPLORE_ITEM_FOUNDED);

                // return;
            }

            GameManager.Instance.EventManager.Publisher<string>(EventChannelType.OnUILog, MessageGame.EXPLORE_NOT_FOUND);
        }

        #region Spawns
        // public void SpawnMonsters(Zone zone)
        // {
        //     // calc num monsters based size map and difficulty
        //     var settings = GameManager.Instance.Settings;
        //     int amountMonsters = settings.AmountEnemiesByDifficulty.Find(f => f.difficulty == settings.difficulty).amountEnemies;

        //     for (var i = 0; i < amountMonsters; i++)
        //     {
        //         // var tile = GameManager.Instance.MapManager.GetTileRandom();
        //         // tile.CreateGameObject(enemies[Random.Range(0, enemies.Count)], null, TileType.Enemy);

        //         // worldIA.SetEnemy(tile);
        //     }
        // }

        public void SpawnChests()
        {
            for (var i = 0; i < 3; i++)
            {
                // var tile = GameManager.Instance.MapManager.GetTileRandom();
                // tile.CreateGameObject(chestPrefab, null, TileType.Chest);
            }
        }

        public void SpawnBoss()
        {
            // var tile = GameManager.Instance.MapManager.GetTileRandom();
            // tile.CreateGameObject(bosses[Random.Range(0, bosses.Count)], null, TileType.Enemy);

            // worldIA.SetEnemy(tile);
        }
        #endregion

        private void Start()
        {
            _limitExploreMap = GameManager.Instance.Settings.limitToExploreMap;

            _exploreElementsTypes = new ExploreElementType[] {
                ExploreElementType.None,
                ExploreElementType.Items,
                ExploreElementType.Enemy,
                ExploreElementType.None,
            };
        }
    }
}