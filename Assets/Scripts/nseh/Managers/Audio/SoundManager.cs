using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nseh.Managers.Audio
{
    public class SoundManager : Service
    {

        #region Private Properties

        private float _masterVolume;
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
            _masterVolume = 1;
            _maxSounds = 20;
            _isActivated = true;
            _volumeSoundFX = 1f;
            _volumeMusic = 1;
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

        public void SetMasterVolume(float volume)
        {
            _masterVolume = volume;
        }

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
                AudioAux.volume = volumen * _volumeSoundFX * _masterVolume;
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
                AudioAux.volume = volumen * _volumeSoundFX * _masterVolume;
                AudioAux.ignoreListenerPause = ignoreListener;
                AudioAux.spatialBlend = spatialBlend;
                AudioAux.pitch += pitch;
                soundList.Add(aux);
                AudioAux.Play();
                MyGame.StartCoroutine(RemovingSound(aux, sound.length));
            }
        }

        public void PlayAudioMusic( AudioSource aux)
        {
            aux.volume = aux.volume * _volumeMusic * _masterVolume;
            aux.ignoreListenerPause = true;
            aux.loop = true;
            aux.Play();
        }

        public void PlayAmbientSounds(List<AudioSource> list)
        {
            if (list != null)
            {
                foreach (AudioSource auxAudio in list)
                {
                    auxAudio.volume = auxAudio.volume * _volumeSoundFX * _masterVolume;
                    auxAudio.ignoreListenerPause = true;
                    auxAudio.loop = true;
                    auxAudio.Play();
                }
            }     
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
