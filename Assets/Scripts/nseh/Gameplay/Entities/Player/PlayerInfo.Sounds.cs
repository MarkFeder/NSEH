using System.Collections.Generic;
using nseh.Gameplay.Combat;
using nseh.Gameplay.Combat.Defense;
using nseh.Managers.Audio;
using nseh.Managers.Main;
using UnityEngine;

namespace nseh.Gameplay.Entities.Player
{
    public partial class PlayerInfo : MonoBehaviour
    {
        #region Private Properties

        [Header("Player's fx sounds")]

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
        private List<AudioClip> _defenseClips;
        [SerializeField]
        private List<AudioClip> _jumpClips;
        [SerializeField]
        private List<AudioClip> _hitClips;

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
        public AudioController GetAudioControllerForDefense(DefenseType type)
        {
            AudioController audioController = null;

            switch (type)
            {
                case DefenseType.NormalDefense:
                    audioController = GetRandomAudioController(ref _defenseClips);
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

		#endregion

		#region Private Methods

        /// <summary>
        /// Initialise all the audio controller lists with all their clips and add them
        /// to the SoundManager.
        /// </summary>
		private void SetupSounds()
        {
			SetupAudioClips(ref _comboAAA01Clips);
			SetupAudioClips(ref _comboAAA02Clips);
			SetupAudioClips(ref _comboAAA03Clips);
			SetupAudioClips(ref _comboBB01Clips);
			SetupAudioClips(ref _comboBB02Clips);
			SetupAudioClips(ref _comboBSharpClips);
			SetupAudioClips(ref _definitiveClips);
			SetupAudioClips(ref _habilityClips);
			SetupAudioClips(ref _defenseClips);
			SetupAudioClips(ref _jumpClips);
			SetupAudioClips(ref _hitClips);
		}

		/// <summary>
		/// Setups the audio contollers for all the clips.
		/// </summary>
		/// <param name="audioClips">The audio clips.</param>
		private void SetupAudioClips(ref List<AudioClip> audioClips)
        {
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

            return GameManager.Instance.SoundManager.GetSoundFX(audioClips[Random.Range(0, audioClips.Count - 1)].name);
		}

		#endregion
	}
}
