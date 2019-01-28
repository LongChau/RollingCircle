using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LC.Ultility;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using DG.Tweening;

namespace RollingCircle
{
    /// <summary>
    /// Menu canvas, use in MenuScene
    /// </summary>
    public class MenuCanvas : MonoBehaviour
    {
        /// <summary>
        /// userName input here
        /// </summary>
        [SerializeField] private InputField m_inputUserName;

        [SerializeField] private GameObject m_btnRegister;
        [SerializeField] private GameObject m_btnLogin;

        [SerializeField] private string m_userName;

        /// <summary>
        /// Boolean for detect wheather btnStart is clicked
        /// </summary>
        private bool m_isStartClicked;

        // Start is called before the first frame update
        void Start()
        {
            var se = new InputField.SubmitEvent();
            se.AddListener(OnInputUserNameEndEdit);
            m_inputUserName.onEndEdit = se;
        }

        // This function is called when the behaviour becomes disabled or inactive
        private void OnDisable()
        {
            EventManager.Instance?.RemoveListener(EventID.OnLoadDataCompleted, Handle_OnLoadDataCompleted);
            EventManager.Instance?.RemoveListener(EventID.OnRegisterSucceed, Handle_OnRegisterSucceed);
            EventManager.Instance?.RemoveListener(EventID.OnAuthSucceed, Handle_OnAuthSucceed);
        }

        // This function is called when the object becomes enabled and active
        private void OnEnable()
        {
            this.RegisterListener(EventID.OnLoadDataCompleted, Handle_OnLoadDataCompleted);
            this.RegisterListener(EventID.OnRegisterSucceed, Handle_OnRegisterSucceed);
            this.RegisterListener(EventID.OnAuthSucceed, Handle_OnAuthSucceed);
        }

        private void Handle_OnLoadDataCompleted(object obj)
        {
            if (PlayerData.Instance.Player.Name.IsNOTNullOrEmpty())
            {
                m_userName = PlayerData.Instance.Player.Name;
                m_inputUserName.text = m_userName;
                //m_btnRegister.SetActive(false);
            }
        }

        private void Handle_OnRegisterSucceed(object obj)
        {
            HideMenuInfo();
        }

        private void Handle_OnAuthSucceed(object obj)
        {
            HideMenuInfo();

            if (m_isStartClicked)
            {
                SceneManager.LoadScene((int)EScene.GameScene);
                //DelayLoadScene();
            }
        }

        private void HideMenuInfo()
        {
            m_inputUserName.gameObject.SetActive(false);
            m_btnRegister.SetActive(false);
            m_btnLogin.SetActive(false);
        }

        private void OnInputUserNameEndEdit(string value)
        {
            m_userName = value;
        }

        public void OnBtnStartClicked()
        {
            Log.Info("MenuCanvas.OnBtnStartClicked()");

            SoundManager.Instance.PlaySound(ESoundEffect.BtnClicked);

            //if (GameManager.Instance.IsInternetConnected())
            //    NetworkManager.Instance.GSAuthenticate();

            if (PlayerData.Instance.Player.Name.IsNOTNullOrEmpty())
            {
                NetworkManager.Instance.GSAuthenticate(m_userName);
                m_isStartClicked = true;
            }
            else
            {
                SceneManager.LoadScene((int)EScene.GameScene);
                //DelayLoadScene();
            }

            this.PostEvent(EventID.OnStartGame);
        }

        private void DelayLoadScene()
        {
            DOVirtual.DelayedCall(3.0f, () =>
            {
                SceneManager.LoadScene((int)EScene.GameScene);
            });
        }

        public void OnBtnQuitClicked()
        {
            Log.Info("MenuCanvas.OnBtnQuitClicked()");
            SoundManager.Instance.PlaySound(ESoundEffect.BtnClicked);
            Application.Quit();
        }

        public void OnBtnRegisterClicked()
        {
            Log.Info("MenuCanvas.OnBtnRegisterClicked()");
            Log.Info($"UserName: {m_userName}");

            SoundManager.Instance.PlaySound(ESoundEffect.BtnClicked);

            //PlayerData.Instance.Player.Name = m_userName;
            //SaveLoadSystem.Save();

            if (GameManager.Instance.IsInternetConnected())
                NetworkManager.Instance.GSRegisterUser(m_userName);
        }

        public void OnBtnLoginClicked()
        {
            Log.Info("MenuCanvas.OnBtnRegisterClicked()");
            Log.Info($"UserName: {m_userName}");

            SoundManager.Instance.PlaySound(ESoundEffect.BtnClicked);

            //PlayerData.Instance.Player.Name = m_userName;
            //SaveLoadSystem.Save();

            if (GameManager.Instance.IsInternetConnected())
                NetworkManager.Instance.GSAuthenticate(m_userName);
        }
    }
}