using System.Collections.Generic;
using TinyTrails.Characters;
using TinyTrails.i18n;
using TinyTrails.Managers;
using TinyTrails.Render;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.Controllers
{
    public class ActionPointController : MonoBehaviour
    {
        [SerializeField] private int maxActionPoints;

        Player _player;
        int _actionPoints;
        (ActionPointType actionType, bool isFinish) _currentAction;
        int _focusUsedInTurn;
        int _defenseUsedInTurn;

        #region Validation
        bool CanApplyAction(ActionPointType actionPointType)
        {
            if (!GameManager.Instance.TurnManager.IsTurnPlayer()) return false;

            if (_currentAction.actionType == actionPointType && !_currentAction.isFinish) return false;

            if (!_currentAction.isFinish) return false;

            // in batte fail
            if (GameManager.Instance.ContextGameManager.IsBattle() && _actionPoints <= 0)
            {
                GameManager.Instance.EventManager.Publisher<string>(EventChannelType.OnUILog, MessageError.ACTION_POINT_EMPTY);
                GameManager.Instance.AudioManager.UIAudio.OutResources();
                return false;
            }

            if (GameManager.Instance.ContextGameManager.IsBattle() && actionPointType == ActionPointType.Concentrate && _focusUsedInTurn >= GameManager.Instance.Settings.limitFocusCanUseByTurn)
            {
                GameManager.Instance.EventManager.Publisher<string>(EventChannelType.OnUILog, MessageError.LIMIT_USAGE_FOCUS);
                GameManager.Instance.AudioManager.UIAudio.OutResources();
                return false;
            }

            if (GameManager.Instance.ContextGameManager.IsBattle() && actionPointType == ActionPointType.Defense && _defenseUsedInTurn >= GameManager.Instance.Settings.limitDefenseCanUseByTurn)
            {
                GameManager.Instance.EventManager.Publisher<string>(EventChannelType.OnUILog, MessageError.LIMIT_USAGE_DEFENSE);
                GameManager.Instance.AudioManager.UIAudio.OutResources();
                return false;
            }

            if (GameManager.Instance.ContextGameManager.IsBattle() && actionPointType != ActionPointType.Concentrate && !GameManager.Instance.FocusController.CanUseFocus())
            {
                GameManager.Instance.EventManager.Publisher<string>(EventChannelType.OnUILog, MessageError.INSUFFICIENT_FOCUS);
                GameManager.Instance.AudioManager.UIAudio.OutResources();
                return false;
            }

            // in explore sucess
            List<ActionPointType> actionInExplore = new() {
                ActionPointType.Attack,
                ActionPointType.Move,
            };

            if (GameManager.Instance.ContextGameManager.IsExplore() && actionInExplore.Contains(actionPointType)) return true;

            // in battle sucess
            List<ActionPointType> actionInBattle = new() {
                ActionPointType.Attack,
                ActionPointType.Move,
                ActionPointType.Concentrate,
                ActionPointType.Defense,
                ActionPointType.HeroicAction,
            };

            if (GameManager.Instance.ContextGameManager.IsBattle() && actionInBattle.Contains(actionPointType)) return true;

            return false;
        }
        #endregion

        #region Events
        private void OnTurnPlayer()
        {
            // reset actions
            _actionPoints = maxActionPoints;
            _focusUsedInTurn = 0;
            _defenseUsedInTurn = 0;

            GameManager.Instance.EventManager.Publisher<int>(EventChannelType.OnFocusAdd, 1); // ganha 1 foco quando começa o turno

            GameManager.Instance.EventManager.Publisher<int>(EventChannelType.OnUIRemainActionPointsChange, _actionPoints);
        }

        void OnActionPointUsed(ActionPointType actionPointType)
        {
            _currentAction.isFinish = true;
            _currentAction.actionType = ActionPointType.None;

            if (GameManager.Instance.ContextGameManager.IsExplore()) return;

            _actionPoints -= 1;

            GameManager.Instance.EventManager.Publisher<int>(EventChannelType.OnUIRemainActionPointsChange, _actionPoints);

            if (_actionPoints == 0) GameManager.Instance.TurnManager.EndTurn();
        }
        #endregion


        #region Button
        public void MoveActionButton()
        {
            bool canApplyAction = CanApplyAction(ActionPointType.Move);

            if (!canApplyAction) return;

            _currentAction.actionType = ActionPointType.Move;
            _currentAction.isFinish = false;

            GameManager.Instance.MoveAction.Action();
        }

        public void AttackActionButton()
        {

            bool canApplyAction = CanApplyAction(ActionPointType.Attack);

            if (!canApplyAction) return;

            _currentAction.isFinish = false;
            _currentAction.actionType = ActionPointType.Attack;

            GameManager.Instance.AttackAction.Action();
        }

        public void DefenseActionButton()
        {

            bool canApplyAction = CanApplyAction(ActionPointType.Defense);

            if (!canApplyAction) return;

            _currentAction.isFinish = false;
            _currentAction.actionType = ActionPointType.Defense;

            GameManager.Instance.DefenseAction.Action();
        }

        public void HeroicActionButton()
        {
            // _stateAction.isFinish = false;
            // _stateAction.actionType = ActionPointType.HeroicAction;

            GameManager.Instance.EventManager.Publisher<string>(EventChannelType.OnUILog, MessageError.ACTION_POINT_EMPTY);
        }

        public void FocusActionButton()
        {
            bool canApplyAction = CanApplyAction(ActionPointType.Concentrate);

            if (!canApplyAction) return;

            _currentAction.isFinish = false;
            _currentAction.actionType = ActionPointType.Concentrate;
            _focusUsedInTurn += 1;

            GameManager.Instance.EventManager.Publisher<int>(EventChannelType.OnFocusAdd, 1);

            UIRender.FocusPushLabelUIRender(GameManager.Instance.Player.transform.position);

            GameManager.Instance.EventManager.Publisher<ActionPointType>(EventChannelType.OnActionFinish, ActionPointType.Concentrate);
        }

        public void ItemActionButton()
        {
            bool canApplyAction = CanApplyAction(ActionPointType.UseItem);

            if (!canApplyAction) return;

            _currentAction.isFinish = false;
            _currentAction.actionType = ActionPointType.UseItem;
            
            GameManager.Instance.EventManager.Publisher<int>(EventChannelType.OnFocusReduce, GameManager.Instance.Settings.costFocusToItem);
            GameManager.Instance.EventManager.Publisher<ActionPointType>(EventChannelType.OnActionFinish, ActionPointType.UseItem);
        }
        #endregion

        void Start()
        {
            _player = GameManager.Instance.Player;
            _actionPoints = maxActionPoints;
            _currentAction = (ActionPointType.None, true);
            _focusUsedInTurn = 0;
            _defenseUsedInTurn = 0;

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