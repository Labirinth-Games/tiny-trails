using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private Sprite floorSprite;
        [SerializeField] private List<SpriteDirection> trapFloorSprite;
        [SerializeField] private GameObject doorPrefab;
        [SerializeField] private List<SpriteDirection> wallSprites;
        [SerializeField] private List<SpriteDirection> borderSprites;

        private GameObject _container;
        List<GameObject> _mapZoneinstances = new();

        public void Render(Zone zone)
        {
            _container = new GameObject();
            _container.name = "World::Container Map";

            _mapZoneinstances.Add(FloowSpawn(zone.FloorsPositions, _container.transform));
            _mapZoneinstances.Add(FloowSpawn(zone.WayPositions, _container.transform));

            _mapZoneinstances.AddRange(WallSpawn(zone, _container.transform));

            _mapZoneinstances.AddRange(DoorSpawnRender(zone, _container.transform));
            _mapZoneinstances.AddRange(EnemySpawnRender(zone, _container.transform));
            _mapZoneinstances.AddRange(TreasureSpawnRender(zone, _container.transform));
            _mapZoneinstances.AddRange(TrapSpawnRender(zone, _container.transform));
            _mapZoneinstances.Add(BossSpawnRender(zone, _container.transform));
        }

        List<GameObject> WallSpawn(Zone zone, Transform _mapZoneContainer)
        {
            List<GameObject> instances = new();
            GameObject _container = new GameObject($"Wall Container");
            _container.transform.SetParent(_mapZoneContainer);

            List<SpriteDirection> sprites = new();
            sprites.AddRange(wallSprites);
            sprites.AddRange(borderSprites);

            // clear isntances duplicatas positions

            // TODO - possivel gap de memoria aqui, abordagem usada 
            // para remover tiles que são sobresposto por outros 
            // quando uma subzona nova é aberta
            _mapZoneinstances.ForEach(instance =>
            {
                if (instance == null) return;

                if (zone.WallPositions.Exists(wall => (Vector2)instance.transform.position == (Vector2)wall.GetAbsolutePosition()))
                {
                    Destroy(instance);
                }
            });


            foreach (TileLayer tileLayer in zone.WallPositions)
            {

                GameObject instance = new GameObject("Wall", typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(TileInfoDebug), typeof(TileBehaviour));

                if (sprites.Count > 0)
                {
                    SpriteRenderer spriteRenderer = instance.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = sprites.Find(f => f.direction == tileLayer.WallDirection)?.sprite;
                    spriteRenderer.sortingLayerName = "Floor";
                }

                BoxCollider2D boxCollider2D = instance.GetComponent<BoxCollider2D>();
                boxCollider2D.size = Vector2.one;

                instance.transform.position = (Vector2)tileLayer.GetAbsolutePosition();

                instance.transform.SetParent(_container.transform);

                TileBehaviour tileBehaviour = instance.GetComponent<TileBehaviour>();
                tileBehaviour.Tile.SetTileType(TileType.Wall);
                tileBehaviour.Tile.gameObject = tileBehaviour;

                GameManager.Instance.MapManager.Register(instance.transform.position, instance.GetComponent<TileBehaviour>().Tile);

                instances.Add(instance);
            }

            return instances;
        }


        GameObject FloowSpawn(List<TileLayer> tileLayers, Transform _mapZoneContainer)
        {
            GameObject _container = new GameObject($"Floor Container");
            _container.transform.SetParent(_mapZoneContainer);

            foreach (TileLayer tileLayer in tileLayers)
            {
                GameObject instance = new GameObject("Floor", typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(TileInfoDebug), typeof(TileBehaviour));

                if (floorSprite != null)
                {
                    SpriteRenderer spriteRenderer = instance.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = floorSprite;
                    spriteRenderer.sortingLayerName = "Floor";
                }

                BoxCollider2D boxCollider2D = instance.GetComponent<BoxCollider2D>();
                boxCollider2D.size = Vector2.one;

                instance.transform.position = (Vector2)tileLayer.GetAbsolutePosition();

                instance.transform.SetParent(_container.transform);

                TileBehaviour tileBehaviour = instance.GetComponent<TileBehaviour>();
                tileBehaviour.Tile.SetTileType(TileType.Floor);
                tileBehaviour.Tile.gameObject = tileBehaviour;

                GameManager.Instance.MapManager.Register(instance.transform.position, instance.GetComponent<TileBehaviour>().Tile);
            }

            return _container;
        }


        List<GameObject> EnemySpawnRender(Zone zone, Transform container)
        {
            List<GameObject> instances = new();
            List<TileLayer> tileLayers = zone.EnemyPositions;
            int i = 1;

            foreach (var tileLayer in tileLayers)
            {
                List<GameObject> enemies = GameManager.Instance.WorldManager.GetEnemies();

                GameObject instance = Instantiate(enemies[Random.Range(0, enemies.Count)], container);

                Enemy enemy = instance.GetComponent<Enemy>();
                enemy.Init(tileLayer.GetAbsolutePosition());

                GameManager.Instance.WorldManager.SetEnemyIA(enemy.Tile);

                instance.name = $"{enemy.Stats.displayName} {i}";
                instances.Add(instance);

                i++;
            }

            return instances;
        }

        GameObject BossSpawnRender(Zone zone, Transform container)
        {
            if (zone.BossPosition == null) return null;

            List<GameObject> bosses = GameManager.Instance.WorldManager.GetBosses();

            GameObject instance = Instantiate(bosses[Random.Range(0, bosses.Count)], container);

            Enemy enemy = instance.GetComponent<Enemy>();
            enemy.Init(zone.BossPosition.GetAbsolutePosition());

            GameManager.Instance.WorldManager.SetEnemyIA(enemy.Tile);

            return instance;
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

        List<GameObject> TrapSpawnRender(Zone zone, Transform container)
        {
            if (zone.TrapsPositions.Count == 0) return new();

            List<GameObject> instances = new();
            List<TileLayer> trapPositions = zone.TrapsPositions;

            TileLayer orbTileLayer = zone.GetCurrentSubZone().GetRandomTileLayer(1);
            GameObject orbPrefab = GameManager.Instance.WorldManager.GetOrb();

            GameObject orb = Instantiate(orbPrefab, container);
            orb.transform.position = (Vector2)orbTileLayer.GetAbsolutePosition();
            instances.Add(orb);

            foreach (var tileLayer in trapPositions)
            {
                GameObject trapTriggerPrefab = Resources.Load<GameObject>("World/trap_trigger_prefab");

                GameObject instance = Instantiate(trapTriggerPrefab, container);
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
                if (tileLayer.DoorDirection == DirectionType.None) continue;

                GameObject instance = Instantiate(doorPrefab, container);
                Door door = instance.GetComponent<Door>();
                instance.transform.position = (Vector2)tileLayer.GetAbsolutePosition();

                door.Init(tileLayer.DoorDirection, tileLayer.DoorNextSubZone.RoomType == RoomType.Boss);

                instances.Add(instance);
            }

            return instances;
        }
    }

    [System.Serializable]
    public class SpriteDirection
    {
        public DirectionType direction;
        public Sprite sprite;
    }

    [System.Serializable]
    public class DoorDirection
    {
        public DirectionType direction;
        public GameObject prefab;
    }
}