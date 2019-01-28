using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace RollingCircle
{
    /// <summary>
    /// All packet type goes here
    /// </summary>
    public enum PacketType
    {
        None = -1,
        Server = 0,
        Empty = 1,

        c2s_Leaderboard_Score,
    }

    /// <summary>
    /// Packet of c2s_Leaderboard_Score
    /// </summary>
    [Serializable]
    public sealed class c2s_Leaderboard_Score
    {
        public string eventKey;
        public int SCORE;
    }
}