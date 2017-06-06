using System.Collections.Generic;
using nseh.Gameplay.Combat;
using nseh.Gameplay.Combat.Defense;
using nseh.Managers.Audio;
using nseh.Managers.Main;
using UnityEngine;

namespace nseh.Gameplay.Entities.Player
{
    public class PlayerSounds : MonoBehaviour
    {
        #region Private Properties

        [SerializeField]
        private List<AudioClip> _comboAAA01Clips;
        [SerializeField]
        private List<AudioClip> _comboAAA02Clips;
        [SerializeField]
        private List<AudioClip> _comboAAA03Clips;
        [SerializeField]
        private List<AudioClip> _comboBB01Clips;
        [SerializeField]
        private List<AudioClip> _comboBB02Clips;
        [SerializeField]
        private List<AudioClip> _comboBSharpClips;
        [SerializeField]
        private List<AudioClip> _definitiveClips;
        [SerializeField]
        private List<AudioClip> _habilityClips;
        [SerializeField]
        private List<AudioClip> _impactDefenseClips;
        [SerializeField]
        private List<AudioClip> _jumpClips;
        [SerializeField]
        private List<AudioClip> _hitClips;
        [SerializeField]
        private List<AudioClip> _fallClips;
        [SerializeField]
        private List<AudioClip> _deathClips;

		#endregion

		#region Private Methods

		private void Start()
		{
			RegisterPlayerSounds();
		}

		/// <summary>
		/// Initialise all the audio controller lists with all their clips and add them
		/// to the SoundManager.
		/// </summary>
		private void RegisterPlayerSounds()
		{
			RegisterAudioClips(ref _comboAAA01Clips);
			RegisterAudioClips(ref _comboAAA02Clips);
			RegisterAudioClips(ref _comboAAA03Clips);
			RegisterAudioClips(ref _comboBB01Clips);
			RegisterAudioClips(ref _comboBB02Clips);
			RegisterAudioClips(ref _comboBSharpClips);
			RegisterAudioClips(ref _definitiveClips);
			RegisterAudioClips(ref _habilityClips);
			RegisterAudioClips(ref _impactDefenseClips);
			RegisterAudioClips(ref _jumpClips);
			RegisterAudioClips(ref _hitClips);
            RegisterAudioClips(ref _fallClips);
            RegisterAudioClips(ref _deathClips);
		}

		/// <summary>
		/// Setups the audio contollers for all the clips.
		/// </summary>
		/// <param name="audioClips">The audio clips.</param>
		private void RegisterAudioClips(ref List<AudioClip> audioClips)
		{
			if (audioClips == null || audioClips.Count == 0)
			{
				Debug.LogWarning("The audio clips could not be instantiated because the container list is empty or null");
				return;
			}

			for (int i = 0; i < audioClips.Count; i++)
			{
				AudioController audioController = GameManager.Instance.SoundManager.LoadSoundFX(audioClips[i], false);
				if (audioController == null)
				{
					Debug.LogError(string.Format("The AudioController could not be instantiated for the audioclip: {0}", audioClips[i].name));
					continue;
				}
			}
		}

		/// <summary>
		/// Gets a random audio controller from the list.
		/// </summary>
		/// <returns>The random audio controller.</returns>
		/// <param name="audioClips">The Audio clips.</param>
		private AudioController GetRandomAudioController(ref List<AudioClip> audioClips)
		{
			if (audioClips == null || audioClips.Count == 0)
			{
				Debug.LogError("The AudioController could not be retrieved because the list is empty or null");
				return null;
			}

			return GameManager.Instance.SoundManager.GetSoundFX(audioClips[UnityEngine.Random.Range(0, audioClips.Count - 1)].name);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Gets a random audio controller for the attack type.
		/// </summary>
		/// <returns>The audio controller for attack.</returns>
		/// <param name="attackType">Attack type.</param>
		public AudioController GetAudioControllerForAttack(AttackType attackType)
        {
            AudioController audioController = null;

            switch (attackType)
			{
				case AttackType.CharacterAttackAStep1:
                    audioController = GetRandomAudioController(ref _comboAAA01Clips);
					break;

				case AttackType.CharacterAttackAStep2:
					audioController = GetRandomAudioController(ref _comboAAA02Clips);
					break;

				case AttackType.CharacterAttackAStep3:
					audioController = GetRandomAudioController(ref _comboAAA03Clips);
					break;

				case AttackType.CharacterAttackBStep1:
                    audioController = GetRandomAudioController(ref _comboBB01Clips);
                    break;

				case AttackType.CharacterAttackBStep2:
					audioController = GetRandomAudioController(ref _comboBB02Clips);
                    break;

				case AttackType.CharacterAttackBSharp:
                    audioController = GetRandomAudioController(ref _comboBSharpClips);
                    break;

				case AttackType.CharacterHability:
                    audioController = GetRandomAudioController(ref _habilityClips);
                    break;

				case AttackType.CharacterDefinitive:
                    audioController = GetRandomAudioController(ref _definitiveClips);
                    break;

				default:
					Debug.Log("AttackType unrecognized!");
					break;
			}

            return audioController;
		}

        /// <summary>
        /// Gets a random audio controller for the defense type.
        /// </summary>
        /// <returns>The audio controller for defense.</returns>
        /// <param name="type">Type.</param>
        public AudioController GetAudioControllerForImpactDefense(DefenseType type)
        {
            AudioController audioController = null;

            switch (type)
            {
                case DefenseType.NormalDefense:
                    audioController = GetRandomAudioController(ref _impactDefenseClips);
                    break;

                default:
                    Debug.Log("DefenseType unrecognized!");
                    break;
            }

            return audioController;
        }

        /// <summary>
        /// Gets a random audio controller for the jump.
        /// </summary>
        /// <returns>The audio controller for jump.</returns>
        public AudioController GetAudioControllerForJump()
        {
            return GetRandomAudioController(ref _jumpClips);
        }

        /// <summary>
        /// Gets a random audio controller for the hit.
        /// </summary>
        /// <returns>The audio controller for hit.</returns>
        public AudioController GetAudioControllerForHit()
        {
            return GetRandomAudioController(ref _hitClips);
        }

        /// <summary>
        /// Gets a random audio controller for death.
        /// </summary>
        /// <returns>The audio controller for death.</returns>
        public AudioController GetAudioControllerForDeath()
        {
            return GetRandomAudioController(ref _deathClips);
        }

        /// <summary>
        /// Gets a random audio controller for fall.
        /// </summary>
        /// <returns>The audio controller for fall.</returns>
        public AudioController GetAudioControllerForFall()
        {
            return GetRandomAudioController(ref _fallClips);
        }

        #endregion
    }
}
