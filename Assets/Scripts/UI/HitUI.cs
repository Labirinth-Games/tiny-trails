using UnityEngine;
using DG.Tweening;
using TMPro;

namespace TinyTrails.UI
{
    public class PushLabelUI : MonoBehaviour
    {
        [SerializeField] TextMeshPro label;

        public void Init(string value, Vector2 position)
        {
            label.text = value;
            transform.position = position;

            transform.DOMove(position + Vector2.up * .7f, .3f);
            Destroy(gameObject, 1f);
        }
    }
}