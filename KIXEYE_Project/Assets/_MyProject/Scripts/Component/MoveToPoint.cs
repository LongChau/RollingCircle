using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LC.Ultility;
using System;

namespace RollingCircle
{
    public class MoveToPoint : MonoBehaviour
    {
        public Vector3 destination;
        public float speed;

        public GameObject bomb;
        public GameObject explode;

        public bool canRun;

        // Start is called before the first frame update
        void Start()
        {
            this.RegisterListener(EventID.OnStartGame, Handle_OnStartGame);
        }

        private void OnDestroy()
        {
            EventManager.Instance?.RemoveListener(EventID.OnStartGame, Handle_OnStartGame);
        }

        private void Handle_OnStartGame(object obj)
        {
            canRun = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (!canRun)
                return;

            transform.Translate(Vector2.right * speed * Time.deltaTime);

            if (transform.position.x >= destination.x)
            {
                bomb.SetActive(false);
                explode.SetActive(true);
                canRun = false;
                SoundManager.Instance.PlaySound(ESoundEffect.Explosive);
            }
        }
    }
}