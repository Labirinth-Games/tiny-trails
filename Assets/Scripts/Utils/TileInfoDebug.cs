using System.Linq;
using TinyTrails.DTO;
using TinyTrails.Generators;
using TinyTrails.Managers;
using UnityEngine;

public class TileInfoDebug : MonoBehaviour
{
    void OnMouseDown()
    {
        TileLayer[,] grid = GameManager.Instance.MapManager.GetMapGrid();
        TileLayer tileLayer = grid[(int)transform.position.x, (int)transform.position.y];

        Vector2 absolute = tileLayer.GetAbsolutePosition();
        Vector2 relative = tileLayer.GetRelativePosition();

        Debug.Log($"Absolute x:{absolute.x}, y:{absolute.y} - Relative x:{relative.x}, y:{relative.y} - Tiles: {string.Join(", ", tileLayer.GetTiles().Select(e => e.TileType).ToArray())}");
    }
}