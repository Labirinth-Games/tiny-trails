using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.Managers
{
    public class TurnManager : MonoBehaviour
    {
        private bool _isPlayerTurn = true;

        public bool IsTurnPlayer() => _isPlayerTurn;

        public void EndTurn()
        {
            if (_isPlayerTurn)
            {
                _isPlayerTurn = false;
                GameManager.Instance.EventManager.Publisher(EventChannelType.OnTurnWorldStart);

                return;
            }

            _isPlayerTurn = true;
            GameManager.Instance.EventManager.Publisher(EventChannelType.OnTurnPlayerStart);
        }
    }
}