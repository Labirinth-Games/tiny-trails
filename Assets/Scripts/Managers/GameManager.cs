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
using TinyTrails.UI;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace TinyTrails.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        // this options remove after when create screen to choose/mount the character to play
        public CharacterSheetSO characterSheet;
        public CharacterClassesSO characterClassesSO;
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
        public AudioManager AudioManager;
        public MenuManager MenuManager;
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
        public DefenseAction DefenseAction;
        #endregion

        [Space()]
        public Player Player;
        public WorldIA WorldIA;

        #region UI
        [Header("UI")]
        public ActionPointUI ActionPointUI;
        public DefenseMeditorUI DefenseMeditorUI;
        public EndTurnUI EndTurnUI;
        public FocusMeditorUI FocusMeditorUI;
        public HPMeditorUI HPMeditorUI;
        public LogUI LogUI;
        public ObtainItemUI ObtainItemUI;
        public TurnLabelUI TurnLabelUI;
        #endregion

        public void StartGame(CharacterClassesSO playerClass)
        {
            MenuManager.CloseMenus();
            MenuManager.OpenGameHud();

            characterSheet.characterClass = playerClass;

            // UI
            ActionPointUI.Init();
            DefenseMeditorUI.Init();
            EndTurnUI.Init();
            FocusMeditorUI.Init();
            HPMeditorUI.Init();
            LogUI.Init();
            ObtainItemUI.Init();
            // TurnLabelUI.Init();

            // Managers
            MapManager.CreateMap();
            FocusController.Init(characterSheet.Focus);
            WorldIA.Init();

            // spawn player
            Vector3 pos = (Vector3Int)MapManager.Zone.PlayerPosition.GetAbsolutePosition() + Vector3Int.forward * -1;
            Player = Instantiate(characterSheet.characterClass.prefab, pos, Quaternion.identity).GetComponent<Player>();
            MapManager.Register(Player.transform.position, Player.Tile);

            Player.Init();

            // Actions
            DefenseAction.Init();

            MenuManager.CloseLoadScreen();
            EventManager.Publisher<ContextGameType>(EventChannelType.OnContextGameChangeStatus, ContextGameType.Explore);
        }

        public void EndGame()
        {
            MapRender.Clear();

            MenuManager.CloseGameHud();
            MenuManager.Navigate(MenuType.Main_Menu);
            EventManager.Publisher<ContextGameType>(EventChannelType.OnContextGameChangeStatus, ContextGameType.Menu);
        }

        void Start()
        {
            ContextGameManager.Init();

            // sounds
            AudioManager.Init();
        }
    }
}