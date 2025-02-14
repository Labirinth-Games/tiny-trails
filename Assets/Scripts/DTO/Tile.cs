using DG.Tweening;
using TinyTrails.Behaviours;
using TinyTrails.Managers;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.DTO
{
    public class Tile
    {
        public TileType TileType { get; private set; }
        public TileBehaviour gameObject;

        public void SetTileType(TileType tileType) => TileType = tileType;

    }
}