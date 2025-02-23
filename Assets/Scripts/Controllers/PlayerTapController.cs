using TinyTrails.Managers;
using UnityEngine;

namespace TinyTrails.Helpers
{
    public class PlayerTapController : MonoBehaviour
    {
        void OnMouseDown()
        {
            GameManager.Instance.ActionPointController.MoveActionButton();
        }
    }

}