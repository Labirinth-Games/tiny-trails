using System.Collections.Generic;
using TinyTrails.Generators;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.World
{
    public class Zone
    {
        public List<TileLayer> EnemyPositions { get; private set; } = new();
        public TileLayer BossPosition { get; private set; }
        public List<TileLayer> DoorPositions { get; private set; } = new();
        public List<TileLayer> FloorsPositions { get; private set; } = new();
        public List<TileLayer> TrapsPositions { get; private set; } = new();
        public List<TileLayer> WayPositions { get; private set; } = new();
        public TileLayer PlayerPosition { get; private set; }
        public List<TileLayer> TreasurePositions { get; private set; } = new();
        public List<TileLayer> Walls { get; private set; } = new();
        public List<SubZone> SubZones { get; private set; } = new();

        public TileLayer[,] Grid { get; set; }

        SubZone _currentSubZone;

        public Zone(TileLayer[,] zone, List<SubZone> subZones, ZoneConfigSO zoneConfig)
        {
            Grid = zone;
            SubZones = subZones;

            SetCurrentSubZone(subZones.Find(f => f.RoomType == RoomType.None));
        }

        public void SetCurrentSubZone(SubZone subZone) => _currentSubZone = subZone;
        public SubZone GetCurrentSubZone() => _currentSubZone;

        /// <summary>
        /// Metodo responsavel por pegar um tileLayer da subzona atual
        /// pela posição absoluta
        /// </summary>
        /// <param name="position">posição absoluto do tile</param>
        /// <returns></returns>
        public TileLayer GetTileLayerOfCurrentSubZone(Vector2 position)
        {
            for (int x = 0; x < Grid.GetLength(0); x++)
                for (int y = 0; y < Grid.GetLength(1); y++)
                {
                    if (position == Grid[x, y].GetAbsolutePosition())
                    {
                        return Grid[x, y];
                    }
                }

            return null;
        }

        void Clear()
        {
            EnemyPositions.Clear();
            DoorPositions.Clear();
            FloorsPositions.Clear();
            WayPositions.Clear();
            TreasurePositions.Clear();
            TrapsPositions.Clear();
            Walls.Clear();

            BossPosition = null;
        }

        public void LoadTilesSubZonePositions()
        {
            TileLayer[,] subZone = _currentSubZone.TileLayersGrid;
            Clear();

            for (int x = 0; x < subZone.GetLength(0); x++)
                for (int y = 0; y < subZone.GetLength(1); y++)
                {
                    Vector2Int pos = subZone[x, y].GetAbsolutePosition();

                    TileLayer tileLayer = Grid[pos.x, pos.y];

                    if (tileLayer == null) continue;

                    if (tileLayer.HasTile(TileType.Floor)) FloorsPositions.Add(tileLayer);
                    if (tileLayer.HasTile(TileType.Enemy)) EnemyPositions.Add(tileLayer);
                    if (tileLayer.HasTile(TileType.Door))
                    {
                        DoorPositions.Add(tileLayer);

                        if (tileLayer.WaysBetweenSubZone != null)
                        {
                            tileLayer.WaysBetweenSubZone.ForEach(f => f.ClearTiles());
                            WayPositions.AddRange(tileLayer.WaysBetweenSubZone);
                        }
                    }
                    if (tileLayer.HasTile(TileType.Player)) PlayerPosition = tileLayer;
                    if (tileLayer.HasTile(TileType.Wall)) Walls.Add(tileLayer);
                    if (tileLayer.HasTile(TileType.Chest)) TreasurePositions.Add(tileLayer);
                    if (tileLayer.HasTile(TileType.Trap)) TrapsPositions.Add(tileLayer);
                    if (tileLayer.HasTile(TileType.Boss)) BossPosition = tileLayer;
                }
        }

        public void LoadAllTilesPositions()
        {
            Clear();

            for (int x = 0; x < Grid.GetLength(0); x++)
                for (int y = 0; y < Grid.GetLength(1); y++)
                {
                    TileLayer tileLayer = Grid[x, y];

                    if (tileLayer == null) continue;

                    if (tileLayer.HasTile(TileType.Floor)) FloorsPositions.Add(tileLayer);
                    if (tileLayer.HasTile(TileType.Enemy)) EnemyPositions.Add(tileLayer);
                    if (tileLayer.HasTile(TileType.Door))
                    {
                        DoorPositions.Add(tileLayer);

                        if (tileLayer.WaysBetweenSubZone != null)
                        {
                            tileLayer.WaysBetweenSubZone.ForEach(f => f.ClearTiles());
                            WayPositions.AddRange(tileLayer.WaysBetweenSubZone);
                        }
                    }
                    if (tileLayer.HasTile(TileType.Player)) PlayerPosition = tileLayer;
                    if (tileLayer.HasTile(TileType.Wall)) Walls.Add(tileLayer);
                    if (tileLayer.HasTile(TileType.Chest)) TreasurePositions.Add(tileLayer);
                    if (tileLayer.HasTile(TileType.Trap)) TrapsPositions.Add(tileLayer);
                }
        }
    }
}