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
            if(_isPlayerTurn) GameManager.Instance.EventManager.Publisher(EventChannelType.OnTurnWorldStart);
            else GameManager.Instance.EventManager.Publisher(EventChannelType.OnTurnPlayerStart);

            _isPlayerTurn = !_isPlayerTurn;
        }
    }
}