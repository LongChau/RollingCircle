using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using LC.Ultility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RollingCircle
{
    /// <summary>
    /// For networking, contains GameSpark API
    /// </summary>
    public class NetworkManager : MonoSingletonExt<NetworkManager>
    {
        /// <summary>
        /// Check for authentication
        /// </summary>
        public bool IsAuthenticated;

        public override void Init()
        {
            base.Init();
            DontDestroyOnLoad(this);
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        #region Register, Authentication
        /// <summary>
        /// Register with user name
        /// </summary>
        /// <param name="name"></param>
        public void GSRegisterUser(string name)
        {
            Debug.Log("GSRegisterUser()");

            string displayName = name;
            string userName = name;
            string pass = "";

            Debug.Log("UserName: " + userName);
            Debug.Log("Pass: " + pass);

            RegistrationRequest registReq = new RegistrationRequest();
            registReq.SetDisplayName(displayName);
            registReq.SetUserName(userName);
            registReq.SetPassword(pass);

            // send request and get response
            registReq.Send(response =>
            {
                // Check if any erros occur
                if (response.HasErrors)
                {
                    Debug.LogWarning("Error in GSRegisterUser()");
                    Debug.Log("All response: " + response.JSONString);
                    Debug.Log("Error: " + response.Errors.JSON);

                    var errorBase = JsonUtility.FromJson<ErrorMsgBase>(response.Errors.JSON);
                    if (errorBase != null && errorBase.errorMsg.errorCode == (int)EErrorCode.InvalidUserNameSupplied)
                    {
                        Debug.LogWarning($"CustomError msg: {errorBase.Dump()}");
                        UIManager.Instance.ShowNotification("Invalid UserName Supplied");
                    }
                    else
                    {
                        UIManager.Instance.ShowNotification("User name already been taken");
                    }
                }
                else
                {
                    Debug.Log("GSRegisterUser() succeed");
                    Debug.Log(response.JSONString);

                    // notify that user succeed registeration
                    this.PostEvent(EventID.OnRegisterSucceed);

                    // then automate authenticate
                    GSAuthenticate(name);

                    SaveLoadSystem.Save();
                }
            });
        }

        /// <summary>
        /// Request for authentication
        /// </summary>
        /// <param name="name"></param>
        public void GSAuthenticate(string name)
        {
            Debug.Log("GSAuthenticate()");

            string userName = "";
            string password = "";
            userName = name;

            // make an authentication request
            AuthenticationRequest authReq = new AuthenticationRequest();
            authReq.SetPassword(password)
                .SetUserName(userName)
                .Send((response) =>
                {
                    // if there is no errors
                    if (!response.HasErrors)
                    {
                        Debug.LogWarning("Authen succeed");
                        string authToken = response.AuthToken;

                        var errorBase = JsonUtility.FromJson<ErrorMsgBase>(response.ScriptData.JSON);
                        if (errorBase.errorMsg.errorCode == (int)EErrorCode.Ok)
                        {
                            UIManager.Instance.ShowNotification($"Welcome, {name}");
                            this.PostEvent(EventID.OnAuthSucceed);

                            PlayerData.Instance.Player.Name = name;
                            SaveLoadSystem.Save();

                            IsAuthenticated = true;
                        }
                    }
                    // if it has error
                    else
                    {
                        Debug.LogWarningFormat("Authen failed: {0}", response.Errors.JSON);

                        var errorBase = JsonUtility.FromJson<ErrorMsgBase>(response.Errors.JSON);
                        if (errorBase != null)
                        {
                            Log.Info($"CustomError msg: {errorBase.Dump()}");

                            if (errorBase.errorMsg.errorCode == (int)EErrorCode.UserNameNotFound)
                            {
                                // show notification to user
                                UIManager.Instance.ShowNotification("Your username not found!");
                            }
                        }
                    }
                });
        }
        #endregion

        /// <summary>
        /// Log out 
        /// </summary>
        public void GSLogOut()
        {
            Log.Info("GSLogOut()");
            GS.Reset();
        }

        #region Leaderboard
        /// <summary>
        /// Request leaderboard
        /// </summary>
        /// <param name="request"></param>
        public void GSRequestLeaderboard(LeaderboardDataRequestData request)
        {
            Log.Info("GSRequestLeaderboard");

            LeaderboardDataRequest req = new LeaderboardDataRequest();
            req.SetLeaderboardShortCode(request.LeaderboardShortCode);
            req.SetEntryCount(request.entryCount);

            req.Send(response =>
            {
                if (response.HasErrors)
                {
                    Debug.LogWarning("Error in GSRequestLeaderboard()");
                    Debug.Log("Error: " + response.Errors.JSON);
                }
                else
                {
                    Debug.Log("GSRequestLeaderboard() succeed");
                    Debug.Log(response.JSONString);

                    var data = response.Data;

                    List<RankData> listModel = new List<RankData>();

                    foreach (var item in data)
                    {
                        Debug.Log(item.JSONString);
                        RankData rankModel = new RankData(item.UserId, (int)item.GetNumberValue("SCORE"),
                            item.When, item.City, item.Country, item.UserName, item.ExternalIds, (int)item.Rank);
                        Log.Info(rankModel.ToString());

                        listModel.Add(rankModel);
                    }

                    GameManager.Instance.ListGlobalRankData = listModel;

                    this.PostEvent(EventID.OnRequestLeaderboardDone);
                }
            });
        }

        public void GSPostLeaderboardScore(c2s_Leaderboard_Score packet)
        {
            Log.Info("GSPostLeaderboardScore");

            LogEventRequest logEventRequest = new LogEventRequest();
            logEventRequest.SetEventKey(packet.eventKey);
            logEventRequest.SetEventAttribute("SCORE", packet.SCORE);

            Log.InfoFormat("Data send to server: {0}", packet.ToString());

            logEventRequest.Send(response =>
            {
                if (response.HasErrors)
                {
                    Debug.LogWarning("Error in GSPostLeaderboardScore()");
                    Debug.Log("Error: " + response.Errors.JSON);
                }
                else
                {
                    Debug.Log("GSPostLeaderboardScore() succeed");
                    Debug.Log(response.JSONString);
                }
            });
        }
        #endregion

        internal override void OnApplicationQuit()
        {
            GSLogOut();
            base.OnApplicationQuit();
        }
    }


    /// <summary>
    /// For:
    /// * 404 - Username not found (user has not registered with the leaderboard service)
    /// * 405 - Invalid Username supplied
    /// * 200 - Ok
    /// </summary>
    [Serializable]
    public sealed class ErrorMsg
    {
        public string error;
        public int errorCode;
    }

    /// <summary>
    /// A base error message contains ErrorMsg class.
    /// Use this to convert from JSON
    /// </summary>
    [Serializable]
    public sealed class ErrorMsgBase
    {
        public ErrorMsg errorMsg;

        public string Dump()
        {
            return JsonUtility.ToJson(this);
        }
    }

    /// <summary>
    /// Leaderboard request data
    /// </summary>
    [Serializable]
    public class LeaderboardDataRequestData
    {
        public string LeaderboardShortCode;
        public int entryCount;

        public LeaderboardDataRequestData()
        {
            this.LeaderboardShortCode = "";
            entryCount = 0;
        }

        public LeaderboardDataRequestData(string leaderboardShortCode, int entryCount)
        {
            LeaderboardShortCode = leaderboardShortCode;
            this.entryCount = entryCount;
        }
    }

    /// <summary>
    /// Contains many info for user rank
    /// </summary>
    [Serializable]
    public class RankData
    {
        public string userID;
        public int SCORE;
        public string when;
        public string city;
        public string country;
        public string userName;
        public GSData externalIds;
        public int rank;

        public RankData()
        {
            this.userID = "";
            SCORE = 0;
            this.when = "";
            this.city = "";
            this.country = "";
            this.userName = "";
            this.externalIds = new GSData();
            this.rank = 0;
        }

        public RankData(string userID, int score, string when, string city, string country, string userName, GSData externalIds, int rank)
        {
            this.userID = userID;
            this.SCORE = score;
            this.when = when;
            this.city = city;
            this.country = country;
            this.userName = userName;
            this.externalIds = externalIds;
            this.rank = rank;
        }
    }

    /// <summary>
    /// Packet recieve for rankData
    /// </summary>
    [Serializable]
    public class RankData_ReceivedPacket
    {
        public List<RankData> listRanks;

        public RankData_ReceivedPacket()
        {
            this.listRanks = new List<RankData>();
        }

        public RankData_ReceivedPacket(List<RankData> listRanks)
        {
            this.listRanks = listRanks;
        }
    }
}