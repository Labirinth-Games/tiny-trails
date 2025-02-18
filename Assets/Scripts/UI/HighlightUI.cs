using DG.Tweening;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.UI
{
    public class HighLightUI : MonoBehaviour
    {
        [SerializeField] GameObject overlayerPrefab;

        ActionPointType _actionPointType;
        System.Action<Vector2> OnSelect;

        public void Init(Vector2 position, System.Action<Vector2> OnSelect)
        {
            transform.position = position;
            this.OnSelect = OnSelect;

            // animation
            transform.DOScale(.3f, .3f).From();
        }

        public GameObject GetOverlay() => overlayerPrefab;

        #region Event Mouse
        void OnMouseDown()
        {
            OnSelect?.Invoke(transform.position);
        }

        void OnMouseEnter()
        {
            overlayerPrefab.SetActive(true);
        }

        void OnMouseExit()
        {
            overlayerPrefab.SetActive(false);
        }
        #endregion
    }
}