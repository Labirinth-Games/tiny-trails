using System.Collections.Generic;
using System.Linq;
using TinyTrails.Generators;
using TinyTrails.Managers;
using TinyTrails.Types;
using TinyTrails.World;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    TileLayer[,] mapZoneGrid;
    readonly List<SubZone> subZones = new();

    public Zone Build(ZoneConfigSO _zoneConfig)
    {
        CreateZone(_zoneConfig);
        CreateSubZones(_zoneConfig);

        // apply elements inside subZones
        EnemySpawnPoints(_zoneConfig);
        TreasureSpawnPoints(_zoneConfig);
        PlayerSpawnPoint(_zoneConfig);
        TrapSpawnPoint(_zoneConfig);
        BossSpawnPoints(_zoneConfig);

        // connect all subzones and apply inside zone;
        ConnectSubZones(_zoneConfig);

        Debug.Log($"Sub Zones generated: {string.Join(" | ", subZones.Select(f => f.RoomType).ToArray())}");

        return new Zone(mapZoneGrid, subZones, _zoneConfig);
    }

    #region Spawn Points Elements
    void EnemySpawnPoints(ZoneConfigSO _zoneConfig)
    {
        // calc num monsters based size map and difficulty
        var settings = GameManager.Instance.Settings;
        int amountMonsters = settings.AmountEnemiesByDifficulty.Find(f => f.difficulty == settings.difficulty).amountEnemies;

        foreach (SubZone subZone in subZones.FindAll(f => f.RoomType == RoomType.Enemy))
        {
            for (int _i = 0; _i < amountMonsters; _i++)
            {
                Vector2Int position = subZone.GetRandomPosition(1);

                subZone.AddTileLayer(position, TileType.Enemy);
            }

            // change to show chest in enemy zone
            bool chanceSpawnChest = Random.value > GameManager.Instance.Settings.chanceSpawnChestInEnemyZone;

            if (chanceSpawnChest)
            {
                Vector2Int positionChest = subZone.GetRandomPosition(1);

                subZone.AddTileLayer(positionChest, TileType.Chest);
            }
        }
    }

    void BossSpawnPoints(ZoneConfigSO _zoneConfig)
    {
        foreach (SubZone _subZone in subZones.FindAll(f => f.RoomType == RoomType.Boss))
        {
            Vector2Int _position = _subZone.GetRandomPosition();

            _subZone.AddTileLayer(_position, TileType.Boss);
        }
    }

    void TreasureSpawnPoints(ZoneConfigSO _zoneConfig)
    {
        foreach (SubZone _subZone in subZones.FindAll(f => f.RoomType == RoomType.Treasure || f.RoomType == RoomType.Trap))
        {
            Vector2Int _position = _subZone.GetRandomPosition(1);

            _subZone.AddTileLayer(_position, TileType.Chest);
        }
    }

    void PlayerSpawnPoint(ZoneConfigSO _zoneConfig)
    {
        SubZone _subZone = subZones.Find(f => f.RoomType == RoomType.None);

        _subZone.HasPlayer = true;

        Vector2Int _position = _subZone.GetRandomPosition();
        _subZone.AddTileLayer(_position, TileType.Player);
    }

    void TrapSpawnPoint(ZoneConfigSO zoneConfig)
    {

        foreach (SubZone subZone in subZones.FindAll(f => f.RoomType == RoomType.Trap))
        {
            int amountTraps = Mathf.FloorToInt((subZone.Width * subZone.Height) * zoneConfig.amountTraps);

            for (int i = 0; i < amountTraps; i++)
            {
                Vector2Int position = subZone.GetRandomPosition();

                subZone.AddTileLayer(position, TileType.Trap);
            }
        }
    }
    #endregion

    #region Creates
    void CreateZone(ZoneConfigSO mapConfig)
    {
        // map zone real
        int zoneSize = (mapConfig.amountSubZones + 1) * mapConfig.maxSubZoneSize * 2;
        mapZoneGrid = CreateGridTiles(zoneSize, zoneSize, TileType.None);
    }

    void CreateSubZones(ZoneConfigSO zoneConfig)
    {
        for (int i = 0; i < zoneConfig.amountSubZones; i++)
        {
            int width = Random.Range(zoneConfig.minSubZoneSize, zoneConfig.maxSubZoneSize) + 2; // +2 pq é o tamanho das bordas onde iram ficar as paredes
            int height = Random.Range(zoneConfig.minSubZoneSize, zoneConfig.maxSubZoneSize) + 2;

            RoomType roomType = zoneConfig.roomTypes[Random.Range(0, zoneConfig.roomTypes.Count)];

            SubZone subZone = new SubZone()
            {
                Width = width,
                Height = height,
                RoomType = i == 0 ? RoomType.None : roomType,
                TileLayersGrid = CreateSubZoneGrid(width, height),
            };

            subZones.Add(subZone);
        }

        // create zone boss
        int widthZoneBoss = (int)Random.Range(zoneConfig.minSubZoneSize * 1.5f, zoneConfig.maxSubZoneSize * 1.5f) + 2; // +2 pq é o tamanho das bordas onde iram ficar as paredes
        int heightZoneBoss = (int)Random.Range(zoneConfig.minSubZoneSize * 1.5f, zoneConfig.maxSubZoneSize * 1.5f) + 2;

        SubZone subZoneBoss = new SubZone()
        {
            Width = widthZoneBoss,
            Height = heightZoneBoss,
            RoomType = RoomType.Boss,
            TileLayersGrid = CreateSubZoneGrid(widthZoneBoss, heightZoneBoss),
        };

        subZones.Add(subZoneBoss);
    }

    /// <summary>
    /// Metodo responsavel por adicionar as subzonas no mapa e fazer os caminhos entre eles
    /// </summary>
    /// <param name="_mapConfig"></param>
    void ConnectSubZones(ZoneConfigSO _mapConfig)
    {
        if (subZones.Count == 0) return;

        SubZone subZone;
        SubZone lastSubZone = null;
        List<SubZone> allSubZones = new List<SubZone>();

        for (int i = 0; i < subZones.Count; i++)
        {
            subZone = subZones[i];
            allSubZones.Add(subZone);

            if (i == 0)
            {
                // pegando uma posição do centro para iniciar
                int centerMapPosX = mapZoneGrid.GetLength(0) / 2 - subZone.Width / 2;
                int centerMapPosY = mapZoneGrid.GetLength(1) / 2 - subZone.Height / 2;

                subZone.CenterPointAbsolute = new Vector2Int(centerMapPosX, centerMapPosY);

                // aplicando a zona nomeio do mapa
                DrawSubZoneInsideZone(subZone);

                lastSubZone = subZone;
                continue;
            }

            int neighborDistance = Random.Range(_mapConfig.minSpaceBeetweenSubZones, _mapConfig.maxSpaceBeetweenSubZones);
            (bool hasConflictSubZone, Vector2Int centerPoint) directionNeigbor = GetDirectionNeighbor(lastSubZone, subZone, neighborDistance + 1);

            if (directionNeigbor.hasConflictSubZone) continue;

            subZone.CenterPointAbsolute = directionNeigbor.centerPoint;

            DrawSubZoneInsideZone(subZone);

            PathFinder(lastSubZone.CenterPointAbsolute, subZone.CenterPointAbsolute);

            lastSubZone = allSubZones[Random.Range(0, allSubZones.Count)];
        }
    }

    /// <summary>
    /// Metodo que pega cada subzona e adiciona na zona principal e 
    /// adiciona a posiçao absoluta dele no mundo
    /// </summary>
    /// <param name="subZone"></param>
    void DrawSubZoneInsideZone(SubZone subZone)
    {
        Vector2Int relativePosition = Vector2Int.zero;

        // posiçoes do quadrante X
        int initalPointX = subZone.CenterPointAbsolute.x - subZone.Width / 2;
        int endPointX = subZone.CenterPointAbsolute.x + subZone.Width / 2;

        // posiçoes do quadrante y
        int initalPointY = subZone.CenterPointAbsolute.y - subZone.Height / 2;
        int endPointY = subZone.CenterPointAbsolute.y + subZone.Height / 2;

        for (int x = initalPointX; x <= endPointX; x++)
        {
            for (int y = initalPointY; y <= endPointY; y++)
            {
                if (!IsInsideGrid(x, y, mapZoneGrid) || !IsInsideGrid(relativePosition.x, relativePosition.y, subZone.TileLayersGrid)) continue;

                subZone.SetTileAbsolutePosition(relativePosition, new Vector2Int(x, y));
                mapZoneGrid[x, y] = subZone.GetTileLayerByRelativePosition(relativePosition);

                relativePosition.y++;
            }

            relativePosition.x++;
            relativePosition.y = 0;
        }
    }

    #endregion

    #region Utils
    /// <summary>
    /// Metodo de pathfinder que monta um caminho entre dois pontos
    /// </summary>
    /// <param name="startPoint"></param>
    /// <param name="endPoint"></param>
    void PathFinder(Vector2Int startPoint, Vector2Int endPoint)
    {
        Vector2Int step = startPoint;
        Vector2Int lastStep = startPoint;
        int attemps = 0;
        int attempsLimit = 300;

        List<TileLayer> ways = new();
        TileLayer door = null;

        while (true)
        {
            if (attemps > attempsLimit) break;

            // direction to move step
            List<(Vector2Int direction, float distance)> directions = new List<(Vector2Int direction, float distance)>() {
                    (step + Vector2Int.up, Vector2Int.Distance(endPoint, step + Vector2Int.up)),
                    (step + Vector2Int.down, Vector2Int.Distance(endPoint, step + Vector2Int.down)),
                    (step + Vector2Int.left, Vector2Int.Distance(endPoint, step + Vector2Int.left)),
                    (step + Vector2Int.right, Vector2Int.Distance(endPoint, step + Vector2Int.right)),
                };

            lastStep = step;
            step = directions.Find(_direction => _direction.distance == directions.Min(_directionMin => _directionMin.distance)).direction;

            Vector2Int directionPath = step - lastStep;

            if (step == endPoint) break;

            if (!IsInsideGrid(step.x, step.y, mapZoneGrid)) continue;

            TileLayer tileLayer = mapZoneGrid[step.x, step.y];

            // quando o pathfinder chega numa parede (borda da zone) ele remove a parede
            // e coloca uma porta e ja vincula qual é a zona destino 
            if (tileLayer.HasTile(TileType.Wall))
            {
                tileLayer.RemoveTile(TileType.Wall);
                DirectionType doorDirection = DirectionType.None;

                // direction door
                if (mapZoneGrid[step.x + 1, step.y].IsEmpty()) // when vertical - right
                    doorDirection = DirectionType.Right;

                if (mapZoneGrid[step.x - 1, step.y].IsEmpty()) // when vertical - left
                    doorDirection = DirectionType.Left;

                if (mapZoneGrid[step.x, step.y + 1].IsEmpty() || mapZoneGrid[step.x, step.y - 1].IsEmpty()) // when horizontal
                    doorDirection = DirectionType.Top;

                tileLayer.AddTileDoor(doorDirection);
                tileLayer.DoorNextSubZone = subZones.Find(f => f.CenterPointAbsolute == endPoint);
                door = tileLayer;

                ways.Add(tileLayer);
            }

            if (tileLayer.IsEmpty())
            {
                tileLayer.AddTile(TileType.Way);
                tileLayer.SetAbsolutePosition(step);
                ways.Add(tileLayer);

                // adicionando paredes quando p caminho é criado na horizontal
                if (directionPath.x != 0)
                {
                    // add wall top way
                    TileLayer wallTopTileLayer = mapZoneGrid[step.x, step.y + 1];
                    wallTopTileLayer.AddTileWall(DirectionType.Top);
                    wallTopTileLayer.SetAbsolutePosition(new Vector2Int(step.x, step.y + 1));

                    ways.Add(wallTopTileLayer);

                    // add wall bottom way
                    TileLayer wallBottomTileLayer = mapZoneGrid[step.x, step.y - 1];
                    wallBottomTileLayer.AddTileWall(DirectionType.Top);
                    wallBottomTileLayer.SetAbsolutePosition(new Vector2Int(step.x, step.y - 1));

                    ways.Add(wallBottomTileLayer);
                }

                // adicionando paredes quando p caminho é criado na vertical
                if (directionPath.y != 0)
                {
                    // add wall right way
                    TileLayer wallRightTileLayer = mapZoneGrid[step.x + 1, step.y];
                    wallRightTileLayer.AddTileWall(DirectionType.Right);
                    wallRightTileLayer.SetAbsolutePosition(new Vector2Int(step.x + 1, step.y));

                    ways.Add(wallRightTileLayer);

                    // add wall left way
                    TileLayer wallLeftTileLayer = mapZoneGrid[step.x - 1, step.y];
                    wallLeftTileLayer.AddTileWall(DirectionType.Left);
                    wallLeftTileLayer.SetAbsolutePosition(new Vector2Int(step.x - 1, step.y));

                    ways.Add(wallLeftTileLayer);
                }
            }

            attemps++;
        }

        // aplicando dentro do tileLayer da porta o caminho entre currente sala e a proxima sala
        if (door != null && ways.Count > 0)
            door.WaysBetweenSubZone = ways;
    }

    /// <summary>
    /// Metodo responsavel por buscar uma direção que possa adicioanr uma nova zona 
    /// que não ocorra sobreposição entre subzonas
    /// </summary>
    /// <param name="lastSubZone"></param>
    /// <param name="actualSubZone"></param>
    /// <param name="neighborDistance"></param>
    /// <returns>Returna uma tupla com o bool informando se encontrou uma posição e a posição em si</returns>
    (bool hasConflictSubZone, Vector2Int position) GetDirectionNeighbor(SubZone lastSubZone, SubZone actualSubZone, int neighborDistance)
    {
        List<DirectionType> directions = new()
        {
            DirectionType.Top,
            DirectionType.Bottom,
            DirectionType.Left,
            DirectionType.Right,
        };

        int _attemps = 0;
        int _attempsLimit = 10;

        while (true)
        {
            if (_attemps > _attempsLimit) break;

            DirectionType direction = directions[Random.Range(0, directions.Count)];
            Vector2Int newCenterPointer = Vector2Int.zero;

            switch (direction)
            {
                case DirectionType.Top:
                    newCenterPointer = new Vector2Int(
                        lastSubZone.CenterPointAbsolute.x,
                        lastSubZone.CenterPointAbsolute.y + lastSubZone.Height / 2 + neighborDistance + actualSubZone.Height / 2
                    );
                    break;
                case DirectionType.Bottom:
                    newCenterPointer = new Vector2Int(
                        lastSubZone.CenterPointAbsolute.x,
                        lastSubZone.CenterPointAbsolute.y - lastSubZone.Height / 2 - neighborDistance - actualSubZone.Height / 2
                    );
                    break;
                case DirectionType.Left:
                    newCenterPointer = new Vector2Int(
                        lastSubZone.CenterPointAbsolute.x - lastSubZone.Width / 2 - neighborDistance - actualSubZone.Width / 2,
                        lastSubZone.CenterPointAbsolute.y
                    );
                    break;
                case DirectionType.Right:
                    newCenterPointer = new Vector2Int(
                        lastSubZone.CenterPointAbsolute.x + lastSubZone.Width / 2 + neighborDistance + actualSubZone.Width / 2,
                        lastSubZone.CenterPointAbsolute.y
                    );
                    break;

            }

            if (!IsInsideGrid(newCenterPointer.x, newCenterPointer.y, mapZoneGrid)) continue; // vendo se ta fora do grid map

            if (mapZoneGrid[newCenterPointer.x, newCenterPointer.y].IsEmpty()) return (false, newCenterPointer);

            _attemps++;
        }

        return (true, Vector2Int.zero);
    }

    /// <summary>
    /// Metodo responsavel por criar uma grid de tiles
    /// </summary>
    /// <param name="_width"></param>
    /// <param name="_height"></param>
    /// <returns></returns>
    TileLayer[,] CreateGridTiles(int _width, int _height, TileType _tileType)
    {
        TileLayer[,] tileLayerGrid = new TileLayer[_width, _height];

        for (int x = 0; x < _width; x++)
            for (int y = 0; y < _height; y++)
                tileLayerGrid[x, y] = new TileLayer(new Vector2Int(x, y), _tileType);

        return tileLayerGrid;
    }

    TileLayer[,] CreateSubZoneGrid(int width, int height)
    {
        TileLayer[,] tileLayerGrid = new TileLayer[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                DirectionType directionType = DirectionType.None;

                // dependendo da posição validando qual a melhor direção para a parede
                if (x == 0 && y == 0) directionType = DirectionType.Bottom_Left;
                else if (x == width - 1 && y == 0) directionType = DirectionType.Bottom_Right;
                else if (x == width - 1 && y == height - 1) directionType = DirectionType.Top_Right;
                else if (x == 0 && y == height - 1) directionType = DirectionType.Top_Left;
                else if (x == 0) directionType = DirectionType.Left;
                else if (x == width - 1) directionType = DirectionType.Right;
                else if (y == 0 || y == height - 1) directionType = DirectionType.Top;

                if (y == 0 || y == height - 1 || x == 0 || x == width - 1) // adicionar paredes apenas nas boradas da subzone
                {
                    TileLayer tileLayer = new TileLayer(new Vector2Int(x, y), TileType.Wall);
                    tileLayer.SetWallDirection(directionType);
                    tileLayerGrid[x, y] = tileLayer;
                }
                else
                {
                    tileLayerGrid[x, y] = new TileLayer(new Vector2Int(x, y), TileType.Floor);
                }
            }
        }

        return tileLayerGrid;
    }

    /// <summary>
    /// Validando se a posição está dentro de uma grid
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <param name="grid">grid a ser validada</param>
    /// <returns></returns>
    bool IsInsideGrid(int _x, int _y, TileLayer[,] grid) => _x >= 0 && _x < grid.GetLength(0) && _y >= 0 && _y < grid.GetLength(1);
    #endregion
}