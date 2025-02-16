using System.Collections.Generic;
using TinyTrails.DTO;
using TinyTrails.Enemies;
using TinyTrails.IA;
using TinyTrails.SO;
using UnityEngine;

namespace TinyTrails.Managers
{
    public class WorldManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> enemies;
        [SerializeField] private List<GameObject> bosses;
        [SerializeField] private List<ItemSO> items;
        [SerializeField] private GameObject chestPrefab;
        [SerializeField] private GameObject orbPrefab;
        [SerializeField] private WorldIA worldIA;

        #region Enemies
        public void EnemyRemove(Enemy enemy) => worldIA.EnemyRemove(enemy);
        public List<GameObject> GetEnemies() => enemies;
        public List<GameObject> GetBosses() => bosses;
        public void SetEnemyIA(Tile enemy) => worldIA.SetEnemy(enemy);
        #endregion

        public List<ItemSO> GetItems() => items;

        public GameObject GetChest() => chestPrefab;
        public GameObject GetOrb() => orbPrefab;

        #region Spawns

        public void SpawnBoss()
        {
            // var tile = GameManager.Instance.MapManager.GetTileRandom();
            // tile.CreateGameObject(bosses[Random.Range(0, bosses.Count)], null, TileType.Enemy);

            // worldIA.SetEnemy(tile);
        }
        #endregion
    }
}