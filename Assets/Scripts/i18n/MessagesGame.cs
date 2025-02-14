namespace TinyTrails.i18n
{
    public struct MessageGame
    {
        // Door interation
        public static string DOOR_IS_FAR_PLAYER = "You are far from the door!";
        public static string DOOR_IS_LOCKED = "Door its locked!";
        public static string DOOR_IS_OPENED = "You opened the door..";

        // Explore
        public static string EXPLORE_SEARCHING(int attemp) => $"Searching..{attemp}";
        public static string EXPLORE_NOT_CAN_MORE_SEARCHING = "You no can more searching :/";
        public static string EXPLORE_NOT_FOUND = "Oops, nothing was found";
        public static string EXPLORE_ENEMY_APPEARED = "An enemy appeared!";
        public static string EXPLORE_ITEM_FOUNDED = "An item has funded!";

        // Items
        public static string ITEM_OPEN_CHEST = "Chest is opened!";

        // enemies
        public static string ENEMY_HIT(string damage, string enemy) => $"You attacked {enemy} with {damage} damage!";
        public static string ENEMY_HIT_MISS(string enemy) => $"You missed the attack in {enemy}";
        public static string ENEMY_HIT_CRITICAL(string enemy) => $"Woow! critical hit in {enemy}";
        public static string ENEMY_PLAYER_IS_FAR = $"The monster is far away from you.";
        public static string ENEMY_DIE_PLAYER_TAKE_XP(int xp) => $"The monster is die, you receive {xp}xp.";

        // player
        public static string PLAYER_HIT(string damage) => $"You reiceved {damage} damage!";
        public static string PLAYER_HIT_MISS = $"You evaded the attack!";
        public static string PLAYER_HIT_CRITICAL = $"No! it was a critical blow";
    }
}