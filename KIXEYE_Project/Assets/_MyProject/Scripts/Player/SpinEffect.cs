using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LC.Ultility;

namespace RollingCircle
{
    /// <summary>
    /// A spin effect component
    /// </summary>
    public class SpinEffect : MonoBehaviour
    {
        public float rotateSpeed;

        private Vector3 rotateVec;

        // Start is called before the first frame update
        void Start()
        {
            rotateVec.z = rotateSpeed;
        }

        // Update is called once per frame
        void Update()
        {
            rotateVec.z = rotateSpeed;
            GlobalFunc.RotateAround(transform, rotateVec);
        }
    }
}