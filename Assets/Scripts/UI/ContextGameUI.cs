using System.Collections;
using TinyTrails.Managers;
using TinyTrails.Types;
using TMPro;
using UnityEngine;
using DG.Tweening;


namespace TinyTrails.UI
{
    public class ContextGameUI : MonoBehaviour
    {
        [SerializeField] GameObject display;
        [SerializeField] TextMeshProUGUI label;

        void OnChangeContextGame(ContextGameType contextGameType)
        {
            StartCoroutine(DisplayTimer(contextGameType));
        }

        IEnumerator DisplayTimer(ContextGameType contextGameType)
        {
            display.SetActive(true);
            
            string text = contextGameType == ContextGameType.Battle ? "Start Game" : "End Game";
            label.text = text;

            yield return new WaitForSeconds(5f);
            display.SetActive(false);
        }

        void Start()
        {
            GameManager.Instance.EventManager.Subscriber<ContextGameType>(EventChannelType.OnContextGameChangeStatus, OnChangeContextGame);
        }
    }
}