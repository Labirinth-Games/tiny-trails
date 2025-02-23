using System.Collections;
using System.Collections.Generic;
using TinyTrails.Managers;
using TinyTrails.SO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TinyTrails.UI
{
    public class MenuChooseHeroUI : MenuUI
    {
        [SerializeField] GameObject container;
        [SerializeField] List<CharacterClassesSO> charactersClasses;
        [SerializeField] GameObject heroPrefab;

        bool _isContinue;
        MenuChooseHeroItemUI _selectedHero;

        #region Events
        void OnSelectedHero(MenuChooseHeroItemUI menuHeroUI)
        {
            GameObject background;

            if (_selectedHero != null)
            {
                background = _selectedHero.GetBackground();
                background.GetComponent<Image>().color = Color.white;
            }

            _selectedHero = menuHeroUI;

            background = _selectedHero.GetBackground();
            background.GetComponent<Image>().color = Color.yellow;
        }
        #endregion

        #region Buttons
        public void StartGame()
        {
            if (_selectedHero == null) return;
            
            GameManager.Instance.MenuManager.OpenLoadScreen();

            PlayerPrefs.SetString("character", _selectedHero.GetCharacterClass().name);

            CharacterClassesSO playerClass = Instantiate(Resources.Load<CharacterClassesSO>($"Classes/{_selectedHero.GetCharacterClass().name}"));

            GameManager.Instance.StartGame(playerClass);
        }

        #endregion

        void Start()
        {
            foreach (var character in charactersClasses)
            {
                GameObject instance = Instantiate(heroPrefab, container.transform);
                instance.GetComponent<MenuChooseHeroItemUI>().Init(character, OnSelectedHero);
            }
        }
    }
}