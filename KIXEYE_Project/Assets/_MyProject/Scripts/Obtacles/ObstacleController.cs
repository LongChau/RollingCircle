using LC.Ultility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZ_Pooling;

namespace RollingCircle
{
    /// <summary>
    /// ObstacleController use to control obstacle in scene. 
    /// Detect player circle by raycast
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Movement))]
    public class ObstacleController : MonoBehaviour
    {
        [SerializeField] private EObstacleType m_obstacleType;

        /// <summary>
        /// distance for raycast detect player ball
        /// </summary>
        [SerializeField] private float m_distance;

        [SerializeField] private bool m_isCollided;

        private Rigidbody2D m_rigid2D;

        private Movement m_movement;

        private bool m_isRunning;

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            m_rigid2D = GetComponent<Rigidbody2D>();
            m_movement = GetComponent<Movement>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // This function is called when the object becomes enabled and active
        private void OnEnable()
        {
            if (m_obstacleType == EObstacleType.Runner && GameManager.Instance.IsGameStart)
                SoundManager.Instance.PlaySound(ESoundEffect.BusHorn);
        }

        private void FixedUpdate()
        {
            CheckRaycast();
        }

        /// <summary>
        /// use to detect player
        /// </summary>
        private void CheckRaycast()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, m_distance);
            Debug.DrawRay(transform.position, Vector2.up * m_distance, Color.red);

            if (hit.collider != null && !m_isCollided)
            {
                if (hit.collider.gameObject.CompareTag(GameTags.PLAYER))
                {
                    Log.Info($"Collide {hit.collider.name}");
                    m_isCollided = true;
                    OnRaycastToPlayer();
                }
            }
        }

        private void OnRaycastToPlayer()
        {
            Log.Info("ObstacleController.OnRaycastToPlayer()");
            this.PostEvent(EventID.OnRaycastToPlayer);
        }

        private void OnBecameInvisible()
        {
            ResetData();
        }

        /// <summary>
        /// Reset this to pool obj again
        /// </summary>
        private void ResetData()
        {
            //Log.Info("OnBecameInvisible()");
            m_isCollided = false;
            m_isRunning = false;
            EZ_PoolManager.Despawn(this.transform);
        }

        /// <summary>
        /// Set this movement speed
        /// </summary>
        /// <param name="speed"></param>
        public void SetMovementSpeed(float speed)
        {
            if (m_obstacleType == EObstacleType.Static)
                m_movement.speed = speed;

            m_isRunning = true;
        }
    }
}