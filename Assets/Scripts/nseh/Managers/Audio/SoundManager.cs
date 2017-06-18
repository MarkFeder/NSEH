﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Managers.Main;

namespace nseh.Managers.Audio
{
    public class SoundManager : Service
    {
        #region Private Properties

        private float _volumeSoundFX;
        private float _volumeMusic;
        private int _maxSounds;
        private static SoundManager _instance;
        public static SoundManager Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                return null;
            }
        }
        #endregion

        #region Public Properties
        public List<GameObject> soundList;

        #endregion

        #region Service Methods

        public override void Activate()
        {

            _instance = this;
            _maxSounds = 20;
            _isActivated = true;
            _volumeSoundFX = 1;
            soundList = new List<GameObject>();
        }

        public override void Tick()
        {
           
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

        public void PlayAudioFX(AudioClip sound, float volumen, bool ignoreListener, Vector3 position, float spatialBlend)
        {

            if(soundList.Count < _maxSounds)
            {
                GameObject aux = new GameObject(sound.name);
                AudioSource AudioAux = aux.AddComponent<AudioSource>();
                Debug.Log(aux);
                AudioAux.clip = sound;
                AudioAux.volume = volumen * _volumeSoundFX;
                AudioAux.ignoreListenerPause = ignoreListener;
                AudioAux.spatialBlend = spatialBlend;
                soundList.Add(aux);
                AudioAux.Play();
                RemoveAudioSource(MyGame, aux, sound.length);        
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


        private void RemoveAudioSource(MonoBehaviour myMonoBehaviour, GameObject audioAux,float time)
        {
            myMonoBehaviour.StartCoroutine(RemovingSound(audioAux, time));
        }

        private IEnumerator RemovingSound(GameObject audioAux, float time)
        {
            yield return new WaitForSeconds(time);
            soundList.Remove(audioAux);
            Object.Destroy(audioAux);
        }
            #endregion

        }
}
