using TinyTrails.Managers;
using UnityEngine;

namespace TinyTrails.Helpers
{
    public class EnemyTapController : MonoBehaviour
    {
        void OnMouseDown()
        {
            GameManager.Instance.ActionPointController.AttackActionButton();
        }
    }

}