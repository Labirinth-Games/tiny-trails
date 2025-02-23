using TinyTrails.Managers;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.UI
{
    public class MainMenuUI : MenuUI
    {
        [SerializeField] GameObject continueOptionUI;

        bool _isContinue;

        void Start()
        {
            // escondendo o botao de continue quando for o primeiro game
            continueOptionUI.SetActive(_isContinue);
        }

        #region Button
        public void StartGameButton() => GameManager.Instance.MenuManager.Navigate(Types.MenuType.Choose_Hero);

        public void TutorialButton() => GameManager.Instance.MenuManager.Navigate(Types.MenuType.Tutorial);

        public void ExitButton() {}
        #endregion
    }
}