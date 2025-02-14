using TinyTrails.Managers;
using UnityEngine;

namespace TinyTrails.UI
{
    public class EndTurnUI : MonoBehaviour
    {
        private bool _canFinishTurn = true;
        public void EndTurn()
        {
            if (!_canFinishTurn) return;

            GameManager.Instance.TurnManager.EndTurn();
        }

        private void Start()
        {
            GameManager.Instance.EventManager.Subscriber(Types.EventChannelType.OnTurnPlayerStart, () => _canFinishTurn = true);
            GameManager.Instance.EventManager.Subscriber(Types.EventChannelType.OnTurnWorldStart, () => _canFinishTurn = false);
        }
    }
}