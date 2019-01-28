using LC.Ultility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;

namespace RollingCircle
{
    /// <summary>
    /// Control all UI flow in GameScene
    /// </summary>
    public class GameSceneMainCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject m_pnlPause;
        [SerializeField] private GameObject m_pnlGameOver;

        [SerializeField] private TextMeshProUGUI m_txtScore;
        [SerializeField] private TextMeshProUGUI m_txtHighScore;
        [SerializeField] private TextMeshProUGUI m_txtFinalScore;

        // Start is called before the first frame update
        void Start()
        {
            m_txtScore.SetText($"SCORE: {PlayerData.Instance.Player.Score}");
            m_txtHighScore.SetText($"HIGH SCORE: {PlayerData.Instance.Player.HighScore}");
            m_txtFinalScore.SetText($"YOUR SCORE: {PlayerData.Instance.Player.Score}");
        }

        // This function is called when the behaviour becomes disabled or inactive
        private void OnDisable()
        {
            EventManager.Instance?.RemoveListener(EventID.OnPlayerDie, Handle_OnPlayerDie);
            EventManager.Instance?.RemoveListener(EventID.OnRaycastToPlayer, Handle_OnRaycastToPlayer);
        }

        // This function is called when the object becomes enabled and active
        private void OnEnable()
        {
            this.RegisterListener(EventID.OnPlayerDie, Handle_OnPlayerDie);
            this.RegisterListener(EventID.OnRaycastToPlayer, Handle_OnRaycastToPlayer);
        }

        private void Handle_OnPlayerDie(object obj)
        {
            SoundManager.Instance.PlaySound(ESoundEffect.Explosive);

            m_txtFinalScore.SetText($"YOUR SCORE: {PlayerData.Instance.Player.Score}");
            m_pnlGameOver.SetActive(true);
            GlobalFunc.TweenShowPopup(m_pnlGameOver.transform.GetChild(0));
        }

        private void Handle_OnRaycastToPlayer(object obj)
        {
            SoundManager.Instance.PlaySound(ESoundEffect.Item_Taken);

            m_txtScore.SetText($"SCORE: {PlayerData.Instance.Player.Score}");

            if (PlayerData.Instance.Player.Score >= PlayerData.Instance.Player.HighScore)
            {
                PlayerData.Instance.Player.HighScore = PlayerData.Instance.Player.Score;
                m_txtHighScore.SetText($"HIGH SCORE: {PlayerData.Instance.Player.HighScore}");
            }
        }

        public void OnBtnPauseClicked()
        {
            Log.Info("GameSceneMainCanvas.OnBtnPauseClicked()");

            SoundManager.Instance.PlaySound(ESoundEffect.BtnClicked);

            m_pnlPause.SetActive(true);
            GlobalFunc.TweenShowPopup(m_pnlPause.transform.GetChild(0));

            // resume game
            GameManager.Instance.PauseGame();
        }

        public void OnBtnContinueClicked()
        {
            Log.Info("GameSceneMainCanvas.OnBtnContinueClicked()");

            SoundManager.Instance.PlaySound(ESoundEffect.BtnClicked);

            //var curPopup = EventSystem.current.currentSelectedGameObject.transform;
            m_pnlPause.SetActive(false);

            // resume game
            GameManager.Instance.ResumeGame();
        }

        public void OnBtnExitClicked()
        {
            Log.Info("GameSceneMainCanvas.OnBtnExitClicked()");

            SoundManager.Instance.PlaySound(ESoundEffect.BtnClicked);

            SceneManager.LoadScene((int)EScene.MenuScene);

            // resume game
            GameManager.Instance.ResumeGame();
        }

        public void OnBtnReplayClicked()
        {
            Log.Info("GameSceneMainCanvas.OnBtnReplayClicked()");

            SoundManager.Instance.PlaySound(ESoundEffect.BtnClicked);

            // reset player score here because we'll replay
            PlayerData.Instance.Player.ResetScore();

            // resume game
            GameManager.Instance.ResumeGame();

            SceneManager.LoadScene((int)EScene.GameScene);
        }
    }
}