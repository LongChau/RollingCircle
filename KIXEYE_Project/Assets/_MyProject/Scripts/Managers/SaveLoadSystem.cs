using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LC.Ultility;

namespace RollingCircle
{
    public static class SaveLoadSystem
    {
        public static Player playerInfo = new Player();

#if UNITY_EDITOR
        static bool isSecure = false;       // if dont want secure set this = false. Otherwise set it true.
#else
        static bool isSecure = true;
#endif

        static string pass = "L0ngCh@u_1210";
        static string encryptStr = $"?encrypt=true&password={pass}";

        // using EasySave 2 plugin
        public static void Save()
        {
#if UNITY_EDITOR
            //Debug.Log("SaveLoadSystem.SaveByES2()");
            Debug.Log("SaveLoadSystem.Save()");
#endif

#if UNITY_EDITOR
            Log.Info(encryptStr);
#endif

            playerInfo = PlayerData.Instance.Player;
            string info = JsonUtility.ToJson(playerInfo);
            if (isSecure)
                //ES2.Save(info, Application.persistentDataPath + "/userInfo.gd?encrypt=true&password=pass");
                ES2.Save(info, GameConst.SAVE_PATH + encryptStr);
            else
                ES2.Save(info, GameConst.SAVE_PATH);

            //if (GameManager.Instance.IsInternetConnected())
            //{
            //    //TODO: Sync this save to server
            //}

            Debug.Log("Save Done");
        }

        public static void Load()
        {
#if UNITY_EDITOR
            //Debug.Log("SaveLoadSystem.LoadByES2()");
            Debug.Log("SaveLoadSystem.Load()");
#endif
            if (File.Exists(GameConst.SAVE_PATH))
            {
                Debug.Log("File save exist");
                string info = "";
                if (isSecure)
                    info = ES2.Load<string>(GameConst.SAVE_PATH + encryptStr);
                else
                    info = ES2.Load<string>(GameConst.SAVE_PATH);
                playerInfo = JsonUtility.FromJson<Player>(info);
                PlayerData.Instance.Player = playerInfo;
            }
            else
            {
                //Debug.Log(PlayerData.Instance.player.isFirstTime);
                Debug.LogWarning("SaveLoadSystem --- Cant find file 'userInfo.gd'");
                CreateSaveFile();
            }
        }

        public static void CreateSaveFile()
        {
#if UNITY_EDITOR
            Debug.Log("SaveLoadSystem.CreateSaveFile()");
#endif
            PlayerData.Instance.Player = new Player();
            string info = JsonUtility.ToJson(PlayerData.Instance.Player);
            if (isSecure)
                ES2.Save(info, GameConst.SAVE_PATH + encryptStr);
            else
                ES2.Save(info, GameConst.SAVE_PATH);
            Debug.Log("Create save file done");
        }

        public static void CreateNewSaveFile()
        {
#if UNITY_EDITOR
            Debug.Log("SaveLoadSystem.CreateNewSaveFile()");
#endif
            if (File.Exists(GameConst.SAVE_PATH))
            {
                File.Delete(GameConst.SAVE_PATH);
                CreateSaveFile();
            }
        }
    }
}