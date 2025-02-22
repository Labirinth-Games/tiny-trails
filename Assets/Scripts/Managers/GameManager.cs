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

        public GameObject loadscreen;

        void LoadSettingsPlayer()
        {
            string characterName = PlayerPrefs.GetString("character");

            if (characterName != "")
                characterSheet.characterClass = Instantiate(Resources.Load<CharacterClassesSO>($"Classes/{characterName}"));
            else
                characterSheet.characterClass = Instantiate(characterClassesSO);
        }

        void Start()
        {
            LoadSettingsPlayer();

            // UI
            ActionPointUI.Init();
            DefenseMeditorUI.Init();
            EndTurnUI.Init();
            FocusMeditorUI.Init();
            HPMeditorUI.Init();
            LogUI.Init();
            ObtainItemUI.Init();
            TurnLabelUI.Init();

            // Managers
            MapManager.CreateMap();
            FocusController.Init(characterSheet.Focus);
            ContextGameManager.Init();
            WorldIA.Init();


            // spawn player
            Player = Instantiate(characterSheet.characterClass.prefab, (Vector2)MapManager.Zone.PlayerPosition.GetAbsolutePosition(), Quaternion.identity).GetComponent<Player>();
            MapManager.Register(Player.transform.position, Player.Tile);

            Player.Init();

            // Actions
            DefenseAction.Init();

            // sounds
            AudioManager.Init();

            Debug.Log($"Load {PlayerPrefs.GetString("character")}");

            loadscreen.SetActive(false);
        }

        void OnDestroy()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}