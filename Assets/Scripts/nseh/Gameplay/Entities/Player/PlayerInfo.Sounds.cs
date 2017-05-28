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

        private List<AudioController> _comboAAA01AudioControllers;
        private List<AudioController> _comboAAA02AudioControllers;
        private List<AudioController> _comboAAA03AudioControllers;
        private List<AudioController> _comboBB01AudioControllers;
        private List<AudioController> _comboBB02AudioControllers;
        private List<AudioController> _comboCSharpAudioControllers;
        private List<AudioController> _definitiveAudioControllers;
        private List<AudioController> _habilityAudioControllers;
        private List<AudioController> _defenseAudioControllers;
        private List<AudioController> _jumpAudioControllers;
        private List<AudioController> _hitAudioControllers;

        #endregion

        #region Public Properties

        public AudioController ComboAAA01AudioController
        {
            get
            {
                if (_comboAAA01AudioControllers != null && _comboAAA01AudioControllers.Count > 0)
                {
                    return _comboAAA01AudioControllers[Random.Range(0, _comboAAA01AudioControllers.Count - 1)];
                }

                return null;
            }
        }

		public AudioController ComboAAA02AudioController
		{
			get
			{
				if (_comboAAA02AudioControllers != null && _comboAAA02AudioControllers.Count > 0)
				{
					return _comboAAA02AudioControllers[Random.Range(0, _comboAAA02AudioControllers.Count - 1)];
				}

				return null;
			}
		}

		public AudioController ComboAAA03AudioController
		{
			get
			{
				if (_comboAAA03AudioControllers != null && _comboAAA03AudioControllers.Count > 0)
				{
					return _comboAAA03AudioControllers[Random.Range(0, _comboAAA03AudioControllers.Count - 1)];
				}

				return null;
			}
		}

		public AudioController ComboBB01AudioController
		{
			get
			{
				if (_comboBB01AudioControllers != null && _comboBB01AudioControllers.Count > 0)
				{
					return _comboBB01AudioControllers[Random.Range(0, _comboBB01AudioControllers.Count - 1)];
				}

				return null;
			}
		}

		public AudioController ComboBB02AudioController
		{
			get
			{
				if (_comboBB02AudioControllers != null && _comboBB02AudioControllers.Count > 0)
				{
					return _comboBB02AudioControllers[Random.Range(0, _comboBB02AudioControllers.Count - 1)];
				}

				return null;
			}
		}

		public AudioController ComboCSharpAudioController
		{
			get
			{
				if (_comboCSharpAudioControllers != null && _comboCSharpAudioControllers.Count > 0)
				{
					return _comboCSharpAudioControllers[Random.Range(0, _comboCSharpAudioControllers.Count - 1)];
				}

				return null;
			}
		}

        public AudioController DefinitiveAudioController
		{
			get
			{
				if (_definitiveAudioControllers != null && _definitiveAudioControllers.Count > 0)
				{
					return _definitiveAudioControllers[Random.Range(0, _definitiveAudioControllers.Count - 1)];
				}

				return null;
			}
		}

		public AudioController HabilityAudioController
		{
			get
			{
				if (_habilityAudioControllers != null && _habilityAudioControllers.Count > 0)
				{
					return _habilityAudioControllers[Random.Range(0, _habilityAudioControllers.Count - 1)];
				}

				return null;
			}
		}

        public AudioController DefenseAudioController
		{
			get
			{
				if (_defenseAudioControllers != null && _defenseAudioControllers.Count > 0)
				{
					return _defenseAudioControllers[Random.Range(0, _defenseAudioControllers.Count - 1)];
				}

				return null;
			}
		}

		public AudioController JumpAudioController
		{
			get
			{
				if (_jumpAudioControllers != null && _jumpAudioControllers.Count > 0)
				{
					return _jumpAudioControllers[Random.Range(0, _jumpAudioControllers.Count - 1)];
				}

				return null;
			}
		}

		public AudioController HitAudioController
		{
			get
			{
				if (_hitAudioControllers != null && _hitAudioControllers.Count > 0)
				{
					return _hitAudioControllers[Random.Range(0, _hitAudioControllers.Count - 1)];
				}

				return null;
			}
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
                    audioController = GetRandomAudioController(_comboAAA01AudioControllers);
					break;

				case AttackType.CharacterAttackAStep2:
					audioController = GetRandomAudioController(_comboAAA02AudioControllers);
                    break;

				case AttackType.CharacterAttackAStep3:
					audioController = GetRandomAudioController(_comboAAA03AudioControllers);
                    break;

				case AttackType.CharacterAttackBStep1:
					audioController = GetRandomAudioController(_comboBB01AudioControllers);
                    break;

				case AttackType.CharacterAttackBStep2:
					audioController = GetRandomAudioController(_comboBB02AudioControllers);
                    break;

				case AttackType.CharacterAttackBSharp:
					audioController = GetRandomAudioController(_comboCSharpAudioControllers);
                    break;

				case AttackType.CharacterHability:
					audioController = GetRandomAudioController(_habilityAudioControllers);
                    break;

				case AttackType.CharacterDefinitive:
					audioController = GetRandomAudioController(_definitiveAudioControllers);
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
                    audioController = GetRandomAudioController(_defenseAudioControllers);
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
            return GetRandomAudioController(_jumpAudioControllers);
        }

        /// <summary>
        /// Gets a random audio controller for the hit.
        /// </summary>
        /// <returns>The audio controller for hit.</returns>
        public AudioController GetAudioControllerForHit()
        {
            return GetRandomAudioController(_hitAudioControllers);
        }

		#endregion

		#region Private Methods

        /// <summary>
        /// Initialise all the audio controller lists with all their clips and add them
        /// to the SoundManager.
        /// </summary>
		private void SetupSounds()
        {
            // Initialize audio controllers lists
    		_comboAAA01AudioControllers = new List<AudioController>();
    		_comboAAA02AudioControllers = new List<AudioController>();
    		_comboAAA03AudioControllers = new List<AudioController>();
    		_comboBB01AudioControllers = new List<AudioController>();
    		_comboBB02AudioControllers = new List<AudioController>();
    		_comboCSharpAudioControllers = new List<AudioController>();
    		_definitiveAudioControllers = new List<AudioController>();
    		_habilityAudioControllers = new List<AudioController>();
            _defenseAudioControllers = new List<AudioController>();
            _jumpAudioControllers = new List<AudioController>();
            _hitAudioControllers = new List<AudioController>();

            // Setup audio controllers
            SetupAudioContollersForClips(_comboAAA01AudioControllers, _comboAAA01Clips);
            SetupAudioContollersForClips(_comboAAA02AudioControllers, _comboAAA02Clips);
            SetupAudioContollersForClips(_comboAAA03AudioControllers, _comboAAA03Clips);
            SetupAudioContollersForClips(_comboBB01AudioControllers, _comboBB01Clips);
            SetupAudioContollersForClips(_comboBB02AudioControllers, _comboBB02Clips);
			SetupAudioContollersForClips(_comboCSharpAudioControllers, _comboBSharpClips);
			SetupAudioContollersForClips(_definitiveAudioControllers, _definitiveClips);
            SetupAudioContollersForClips(_habilityAudioControllers, _habilityClips);
            SetupAudioContollersForClips(_defenseAudioControllers, _defenseClips);
			SetupAudioContollersForClips(_jumpAudioControllers, _jumpClips);
			SetupAudioContollersForClips(_hitAudioControllers, _hitClips);
		}

        /// <summary>
        /// Setups the audio contollers for all the clips.
        /// </summary>
        /// <param name="audioControllers">The audio controllers.</param>
        /// <param name="audioClips">The audio clips.</param>
        private void SetupAudioContollersForClips(List<AudioController> audioControllers, List<AudioClip> audioClips)
        {
			for (int i = 0; i < audioClips.Count; i++)
			{
				AudioController audioController = GameManager.Instance.SoundManager.LoadSoundFX(audioClips[i], false);
				if (audioController == null)
				{
					Debug.LogError(string.Format("The AudioController could not be instantiated for the audioclip: {0}", audioClips[i].name));
					continue;
				}

				audioControllers.Add(audioController);
			}
        }

        /// <summary>
        /// Gets a random audio controller from the list.
        /// </summary>
        /// <returns>The random audio controller if exists; null otherwise.</returns>
        /// <param name="audioControllers">Audio controllers.</param>
        private AudioController GetRandomAudioController(List<AudioController> audioControllers)
        {
			if (audioControllers == null || audioControllers.Count > 0)
			{
                Debug.LogError("The AudioController could not be retrieved because the list is empty or null");
                return null;
            }

            return audioControllers[Random.Range(0, audioControllers.Count - 1)];
        }

        #endregion
    }
}
