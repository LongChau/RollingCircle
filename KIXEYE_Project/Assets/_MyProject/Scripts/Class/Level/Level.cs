using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RollingCircle
{
    [Serializable]
    public class Level
    {
        public List<GameObject> ListObstacles = new List<GameObject>();
        public float TimeInterval;
        public float MovementSpeed;
    }
}