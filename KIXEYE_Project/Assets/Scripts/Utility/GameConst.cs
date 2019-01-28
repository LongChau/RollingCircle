using UnityEngine;

public static class GameConst
{
    /// <summary>
    /// LEADERBOARD_SHORTCODE = "SCORE_LEADERBOARD";
    /// </summary>
    public static readonly string LEADERBOARD_SHORTCODE = "SCORE_LEADERBOARD";

    /// <summary>
    /// SCORE_ADDED = 10
    /// </summary>
    public static readonly int SCORE_ADDED = 10;

    /// <summary>
    /// if your current score >= HARD_LIMIT then level gonna be hard
    /// </summary>
    public static readonly int HARD_LIMIT = 5;

    /// <summary>
    ///  SAVE_PATH = Application.persistentDataPath + "/Save/userInfo.gd"
    /// </summary>
    public static readonly string SAVE_PATH = Application.persistentDataPath + "/Save/userInfo.gd";

    /// <summary>
    /// After pass HARD_LIMIT ball will rotate faster
    /// </summary>
    public static float BALLROTATE_ADDED = 5.0f;
}

public static class GameTags
{
    public static readonly string PLAYER = "Player";
    public static readonly string LAND = "Land";
    public static readonly string OBSTACLE = "Obstacle";
    public static readonly string UI = "UI";
}