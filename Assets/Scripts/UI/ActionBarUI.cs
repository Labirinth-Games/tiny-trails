using TinyTrails.Managers;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.UI
{
    public class ActionBarUI : MonoBehaviour
    {
        [SerializeField] GameObject display;

        void Start()
        {
            // aparece a barra de açoes quando o jogador entra em batalha
            GameManager.Instance.EventManager.Subscriber<ContextGameType>(Types.EventChannelType.OnContextGameChangeStatus, (context) => display.SetActive(context == ContextGameType.Battle));
        }
    }

}