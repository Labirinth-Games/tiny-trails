using UnityEngine;

namespace TinyTrails.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] GameObject display;
        [SerializeField] GameObject continueOptionUI;

        [Header("Menus")]
        [SerializeField] GameObject ChooseHeroMenu;
        [SerializeField] GameObject tutorialMenu;

        bool _isContinue;

        void Start()
        {
            // escondendo o botao de continue quando for o primeiro game
            continueOptionUI.SetActive(_isContinue);
        }

        #region Button
        public void StartGameButton()
        {
            ChooseHeroMenu.SetActive(true);
            display.SetActive(false);
        }

        public void TutorialButton()
        {
            tutorialMenu.SetActive(true);
            display.SetActive(false);
        }

        public void ExitButton() {}
        #endregion
    }
}