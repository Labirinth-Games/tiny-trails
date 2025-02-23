using TinyTrails.Managers;
using UnityEngine;

namespace TinyTrails.UI
{
    public class WinGameUI : MonoBehaviour
    {
        [SerializeField] GameObject display;

        #region Button
        public void ExitGameButton()
        {
            display.SetActive(false);
            GameManager.Instance.MenuManager.CloseGameHud();
            GameManager.Instance.MenuManager.Navigate(Types.MenuType.Main_Menu);
        }
        #endregion

        void Start()
        {
            GameManager.Instance.EventManager.Subscriber(Types.EventChannelType.OnGameWin, () => display.SetActive(true));
        }
    }

}