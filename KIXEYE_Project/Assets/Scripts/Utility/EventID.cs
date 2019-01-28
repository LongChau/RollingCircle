// Game event ID enum
// use: class {eventName} for easy debug.
public enum EventID
{
    None = -1,
    OnInstantiateDemoObject = 0,
    OnCountDown,

    // class PopupConfirm
    OnBtnYesClicked,
    OnBtnNoClicked,

    // class ObstacleController
    OnRaycastToPlayer,

    // class RollingBombController
    OnPlayerDie,

    // class LevelController
    OnHardLimitReach,

    // class NetworkManager
    OnAuthSucceed,
    OnRegisterSucceed,
    OnRequestLeaderboardDone,

    // class GameManager
    OnLoadDataCompleted,

    // class MenuCanvas
    OnStartGame,
}
