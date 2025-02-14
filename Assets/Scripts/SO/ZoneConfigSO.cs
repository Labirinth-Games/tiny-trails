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

    [Header("Player")]
    public bool canSpawnPlayerTogetherDoor;

    // enemies
    [Header("Enemies")]
    public int amountEnemies;

    [Header("Element Scenery")]
    public int amountTraps;

    // doors
    [Header("Doors")]
    public int amountDoors;
    public int amountDoorsBySubZone;
}