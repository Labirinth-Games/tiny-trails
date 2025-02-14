using TinyTrails.i18n;
using TinyTrails.Managers;
using TinyTrails.SO;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.Controllers
{
    public class FocusController : MonoBehaviour
    {
        int _focus;

        public void Init(int focus)
        {
            _focus = focus;

            // Subscribers
            GameManager.Instance.EventManager.Subscriber<int>(EventChannelType.OnFocusReduce, OnReduceFocus);
            GameManager.Instance.EventManager.Subscriber<int>(EventChannelType.OnFocusAdd, OnAddFocus);
        }

        #region Gets/Sets
        public void SetFocus(int amount) => _focus += amount;
        #endregion

        #region Events
        void OnReduceFocus(int amount)
        {
            if (GameManager.Instance.ContextGameManager.IsExplore()) return;

            _focus -= amount;

            GameManager.Instance.EventManager.Publisher<int>(EventChannelType.OnUIFocusChange, _focus);
        }
        void OnAddFocus(int amount)
        {
            if (GameManager.Instance.ContextGameManager.IsExplore()) return;

            if (_focus + amount > GameManager.Instance.Player.Stats.Focus)
            {
                GameManager.Instance.EventManager.Publisher<string>(EventChannelType.OnUILog, MessageError.MAX_FOCUS);
                return;
            }

            _focus += amount;

            GameManager.Instance.EventManager.Publisher<int>(EventChannelType.OnUIFocusChange, _focus);
        }
        #endregion

        #region Validations
        public bool CanUseFocus() => _focus > 0;
        #endregion
    }
}