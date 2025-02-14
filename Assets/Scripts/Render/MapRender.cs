using System.Collections.Generic;
using TinyTrails.Behaviours;
using TinyTrails.DTO;
using TinyTrails.Enemies;
using TinyTrails.Generators;
using TinyTrails.Managers;
using TinyTrails.Types;
using TinyTrails.World;
using UnityEngine;

namespace TinyTrails.Render
{
    public class MapRender : MonoBehaviour
    {
        [SerializeField] private List<SpriteDirection> floorSprite;
        [SerializeField] private List<DoorDirection> doorPrefabs;
        [SerializeField] private List<SpriteDirection> wallSprites;
        [SerializeField] private List<SpriteDirection> borderSprites;

        private GameObject _container;
        List<GameObject> mapZoneinstances = new();

        public void Render(Zone zone)
        {
            _container = new GameObject();
            _container.name = "World::Container Map";

            GameObject _floorInstance = SpawnSprite(zone.FloorsPositions, _container.transform, floorSprite, "Floor", TileType.Floor);
            mapZoneinstances.Add(_floorInstance);

            GameObject _floorInstancew = SpawnSprite(zone.WayPositions, _container.transform, floorSprite, "Way", TileType.Way);
            mapZoneinstances.Add(_floorInstancew);

            SpawnSprite(zone.Walls, _container.transform, wallSprites, "Wall", TileType.Wall);

            mapZoneinstances.AddRange(DoorSpawnRender(zone, _container.transform));
            mapZoneinstances.AddRange(EnemySpawnRender(zone, _container.transform));
            mapZoneinstances.AddRange(TreasureSpawnRender(zone, _container.transform));
        }

        GameObject SpawnSprite(List<TileLayer> tileLayers, Transform _mapZoneContainer, List<SpriteDirection> sprites, string displayName, TileType tileType)
        {
            GameObject _container = new GameObject($"{displayName} Container");
            _container.transform.SetParent(_mapZoneContainer);

            foreach (TileLayer tileLayer in tileLayers)
            {
                GameObject instance = new GameObject(displayName, typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(TileInfoDebug), typeof(TileBehaviour));

                if (sprites.Count > 0)
                {
                    SpriteRenderer spriteRenderer = instance.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = sprites.Find(f => f.direction == tileLayer.WallDirection)?.sprite;
                    spriteRenderer.sortingLayerName = "Floor";
                    spriteRenderer.sortingOrder = (int)tileLayer.GetAbsolutePosition().y * -1;
                }

                BoxCollider2D boxCollider2D = instance.GetComponent<BoxCollider2D>();
                boxCollider2D.size = Vector2.one;

                instance.transform.position = (Vector2)tileLayer.GetAbsolutePosition();

                instance.transform.SetParent(_container.transform);

                TileBehaviour tileBehaviour = instance.GetComponent<TileBehaviour>();
                tileBehaviour.Tile.SetTileType(tileType);
                tileBehaviour.Tile.gameObject = tileBehaviour;

                GameManager.Instance.MapManager.Register(instance.transform.position, instance.GetComponent<TileBehaviour>().Tile);
            }

            return _container;
        }

        List<GameObject> EnemySpawnRender(Zone zone, Transform container)
        {
            List<GameObject> instances = new();
            List<TileLayer> tileLayers = zone.EnemyPositions;

            foreach (var tileLayer in tileLayers)
            {
                List<GameObject> enemies = GameManager.Instance.WorldManager.GetEnemies();

                GameObject instance = Instantiate(enemies[Random.Range(0, enemies.Count)], container);
                
                Enemy enemy = instance.GetComponent<Enemy>();
                enemy.Init(tileLayer.GetAbsolutePosition());

                GameManager.Instance.WorldManager.SetEnemyIA(enemy.Tile);

                instances.Add(instance);
            }

            return instances;
        }

        List<GameObject> TreasureSpawnRender(Zone zone, Transform container)
        {
            List<GameObject> instances = new();
            List<TileLayer> tileLayers = zone.TreasurePositions;

            foreach (var tileLayer in tileLayers)
            {
                GameObject chestPrefab = GameManager.Instance.WorldManager.GetChest();

                GameObject instance = Instantiate(chestPrefab, container);
                instance.transform.position = (Vector2)tileLayer.GetAbsolutePosition();

                instances.Add(instance);
            }

            return instances;
        }

        List<GameObject> DoorSpawnRender(Zone zone, Transform container)
        {
            List<GameObject> instances = new();
            List<TileLayer> tileLayers = zone.DoorPositions;

            foreach (var tileLayer in tileLayers)
            {
                if (tileLayer.DoorDirection == Vector2.zero) continue;

                GameObject instance = Instantiate(doorPrefabs.Find(f => f.direction == tileLayer.DoorDirection).prefab, container);
                instance.transform.position = (Vector2)tileLayer.GetAbsolutePosition();

                instances.Add(instance);
            }

            return instances;
        }
    }

    [System.Serializable]
    public class SpriteDirection
    {
        public Vector2Int direction;
        public Sprite sprite;
    }

    [System.Serializable]
    public class DoorDirection
    {
        public Vector2Int direction;
        public GameObject prefab;
    }
}