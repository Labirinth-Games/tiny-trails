using TinyTrails.Helpers;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.SO
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 1)]
    public class EnemySO : ScriptableObject
    {
        public string displayName;

        [Header("Stats Base")]
        public int hp;
        public int strength;
        public int Level = 1;
        public int movement;
        public DiceType diceDamage;
        public int actionPoints = 1;
        public int distanceAttack = 1;
        public bool isAggre;

        public int Damage
        {
            get
            {
                return strength + DiceHelper.Roll(diceDamage);
            }
        }
    }
}