using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.SO
{
    [CreateAssetMenu(fileName = "Rewards", menuName = "ScriptableObjects/Rewards Config", order = 2)]
    public class RewardsSO : ScriptableObject
    {
       // xp monsters
       public int BaseXpWhenMonsterDie = 2;
       public int XpBoss = 10;
    }
}