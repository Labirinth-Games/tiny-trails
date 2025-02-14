using System.Collections.Generic;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.SO
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/Game Settings", order = 1)]
    public class GameSettingsSO : ScriptableObject
    {
        public DifficultyType difficulty;

        [Space()]
        [Header("Map")]
        public int mapWidthSize = 8;
        public int mapHeightSize = 12;
        public int minSizeRoom = 3;
        public int maxCells = 12;

        [Space()]
        [Header("Rewards")]
        public RewardsSO Rewards;

        [Space()]
        [Header("Enemies")]
        public float enemyWaitSecondsToNextAction;

        [Space()]
        [Header("World")]
        public float VelocityMovementPieces = .5f;
        public int limitToExploreMap = 3;
        public List<(DifficultyType difficulty, int amountEnemies)> AmountEnemiesByDifficulty = new List<(DifficultyType difficulty, int amountEnemies)>() {
            (DifficultyType.Low, 2),
            (DifficultyType.Normal, 4),
            (DifficultyType.High, 6),
            (DifficultyType.Extreme, 8),
        };

        [Space()]
        [Header("Focus Points to Actions")]
        public int costFocusToAttack = 1;
        public int costFocusToMove = 1;
        public int costFocusToDefense = 1;
        public int costFocusToHeroic = 3;
        public int costFocusToAntecipation = 2;
        public int costFocusToItem = 1;
        public int costFocusToConcentrate = 0;

    }
}