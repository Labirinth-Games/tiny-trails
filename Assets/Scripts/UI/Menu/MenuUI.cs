using UnityEngine;

namespace TinyTrails.UI
{
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] protected GameObject display;

        public virtual void Open() => display.SetActive(true);
        public virtual void Close() => display.SetActive(false);
    }

}