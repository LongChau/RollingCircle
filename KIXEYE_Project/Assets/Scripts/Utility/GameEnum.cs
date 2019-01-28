// others game enum

public enum OtherType
{
    None = -1
}

public enum EScene
{
    None = -1,
    MenuScene = 0,
    GameScene = 1,
}

public enum EMoveDirection
{
    None = 0,
    Left = -1,
    Right = 1,
}

public enum EObstacleType
{
    None = 0,
    /// <summary>
    /// Static will increase time with level
    /// </summary>
    Static,
    /// <summary>
    /// Runner is not increase time with level
    /// </summary>
    Runner,
}

public enum EGameLayer
{
    Default = 0,
    TransparenFX = 1,
    IgnoreRaycast = 2,
    Water = 4,
    UI = 5,
    Obstacle = 8,
    Player = 9,
}

public enum EErrorCode
{
    UserNameNotFound = 404,
    InvalidUserNameSupplied = 405,
    Ok = 200,
}

public enum ESoundEffect
{
    None = -1,
    BtnClicked = 0,
    Explosive,
    GunShot,
    Item_Taken,
    Lose,
    TingTing,
    Win,
    BusHorn,
}