using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RollingCircle
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/LevelConfig", order = 0)]
    [Serializable]
    public class LevelConfig : ScriptableObject
    {
        public Level level;
    }
}