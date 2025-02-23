using TinyTrails.Managers;
using UnityEngine;

namespace TinyTrails.Enemies
{

    public class Boss : Enemy
    {
        protected override void Death()
        {
            base.Death();

            GameManager.Instance.EventManager.Publisher(Types.EventChannelType.OnGameWin);
        }
    }
}