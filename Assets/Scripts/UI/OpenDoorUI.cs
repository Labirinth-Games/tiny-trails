using TinyTrails.World;
using UnityEngine;

public class OpenDoorUI : MonoBehaviour
{
    [SerializeField] Door door;

    void OnMouseDown()
    {
        door.OpenDoor();
    }
}