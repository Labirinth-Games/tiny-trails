using TinyTrails.i18n;
using TinyTrails.Managers;
using TinyTrails.Types;
using TMPro;
using UnityEngine;

namespace TinyTrails.UI
{
    public class TurnLabelUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;

        private void SetTitleTurnPlayer() => label.text = MessageUI.TURN_PLAYER;
        private void SetTitleTurnWorld() => label.text = MessageUI.TURN_WORLD;

        void Start()
        {
            SetTitleTurnPlayer();
            GameManager.Instance.EventManager.Subscriber(EventChannelType.OnTurnPlayerStart, SetTitleTurnPlayer);
            GameManager.Instance.EventManager.Subscriber(EventChannelType.OnTurnWorldStart, SetTitleTurnWorld);
        }
    }
}