using System.Collections.Generic;
using TinyTrails.Behaviours;
using TinyTrails.DTO;
using TinyTrails.Enemies;
using TinyTrails.Generators;
using TinyTrails.i18n;
using TinyTrails.Interfaces;
using TinyTrails.Managers;
using TinyTrails.SO;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.World
{
    public class Chest : TileBehaviour, IInteractable
    {
        [SerializeField] GameObject chestButtonUI;
        [SerializeField] Sprite chestOpenSprite;

        bool isOpen;
        ItemSO _item;

        public void Open()
        {
            isOpen = true;

            var items = GameManager.Instance.WorldManager.GetItems();
            _item = items[Random.Range(0, items.Count)];

            GameManager.Instance.EventManager.Publisher<ItemSO>(EventChannelType.OnChestObtainItem, _item);

            GetComponent<SpriteRenderer>().sprite = chestOpenSprite;
            chestButtonUI.SetActive(false);
        }

        void ChanceOfSpawnEnemies()
        {
            if (Random.value > GameManager.Instance.Settings.chanceSpawnEnemiesWhenOpenChest) return;

            int amountEnemies = Random.Range(1, GameManager.Instance.Settings.amountSpawnEnemiesWhenOpenChest);

            for (var i = 0; i < amountEnemies; i++)
            {
                TileLayer tileLayer = GameManager.Instance.MapManager.GetTileLayerRandom();
                List<GameObject> enemies = GameManager.Instance.WorldManager.GetEnemies();

                GameObject instance = Instantiate(enemies[Random.Range(0, enemies.Count)]);

                Enemy enemy = instance.GetComponent<Enemy>();
                enemy.Init(tileLayer.GetAbsolutePosition());

                GameManager.Instance.WorldManager.SetEnemyIA(enemy.Tile);
            }

            GameManager.Instance.ContextGameManager.StartBattle(); // start context battle
            GameManager.Instance.EventManager.Publisher(EventChannelType.OnTurnWorldStart); // enemy init attack
        }

        #region Events
        void OnObtainItemUIClose()
        {
            ChanceOfSpawnEnemies(); // change de aparecer monstros depois de abrir o bau

            GameManager.Instance.InventaryManager.Add(_item);
        }
        #endregion

        #region Collider
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (GameManager.Instance.ContextGameManager.IsBattle() || isOpen) return;

            chestButtonUI.SetActive(true);
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            chestButtonUI.SetActive(false);
        }
        #endregion

        void Start()
        {
            Tile.SetTileType(TileType.Chest);
            Tile.gameObject = this;

            GameManager.Instance.MapManager.Register(transform.position, Tile);

            // subscribers
            GameManager.Instance.EventManager.Subscriber(EventChannelType.OnChestObtainItemClose, OnObtainItemUIClose);
        }
    }
}