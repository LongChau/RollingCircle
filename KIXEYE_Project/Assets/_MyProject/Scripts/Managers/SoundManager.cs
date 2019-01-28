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
            m_audioClipDict[soundEffect].Play();
        }
    }
}