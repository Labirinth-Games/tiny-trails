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

        public int MaxDistanceAttack
        {
            get
            {
                return characterClass.maxDistanceAttack + additionalStats.maxDistanceAttack;
            }
        }

        public int MinDistanceAttack
        {
            get
            {
                return characterClass.minDistanceAttack + additionalStats.minDistanceAttack;
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
            set
            {
                Defense = value;
            }
        }

        public int Damage
        {
            get
            {
                return characterClass.strength + additionalStats.strength + DiceHelper.Roll(characterClass.power);
            }
        }
        #endregion

        #region Sets
        public void SetHP(int hp) => additionalStats.hp += hp;
        public void SetStrength(int value) => additionalStats.strength += value;
        public void SetMovement(int value) => additionalStats.movement += value;
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

        public int ReduceDefense(int value)
        {
            int remainDamager = 0;

            if (value <= additionalStats.defense)
            {
                additionalStats.defense -= value;
            }
            else if (value - additionalStats.defense <= characterClass.defense)
            {
                additionalStats.defense = 0;
                characterClass.defense -= value;
            }
            else
            {
                additionalStats.defense = 0;
                characterClass.defense = 0;

                remainDamager = value - characterClass.defense - additionalStats.defense;
            }

            GameManager.Instance.EventManager.Publisher<int>(EventChannelType.OnUIDefenseChange, Defense);

            return remainDamager;
        }
        #endregion
    }
}
