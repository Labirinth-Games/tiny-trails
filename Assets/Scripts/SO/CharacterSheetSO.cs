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
        public CharacterClassesSO characterClasses;
        public AddittionalCharacterClassesSO additionalStats = new();

        [Header("Stats Base")]
        public DiceType diceDamage;

        #region Metodos Stats
        public int HP
        {
            get
            {
                return characterClasses.hp + additionalStats.hp;
            }
        }

        public int Movement
        {
            get
            {
                return characterClasses.movement + additionalStats.movement;
            }
        }

        public int DistanceAttack
        {
            get
            {
                return characterClasses.distanceAttack + additionalStats.distanceAttack;
            }
        }

        public int Focus
        {
            get
            {
                return characterClasses.focus + additionalStats.focus;
            }
        }
        public int Defense
        {
            get
            {
                return characterClasses.defense + additionalStats.defense;
            }
        }

        public int Damage
        {
            get
            {
                return characterClasses.strength + DiceHelper.Roll(diceDamage);
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
