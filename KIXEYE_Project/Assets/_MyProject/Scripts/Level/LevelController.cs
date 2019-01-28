using LC.Ultility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZ_Pooling;

namespace RollingCircle
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private List<LevelConfig> m_listLevelConfig = new List<LevelConfig>();

        [SerializeField] private LevelConfig m_curLvlConfig;

        [SerializeField] private int m_curObstaclePassThrough = 0;
        [SerializeField] private int m_curLvlConfigIndex = 0;

        private float m_timer;

        [SerializeField] private Transform m_spawnerPlace;

        // Start is called before the first frame update
        void Start()
        {
            m_curLvlConfig = m_listLevelConfig[m_curLvlConfigIndex];
            m_timer = m_curLvlConfig.level.TimeInterval;

            GameManager.Instance.DelayStart();
        }

        private void Update()
        {
            if (!GameManager.Instance.IsGameStart)
            {
                return;
            }

            if (m_timer > 0)
                m_timer -= Time.deltaTime;
            else
            {
                Log.Info("Spawn()");

                m_timer = m_curLvlConfig.level.TimeInterval;

                int randIndex = UnityEngine.Random.Range(0, m_curLvlConfig.level.ListObstacles.Count);

                var objPrefab = m_curLvlConfig.level.ListObstacles[randIndex];

                var objSpawned = EZ_PoolManager.Spawn(objPrefab.transform, m_spawnerPlace.position, m_spawnerPlace.rotation);

                var obstacleCtrl = objSpawned.GetComponent<ObstacleController>();
                obstacleCtrl.SetMovementSpeed(m_curLvlConfig.level.MovementSpeed);
            }
        }

        private IEnumerator IELevelCheck()
        {
            var wait = new WaitForSeconds(1.0f);

            while (true)
            {
                
                yield return wait;
            }
        }

        // This function is called when the behaviour becomes disabled or inactive
        private void OnDisable()
        {
            EventManager.Instance?.RemoveListener(EventID.OnRaycastToPlayer, Handle_OnRaycastToPlayer);
        }

        // This function is called when the object becomes enabled and active
        private void OnEnable()
        {
            this.RegisterListener(EventID.OnRaycastToPlayer, Handle_OnRaycastToPlayer);
        }

        private void Handle_OnRaycastToPlayer(object obj)
        {
            m_curObstaclePassThrough++;
            if (m_curObstaclePassThrough == GameConst.HARD_LIMIT)
            {
                Log.Info("BEGIN HARD MODE");
                m_curLvlConfigIndex++;
                m_curLvlConfig = m_listLevelConfig[m_curLvlConfigIndex];

                this.PostEvent(EventID.OnHardLimitReach);
            }
        }
    }
}