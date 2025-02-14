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
            Zone.LoadTilesSubZonePositions();
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
        public List<Vector2> GetEmptyAround(Vector2 origin, int magnitude)
        {
            var initialPointX = origin.x - magnitude;
            var finalPointX = origin.x + magnitude;
            var initialPointY = origin.y - magnitude;
            var finalPointY = origin.y + magnitude;

            List<Vector2> positions = new List<Vector2>();

            for (var y = initialPointY; y <= finalPointY; y++)
            {
                for (var x = initialPointX; x <= finalPointX; x++)
                {
                    if (IsInsideMap(new Vector2(x, y)))
                    {
                        var tile = Zone.Grid[(int)x, (int)y];

                        if (tile.HasFloor()) positions.Add(new Vector2(x, y));
                    }
                }
            }

            positions.Remove(origin);

            return positions;
        }

        /// <summary>
        /// metodo responsavel por pegar todos os tiles de um
        /// tipo especifico a sua volta baseado na magnitude(distancia de busca)
        /// </summary>
        /// <param name="tileType"></param>
        /// <param name="origin"></param>
        /// <param name="magnitude"></param>
        /// <returns></returns>
        public List<Tile> GetTileTypeAround(TileType tileType, Vector2 origin, int magnitude)
        {
            var initialPointX = origin.x - magnitude;
            var finalPointX = origin.x + magnitude;
            var initialPointY = origin.y - magnitude;
            var finalPointY = origin.y + magnitude;

            List<Tile> positions = new List<Tile>();

            for (var y = initialPointY; y <= finalPointY; y++)
            {
                for (var x = initialPointX; x <= finalPointX; x++)
                {
                    if (IsInsideMap(new Vector2(x, y)))
                    {
                        TileLayer tileLayer = Zone.Grid[(int)x, (int)y];

                        if (tileLayer.HasTile(tileType)) positions.Add(tileLayer.GetTile(tileType));
                    }
                }
            }

            return positions;
        }

        public List<Vector2> GetAttackAround(Vector2 origin, int magnitude)
        {
            var initialPointX = origin.x - magnitude;
            var finalPointX = origin.x + magnitude;
            var initialPointY = origin.y - magnitude;
            var finalPointY = origin.y + magnitude;

            List<Vector2> positions = new List<Vector2>();

            for (var y = initialPointY; y <= finalPointY; y++)
            {
                for (var x = initialPointX; x <= finalPointX; x++)
                {
                    if (IsInsideMap(new Vector2(x, y)))
                    {
                        var tile = Zone.Grid[(int)x, (int)y];

                        if (tile.CanAttack()) positions.Add(new Vector2(x, y));
                    }
                }
            }

            positions.Remove(origin);

            return positions;
        }

        public TileLayer GetTile(Vector2 pos) => IsInsideMap(pos) ? Zone.Grid[(int)pos.x, (int)pos.y] : null;

        public TileLayer GetTileRandom()
        {
            if (Zone.Grid is null) return null;

            TileLayer tile;

            while (true)
            {
                var x = Random.Range(0, Zone.Grid.GetLength(1));
                var y = Random.Range(0, Zone.Grid.GetLength(0));

                var tileEmpty = GetTile(new Vector2(x, y));

                if (tileEmpty != null && tileEmpty.IsEmpty())
                {
                    tile = tileEmpty;
                    break;
                }
            }

            return tile;
        }

        public TileLayer[,] GetMapGrid() => Zone.Grid;
        #endregion

        #region Utils
        public bool MoveTile(Vector2 currentPos, Vector2 targetPos, Tile tile)
        {
            if (IsInsideMap(targetPos) && Zone.Grid[(int)targetPos.x, (int)targetPos.y].HasFloor())
            {
                Zone.Grid[(int)currentPos.x, (int)currentPos.y].RemoveTile(tile);
                Zone.Grid[(int)targetPos.x, (int)targetPos.y].AddTile(tile);

                return true;
            }

            return false;
        }

        public List<Vector2> Pathfinder(Vector2 originPosition, Vector2 destinyPosition)
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
                };

                // filtrando das direcoes possiveis quais que podem ser usadas
                List<(Vector2 direction, float distance)> directionsWithoutObstacules = new();

                foreach (var direction in directions)
                {
                    if (!IsInsideMap(direction.direction)) continue;

                    TileLayer tileLayerDir = Zone.Grid[(int)direction.direction.x, (int)direction.direction.y];

                    if (tileLayerDir.HasFloor()) directionsWithoutObstacules.Add(direction);
                }

                // buscando o menor caminho entre as conhecidas sem obstaculos
                lastStep = step;
                step = directionsWithoutObstacules.Find(_direction => _direction.distance == directions.Min(_directionMin => _directionMin.distance)).direction;

                TileLayer tileLayer = Zone.Grid[(int)step.x, (int)step.y];

                // quando chega no destino ele para de fazer o pathfinder
                if (step == destinyPosition)
                {
                    steps.Add(tileLayer.GetAbsolutePosition());
                    break;
                }

                // adicionando os caminhos
                if (tileLayer.HasFloor())
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