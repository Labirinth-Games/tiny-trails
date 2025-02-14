using System.Collections;
using TinyTrails.Actions;
using TinyTrails.Characters;
using TinyTrails.Controllers;
using TinyTrails.Core;
using TinyTrails.DTO;
using TinyTrails.IA;
using TinyTrails.Render;
using TinyTrails.SO;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        // this options remove after when create screen to choose/mount the character to play
        public CharacterSheetSO characterSheet;
        public GameSettingsSO Settings;
        public ZoneConfigSO ZoneConfig;
        public bool IsPlayerDie = false;


        [Space()]

        #region Renders
        [Header("Renders")]
        public MapRender MapRender;
        #endregion

        #region Managers
        [Header("Managers")]
        public MapManager MapManager;
        public EventManager EventManager;
        public TurnManager TurnManager;
        public InventaryManager InventaryManager;
        public WorldManager WorldManager;
        public ContextGameManager ContextGameManager;
        #endregion

        #region Controllers
        [Header("Controllers")]
        public ActionPointController ActionPointController;
        public FocusController FocusController;
        #endregion

        #region Actions
        [Header("Actions")]
        public MoveAction MoveAction;
        public AttackAction AttackAction;
        #endregion

        [Space()]
        public Player Player;
        public WorldIA WorldIA;

        void Start()
        {
            MapManager.CreateMap();
            FocusController.Init(characterSheet.Focus);
            ContextGameManager.Init();
            WorldIA.Init();

            StartCoroutine(WaitToLoad());
        }

        IEnumerator WaitToLoad() // wait all load before spawn player
        {
            yield return new WaitForEndOfFrame();

            // spawn player
            Player = Instantiate(characterSheet.body, (Vector2)MapManager.Zone.PlayerPosition.GetAbsolutePosition(), Quaternion.identity).GetComponent<Player>();
            MapManager.Register(Player.transform.position, Player.Tile);

            Player.Init();
        }
    }
}