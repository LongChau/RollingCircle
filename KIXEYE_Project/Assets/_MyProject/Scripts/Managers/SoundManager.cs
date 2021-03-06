﻿using LC.Ultility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RollingCircle
{
    /// <summary>
    /// Manage all sounds through out the game
    /// </summary>
    public class SoundManager : MonoSingletonExt<SoundManager>
    {
        [SerializeField] private AudioClipDictionary m_audioClipDict = new AudioClipDictionary();

        public override void Init()
        {
            base.Init();
            DontDestroyOnLoad(this);
        }

        // Start is called before the first frame update
        void Start()
        {
            //m_audioClipDict[ESoundEffect.BtnClicked].Play
        }

        public void PlaySound(ESoundEffect soundEffect)
        {
            //@LONG 9:45 P.M 29/01 check for null exception
            if (m_audioClipDict.ContainsKey(soundEffect))
                m_audioClipDict[soundEffect].Play();
            //@LONG 12:32 A.M 30/01: Added this for notice designer or devs that Key and Value are not mapping correctly.
            else
                Log.Warning($"Key{soundEffect} is not mapping with any value in dictionary. Check SoundManager object dictionary in inspector");
        }
    }
}