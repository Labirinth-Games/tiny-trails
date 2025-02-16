using TinyTrails.Interfaces;
using UnityEngine;

public class IteractableUI : MonoBehaviour
{
    [SerializeField] GameObject iteractable;

    void OnMouseDown()
    {
        iteractable.GetComponent<IInteractable>().Open();
    }
}