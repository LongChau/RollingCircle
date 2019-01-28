using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RollingCircle
{
    /// <summary>
    /// Movement component
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        /// <summary>
        /// Moving direction
        /// </summary>
        public EMoveDirection moveDirection;

        public float speed;

        public bool isAbleToMove;

        private Rigidbody2D m_rigid2D;

        private Vector2 movementVec;

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            m_rigid2D = GetComponent<Rigidbody2D>();
        }

        // Start is called before the first frame update
        void Start()
        {
            movementVec = new Vector2((int)moveDirection * speed * Time.fixedDeltaTime, m_rigid2D.velocity.y);
        }

        private void FixedUpdate()
        {
            Moving();
        }

        private void Moving()
        {
            if (isAbleToMove)
                m_rigid2D.velocity = movementVec;
        }
    }
}