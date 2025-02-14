using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.Managers
{
    public class ContextGameManager : MonoBehaviour
    {
        ContextGameType _currentContextGame;

        public void Init()
        {
            _currentContextGame = ContextGameType.Explore;

            // Subscribers
            GameManager.Instance.EventManager.Subscriber<ContextGameType>(EventChannelType.OnContextGameChangeStatus, OnChangeContextGame);
        }

        void OnChangeContextGame(ContextGameType context)
        {
            if (context == _currentContextGame) return;

            Debug.Log($"Change context game to {context}");
            
            _currentContextGame = context;
        }

        #region Validation
        public bool IsBattle() => _currentContextGame == ContextGameType.Battle;
        public bool IsExplore() => _currentContextGame == ContextGameType.Explore;
        #endregion
    }
}