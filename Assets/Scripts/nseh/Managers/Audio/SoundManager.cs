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

        public List<GameObject> soundList;

        #endregion

        #region Service Methods

        public override void Activate()
        {
            _maxSounds = 20;
            _isActivated = true;
            _volumeSoundFX = 0.75f;
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
                AudioAux.clip = sound;
                AudioAux.volume = volumen * _volumeSoundFX;
                AudioAux.ignoreListenerPause = ignoreListener;
                AudioAux.spatialBlend = spatialBlend;
                soundList.Add(aux);
                AudioAux.Play();
                MyGame.StartCoroutine(RemovingSound (aux,sound.length));    
            }
        }

        public void PlayAudioFX(AudioClip sound, float volumen, bool ignoreListener, Vector3 position, float spatialBlend, float pitch)
        {

            if (soundList.Count < _maxSounds)
            {
                GameObject aux = new GameObject(sound.name);
                AudioSource AudioAux = aux.AddComponent<AudioSource>();
                AudioAux.clip = sound;
                AudioAux.volume = volumen * _volumeSoundFX;
                AudioAux.ignoreListenerPause = ignoreListener;
                AudioAux.spatialBlend = spatialBlend;
                AudioAux.pitch += pitch;
                soundList.Add(aux);
                AudioAux.Play();
                MyGame.StartCoroutine(RemovingSound(aux, sound.length));
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

        private IEnumerator RemovingSound(GameObject audioAux, float time)
        {
            yield return new WaitForSeconds(time);
            soundList.Remove(audioAux);
            Object.Destroy(audioAux);
        }

        #endregion

    }
}
