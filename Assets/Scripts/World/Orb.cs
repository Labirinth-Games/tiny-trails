using System.Collections;
using DG.Tweening;
using TinyTrails.Behaviours;
using TinyTrails.Managers;
using TinyTrails.Render;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.World
{
    public class Orb : TileBehaviour
    {
        [Header("References")]
        [SerializeField] Sprite projectileSprite;

        [Header("Settings")]
        [SerializeField] int health;

        [Header("Audio")]
        [SerializeField] AudioClip projectileAudio;

        #region Projectile
        GameObject CreateProjectile()
        {
            GameObject projectile = new GameObject("Projectile Orb", typeof(SpriteRenderer), typeof(BoxCollider2D));

            SpriteRenderer spriteRenderer = projectile.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = projectileSprite;

            BoxCollider2D boxCollider2D = projectile.GetComponent<BoxCollider2D>();
            boxCollider2D.size = Vector2.one;
            boxCollider2D.isTrigger = true;

            projectile.transform.SetParent(gameObject.transform);
            projectile.transform.position = transform.position;

            float distance = Vector2.Distance(GameManager.Instance.Player.transform.position, transform.position);

            Vector2 direction = GameManager.Instance.Player.transform.position - projectile.transform.position;
            projectile.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);

            // audio
            GameManager.Instance.AudioManager.Play(projectileAudio);

            projectile.transform.DOMove(GameManager.Instance.Player.transform.position, .2f).OnComplete(() =>
            {
                Destroy(projectile);
                GameManager.Instance.Player.Hit(3);
            });

            return projectile;
        }
        #endregion

        #region Events
        void OnPlayerActiveTrap()
        {
            if (this == null) return;
            CreateProjectile();
        }
        #endregion

        public override void Hit(int damage)
        {
            StartCoroutine(HitBlinkEffect());

            UIRender.HitPushLabelUIRender(damage, transform.position);

            health -= damage;

            if (health <= 0) Destroy(gameObject);

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

        void Start()
        {
            Tile.SetTileType(TileType.Enemy);
            Tile.gameObject = this;
            GameManager.Instance.MapManager.Register(transform.position, Tile);

            GameManager.Instance.EventManager.Subscriber(EventChannelType.OnTrapTriggerActive, OnPlayerActiveTrap);
        }
    }
}