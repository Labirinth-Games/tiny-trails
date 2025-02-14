using System.Collections.Generic;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.Generators
{
    public class SubZone
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public RoomType RoomType { get; set; }

        public Vector2Int CenterPoint
        {
            get
            {
                return new Vector2Int(InitialCellGrid.x + Width / 2, InitialCellGrid.y + Height / 2);
            }
        }
        public Vector2Int InitialCellGrid { get; set; }
        public Vector2Int FinalCellGrid
        {
            get
            {
                return new Vector2Int(InitialCellGrid.x + Width, InitialCellGrid.y + Height);
            }
        }
        public int AmountDoors { get; set; }
        public bool HasPlayer { get; set; }

        public TileLayer[,] TileLayersGrid { get; set; }

        /// <summary>
        /// Metodo busca um tile com posição relativa a zona.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public void SetTileAbsolutePosition(Vector2Int _relativePosition, Vector2Int _absolutePosition)
        {
            TileLayer _tileLayer = TileLayersGrid[_relativePosition.x, _relativePosition.y];

            _tileLayer.SetAbsolutePosition(_absolutePosition);
        }

        public TileLayer GetTileLayerByRelativePosition(Vector2Int relativePosition) => TileLayersGrid[relativePosition.x, relativePosition.y];

        public TileLayer GetTileLayerByAbsolutePosition(Vector2 absolutePosition)
        {
            TileLayer tileLayer = null;

            for (int x = 0; x < TileLayersGrid.GetLength(0); x++)
                for (int y = 0; y < TileLayersGrid.GetLength(0); y++)
                {
                    if (TileLayersGrid[x, y].GetAbsolutePosition() == absolutePosition)
                    {
                        return TileLayersGrid[x, y];
                    }
                }

            return tileLayer;
        }

        /// <summary>
        /// Adiciona um novo tile na camada da posição passada usando a posição relative
        /// </summary>
        /// <param name="_relativePosition"></param>
        /// <param name="_tileType"></param>
        public void AddTileLayer(Vector2Int _relativePosition, TileType _tileType) => TileLayersGrid[_relativePosition.x, _relativePosition.y].AddTile(_tileType);
        public void AddTileLayerDoor(Vector2Int _relativePosition, Vector2Int _direction) => TileLayersGrid[_relativePosition.x, _relativePosition.y].AddTileDoor(_direction);

        #region Utils
        /// <summary>
        /// Metodo para coletar uma posição aleátoria onde tenha pelo menos
        /// um chão no local
        /// </summary>
        /// <param name="ruleCallback"></param>
        /// <returns></returns>
        public Vector2Int GetRandomPosition()
        {
            int _attemps = 0;
            int _attempsLimit = 40;

            while (true)
            {
                if (_attemps > _attempsLimit) break;

                int _x = Random.Range(0, Width);
                int _y = Random.Range(0, Height);

                TileLayer _tileLayer = TileLayersGrid[_x, _y];

                if (_tileLayer.CanSpawn()) return _tileLayer.GetRelativePosition();

                _attemps++;
            }

            return Vector2Int.zero;
        }

        public (Vector2Int position, Vector2Int direction) GetRandomPositionEmptyInBorder()
        {
            // contolador de tentativas para coletar uma posiçao vazia
            int _attemps = 0;
            int _attempsLimit = 10;

            List<Vector2Int> _directions = new List<Vector2Int>() {
                Vector2Int.up,
                Vector2Int.down,
                Vector2Int.right,
                Vector2Int.left,
            };

            while (true)
            {
                if (_attemps > _attempsLimit) break;

                Vector2Int _direction = _directions[Random.Range(0, _directions.Count)];
                TileLayer _tileLayerBorder = null;

                int _x = Random.Range(1, Width - 1);
                int _y = Random.Range(1, Height - 1);

                if (_direction == Vector2Int.up)
                    _tileLayerBorder = TileLayersGrid[_x, Height - 1];
                else if (_direction == Vector2Int.down)
                    _tileLayerBorder = TileLayersGrid[_x, 0];
                else if (_direction == Vector2Int.left)
                    _tileLayerBorder = TileLayersGrid[0, _y];
                else if (_direction == Vector2Int.right)
                    _tileLayerBorder = TileLayersGrid[Width - 1, _y];

                if (_tileLayerBorder.HasFloor()) return (_tileLayerBorder.GetRelativePosition(), _direction * -1);

                _attemps++;
            }

            return (Vector2Int.one * -1, Vector2Int.one * -1);
        }
        #endregion
    }
}