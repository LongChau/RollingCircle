using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LC.Ultility;
using System;
using EZ_Pooling;
using UnityEngine.EventSystems;

namespace RollingCircle
{
    /// <summary>
    /// Control the circle, input, handle events etc.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpinEffect))]
    public class RollingBombController : MonoBehaviour
    {
        [SerializeField] private float m_jumpForce;

        //[SerializeField] private float m_speed;

        [SerializeField] private bool m_isJumping = false;
        [SerializeField] private bool m_isDead = false;

        /// <summary>
        /// Explosion prefab
        /// </summary>
        [SerializeField] private GameObject m_explodeObj;

        private Rigidbody2D m_rigid2D;

        private SpinEffect m_spinEffect;

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            m_rigid2D = GetComponent<Rigidbody2D>();
            m_spinEffect = GetComponent<SpinEffect>();
        }

        // Start is called just before any of the Update methods is called the first time
        private void Start()
        {

        }

        // Update is called every frame, if the MonoBehaviour is enabled
        private void Update()
        {
            if (m_isDead)
                return;

            // when user touch the screen then circle will jump
            if (Input.GetMouseButtonDown(0))
            {
                // check if user point on UI
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                Jump();
            }
        }

        // This function is called when the behaviour becomes disabled or inactive
        private void OnDisable()
        {
            EventManager.Instance?.RemoveListener(EventID.OnHardLimitReach, Handle_OnHardLimitReach);
        }

        // This function is called when the object becomes enabled and active
        private void OnEnable()
        {
            this.RegisterListener(EventID.OnHardLimitReach, Handle_OnHardLimitReach);
        }

        private void Handle_OnHardLimitReach(object obj)
        {
            Log.Info("RollingBombController.Handle_OnHardLimitReach");

            // "-=" for rotate "right"
            m_spinEffect.rotateSpeed -= GameConst.BALLROTATE_ADDED;
        }

        // OnCollisionEnter2D is called when this collider2D/rigidbody2D has begun touching another rigidbody2D/collider2D (2D physics only)
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var objCol = collision.gameObject;

            // if collide LAND then circle can jump again
            if (objCol.CompareTag(GameTags.LAND))
            {
                m_isJumping = false;
            }

            // if  collide obstacle. This circle 'll explode and die
            else if (objCol.CompareTag(GameTags.OBSTACLE))
            {
                m_isDead = true;

                // get the explode prefab from pool,then show it.
                var spawnedObj = EZ_PoolManager.Spawn(m_explodeObj.transform, transform.position, transform.rotation);

                // deactivate circle
                this.gameObject.SetActive(false);

                // check the score. Get the high score
                if (PlayerData.Instance.Player.Score >= PlayerData.Instance.Player.HighScore)
                    PlayerData.Instance.Player.HighScore = PlayerData.Instance.Player.Score;

                this.PostEvent(EventID.OnPlayerDie);
            }
        }

        #region Player funcs
        /// <summary>
        /// Make circle jumping
        /// </summary>
        private void Jump()
        {
            if (m_isJumping)
                return;

            Log.Info("RollingBombController.Jump()");

            SoundManager.Instance.PlaySound(ESoundEffect.GunShot);

            m_rigid2D.AddForce(new Vector2(m_rigid2D.velocity.x, m_jumpForce), ForceMode2D.Impulse);
            m_isJumping = true;
        }
        #endregion
    }
}