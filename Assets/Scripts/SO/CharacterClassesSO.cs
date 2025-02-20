using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.SO
{
    [CreateAssetMenu(fileName = "CharacterClassesSO", menuName = "ScriptableObjects/CharacterClasses", order = 0)]
    public class CharacterClassesSO : ScriptableObject
    {
        public ClassesType classesType;
        public GameObject prefab;
        public Sprite sprite;
        public int distanceAttack;
        public int hp;
        public int strength;
        public DiceType power;
        public int luck;
        public int focus;
        public int defense;
        public int movement;
    }
    
    [System.Serializable]
    public class AddittionalStats
    {
        public int distanceAttack;
        public int hp;
        public int strength;
        public int luck;
        public int focus;
        public int defense;
        public int movement;
    }
}
