using TinyTrails.Managers;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.Helpers
{
    public class DiceHelper
    {
        public static int Roll(DiceType dice = DiceType.D4, bool notifyUI = true)
        {
            int value = Random.Range(1, (int)dice);

            if (notifyUI) GameManager.Instance.EventManager.Publisher<int>(EventChannelType.OnUIRollDiceNotification, value);

            return value;
        }
    }
}