using TinyTrails.Helpers;
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

        [Header("Stats Base")]
        public DiceType diceDamage;

        #region Metodos Stats
        public int HP
        {
            get
            {
                return characterClasses.hp;
            }
        }

        public int Movement
        {
            get
            {
                return characterClasses.movement;
            }
        }

        public int DistanceAttack
        {
            get
            {
                return characterClasses.distanceAttack;
            }
        }

        public int Focus
        {
            get
            {
                return characterClasses.focus;
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
        public void SetHP(int hp) => characterClasses.hp += hp;
        public void ReduceHP(int hp) => characterClasses.hp -= hp;
        #endregion
    }
}
