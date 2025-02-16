using TinyTrails.Behaviours;
using TinyTrails.DTO;
using TinyTrails.Generators;
using TinyTrails.Interfaces;
using TinyTrails.Managers;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.World
{
    public class Door : TileBehaviour, IInteractable
    {
        [SerializeField] GameObject openDoorButtonUI;
        [SerializeField] Sprite doorHorizontalBossSprite;
        [SerializeField] Sprite doorVerticalBossSprite;
        [SerializeField] Sprite doorVerticalSprite;
        [SerializeField] Sprite doorHorizontalSprite;

        [Header("Audio")]
        [SerializeField] AudioClip openDoorAudio;

        public void Init(DirectionType directionType, bool isBoss)
        {
            Sprite sprite = null;

            switch (directionType)
            {
                case DirectionType.Top:
                    sprite = isBoss ? doorHorizontalBossSprite : doorHorizontalSprite;
                    break;
                case DirectionType.Right:
                    sprite = isBoss ? doorVerticalBossSprite : doorVerticalSprite;
                    break;
                case DirectionType.Left:
                    sprite = isBoss ? doorVerticalBossSprite : doorVerticalSprite;
                    GetComponent<SpriteRenderer>().flipX = true;
                    break;
            }

            GetComponent<SpriteRenderer>().sprite = sprite;
        }

        #region Action
        public void Open()
        {
            TileLayer tileLayer = GameManager.Instance.MapManager.Zone.GetTileLayerOfCurrentSubZone(transform.position);

            GameManager.Instance.MapManager.Unregister(transform.position, Tile);

            GameManager.Instance.MapManager.Zone.SetCurrentSubZone(tileLayer.DoorNextSubZone);
            GameManager.Instance.MapManager.MapRender();

            GameManager.Instance.AudioManager.Play(openDoorAudio);

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