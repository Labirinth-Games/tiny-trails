using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TinyTrails.Behaviours;
using TinyTrails.Helpers;
using TinyTrails.i18n;
using TinyTrails.Managers;
using TinyTrails.Render;
using TinyTrails.SO;
using TinyTrails.Types;
using Unity.Cinemachine;
using UnityEngine;

namespace TinyTrails.Characters
{
    public class Player : TileBehaviour
    {
        public CharacterSheetSO Stats { get; private set; }
        protected int AdditionalStrength = 0;

        int _health;

        public void Init()
        {
            Stats = Instantiate(GameManager.Instance.characterSheet);
            Tile.SetTileType(TileType.Player);
            Tile.gameObject = this;
            GameManager.Instance.MapManager.Register(transform.position, Tile);

            _health = Stats.HP;

            // inicializer status
            GameManager.Instance.EventManager.Publisher<int>(EventChannelType.OnUIHPChange, _health);
            GameManager.Instance.EventManager.Publisher<int>(EventChannelType.OnUIFocusChange, Stats.Focus);

            GameObject.FindWithTag("Camera").GetComponent<CinemachineCamera>().Target.TrackingTarget = transform;

            // reset additional values
            GameManager.Instance.EventManager.Subscriber<int>(EventChannelType.OnTurnPlayerStart, (val) => AdditionalStrength = 0);
            GameManager.Instance.EventManager.Subscriber<Vector2>(EventChannelType.OnActionMove, Move);
            GameManager.Instance.EventManager.Subscriber<Vector2>(EventChannelType.OnActionAttack, Attack);
        }

        #region Gets/Sets
        public void SetHP(int val) => _health += val;
        public void SetAdditionalStrength(int val) => AdditionalStrength += val;
        #endregion

        #region Actions
        public override void Hit(int damage)
        {
            _health -= damage;

            StartCoroutine(HitBlinkEffect());

            // render damage hit
            UIRender.HitUIRender(damage, transform.position);

            GameManager.Instance.EventManager.Publisher<int>(EventChannelType.OnUIHPChange, _health);

            if (_health <= 0) Death();
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

        public void Death()
        {
            if (_health > 0) return;

            GameManager.Instance.EventManager.Publisher<string>(EventChannelType.OnUILog, "your death!!");
            GameManager.Instance.IsPlayerDie = true;

            Destroy(gameObject);
        }

        public void Move(Vector2 position)
        {
            // TODO - mover usando pathfinder assim fica mais sistematico e legal sem atravesar paredes
            List<Vector2> steps = GameManager.Instance.MapManager.Pathfinder(transform.position, position);
            StartCoroutine(MoveStep(steps));
        }

        IEnumerator MoveStep(List<Vector2> positions)
        {
            foreach (var position in positions)
            {
                base.MoveTo(position);
                yield return new WaitForSeconds(GameManager.Instance.Settings.VelocityMovementPieces);
            }

            yield return new WaitForSeconds(.5f);
            GameManager.Instance.EventManager.Publisher<ActionPointType>(EventChannelType.OnActionFinish, ActionPointType.Move);
        }

        public void Attack(Vector2 position)
        {
            // TODO - attack

            GameManager.Instance.EventManager.Publisher<ActionPointType>(EventChannelType.OnActionFinish, ActionPointType.Attack);
        }
        #endregion
    }
}