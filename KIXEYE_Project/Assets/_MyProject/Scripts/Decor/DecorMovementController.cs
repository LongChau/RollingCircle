using LC.Ultility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RollingCircle
{
    /// <summary>
    /// Decor control. Use to reset decor position
    /// </summary>
    public class DecorMovementController : MonoBehaviour
    {
        private Vector3 m_startPosition;

        // Start is called before the first frame update
        void Start()
        {
            m_startPosition = transform.position;
        }

        private void OnBecameInvisible()
        {
            //Log.Info($"{name}.OnBecameInvisible()");
            transform.position = m_startPosition;
        }
    }
}