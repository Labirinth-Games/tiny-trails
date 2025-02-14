namespace TinyTrails.Types
{
    public enum EventChannelType
    {
        // Turn Event
        OnTurnPlayerStart,
        OnTurnWorldStart,

        // Notification UI Event
        OnUIActionPointErrorNotification,
        OnUIRollDiceNotification,
        OnUIHPChange,
        OnUIRemainActionPointsChange,
        OnUILog,
        OnUIFocusChange,
        OnUIHighlightRemove,

        // Action
        OnActionFinish,
        OnActionMove,
        OnActionAttack,

        // Action Move
        OnMoveActionHighlightRemove,
        OnMoveActionHighlightSelected,

        // Focus
        OnFocusReduce,
        OnFocusAdd,

        // Context game
        OnContextGameChangeStatus,

        // Enemy
        OnEnemyFinishAction,
    }
}