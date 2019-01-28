using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LC.Ultility;
using System;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace RollingCircle
{
    /// <summary>
    /// Manage through out the game
    /// </summary>
    public class GameManager : MonoSingletonExt<GameManager>
    {
        public bool IsPause;
        public bool IsGameOver;
        public bool IsGameStart;

        public List<RankData> ListGlobalRankData = new List<RankData>();

        public override void Init()
        {
            base.Init();
            DontDestroyOnLoad(this);
        }

        // Start is called before the first frame update
        void Start()
        {
            LoadData();

            SceneManager.sceneLoaded += Handle_SceneLoaded;

            this.RegisterListener(EventID.OnPlayerDie, Handle_OnPlayerDie);
            this.RegisterListener(EventID.OnRaycastToPlayer, Handle_OnRaycastToPlayer);
        }

        private void ResetData()
        {
            IsPause = false;
            IsGameOver = false;
            IsGameStart = false;
        }

        private void Handle_SceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            if (scene.buildIndex == (int)EScene.GameScene)
            {
                RequestLeaderboard();
            }
            else
            {
                UnregisterEvent();
                ResetData();
            }
        }

        private void RequestLeaderboard()
        {
            if (IsInternetConnected() && NetworkManager.Instance.IsAuthenticated)
            {
                Log.Info("RequestLeaderboard()");
                LeaderboardDataRequestData req = new LeaderboardDataRequestData();
                req.entryCount = 10;
                req.LeaderboardShortCode = GameConst.LEADERBOARD_SHORTCODE;

                NetworkManager.Instance.GSRequestLeaderboard(req);
            }
        }

        private void Handle_OnPlayerDie(object obj)
        {
            IsGameOver = true;

            // post player score to leaderboard
            PostScoreToleaderboard();

            // then request new leaderboard
            RequestLeaderboard();
        }

        private void PostScoreToleaderboard()
        {
            if (IsInternetConnected() && NetworkManager.Instance.IsAuthenticated)
            {
                Log.Info("PostScoreToleaderboard()");

                // send leaderboard_score packet to server for leaderboard service
                c2s_Leaderboard_Score packet = new c2s_Leaderboard_Score();
                packet.eventKey = PacketType.c2s_Leaderboard_Score.ToString();
                packet.SCORE = PlayerData.Instance.Player.Score;

                NetworkManager.Instance.GSPostLeaderboardScore(packet);
            }

        }

        public void DelayStart()
        {
            DOVirtual.DelayedCall(1.5f, () =>
            {
                IsGameStart = true;
                IsGameOver = false;
            });
        }

        private void LoadData()
        {
            Log.Info(GameConst.SAVE_PATH);

            PlayerData.Instance = new PlayerData();

            SaveLoadSystem.Load();

            this.PostEvent(EventID.OnLoadDataCompleted);
        }

        // This function is called when the behaviour becomes disabled or inactive
        private void OnDisable()
        {
        }

        // This function is called when the object becomes enabled and active
        private void OnEnable()
        {
        }

        private void Handle_OnRaycastToPlayer(object obj)
        {
            Log.Info("GameManager.Handle_OnRaycastToPlayer");
            PlayerData.Instance.Player.Score += GameConst.SCORE_ADDED;
        }

        internal override void OnApplicationQuit()
        {
            UnregisterEvent();

            base.OnApplicationQuit();

            // reset this score before save
            PlayerData.Instance.Player.ResetScore();
            SaveLoadSystem.Save();
        }

        private void UnregisterEvent()
        {
            SceneManager.sceneLoaded -= Handle_SceneLoaded;
            EventManager.Instance?.RemoveListener(EventID.OnPlayerDie, Handle_OnPlayerDie);
            EventManager.Instance?.RemoveListener(EventID.OnRaycastToPlayer, Handle_OnRaycastToPlayer);
        }

        public void PauseGame()
        {
            IsPause = true;
            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            IsPause = false;
            Time.timeScale = 1;
        }

        public bool IsInternetConnected()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
                return false;
            else
                return true;
        }
    }
}