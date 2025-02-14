using System.Collections.Generic;
using TinyTrails.Characters;
using TinyTrails.i18n;
using TinyTrails.Managers;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.Controllers
{
    public class ActionPointController : MonoBehaviour
    {
        [SerializeField] private int maxActionPoints;

        Player _player;
        int _actionPoints;
        ActionPointType _currentActionPointType;

        #region Validation
        bool CanApplyAction()
        {
            // in explore sucess
            List<ActionPointType> actionInExplore = new() {
                ActionPointType.Attack,
                ActionPointType.Move,
            };

            if (GameManager.Instance.ContextGameManager.IsExplore() && actionInExplore.Contains(_currentActionPointType)) return true;

            // in battle sucess
            List<ActionPointType> actionInBattle = new() {
                ActionPointType.Attack,
                ActionPointType.Move,
                ActionPointType.Concentrate,
                ActionPointType.Defense,
                ActionPointType.HeroicAction,
            };

            if (GameManager.Instance.ContextGameManager.IsBattle() && actionInBattle.Contains(_currentActionPointType)) return true;


            // in batte fail
            if (GameManager.Instance.ContextGameManager.IsBattle() && _actionPoints <= 0)
                GameManager.Instance.EventManager.Publisher<string>(EventChannelType.OnUILog, MessageError.ACTION_POINT_EMPTY);

            if (GameManager.Instance.ContextGameManager.IsBattle() && !GameManager.Instance.FocusController.CanUseFocus())
                GameManager.Instance.EventManager.Publisher<string>(EventChannelType.OnUILog, MessageError.INSUFFICIENT_FOCUS);

            return false;
        }
        #endregion

        #region Events
        private void OnTurnPlayer()
        {
            // reset actions
            _actionPoints = maxActionPoints;

            GameManager.Instance.EventManager.Publisher<int>(EventChannelType.OnUIRemainActionPointsChange, _actionPoints);
        }

        void OnActionPointUsed(ActionPointType actionPointType)
        {
            if (actionPointType == _currentActionPointType)
                _currentActionPointType = ActionPointType.None;

            if (GameManager.Instance.ContextGameManager.IsExplore()) return;

            _actionPoints -= 1;

            GameManager.Instance.EventManager.Publisher<int>(EventChannelType.OnUIRemainActionPointsChange, _actionPoints);

            if (_actionPoints == 0) GameManager.Instance.TurnManager.EndTurn();
        }
        #endregion


        #region Button
        public void MoveActionButton()
        {
            _currentActionPointType = ActionPointType.Move;

            bool canApplyAction = CanApplyAction();

            if (!canApplyAction) return;

            GameManager.Instance.MoveAction.Move();
        }

        public void AttackActionButton()
        {
            _currentActionPointType = ActionPointType.Attack;

            bool canApplyAction = CanApplyAction();

            if (!canApplyAction) return;

            GameManager.Instance.AttackAction.Attack();
        }

        public void HeroicActionButton()
        {
            GameManager.Instance.EventManager.Publisher<string>(EventChannelType.OnUILog, MessageError.ACTION_POINT_EMPTY);
        }

        public void FocusActionButton()
        {
            _currentActionPointType = ActionPointType.Concentrate;

            bool canApplyAction = CanApplyAction();

            if (!canApplyAction) return;


            GameManager.Instance.EventManager.Publisher<int>(EventChannelType.OnFocusAdd, 1);

            OnActionPointUsed(ActionPointType.Concentrate);
        }
        #endregion

        void Start()
        {
            _player = GameManager.Instance.Player;
            _actionPoints = maxActionPoints;

            GameManager.Instance.EventManager.Publisher<int>(EventChannelType.OnUIRemainActionPointsChange, maxActionPoints);

            // subscriptions
            GameManager.Instance.EventManager.Subscriber(EventChannelType.OnTurnPlayerStart, OnTurnPlayer);
            GameManager.Instance.EventManager.Subscriber<ActionPointType>(EventChannelType.OnActionFinish, OnActionPointUsed);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) MoveActionButton();
            if (Input.GetKeyDown(KeyCode.Alpha2)) AttackActionButton();
            if (Input.GetKeyDown(KeyCode.Alpha3)) HeroicActionButton();
            if (Input.GetKeyDown(KeyCode.Alpha4)) FocusActionButton();
        }
    }
}