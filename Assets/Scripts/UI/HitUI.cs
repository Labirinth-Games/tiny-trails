using UnityEngine;
using DG.Tweening;
using TMPro;

namespace TinyTrails.UI
{
    public class HitUI : MonoBehaviour
    {
        [SerializeField] TextMeshPro label;

        public void Init(int damage, Vector2 position)
        {
            label.text = damage.ToString();
            transform.position = position;

            transform.DOMove(position + Vector2.up * .5f, .3f);
            Destroy(gameObject, 1f);
        }
    }
}