using System.Collections.Generic;
using TinyTrails.Types;
using TinyTrails.UI;
using Unity.VisualScripting;
using UnityEngine;

namespace TinyTrails.Managers
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] MenuUI mainMenu;
        [SerializeField] GameObject loadScreen;
        [SerializeField] GameObject InGameHuds;
        [SerializeField] List<MenuSetting> menus;

        MenuSetting _currentMenu;
        MenuSetting _lastMenu;

        public void Navigate(MenuType menuType)
        {
            MenuSetting menu = menus.Find(f => f.menuType == menuType);

            if (menu == null)
            {
                Debug.LogError($"Menu {menuType} not found", this);
                return;
            }

            _currentMenu.menu.Close();

            _lastMenu = _currentMenu;

            _currentMenu = menu;

            _currentMenu.menu.Open();
        }

        public void CloseMenus() => _currentMenu.menu.Close();

        public void OpenLoadScreen() => loadScreen.SetActive(true);
        public void CloseLoadScreen() => loadScreen.SetActive(false);

        public void OpenGameHud() => InGameHuds.SetActive(true);
        public void CloseGameHud() => InGameHuds.SetActive(false);

        void Start()
        {
            _currentMenu = new MenuSetting() { menuType = MenuType.Main_Menu, menu = mainMenu };
        }
    }

    [System.Serializable]
    public class MenuSetting
    {
        public MenuType menuType;
        public MenuUI menu;
    }

}