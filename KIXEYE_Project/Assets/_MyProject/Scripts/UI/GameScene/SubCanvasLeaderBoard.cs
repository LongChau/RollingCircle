using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LC.Ultility;
using System;

namespace RollingCircle
{
    /// <summary>
    /// Leaderboard Canvas
    /// </summary>
    public class SubCanvasLeaderBoard : MonoBehaviour
    {
        /// <summary>
        /// This is leaderBoard entry prefab.
        /// Future use if request more than 10 entries
        /// </summary>
        [SerializeField] private GameObject m_pnlLeaderBoardEntryPrefab;

        /// <summary>
        /// Content is parent obj of all LeaderBoardEntry.
        /// Future use if using dynamic instantiate
        /// </summary>
        [SerializeField] private GameObject m_content;

        /// <summary>
        /// Current canvas
        /// </summary>
        private Canvas m_canvas;

        /// <summary>
        /// Manuel of all LeaderBoardEntry. 
        /// Hard code for 10 entries.
        /// Future use: may be deprecated.
        /// </summary>
        [SerializeField] private List<PnlLeaderboardEntry> m_listLeaderboardEntries = new List<PnlLeaderboardEntry>();

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            m_canvas = GetComponent<Canvas>();
        }

        // Start is called before the first frame update
        void Start()
        {
            //m_canvas.enabled = false;

            this.RegisterListener(EventID.OnPlayerDie, Handle_OnPlayerDie);
            this.RegisterListener(EventID.OnRequestLeaderboardDone, Handle_OnRequestLeaderboardDone);
        }

        private void OnDestroy()
        {
            EventManager.Instance?.RemoveListener(EventID.OnPlayerDie, Handle_OnPlayerDie);
            EventManager.Instance?.RemoveListener(EventID.OnRequestLeaderboardDone, Handle_OnRequestLeaderboardDone);
        }

        private void Handle_OnPlayerDie(object obj)
        {
            Log.Info("SubCanvasLeaderBoard.Handle_OnPlayerDie");
            m_canvas.enabled = true;
        }

        private void Handle_OnRequestLeaderboardDone(object obj)
        {
            Log.Info("Handle_OnRequestLeaderboardDone");
            int index = 0;
            foreach (var info in GameManager.Instance.ListGlobalRankData)
            {
                m_listLeaderboardEntries[index].gameObject.SetActive(true);
                m_listLeaderboardEntries[index].SetData(info.userName, info.SCORE);
                index++;
            }
        }
    }
}
