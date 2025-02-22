using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.Helpers
{
    public class RarityHelper
    {
        public static RarityType Rarity()
        {
            RarityType rarity = RarityType.Commom;
            float probability = Random.value;

            // escolhendo a tipo de raridade
            if (probability < .05f) rarity = RarityType.Divine;
            if (probability < .1f) rarity = RarityType.None; // n encontra nada
            if (probability < .25f) rarity = RarityType.Epic;
            else if (probability < .5f) rarity = RarityType.Rare;

            return rarity;
        }
    }
}