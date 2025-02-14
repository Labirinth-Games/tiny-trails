using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.DTO
{
    [System.Serializable]
    public class CellDTO
    {
        public Vector2 startpoint;
        public Vector2 endpoint;
        public Vector2 pointDoor;
        public Vector2 GetDistance() => new Vector2(endpoint.x - startpoint.x, endpoint.y - startpoint.y);

        public void SetInitPoint(Vector2 pos) => startpoint = pos;
        public void SetEndPoint(Vector2 pos) => endpoint = pos;
        public void SetDoorPoint(Vector2 pos) => pointDoor = pos;

        public string DisplaySize() => $"Init: {startpoint} End: {endpoint} isWall {IsWall}";

        #region Validators
        public bool IsRoom = false;
        public bool IsDoor = false;
        public bool IsWall = false;
        public bool IsDoorPoint(Vector2 pos) => pos == pointDoor;
        #endregion
    }
}