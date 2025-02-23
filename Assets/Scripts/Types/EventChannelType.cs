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
        OnUIDefenseChange,
        OnUIHighlightCancel, // quando cancela uma ação que usa highlight 
        OnUIHighlightOpen, // quando clica em ação que abre os highlights esse evento avisa sobre

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

        // Game State
        OnGameOver,
        OnGameWin,
    }
}