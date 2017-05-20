using UnityEngine;

namespace nseh.Managers.Audio
{
    public class AudioController
    {
        #region Private Properties

        private AudioSource _audioSource;
        private string _audioName;
        private float _maxVolume;

        #endregion

        #region Public Properties

        public AudioSource AudioSource
        {
            get
            {
                return _audioSource;
            }
        }

        public string AudioName
        {
            get
            {
                return _audioName;
            }
        }

        public float Volume
        {
            get
            {
                if (Mathf.Approximately(_maxVolume, 0.0f))
                {
                    return 0.0f;
                }

                return _audioSource.volume / _maxVolume;
            }
        }

        public float MaxVolume
        {
            get { return _maxVolume; }
            set { _maxVolume = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Main Constructor for AudioController.
        /// </summary>
        /// <param name="audioSource">The Unity's AudioSource.</param>
        /// <param name="audioName">The audio's file name (or the audioClip name).</param>
        /// <param name="maxVolume">The Unity's internal max volume.</param>
        public AudioController(AudioSource audioSource, string audioName, float maxVolume)
        {
            _audioSource = audioSource;
            _audioName = audioName;
            _maxVolume = maxVolume;
        }

        #endregion
    }
}
