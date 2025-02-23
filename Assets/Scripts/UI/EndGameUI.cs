using TinyTrails.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TinyTrails.UI
{
    public class EndGameUI : MonoBehaviour
    {
        [SerializeField] GameObject display;

        #region Button
        public void ExitGameButton()
        {
            display.SetActive(false);
            GameManager.Instance.EndGame();
        }
        #endregion

        void Start()
        {
            GameManager.Instance.EventManager.Subscriber(Types.EventChannelType.OnGameOver, () => display.SetActive(true));
        }
    }

}