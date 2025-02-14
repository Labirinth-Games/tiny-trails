using TinyTrails.Behaviours;
using TinyTrails.DTO;
using TinyTrails.Generators;
using TinyTrails.Managers;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.World
{
    public class Door : TileBehaviour
    {
        [SerializeField] GameObject openDoorButtonUI;

        #region Action
        public void OpenDoor()
        {
            TileLayer tileLayer = GameManager.Instance.MapManager.Zone.GetTileLayerOfCurrentSubZone(transform.position);

            if (tileLayer == null) return;

            GameManager.Instance.MapManager.Unregister(transform.position, Tile);

            GameManager.Instance.MapManager.Zone.SetCurrentSubZone(tileLayer.DoorNextSubZone);
            GameManager.Instance.MapManager.MapRender();

            Destroy(gameObject);
        }
        #endregion

        #region Collider
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (GameManager.Instance.ContextGameManager.IsBattle()) return;
            
            openDoorButtonUI.SetActive(true);
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            openDoorButtonUI.SetActive(false);
        }
        #endregion

        void Start()
        {
            Tile.SetTileType(TileType.Door);
            Tile.gameObject = this;
            GameManager.Instance.MapManager.Register(transform.position, Tile);
        }
    }
}