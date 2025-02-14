using System.Threading.Tasks;
using DG.Tweening;
using TinyTrails.Managers;
using TMPro;
using UnityEngine;

namespace TinyTrails.UI
{
    public class DiceItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;

        public void SetValue(int value)
        {
            label.text = value.ToString();
            Destroy(gameObject, 2);
        }
    }
}