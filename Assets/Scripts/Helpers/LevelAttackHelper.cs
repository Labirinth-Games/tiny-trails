using TinyTrails.Types;

namespace TinyTrails.Helpers
{
    public class LevelAttackHelper
    {
        public static (int, bool isMiss, bool isCritical) AnalyzeHit(int damage, DiceType dice)
        {
            if (damage == (int)dice) return (damage + 1, false, true); // critical hit
            if (damage == 1) return (0, true, false); // miss

            return (damage, false, false);
        }
    }
}