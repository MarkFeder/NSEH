using System.Collections.Generic;
using nseh.Managers.Main;
using UnityEngine;

namespace nseh.Managers.Audio
{
    public class GameSounds : MonoBehaviour
    {
        #region Private Properties
/*
        [Header("UI sounds")]

		[SerializeField]
        private AudioClip _backButtonSound;
		[SerializeField]
		private AudioClip _passButtonSound;
        [SerializeField]
        private AudioClip _selectButtonSound;
		[SerializeField]
		private AudioClip _startButtonSound;

        [Space(10)]
        [Header("General sounds")]

        [SerializeField]
        private List<AudioClip> _levelMusic;
        [SerializeField]
        private List<AudioClip> _menuMusic;
        [SerializeField]
        private List<AudioClip> _bossMusic;
        [SerializeField]
        private List<AudioClip> _scoreMusic;

        [Space(10)]
        [Header("Misc sounds")]

        [SerializeField]
        private AudioClip _defeatMusic;
		[SerializeField]
		private AudioClip _narratorHurryUp;
        [SerializeField]
        private AudioClip _narratorCorrect;
        [SerializeField]
        private AudioClip _narratorFinalRound;
		[SerializeField]
		private AudioClip _narratorPowerUp;
        [SerializeField]
        private AudioClip _narratorOne;
        [SerializeField]
        private AudioClip _narratorTwo;
        [SerializeField]
        private AudioClip _narratorThree;
        [SerializeField]
        private AudioClip _narratorFour;
        [SerializeField]
        private AudioClip _narratorFive;

        [Space(10)]
		[Header("Chest sounds")]

		[SerializeField]
		private List<AudioClip> _chestBombSounds;
        [SerializeField]
        private List<AudioClip> _chestConfusionSounds;
		[SerializeField]
		private List<AudioClip> _chestHealthSounds;
		[SerializeField]
		private List<AudioClip> _chestEnergySounds;
		[SerializeField]
		private List<AudioClip> _chestInvulnerabilitySounds;
		[SerializeField]
		private List<AudioClip> _chestJumpSounds;
		[SerializeField]
		private List<AudioClip> _chestVelocitySounds;

		[Space(10)]
		[Header("Environment sounds")]

		[SerializeField]
		private List<AudioClip> _bubbleSounds;
		[SerializeField]
		private List<AudioClip> _respawnSounds;
		[SerializeField]
		private List<AudioClip> _teletransportSounds;
		[SerializeField]
		private AudioClip _volcanoAlarmSound;
		[SerializeField]
		private AudioClip _volcanoLavaSound;

        #endregion

        #region Private Methods

		/// <summary>
		/// Registers the audio contollers for all the fx clips in SoundManager.
		/// </summary>
		/// <param name="audioClips">The audio clips.</param>
		private void RegisterFXSounds(ref List<AudioClip> audioClips, bool is2D = false)
		{
			if (audioClips == null || audioClips.Count == 0)
			{
				Debug.LogWarning("The audio clips could not be registered because the container list is empty or null");
				return;
			}

			for (int i = 0; i < audioClips.Count; i++)
			{
				AudioController audioController = GameManager.Instance.SoundManager.LoadSoundFX(audioClips[i], is2D);
				if (audioController == null)
				{
					Debug.LogError(string.Format("The AudioController could not be instantiated for the audioclip: {0}", audioClips[i].name));
					continue;
				}
			}
		}

        /// <summary>
        /// Registers the FXS sound.
        /// </summary>
        /// <param name="audioClip">The audio clip.</param>
        private void RegisterFXSound(ref AudioClip audioClip, bool is2D = false)
        {
            if (audioClip == null)
            {
                Debug.LogWarning("The audio clip could not be registered because it is null");
                return;
            }

            AudioController audioController = GameManager.Instance.SoundManager.LoadSoundFX(audioClip, is2D);
            if (audioController == null)
            {
                Debug.LogError(string.Format("The AudioController could not be instantiated for the audioclip : {0}", audioClip.name));
            }
        }

		/// <summary>
		/// Unregisters all the audio controllers for all the music clips in SoundManager.
		/// </summary>
		/// <param name="audioClips">The audio clips.</param>
		private void UnRegisterFXSounds(ref List<AudioClip> audioClips)
		{
			if (audioClips == null || audioClips.Count == 0)
			{
				Debug.LogWarning("The audio clips could not be unregistered because the container list is empty or null");
				return;
			}

			for (int i = 0; i < audioClips.Count; i++)
			{
				GameManager.Instance.SoundManager.FreeAudio(GameManager.Instance.SoundManager.GetSoundFX(audioClips[i].name));
			}
		}

		/// <summary>
		/// Unregisters the audio clip in SoundManager.
		/// </summary>
		/// <param name="audioClip">The audio clip.</param>
		private void UnRegisterFXSound(ref AudioClip audioClip)
		{
			if (audioClip == null)
			{
				Debug.LogWarning("The audio clips could not be unregistered because it is null");
				return;
			}
			
			GameManager.Instance.SoundManager.FreeAudio(GameManager.Instance.SoundManager.GetSoundFX(audioClip.name));
		}

		/// <summary>
		/// Registers the audio contollers for all the music clips in SoundManager.
		/// </summary>
		/// <param name="audioClips">The audio clips.</param>
		private void RegisterMusicSounds(ref List<AudioClip> audioClips, bool is2D = false)
		{
			if (audioClips == null || audioClips.Count == 0)
			{
				Debug.LogWarning("The audio clips could not be registered because the container list is empty or null");
				return;
			}

			for (int i = 0; i < audioClips.Count; i++)
			{
                AudioController audioController = GameManager.Instance.SoundManager.LoadMusic(audioClips[i], is2D);
				if (audioController == null)
				{
					Debug.LogError(string.Format("The AudioController could not be instantiated for the audioclip: {0}", audioClips[i].name));
					continue;
				}
			}
		}

		/// <summary>
		/// Registers the FXS sound.
		/// </summary>
		/// <param name="audioClip">The audio clip.</param>
		private void RegisterMusicSound(ref AudioClip audioClip, bool is2D = false)
		{
			if (audioClip == null)
			{
				Debug.LogWarning("The audio clip could not be registered because it is null or empty");
				return;
			}

			AudioController audioController = GameManager.Instance.SoundManager.LoadMusic(audioClip, is2D);
			if (audioController == null)
			{
				Debug.LogError(string.Format("The AudioController could not be instantiated for the audioclip : {0}", audioClip.name));
			}
		}

        /// <summary>
        /// Unregisters all the audio controllers for all the music clips in SoundManager.
        /// </summary>
        /// <param name="audioClips">The audio clips.</param>
        private void UnRegisterMusicSounds(ref List<AudioClip> audioClips)
        {
			if (audioClips == null || audioClips.Count == 0)
			{
				Debug.LogWarning("The audio clips could not be unregistered because the container list is empty or null");
				return;
			}

			for (int i = 0; i < audioClips.Count; i++)
			{
                GameManager.Instance.SoundManager.FreeAudio(GameManager.Instance.SoundManager.GetMusic(audioClips[i].name));
			}
        }

		/// <summary>
		/// Unregisters the audio clip ind SoundManager.
		/// </summary>
		/// <param name="audioClip">The audio clip.</param>
		private void UnRegisterMusicSound(ref AudioClip audioClip)
		{
			if (audioClip == null)
			{
				Debug.LogWarning("The audio clips could not be unregistered because it is null");
				return;
			}

            GameManager.Instance.SoundManager.FreeAudio(GameManager.Instance.SoundManager.GetMusic(audioClip.name));
		}

		/// <summary>
		/// Gets a random audio controller from the list.
		/// </summary>
		/// <returns>The random audio controller.</returns>
		/// <param name="audioClips">The audio clips.</param>
		private AudioController GetRandomAudioControllerForFXSounds(ref List<AudioClip> audioClips)
		{
			if (audioClips == null || audioClips.Count == 0)
			{
				Debug.LogError("The AudioController could not be retrieved because the list is empty or null");
				return null;
			}

			return GameManager.Instance.SoundManager.GetSoundFX(audioClips[UnityEngine.Random.Range(0, audioClips.Count - 1)].name);
		}

		/// <summary>
		/// Gets a random audio controller from the list.
		/// </summary>
		/// <returns>The random audio controller.</returns>
		/// <param name="audioClips">The audio clips.</param>
		private AudioController GetRandomAudioControllerForMusicSounds(ref List<AudioClip> audioClips)
		{
			if (audioClips == null || audioClips.Count == 0)
			{
				Debug.LogError("The AudioController could not be retrieved because the list is empty or null");
				return null;
			}

            return GameManager.Instance.SoundManager.GetMusic(audioClips[UnityEngine.Random.Range(0, audioClips.Count - 1)].name);
		}

		/// <summary>
		/// Gets the audio controller for an audio clip.
		/// </summary>
		/// <returns>The audio controller for music sound.</returns>
		/// <param name="audioClip">The audio clip.</param>
		private AudioController GetAudioControllerForMusicSound(ref AudioClip audioClip)
        {
			if (audioClip == null)
			{
				Debug.LogError("The AudioController could not be retrieved because the audio clip is null");
				return null;
			}

            return GameManager.Instance.SoundManager.GetMusic(audioClip.name);
        }

		/// <summary>
		/// Gets the audio controller for an audio clip.
		/// </summary>
		/// <returns>The audio controller for fx sound.</returns>
		/// <param name="audioClip">The audio clip.</param>
		private AudioController GetAudioControllerForFXSound(ref AudioClip audioClip)
		{
			if (audioClip == null)
			{
				Debug.LogError("The AudioController could not be retrieved because the audio clip is null");
				return null;
			}

            return GameManager.Instance.SoundManager.GetSoundFX(audioClip.name);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Registers all the level musics.
		/// </summary>
		public void RegisterLevelMusic()
        {
            RegisterMusicSounds(ref _levelMusic);

            RegisterFXSounds(ref _chestBombSounds);
            RegisterFXSounds(ref _chestJumpSounds);
            RegisterFXSounds(ref _chestHealthSounds);
            RegisterFXSounds(ref _chestEnergySounds);
            RegisterFXSounds(ref _chestVelocitySounds);
			RegisterFXSounds(ref _chestInvulnerabilitySounds);
            RegisterFXSounds(ref _chestConfusionSounds);
		}

        /// <summary>
        /// Unregisters all the level musics.
        /// </summary>
        public void UnRegisterLevelMusic()
        {
            UnRegisterMusicSounds(ref _levelMusic);

			UnRegisterFXSounds(ref _chestBombSounds);
			UnRegisterFXSounds(ref _chestJumpSounds);
			UnRegisterFXSounds(ref _chestHealthSounds);
			UnRegisterFXSounds(ref _chestEnergySounds);
			UnRegisterFXSounds(ref _chestVelocitySounds);
			UnRegisterFXSounds(ref _chestInvulnerabilitySounds);
			UnRegisterFXSounds(ref _chestConfusionSounds);
        }

        /// <summary>
        /// Registers the boss music.
        /// </summary>
        public void RegisterBossMusic()
        {
            RegisterMusicSounds(ref _bossMusic);
            RegisterMusicSound(ref _defeatMusic);
        }

        /// <summary>
        /// Unregisters the register boss music.
        /// </summary>
        public void UnRegisterBossMusic()
        {
            UnRegisterMusicSounds(ref _bossMusic);
            UnRegisterMusicSound(ref _defeatMusic);
        }

		/// <summary>
		/// Registers all the menu musics.
		/// </summary>
		public void RegisterMenuMusic()
		{
            RegisterMusicSounds(ref _menuMusic);
		}

		/// <summary>
		/// Unregisters all the menu musics.
		/// </summary>
		public void UnRegisterMenuMusic()
		{
			UnRegisterMusicSounds(ref _menuMusic);
		}

        /// <summary>
        /// Registers all the score musics.
        /// </summary>
		public void RegisterScoreMusic()
		{
            RegisterMusicSounds(ref _scoreMusic);
		}

        /// <summary>
        /// Unregisters all the score musics.
        /// </summary>
		public void UnRegisterScoreMusic()
		{
            UnRegisterMusicSounds(ref _scoreMusic);
		}

        /// <summary>
        /// Registers the chests' sounds.
        /// </summary>
        public void RegisterChestsSounds()
        {
            RegisterFXSounds(ref _chestBombSounds);
            RegisterFXSounds(ref _chestConfusionSounds);
            RegisterFXSounds(ref _chestHealthSounds);
            RegisterFXSounds(ref _chestEnergySounds);
            RegisterFXSounds(ref _chestInvulnerabilitySounds);
            RegisterFXSounds(ref _chestJumpSounds);
            RegisterFXSounds(ref _chestVelocitySounds);
        }

        /// <summary>
        /// Unregisters the chests' sounds.
        /// </summary>
        public void UnRegisterChestsSounds()
        {
			UnRegisterFXSounds(ref _chestBombSounds);
			UnRegisterFXSounds(ref _chestConfusionSounds);
			UnRegisterFXSounds(ref _chestHealthSounds);
			UnRegisterFXSounds(ref _chestEnergySounds);
			UnRegisterFXSounds(ref _chestInvulnerabilitySounds);
			UnRegisterFXSounds(ref _chestJumpSounds);
			UnRegisterFXSounds(ref _chestVelocitySounds);
        }

        /// <summary>
        /// Registers all the environment sounds.
        /// </summary>
        public void RegisterEnvironmentSounds()
        {
            RegisterFXSounds(ref _bubbleSounds);
            RegisterFXSounds(ref _respawnSounds);
            RegisterFXSounds(ref _teletransportSounds);
            RegisterFXSound(ref _volcanoLavaSound);
            RegisterFXSound(ref _volcanoAlarmSound);
		}

        /// <summary>
        /// Unregisters all the environment sounds.
        /// </summary>
        public void UnRegisterEnvironmentSounds()
        {
            UnRegisterFXSounds(ref _bubbleSounds);
			UnRegisterFXSounds(ref _respawnSounds);
			UnRegisterFXSounds(ref _teletransportSounds);
            UnRegisterFXSound(ref _volcanoLavaSound);
			UnRegisterFXSound(ref _volcanoAlarmSound);
        }

        /// <summary>
        /// Registers all the misc sounds.
        /// </summary>
        public void RegisterMiscSounds()
        {
            RegisterFXSound(ref _narratorOne);
            RegisterFXSound(ref _narratorTwo);
            RegisterFXSound(ref _narratorThree);
            RegisterFXSound(ref _narratorFour);
            RegisterFXSound(ref _narratorFive);
            RegisterFXSound(ref _narratorPowerUp);
            RegisterFXSound(ref _narratorHurryUp);
            RegisterFXSound(ref _narratorCorrect);
            RegisterFXSound(ref _narratorFinalRound);
        }

		/// <summary>
		/// Unregisters all the misc sounds.
		/// </summary>
		public void UnRegisterMiscSounds()
        {
			UnRegisterFXSound(ref _narratorOne);
			UnRegisterFXSound(ref _narratorTwo);
			UnRegisterFXSound(ref _narratorThree);
			UnRegisterFXSound(ref _narratorFour);
			UnRegisterFXSound(ref _narratorFive);
			UnRegisterFXSound(ref _narratorPowerUp);
			UnRegisterFXSound(ref _narratorHurryUp);
			UnRegisterFXSound(ref _narratorCorrect);
			UnRegisterFXSound(ref _narratorFinalRound);
        }

        /// <summary>
        /// Registers the ui sounds.
        /// </summary>
        public void RegisterUISounds()
        {
            RegisterFXSound(ref _backButtonSound);
            RegisterFXSound(ref _passButtonSound);
            RegisterFXSound(ref _startButtonSound);
            RegisterFXSound(ref _selectButtonSound);
        }

		/// <summary>
		/// Unregisters the ui sounds.
		/// </summary>
		public void UnRegisterUISounds()
        {
			UnRegisterFXSound(ref _backButtonSound);
			UnRegisterFXSound(ref _passButtonSound);
			UnRegisterFXSound(ref _startButtonSound);
			UnRegisterFXSound(ref _selectButtonSound);
        }

        /// <summary>
        /// Picks up a random teletransport sound.
        /// </summary>
        /// <returns>The teletransport sound.</returns>
        public AudioController GetTeletransportSound()
        {
            return GetRandomAudioControllerForFXSounds(ref _teletransportSounds);
        }

        /// <summary>
        /// Picks up a random bubble sound.
        /// </summary>
        /// <returns>The bubble sound.</returns>
        public AudioController GetRandomBubbleSound()
        {
            return GetRandomAudioControllerForFXSounds(ref _bubbleSounds);
        }

        /// <summary>
        /// Picks up a random respawn sound.
        /// </summary>
        /// <returns>The respawn sound.</returns>
        public AudioController GetRandomRespawnSound()
        {
            return GetRandomAudioControllerForFXSounds(ref _respawnSounds);
        }

		/// <summary>
		/// Picks up the volcano alarm sound.
		/// </summary>
		/// <returns>The volcano alarm sound.</returns>
		public AudioController GetVolcanoAlarmSound()
        {
            return GetAudioControllerForFXSound(ref _volcanoAlarmSound);
        }

        /// <summary>
        /// Picks up the volcano lava sound.
        /// </summary>
        /// <returns>The volcano lava sound.</returns>
        public AudioController GetVolcanoLavaSound()
        {
            return GetAudioControllerForFXSound(ref _volcanoLavaSound);
        }

        /// <summary>
        /// Picks up a random level music.
        /// </summary>
        /// <returns>The level music.</returns>
        public AudioController GetRandomLevelMusic()
        {
            return GetRandomAudioControllerForMusicSounds(ref _levelMusic);
        }

		/// <summary>
		/// Picks up a random menu music.
		/// </summary>
		/// <returns>The menu music.</returns>
		public AudioController GetRandomMenuMusic()
        {
            return GetRandomAudioControllerForMusicSounds(ref _menuMusic);
        }

		/// <summary>
		/// Picks up a random battle music.
		/// </summary>
		/// <returns>The battle music.</returns>
		public AudioController GetRandomBossMusic()
        {
            return GetRandomAudioControllerForMusicSounds(ref _bossMusic);
        }

		/// <summary>
		/// Picks up a random score music.
		/// </summary>
		/// <returns>The score music.</returns>
		public AudioController GetRandomScoreMusic()
        {
            return GetRandomAudioControllerForMusicSounds(ref _scoreMusic);
        }

        /// <summary>
        /// Picks up the back button sound.
        /// </summary>
        /// <returns>The back button sound.</returns>
        public AudioController GetBackButtonSound()
        {
            return GetAudioControllerForFXSound(ref _backButtonSound);
        }

        /// <summary>
        /// Picks up the select button sound.
        /// </summary>
        /// <returns>The select button sound.</returns>
        public AudioController GetSelectButtonSound()
        {
            return GetAudioControllerForFXSound(ref _selectButtonSound);
        }

        /// <summary>
        /// Picks up the pass button sound.
        /// </summary>
        /// <returns>The pass button sound.</returns>
        public AudioController GetPassButtonSound()
        {
            return GetAudioControllerForFXSound(ref _passButtonSound);
        }

        /// <summary>
        /// Picks up the start button sound.
        /// </summary>
        /// <returns>The start button sound.</returns>
        public AudioController GetStartButtonSound()
        {
            return GetAudioControllerForFXSound(ref _startButtonSound);
        }

        /// <summary>
        /// Picks up a random chest bomb sound.
        /// </summary>
        /// <returns>The chest bomb sound.</returns>
        public AudioController GetChestBombSound()
        {
            return GetRandomAudioControllerForFXSounds(ref _chestBombSounds);
        }

		/// <summary>
		/// Picks up a random chest bomb sound.
		/// </summary>
		/// <returns>The chest bomb sound.</returns>
		public AudioController GetChestConfusionSound()
		{
            return GetRandomAudioControllerForFXSounds(ref _chestConfusionSounds);
		}

		/// <summary>
		/// Picks up a random chest health sound.
		/// </summary>
		/// <returns>The chest bomb sound.</returns>
		public AudioController GetChestHealthSound()
		{
            return GetRandomAudioControllerForFXSounds(ref _chestHealthSounds);
		}

		/// <summary>
		/// Picks up a random chest energy sound.
		/// </summary>
		/// <returns>The chest bomb sound.</returns>
		public AudioController GetChestEnergySound()
		{
            return GetRandomAudioControllerForFXSounds(ref _chestEnergySounds);
		}

		/// <summary>
		/// Picks up a random chest invulnerability sound.
		/// </summary>
		/// <returns>The chest bomb sound.</returns>
		public AudioController GetChestInvulnerabilitySound()
		{
            return GetRandomAudioControllerForFXSounds(ref _chestInvulnerabilitySounds);
		}

		/// <summary>
		/// Picks up a random chest jump sound.
		/// </summary>
		/// <returns>The chest bomb sound.</returns>
		public AudioController GetChestJumpSound()
		{
            return GetRandomAudioControllerForFXSounds(ref _chestJumpSounds);
		}

		/// <summary>
		/// Picks up a random chest velocity sound.
		/// </summary>
		/// <returns>The chest bomb sound.</returns>
		public AudioController GetChestVelocitySound()
		{
            return GetRandomAudioControllerForFXSounds(ref _chestVelocitySounds);
		}
        */
        #endregion
    }
}
