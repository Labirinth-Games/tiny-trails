using System.Collections;
using System.Collections.Generic;
using TinyTrails.SO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TinyTrails.UI
{
    public class MenuChooseHeroUI : MonoBehaviour
    {
        [SerializeField] GameObject container;
        [SerializeField] GameObject loadScreen;
        [SerializeField] List<CharacterClassesSO> charactersClasses;
        [SerializeField] GameObject heroPrefab;

        bool _isContinue;
        MenuHeroUI _selectedHero;

        #region Events
        void OnSelectedHero(MenuHeroUI menuHeroUI)
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
            
            loadScreen.SetActive(true);

            PlayerPrefs.SetString("character", _selectedHero.GetCharacterClass().name);

            StartCoroutine(LoadSceneAsync());
        }

        IEnumerator LoadSceneAsync()
        {
            AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);

            while (!asyncLoadLevel.isDone)
                yield return null;

            yield return new WaitForEndOfFrame();
        }
        #endregion

        void Start()
        {
            foreach (var character in charactersClasses)
            {
                GameObject instance = Instantiate(heroPrefab, container.transform);
                instance.GetComponent<MenuHeroUI>().Init(character, OnSelectedHero);
            }
        }
    }
}