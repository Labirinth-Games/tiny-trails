using TinyTrails.SO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TinyTrails.UI
{
    public class MenuHeroUI : MonoBehaviour
    {
        [SerializeField] GameObject background;
        [SerializeField] Image preview;
        [SerializeField] TextMeshProUGUI title;

        System.Action<MenuHeroUI> OnSelectItem;
        CharacterClassesSO _characterClassesSO;

        public void Init(CharacterClassesSO characterClassesSO, System.Action<MenuHeroUI> OnSelectItem)
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
           OnSelectItem?.Invoke(this);
        }
    }
}