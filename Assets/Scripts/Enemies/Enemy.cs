using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyTrails.Behaviours;
using TinyTrails.Characters;
using TinyTrails.i18n;
using TinyTrails.Managers;
using TinyTrails.Render;
using TinyTrails.SO;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.Enemies
{
    public class Enemy : TileBehaviour
    {
        [SerializeField] private EnemySO stats;
        [SerializeField] Animator animator;

        [Header("Audios")]
        [SerializeField] List<AudioClip> moveStepAudios;
        [SerializeField] List<AudioClip> attackAudios;

        public EnemySO Stats { get; private set; }

        bool _isFinishAction;
        int _amountRemaingActions;

        public void Init(Vector2 position)
        {
            transform.position = position;

            Tile.SetTileType(TileType.Enemy);
            Tile.gameObject = this;

            GameManager.Instance.MapManager.Register(transform.position, Tile);

            Stats = Instantiate(stats);
        }

        public void ExecuteActions(int remainActionPoints)
        {
            Player player = GameManager.Instance.Player;

            if (GameManager.Instance.IsPlayerDie) return;

            float distance = Vector2.Distance(player.transform.position, transform.position);
            _amountRemaingActions = remainActionPoints;

            if (remainActionPoints <= 1) _isFinishAction = true; // quando for a ultima acão do inimigo ele ira mudar a flag

            if (distance > 1) // esta a mais de um quadrado de distancia
            {
                Debug.Log($"Enemy: {name}, remain actions {remainActionPoints}, Action Actual Move");
                Move();
                return;
            }

            Debug.Log($"Enemy: {name}, remain actions {remainActionPoints}, Action Actual Attack");
            Attack();
        }

        #region Actions

        public override void Hit(int damage)
        {
            Stats.hp -= damage;

            StartCoroutine(HitBlinkEffect());

            // render damage hit
            UIRender.HitPushLabelUIRender(damage.ToString(), transform.position);

            if (Stats.hp <= 0) Death();

            base.Hit(damage);
        }

        /// <summary>
        /// Efeito de piscada branca do inimigo quando toma um hit
        /// </summary>
        /// <returns></returns>
        IEnumerator HitBlinkEffect()
        {
            GetComponent<SpriteRenderer>().material.SetFloat("_IsHit", 1f);
            yield return new WaitForSeconds(.2f);
            GetComponent<SpriteRenderer>().material.SetFloat("_IsHit", 0f);
        }

        protected virtual void Death()
        {
            if (Stats.hp > 0) return;

            int xp = GameManager.Instance.Settings.Rewards.BaseXpWhenMonsterDie * Stats.Level;
            GameManager.Instance.EventManager.Publisher<string>(EventChannelType.OnUILog, MessageGame.ENEMY_DIE_PLAYER_TAKE_XP(xp));

            GameManager.Instance.WorldManager.EnemyRemove(this);
            GameManager.Instance.MapManager.Unregister(transform.position, Tile);
        }

        protected virtual void Move()
        {
            List<Vector2> steps = GameManager.Instance.MapManager.Pathfinder(transform.position, GameManager.Instance.Player.transform.position, new List<TileType>() { TileType.Floor, TileType.Way, TileType.Trap }, Stats.movement);

            if (steps.Count == 0)
            {
                _isFinishAction = false;
                Debug.Log($"Enemy: {name}, terminou pq n tem steps no pathfinder");
                GameManager.Instance.EventManager.Publisher(EventChannelType.OnEnemyFinishAction);

                return;
            }

            steps.RemoveAt(steps.Count - 1);

            StartCoroutine(MoveStep(steps));
        }

        IEnumerator MoveStep(List<Vector2> positions)
        {
            animator.SetBool("Walk", true);

            foreach (var position in positions)
            {
                base.MoveTo(position);

                GameManager.Instance.AudioManager.Play(moveStepAudios[Random.Range(0, moveStepAudios.Count)]);

                yield return new WaitForSeconds(GameManager.Instance.Settings.VelocityMovementPieces);
            }

            if (_isFinishAction)
            {
                _isFinishAction = false;
                Debug.Log($"Enemy: {name}, terminou");
                animator.SetBool("Walk", false);
                GameManager.Instance.EventManager.Publisher(EventChannelType.OnEnemyFinishAction);
            }
            else
            {
                yield return new WaitForSeconds(GameManager.Instance.Settings.enemyWaitSecondsToNextAction);
                ExecuteActions(_amountRemaingActions - 1);
            }
        }

        protected virtual async Task Attack()
        {
            Player player = GameManager.Instance.Player;
            List<GameObject> instances = new();

            List<Vector2> positions = GameManager.Instance.MapManager.GetAround(transform.position, Stats.distanceAttack, new List<TileType>() { TileType.Floor, TileType.Way, TileType.Trap });
            instances = UIRender.HighLightAttackEnemyRender(positions, player.transform.position);

            // aguada um tempo depois de mostrar a marcação e da dano no player
            await Task.Delay(300);

            instances.ForEach(f => Destroy(f));

            animator.SetTrigger("Attack");
            player.gameObject.GetComponent<Player>().Hit(Stats.Damage);

            if (_isFinishAction)
            {
                _isFinishAction = false;
                Debug.Log($"Enemy: {name}, terminou");
                GameManager.Instance.EventManager.Publisher(EventChannelType.OnEnemyFinishAction);
            }
            else
            {
                await Task.Delay((int)(GameManager.Instance.Settings.enemyWaitSecondsToNextAction * 1000)); // wait to call next action
                ExecuteActions(_amountRemaingActions - 1);
            }
        }
        #endregion
    }
}