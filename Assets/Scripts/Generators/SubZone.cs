using System.Collections.Generic;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.Generators
{
    public class SubZone
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2Int CenterPointAbsolute { get; set; }
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

        /// <summary>
        /// Adiciona um novo tile na camada da posição passada usando a posição relative
        /// </summary>
        /// <param name="relativePosition"></param>
        /// <param name="tileType"></param>
        public void AddTileLayer(Vector2Int relativePosition, TileType tileType) => TileLayersGrid[relativePosition.x, relativePosition.y].AddTile(tileType);
        public void AddTileLayerDoor(Vector2Int relativePosition, DirectionType direction) => TileLayersGrid[relativePosition.x, relativePosition.y].AddTileDoor(direction);

        #region Utils

        /// <summary>
        /// Metodo para coletar uma posição aleátoria onde tenha pelo menos
        /// um chão no local
        /// </summary>
        /// <param name="distanceToBorder">quantidade de quadrados de distancia da borda da zona a posisao será gerada</param>
        /// <returns></returns>
        public Vector2Int GetRandomPosition(int distanceToBorder = 0)
        {
            int attemps = 0;
            int attempsLimit = 40;

            while (true)
            {
                if (attemps > attempsLimit) break;

                int x = Random.Range(distanceToBorder, Width - distanceToBorder);
                int y = Random.Range(distanceToBorder, Height - distanceToBorder);

                TileLayer tileLayer = TileLayersGrid[x, y];

                if (tileLayer.CanSpawn()) return tileLayer.GetRelativePosition();

                attemps++;
            }

            return Vector2Int.zero;
        }

        public TileLayer GetRandomTileLayer(int distanceToBorder = 0)
        {
            int attemps = 0;
            int attempsLimit = 40;

            while (true)
            {
                if (attemps > attempsLimit) break;

                int x = Random.Range(distanceToBorder, Width - distanceToBorder);
                int y = Random.Range(distanceToBorder, Height - distanceToBorder);

                TileLayer tileLayer = TileLayersGrid[x, y];

                if (tileLayer.CanSpawn()) return tileLayer;

                attemps++;
            }

            return null;
        }

        public (Vector2Int position, DirectionType direction) GetRandomPositionInBorder(Vector2 destiny)
        {
            // contolador de tentativas para coletar uma posiçao vazia
            // int attemps = 0;
            // int attempsLimit = 10;
            DirectionType direction = DirectionType.None;

            var dir = ((Vector2)CenterPoint - destiny).normalized;

            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                if (dir.x > 0) direction = DirectionType.Right;
                if (dir.x < 0) direction = DirectionType.Left;
            }
            else
            {
                if (dir.y > 0) direction = DirectionType.Top;
                if (dir.y < 0) direction = DirectionType.Bottom;
            }

            // List<DirectionType> directions = new() {
            //     DirectionType.Top,
            //     DirectionType.Bottom,
            //     DirectionType.Right,
            //     DirectionType.Left,
            // };

            Vector2Int tileLayerBorder = CenterPoint;

            int x = Random.Range(1, Width - 1);
            int y = Random.Range(1, Height - 1);

            if (direction == DirectionType.Top)
            {
                tileLayerBorder.x = x;
                tileLayerBorder.y += Height / 2;
            }
            else if (direction == DirectionType.Bottom)
            {
                tileLayerBorder.x = x;
                tileLayerBorder.y -= Height / 2;
            }
            else if (direction == DirectionType.Left)
            {
                tileLayerBorder.x = Width / 2;
                tileLayerBorder.y -= y;
            }
            else if (direction == DirectionType.Right)
            {
                tileLayerBorder.x = Width / 2;
                tileLayerBorder.y += y;
            }

            return (tileLayerBorder, direction);
        }
        #endregion
    }
}