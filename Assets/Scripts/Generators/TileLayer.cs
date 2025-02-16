using System.Collections.Generic;
using System.Linq;
using TinyTrails.DTO;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.Generators
{
    public class TileLayer
    {
        public DirectionType DoorDirection { get; private set; }
        public SubZone DoorNextSubZone { get; set; }
        public List<TileLayer> WaysBetweenSubZone { get; set; }

        public DirectionType WallDirection { get; private set; }

        // posição dentro da sub zona
        Vector2Int relativePosition;
        // posição relacionado ao mundo
        Vector2Int absolutePosition;
        List<Tile> tiles = new();

        public TileLayer(Vector2Int _relativePosition, TileType _tileType)
        {
            relativePosition = _relativePosition;

            if (_tileType != TileType.None)
            {
                Tile tile = new Tile();
                tile.SetTileType(_tileType);

                tiles.Add(tile);
            }
        }

        public void AddTile(TileType _tileType)
        {
            Tile tile = new Tile();
            tile.SetTileType(_tileType);

            tiles.Add(tile);
        }

        public void AddTile(Tile tile)
        {
            tiles.Add(tile);
        }

        public void UpdateTile(TileType tileType, Tile tile)
        {
            RemoveTile(tileType);
            tiles.Add(tile);
        }

        public void RemoveTile(Tile tile)
        {
            if (tiles.Count == 0) return;

            tiles.Remove(tile);
        }

        public void RemoveTile(TileType tileType)
        {
            if (tiles.Count == 0) return;

            int tileIndex = tiles.FindIndex(f => f.TileType == tileType);

            if (tileIndex == -1) return;

            tiles.RemoveAt(tileIndex);
        }

        public void ClearTiles() => tiles.Clear();

        public void AddTileDoor(DirectionType direction)
        {
            Tile tile = new Tile();
            tile.SetTileType(TileType.Door);

            tiles.Add(tile);
            DoorDirection = direction;
        }

        public void AddTileWall(DirectionType direction)
        {
            Tile tile = new Tile();
            tile.SetTileType(TileType.Wall);

            tiles.Add(tile);
            WallDirection = direction;
        }

        #region Gets/Sets
        public void SetAbsolutePosition(Vector2Int _absolutePosition) => this.absolutePosition = _absolutePosition;

        public void SetRelativePosition(Vector2Int _relativePosition) => this.relativePosition = _relativePosition;

        public void SetWallDirection(DirectionType direction) => WallDirection = direction;

        public Vector2Int GetAbsolutePosition() => absolutePosition;

        public Vector2Int GetRelativePosition() => relativePosition;

        public Tile GetTile(TileType tileType) => tiles.Find(f => f.TileType == tileType);

        public List<Tile> GetTiles() => tiles;

        #endregion

        #region Validators
        public bool IsEmpty() => tiles.Count == 0;
        public bool HasFloor() => tiles.Exists(f => f.TileType == TileType.Floor || f.TileType == TileType.Way) && tiles.Count == 1;
        public bool HasObstacle() => new List<TileType>() { TileType.Door, TileType.Wall }.Any(type => tiles.Exists(f => f.TileType == type));
        public bool HasDoor() => tiles.Exists(f => f.TileType == TileType.Door);
        public bool HasTile(TileType _tileType) => tiles.Exists(f => f.TileType == _tileType);
        public bool CanSpawn() => !new List<TileType>() { TileType.Enemy, TileType.Door, TileType.Way, TileType.Player }.Any(type => tiles.Exists(f => f.TileType == type)) && tiles.Exists(f => f.TileType == TileType.Floor);
        public bool CanAttack() => tiles.Exists(f => f.TileType == TileType.Enemy || f.TileType == TileType.Floor || f.TileType == TileType.Way);
        public bool CanMove() => tiles.Exists(f => f.TileType == TileType.Floor || f.TileType == TileType.Trap || f.TileType == TileType.Way);
        #endregion
    }
}