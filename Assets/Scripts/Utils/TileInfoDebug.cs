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

        Debug.Log($"Tiles {transform.position}: {string.Join(", ", tileLayer.GetTiles().Select(e => e.TileType).ToArray())}");
    }
}