using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nseh.Managers.Audio
{
    public class SoundManager : Service
    {
        #region Private Properties

        private float _volumeSoundFX;
        private float _volumeMusic;
        private int _maxSounds;

        #endregion

        #region Public Properties
        public List<AudioSource> soundList;

        #endregion

        #region Service Methods

        public override void Activate()
        {
            _isActivated = true;
            soundList = new List<AudioSource>();
        }

        public override void Tick()
        {
            foreach(AudioSource aux in soundList)
            {
                if(AudioListener.pause ==false && aux.isPlaying == false)
                {
                    soundList.Remove(aux);
                }
            }
        }

        public override void Release()
        {
            _isActivated = false;
            soundList.Clear();
        }

        #endregion

        #region Public Methods

        public void SetFXVolume(float volume)
        {
            _volumeSoundFX = volume;
        }

        public void SetMusicVolume(float volume)
        {
            _volumeMusic = volume;
        }

        public void ResetList()
        {
            soundList.Clear();
        }

        public void PauseSounds()
        {
            AudioListener.pause = !AudioListener.pause;
        }

        public void PlayAudioFX(AudioClip sound, float volumen, bool ignoreListener, Vector3 position)
        {
            if(soundList.Count < _maxSounds)
            {
                AudioSource aux = new AudioSource();
                aux.clip = sound;
                aux.volume = volumen * _volumeSoundFX;
                aux.ignoreListenerPause = ignoreListener;
                soundList.Add(aux);
                aux.Play();
            }
        }

        public void PlayAudioMusic(AudioClip sound, float volumen, Camera camera)
        {
            AudioSource aux = camera.GetComponent<AudioSource>();
            aux.clip = sound;
            aux.volume = volumen * _volumeMusic;
            aux.ignoreListenerPause = true;
            aux.Play();
        }

        #endregion
    }
}
