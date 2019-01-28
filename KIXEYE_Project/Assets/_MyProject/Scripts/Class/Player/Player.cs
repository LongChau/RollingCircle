using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using LC.Ultility;

namespace RollingCircle
{
    /// <summary>
    /// Player data
    /// </summary>
    [Serializable]
    public class Player
    {
        /// <summary>
        /// Name of user
        /// </summary>
        public string Name;

        /// <summary>
        /// User score
        /// </summary>
        public int Score;

        /// <summary>
        /// user highest score. For future use.
        /// </summary>
        public int HighScore;

        public string Dump()
        {
            return JsonUtility.ToJson(this);
        }

        public void ResetScore()
        {
            Score = 0;
        }
    }

    /// <summary>
    /// An instance of Player. Make easy to use through the game
    /// </summary>
    [Serializable]
    public class PlayerData
    {
        public static PlayerData Instance { get; set; }
        public Player Player { get; set; }

        public string Dump()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
