using TinyTrails.SO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TinyTrails.UI
{
    public class MenuChooseHeroItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField] GameObject background;
        [SerializeField] Image preview;
        [SerializeField] TextMeshProUGUI title;

        System.Action<MenuChooseHeroItemUI> OnSelectItem;
        CharacterClassesSO _characterClassesSO;

        public void Init(CharacterClassesSO characterClassesSO, System.Action<MenuChooseHeroItemUI> OnSelectItem)
        {
            this.OnSelectItem = OnSelectItem;
            _characterClassesSO = characterClassesSO;

            preview.sprite = characterClassesSO.sprite;
            title.text = characterClassesSO.classesType.ToString();
        }

        public GameObject GetBackground() => background;
        public CharacterClassesSO GetCharacterClass() => _characterClassesSO;

        public void SelectHeroButton()
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // OnSelectItem?.Invoke(this);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            // OnSelectItem?.Invoke(this);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            OnSelectItem?.Invoke(this);
        }
    }
}