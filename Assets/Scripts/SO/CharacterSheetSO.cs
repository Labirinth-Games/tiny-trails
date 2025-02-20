using TinyTrails.Helpers;
using TinyTrails.Managers;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.SO
{
    [CreateAssetMenu(fileName = "CharacterSheet", menuName = "ScriptableObjects/Character", order = 1)]
    public class CharacterSheetSO : ScriptableObject
    {
        public string displayName;
        public GameObject body;
        public CharacterClassesSO characterClass;
        public AddittionalStats additionalStats = new();

        #region Metodos Stats
        public int HP
        {
            get
            {
                return characterClass.hp + additionalStats.hp;
            }
        }

        public int Movement
        {
            get
            {
                return characterClass.movement + additionalStats.movement;
            }
        }

        public int DistanceAttack
        {
            get
            {
                return characterClass.distanceAttack + additionalStats.distanceAttack;
            }
        }

        public int Focus
        {
            get
            {
                return characterClass.focus + additionalStats.focus;
            }
        }
        public int Defense
        {
            get
            {
                return characterClass.defense + additionalStats.defense;
            }
        }

        public int Damage
        {
            get
            {
                return characterClass.strength + DiceHelper.Roll(characterClass.power);
            }
        }
        #endregion

        #region Sets
        public void SetHP(int hp) => additionalStats.hp += hp;
        public void ReduceHP(int hp) => additionalStats.hp -= hp;
        public void SetFocus(int value)
        {
            additionalStats.focus += value;
            GameManager.Instance.EventManager.Publisher<int>(EventChannelType.OnUIFocusChange, Focus);
        }
        public void SetDefense(int value)
        {
            additionalStats.defense += value;
            GameManager.Instance.EventManager.Publisher<int>(EventChannelType.OnUIDefenseChange, Defense);
        }
        #endregion
    }
}
