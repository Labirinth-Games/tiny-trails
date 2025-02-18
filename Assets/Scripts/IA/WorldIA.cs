using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinyTrails.Behaviours;
using TinyTrails.DTO;
using TinyTrails.Enemies;
using TinyTrails.Managers;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.IA
{
    public class WorldIA : MonoBehaviour
    {
        private List<Enemy> _enemies = new List<Enemy>();
        List<Enemy> _enemiesQueue = new List<Enemy>();

        public void Init()
        {
            // Subscribers
            GameManager.Instance.EventManager.Subscriber(EventChannelType.OnTurnWorldStart, OnStartTurnWorld);
            GameManager.Instance.EventManager.Subscriber(EventChannelType.OnEnemyFinishAction, OnEnemyStartAction);
        }

        #region Gets/Sets
        public void SetEnemy(Tile tile)
        {
            _enemies.Add((Enemy)tile.gameObject);
        }

        public void EnemyRemove(Enemy enemy)
        {
            _enemies.Remove(enemy);

            Destroy(enemy.gameObject);

            // quando todos os inimigos da zona é morto ele muda o estado do jogo
            if (_enemies.Count == 0)
                GameManager.Instance.EventManager.Publisher<ContextGameType>(EventChannelType.OnContextGameChangeStatus, ContextGameType.Explore);
        }
        #endregion


        #region Events
        private void OnStartTurnWorld()
        {
            if (_enemies.Count == 0)
            {
                GameManager.Instance.TurnManager.EndTurn();
                GameManager.Instance.EventManager.Publisher(EventChannelType.OnContextGameChangeStatus, ContextGameType.Explore);
                return;
            }

            _enemiesQueue.Clear();
            _enemiesQueue.AddRange(_enemies);

            OnEnemyStartAction();
        }

        void OnEnemyStartAction()
        {
            if (_enemiesQueue.Count == 0)
            {
                GameManager.Instance.TurnManager.EndTurn();
                return;
            }

            Enemy enemy = _enemiesQueue[Random.Range(0, _enemiesQueue.Count)];
            _enemiesQueue.Remove(enemy);

            // StartCoroutine(WorldRunActions(enemy));
            Debug.Log($"Enemy: {enemy.name}, irá iniciar a ação, AP {enemy.Stats.actionPoints}");
            enemy.ExecuteActions(enemy.Stats.actionPoints);
        }

        IEnumerator WorldRunActions(Enemy enemy)
        {
            yield return new WaitForSeconds(Random.Range(.2f, .5f));
        }
        #endregion

    }
}