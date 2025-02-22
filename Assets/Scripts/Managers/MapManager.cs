using System.Collections.Generic;
using System.Linq;
using TinyTrails.DTO;
using TinyTrails.Generators;
using TinyTrails.Types;
using TinyTrails.World;
using UnityEngine;

namespace TinyTrails.Managers
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] bool renderAllMap;
        public Zone Zone { get; private set; }

        public void CreateMap()
        {
            var map = new MapGenerator();
            var settings = GameManager.Instance.ZoneConfig;

            Zone = map.Build(settings);

            MapRender();
        }

        public void MapRender()
        {
            if (renderAllMap) Zone.LoadAllTilesPositions();
            else Zone.LoadTilesSubZonePositions();

            GameManager.Instance.MapRender.Render(Zone);
        }

        #region Registers
        public void Register(Vector2 pos, Tile tile)
        {
            if (Zone.Grid != null && IsInsideMap(pos))
            {
                TileLayer tileLayer = Zone.Grid[(int)pos.x, (int)pos.y];

                if (tileLayer.HasTile(tile.TileType))
                {
                    tileLayer.UpdateTile(tile.TileType, tile);
                    return;
                }

                Zone.Grid[(int)pos.x, (int)pos.y].AddTile(tile);
            }
        }

        public void Unregister(Vector2 pos, Tile tile)
        {
            if (!IsInsideMap(pos) && Zone.Grid[(int)pos.x, (int)pos.y].IsEmpty()) return;

            Zone.Grid[(int)pos.x, (int)pos.y].RemoveTile(tile.TileType);
        }

        #endregion

        #region Gets/Sets
        /// <summary>
        /// metodo responsavel por pegar posiçoes validando pela lista de tiles types e a magnitude que é distancia
        /// </summary>
        /// <param name="origin">posição original</param>
        /// <param name="magnitude">distancia do movimento de ataque</param>
        /// <param name="tileTypeValidate">tile types que sera validaddo, assim ignorando qualquer tile que n esteja na lista</param>
        /// <returns></returns>
        public List<Vector2> GetAround(Vector2 origin, int minDistance, int maxDistance, List<TileType> tileTypeValidate)
        {
            List<(Vector2 direction, bool canStop)> directions = new() {
                (Vector2.up, false),
                (Vector2.down, false),
                (Vector2.left, false),
                (Vector2.right, false),
                // diagonais
                (Vector2.up + Vector2.left, false),
                (Vector2.up + Vector2.right, false),
                (Vector2.down + Vector2.left, false),
                (Vector2.down + Vector2.right, false),
            };
            List<Vector2> dontCanMove = new();
            List<Vector2> positions = new List<Vector2>();

            for (var i = 1; i <= maxDistance; i++)
            {
                if (i <= minDistance) continue;

                foreach (var direction in directions)
                {
                    Vector2 position = origin + direction.direction * i;

                    if (!dontCanMove.Contains(direction.direction) && IsInsideMap(position))
                    {
                        var tile = Zone.Grid[(int)position.x, (int)position.y];

                        if (tile.HasTile(tileTypeValidate)) positions.Add(position);
                        else dontCanMove.Add(direction.direction);
                    }
                }
            }

            positions.Remove(origin);

            return positions;
        }

        public TileLayer GetTile(Vector2 pos) => IsInsideMap(pos) ? Zone.Grid[(int)pos.x, (int)pos.y] : null;

        public TileLayer GetTileLayerRandom() => Zone.GetCurrentSubZone().GetRandomTileLayer();

        public TileLayer[,] GetMapGrid() => Zone.Grid;
        #endregion

        #region Utils
        public bool MoveTile(Vector2 currentPos, Vector2 targetPos, Tile tile)
        {
            if (IsInsideMap(targetPos) && Zone.Grid[(int)targetPos.x, (int)targetPos.y].CanMove())
            {
                Zone.Grid[(int)currentPos.x, (int)currentPos.y].RemoveTile(tile);
                Zone.Grid[(int)targetPos.x, (int)targetPos.y].AddTile(tile);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Metodo de pathfinder com lista de tiles type para ser usada na validação
        /// </summary>
        /// <param name="originPosition"></param>
        /// <param name="destinyPosition"></param>
        /// <param name="tileTypeValidate"></param>
        /// <returns></returns>
        public List<Vector2> Pathfinder(Vector2 originPosition, Vector2 destinyPosition, List<TileType> tileTypeValidate, int limitMovement = -1)
        {
            Vector2 step = originPosition;
            Vector2 lastStep = originPosition;

            int attemps = 0;
            int attempsLimit = 300;

            List<Vector2> steps = new();

            while (true)
            {
                if (attemps > attempsLimit) break;

                // direction to move step
                List<(Vector2 direction, float distance)> directions = new List<(Vector2 direction, float distance)>() {
                    (step + Vector2.up, Vector2.Distance(destinyPosition, step + Vector2.up)),
                    (step + Vector2.down, Vector2.Distance(destinyPosition, step + Vector2.down)),
                    (step + Vector2.left, Vector2.Distance(destinyPosition, step + Vector2.left)),
                    (step + Vector2.right, Vector2.Distance(destinyPosition, step + Vector2.right)),
                    // diagonal
                    (step + Vector2.up + Vector2.right, Vector2.Distance(destinyPosition, step + Vector2.up + Vector2.right)),
                    (step + Vector2.up + Vector2.left, Vector2.Distance(destinyPosition, step + Vector2.up + Vector2.left)),
                    (step + Vector2.down + Vector2.right, Vector2.Distance(destinyPosition, step + Vector2.down + Vector2.right)),
                    (step + Vector2.down + Vector2.left, Vector2.Distance(destinyPosition, step + Vector2.down + Vector2.left)),
                };

                // filtrando das direcoes possiveis quais que podem ser usadas
                var directionsWithoutObstacules = directions.FindAll(tileDirection =>
                {
                    if (!IsInsideMap(tileDirection.direction)) return false;

                    TileLayer tile = Zone.Grid[(int)tileDirection.direction.x, (int)tileDirection.direction.y];

                    // n pode passar por sima desses tiles
                    if (tile.HasTile(new List<TileType>() { TileType.Player, TileType.Enemy, TileType.Chest })) return false;

                    return tile.HasTile(tileTypeValidate);
                });

                // buscando o menor caminho entre as conhecidas sem obstaculos
                lastStep = step;
                step = directionsWithoutObstacules.Find(_direction => _direction.distance == directionsWithoutObstacules.Min(_directionMin => _directionMin.distance)).direction;

                TileLayer tileLayer = Zone.Grid[(int)step.x, (int)step.y];

                if (steps.Contains(tileLayer.GetAbsolutePosition()))
                {
                    lastStep = step;
                    attemps++;
                    continue;
                }

                // quando chega no destino ele para de fazer o pathfinder
                if (step == destinyPosition || limitMovement == steps.Count)
                {
                    steps.Add(tileLayer.GetAbsolutePosition());
                    break;
                }

                // adicionando os caminhos
                steps.Add(tileLayer.GetAbsolutePosition());
                attemps++;
            }

            return steps;
        }
        #endregion

        #region Validations
        public bool IsInsideMap(Vector2 pos) => pos.x >= 0 && pos.x < Zone.Grid.GetLength(0) && pos.y >= 0 && pos.y < Zone.Grid.GetLength(1);
        #endregion
    }
}