using System.Collections.Generic;
using System.Linq;
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
        public List<TileLayer> WallPositions { get; private set; } = new();
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
            WallPositions.Clear();

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

                    // quando é porta ele cria a porta, e a sala visinha tem o caminho ate ela
                    // quando abre a subZone que tem um o caminho ele limpa as celulas para poder
                    // colcoar o caminho e as paredes dela.
                    if (tileLayer.HasTile(TileType.Door))
                    {
                        DoorPositions.Add(tileLayer);

                        if (tileLayer.WaysBetweenSubZone != null)
                        {
                            for (int i = 0; i < tileLayer.WaysBetweenSubZone.Count; i++)
                            {
                                TileLayer wayTileLayer = tileLayer.WaysBetweenSubZone[i];

                                // essas condicionais complexas foram o jeito ate entao que 
                                // encontrei para conseguir mudar a quina da porta quando é 
                                // aberta, ou seja, quando a porta é aberta a quina precisa 
                                // mudar para que a dungeon fique ok visualmente e para isso 
                                // é necessário mudar a orientaçao da parede.. como o way é 
                                // gerado meio que no meio das duas subzonas é complexo 
                                // adicionar paredes e fazer esses micro ajustes para que 
                                // ela trabalhe bem.
                                //
                                // - ele pega o ultimo e o primeiro elemento do array de 
                                // caminho entre zonas, o primeiro e ultimo intem sao as 
                                // portas onde precisa acessar e mudar as quinas
                                if (i == tileLayer.WaysBetweenSubZone.Count - 1)
                                {
                                    Vector2 doorDirection = wayTileLayer.GetAbsolutePosition() - tileLayer.WaysBetweenSubZone.FindAll(f => f.HasTile(TileType.Way)).Last().GetAbsolutePosition();

                                    if (wayTileLayer.HasDoor())
                                    {
                                        if (doorDirection.x != 0)
                                        {
                                            Grid[pos.x, pos.y + 1].WallDirection = DirectionType.Top;
                                            Grid[pos.x, pos.y - 1].WallDirection = doorDirection.x < 0 ? DirectionType.Top_Down_Right : DirectionType.Top_Down_Left;
                                        }
                                        else if (doorDirection.y > 0) // indo para a sala de cima
                                        {
                                            Grid[pos.x + 1, pos.y].WallDirection = DirectionType.Top_Down_Right;
                                            Grid[pos.x - 1, pos.y].WallDirection = DirectionType.Top_Down_Left;
                                        }
                                    }
                                }

                                if (i == 0)
                                {
                                    Vector2 doorDirection = wayTileLayer.GetAbsolutePosition() - tileLayer.WaysBetweenSubZone.FindAll(f => f.HasTile(TileType.Way)).First().GetAbsolutePosition();

                                    if (doorDirection.x != 0)
                                    {
                                        Grid[wayTileLayer.GetAbsolutePosition().x, wayTileLayer.GetAbsolutePosition().y + 1].WallDirection = DirectionType.Top;
                                        Grid[wayTileLayer.GetAbsolutePosition().x, wayTileLayer.GetAbsolutePosition().y - 1].WallDirection = doorDirection.x < 0 ? DirectionType.Top_Down_Right : DirectionType.Top_Down_Left;
                                        WallPositions.Add(Grid[wayTileLayer.GetAbsolutePosition().x, wayTileLayer.GetAbsolutePosition().y + 1]);
                                        WallPositions.Add(Grid[wayTileLayer.GetAbsolutePosition().x, wayTileLayer.GetAbsolutePosition().y - 1]);
                                    }
                                    else if (doorDirection.y > 0) // indo para a sala de cima
                                    {
                                        Grid[wayTileLayer.GetAbsolutePosition().x - 1, wayTileLayer.GetAbsolutePosition().y].WallDirection = DirectionType.Top_Down_Left;
                                        Grid[wayTileLayer.GetAbsolutePosition().x + 1, wayTileLayer.GetAbsolutePosition().y].WallDirection = DirectionType.Top_Down_Right;
                                        WallPositions.Add(Grid[wayTileLayer.GetAbsolutePosition().x - 1, wayTileLayer.GetAbsolutePosition().y]);
                                        WallPositions.Add(Grid[wayTileLayer.GetAbsolutePosition().x + 1, wayTileLayer.GetAbsolutePosition().y]);
                                    }

                                }

                                if (wayTileLayer.HasTile(TileType.Door) || wayTileLayer.IsEmpty())
                                {
                                    wayTileLayer.ClearTiles();
                                    wayTileLayer.AddTile(TileType.Way);
                                }
                            }

                            WayPositions.AddRange(tileLayer.WaysBetweenSubZone.FindAll(f => f.HasTile(TileType.Way)));
                            WallPositions.AddRange(tileLayer.WaysBetweenSubZone.FindAll(f => f.HasTile(TileType.Wall)));
                        }
                    }
                    if (tileLayer.HasTile(TileType.Player)) PlayerPosition = tileLayer;
                    if (tileLayer.HasTile(TileType.Wall)) WallPositions.Add(tileLayer);
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
                    if (tileLayer.HasTile(TileType.Wall)) WallPositions.Add(tileLayer);
                    if (tileLayer.HasTile(TileType.Chest)) TreasurePositions.Add(tileLayer);
                    if (tileLayer.HasTile(TileType.Trap)) TrapsPositions.Add(tileLayer);
                }
        }
    }
}