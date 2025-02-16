using System.Collections.Generic;
using TinyTrails.Types;
using UnityEngine;

[CreateAssetMenu(fileName = "ZoneConfig", menuName = "ScriptableObjects/ZoneConfig", order = 0)]
public class ZoneConfigSO : ScriptableObject
{
    [Header("SubZones")]
    public int minSubZoneSize;
    public int maxSubZoneSize;
    public int amountSubZones;
    public int minSpaceBeetweenSubZones;
    public int maxSpaceBeetweenSubZones;
    public int scaleTilePosition = 1; // escala aplicada a distancia dos tiles
    public List<RoomType> roomTypes;

    [Header("Player")]
    public bool canSpawnPlayerTogetherDoor;

    // enemies
    [Header("Enemies")]
    public int amountEnemies;

    [Header("Traps")]
    [Range(0f, 1f)]
    public float amountTraps;

    // doors
    [Header("Doors")]
    public int amountDoors;
    public int amountDoorsBySubZone;
}