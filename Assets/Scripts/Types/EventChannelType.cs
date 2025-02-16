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
        OnUIDefenseChange,

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

        // Chest
        OnChestObtainItem,
        OnChestObtainItemClose,

        // Trap
        OnTrapTriggerActive,
    }
}