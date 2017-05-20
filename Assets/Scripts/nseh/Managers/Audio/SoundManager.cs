﻿using System.Collections.Generic;
using UnityEngine;

namespace nseh.Managers.Audio
{
    public class SoundManager : Service
    {
        #region Private Properties

        private Dictionary<AudioController, AudioSource> _dictionarySoundFX;
        private Dictionary<AudioController, AudioSource> _dictionaryMusic;

        private float _volumeSoundFX;
        private float _volumeMusic;

        #endregion

        #region Public Properties

        /// <summary>
        /// Read/Write maximum global value for FX sounds.
        /// </summary>
        public float VolumeSoundFX
        {
            get
            {
                return _volumeSoundFX;
            }
            set
            {
                _volumeSoundFX = value;

                foreach(KeyValuePair<AudioController, AudioSource> audioControllerPair in _dictionarySoundFX)
                {
                    audioControllerPair.Value.volume = audioControllerPair.Key.MaxVolume * value;
                }
            }
        }

        /// <summary>
        /// Read/Write maximum global value for Music sounds.
        /// </summary>
        public float VolumeMusic
        {
            get
            {
                return _volumeMusic;
            }
            set
            {
                _volumeMusic = value;

                foreach (KeyValuePair<AudioController, AudioSource> audioControllerPair in _dictionaryMusic)
                {
                    audioControllerPair.Value.volume = audioControllerPair.Key.MaxVolume * value;
                }
            }
        }

        /// <summary>
        /// Mute global sound (for fx and music)
        /// </summary>
        public bool Paused
        {
            get
            {
                return AudioListener.pause;
            }
            set
            {
                AudioListener.pause = false;
            }
        }

        #endregion

        #region Public Methods

        public override void Activate()
        {
            IsActivated = true;

            _dictionarySoundFX = new Dictionary<AudioController, AudioSource>();
            _dictionaryMusic = new Dictionary<AudioController, AudioSource>();

            _volumeSoundFX = 1.0f;
            _volumeMusic = 1.0f;
        }

        public override void Tick()
        {
        }

        public override void Release()
        {
            IsActivated = false;

            Paused = true;
            _dictionarySoundFX.Clear();
            _dictionaryMusic.Clear();

            _volumeSoundFX = 0.0f;
            _volumeMusic = 0.0f;
        }

        /// <summary>
        /// Load specific music.
        /// </summary>
        /// <param name="strAudioName">The name of the audio.</param>
        /// <param name="priority">The priority of this audio.</param>
        /// <param name="maxVolume">The maximum volume this audio has.</param>
        /// <param name="pitch">The pitch of the AudioSource.</param>
        /// <param name="stereoPan">The pan of the AudioSource.</param>
        /// <returns>The AudioController for this Music.</returns>
        public AudioController LoadMusic(string strAudioName, int priority = 128, float maxVolume = 1.0f, float pitch = 1.0f, float stereoPan = 0.0f)
        {
            return LoadAudioInternal(strAudioName, ref _dictionaryMusic, true, priority, maxVolume, pitch, stereoPan);
        }

        /// <summary>
        /// Load specific fx sound.
        /// </summary>
        /// <param name="strAudioName">The name of the audio.</param>
        /// <param name="is2D">Is this audio a 2d audio?.</param>
        /// <param name="priority">The priority of this audio.</param>
        /// <param name="maxVolume">The maximum volume this audio has.</param>
        /// <param name="pitch">The pitch of the AudioSource.</param>
        /// <param name="stereoPan">The pan of the AudioSource.</param>
        /// <returns>The AudioController for this FX sound.</returns>
        public AudioController LoadSoundFX(string strAudioName, bool is2D, int priority = 128, float maxVolume = 1.0f, float pitch = 1.0f, float stereoPan = 0.0f)
        {
            return LoadSoundFX(strAudioName, is2D, priority, maxVolume, pitch, stereoPan);
        }

        /// <summary>
        /// Establish AudioController maximum value. This value is going to be scaled acording to maximum Music and FX sounds.
        /// </summary>
        /// <param name="audioController"></param>
        /// <param name="volume"></param>
        public void SetAudioVolume(AudioController audioController, float volume)
        {
            if (_dictionarySoundFX.ContainsKey(audioController))
            {
                audioController.AudioSource.volume = volume * audioController.MaxVolume * VolumeSoundFX;
            }
            else if (_dictionaryMusic.ContainsKey(audioController))
            {
                audioController.AudioSource.volume = volume * audioController.MaxVolume * VolumeMusic;
            }
            else
            {
                Debug.LogError(string.Format("The AudioController of {0} could not be found", audioController.AudioName));
            }
        }

        /// <summary>
        /// Play an audio which is in dictionary.
        /// </summary>
        /// <param name="audioController">The AudioController of the audio.</param>
        /// <param name="loop">Should be the audio played in loop?.</param>
        /// <param name="volume">The volume of the audio.</param>
        /// <param name="delay">The delay before playing the audio.</param>
        public void PlayAudio(AudioController audioController, bool loop = false, float volume = 1.0f, float delay = 0.0f)
        {
            Debug.Log(string.Format("PlayAudio of {0}", audioController.AudioName));

            if (_dictionarySoundFX.ContainsKey(audioController))
            {
                audioController.AudioSource.volume = volume * audioController.MaxVolume * VolumeSoundFX;
            }
            else if (_dictionaryMusic.ContainsKey(audioController))
            {
                audioController.AudioSource.volume = volume * audioController.MaxVolume * VolumeMusic;
            }
            else
            {
                Debug.LogError(string.Format("The AudioController of {0} could not be found", audioController.AudioName));
            }

            audioController.AudioSource.loop = loop;
            if (delay > 0.0)
            {
                audioController.AudioSource.PlayDelayed(delay);
            }
            else
            {
                audioController.AudioSource.Play();
            }
        }

        /// <summary>
        /// Pause the FX sound or the music.
        /// </summary>
        /// <param name="audioController"></param>
        public void PauseAudio(AudioController audioController)
        {
            Debug.Log(string.Format("Pause the audio: {0}", audioController.AudioName));
            audioController.AudioSource.Pause();
        }

        /// <summary>
        /// Unpause the FX sound or the music.
        /// </summary>
        /// <param name="audioController"></param>
        public void UnPauseAudio(AudioController audioController)
        {
            Debug.Log(string.Format("UnPause the audio: {0}", audioController.AudioName));
            audioController.AudioSource.UnPause();
        }

        /// <summary>
        /// Stop the FX sound or the music.
        /// </summary>
        /// <param name="audioController"></param>
        public void StopAudio(AudioController audioController)
        {
            Debug.Log(string.Format("Stop the audio: {0}", audioController.AudioName));
            audioController.AudioSource.Stop();
        }

        /// <summary>
        /// Free AudioController (FX sound or Music sound).
        /// </summary>
        /// <param name="audioController"></param>
        public void FreeAudio(AudioController audioController)
        {
            Debug.Log(string.Format("Freeing audio of {0}", audioController.AudioName));

            if (_dictionarySoundFX.ContainsKey(audioController))
            {
                GameObject.Destroy(audioController.AudioSource);
                _dictionarySoundFX.Remove(audioController);
            }
            else if (_dictionaryMusic.ContainsKey(audioController))
            {
                GameObject.Destroy(audioController.AudioSource);
                _dictionaryMusic.Remove(audioController);
            }
            else
            {
                Debug.LogError(string.Format("The AudioController for {0} could not be found", audioController.AudioName));
            }
        }

        /// <summary>
        /// Searches for the AudioController of the music audio passed by argument.
        /// </summary>
        /// <param name="audioName">Sound file (path inside Resources folder).</param>
        /// <returns>The audio's AudioController or null if it could not be found.</returns>
        public AudioController GetMusic(string audioName)
        {
            foreach (KeyValuePair<AudioController, AudioSource> audioControllerPair in _dictionaryMusic)
            {
                if (audioControllerPair.Key.AudioName == audioName)
                {
                    return audioControllerPair.Key;
                }
            }

            Debug.LogError(string.Format("The AudioController for {0} could not be found", audioName));
            return null;
        }

        /// <summary>
        /// Searches for the AudioController of the FX sound passed by argument.
        /// </summary>
        /// <param name="audioName">Sound file (path inside Resources folder).</param>
        /// <returns>The sound's AudioController or null if it could not be found.</returns>
        public AudioController GetSoundFX(string audioName)
        {
            foreach (KeyValuePair<AudioController, AudioSource> audioControllerPair in _dictionarySoundFX)
            {
                if (audioControllerPair.Key.AudioName == audioName)
                {
                    return audioControllerPair.Key;
                }
            }

            Debug.LogError(string.Format("The AudioController for {0} could not be found", audioName));
            return null;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load a new AudioClip from Resources folder, create a new AudioSource for it and insert all
        /// the information in the dictionary.
        /// </summary>
        /// <param name="audioName">The name of the audio.</param>
        /// <param name="dictionary">The dictionary this.</param>
        /// <param name="is2d">Is this audio a 2d audio?.</param>
        /// <param name="priority">The priority of this audio.</param>
        /// <param name="maxVolume">The maximum volume this audio has.</param>
        /// <param name="pitch">The pitch of the AudioSource.</param>
        /// <param name="stereoPan">The pan of the AudioSource.</param>
        /// <returns></returns>
        private AudioController LoadAudioInternal(string audioName, ref Dictionary<AudioController, AudioSource> dictionary, bool is2d, int priority = 128, float maxVolume = 1.0f, float pitch = 1.0f, float stereoPan = 0.0f)
        {
            // Load Audioclip from Resources folder
            AudioClip audioClip = Resources.Load<AudioClip>(audioName);

            if (audioClip == null)
            {
                Debug.LogError(string.Format("There was a problem trying to load audio {0}", audioName));
                return null;
            }

            // Create new AudioSource for this Audioclip and attach it to a new gameobject
            GameObject audioGameObject = new GameObject("Audio " + audioName);
            AudioSource newAudioSource = audioGameObject.AddComponent<AudioSource>();
            newAudioSource.clip = audioClip;
            newAudioSource.playOnAwake = false;
            newAudioSource.priority = priority;
            newAudioSource.pitch = pitch;
            newAudioSource.panStereo = stereoPan;
            newAudioSource.spatialBlend = is2d ? 0.0f : 1.0f;

            // Add new AudioSource to the dictionary
            AudioController newAudio = new AudioController(newAudioSource, audioName, maxVolume);
            dictionary.Add(newAudio, newAudioSource);

            return newAudio;
        }

        #endregion
    }
}
