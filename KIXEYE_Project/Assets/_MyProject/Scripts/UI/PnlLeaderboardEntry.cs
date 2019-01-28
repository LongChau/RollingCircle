using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RollingCircle
{
    /// <summary>
    /// Leaderboard entry
    /// </summary>
    public class PnlLeaderboardEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_txtName;
        [SerializeField] private TextMeshProUGUI m_txtScore;

        // Start is called before the first frame update
        void Start()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Set data for this entry
        /// </summary>
        /// <param name="name">name of user</param>
        /// <param name="score">user's score</param>
        public void SetData(string name, int score)
        {
            m_txtName.SetText(name);
            m_txtScore.SetText(score.ToString());
        }
    }
}
