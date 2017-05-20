using nseh.Gameplay.Base.Abstract;
using nseh.Gameplay.Entities.Player;
using UnityEngine;

namespace nseh.Gameplay.Combat.Defense
{
    public enum DefenseType
    {
        None = 0,
        NormalDefense = 1
    }

    public class CharacterDefense : HandledAction
    {
        #region Private Properties

        [SerializeField]
        private DefenseType _currentMode;

        #endregion

        #region Public C# Properties

        public DefenseType CurrentMode { get { return _currentMode; } }

        #endregion

        protected override void Start()
        {
			// Register info
            base.Start();

            _playerInfo = gameObject.transform.root.GetComponent<PlayerInfo>();
            _animator = _playerInfo.Animator;

            _hash = _playerInfo.GetHash(_currentMode);
            _stateName = _playerInfo.GetStateNameInfo(_currentMode);
            _button = _playerInfo.GetButton(_currentMode);

            _paramType = TypeOfParamAnimator(_hash);

			// Set this defense to be enabled from the start
			_enabled = true;
        }
    }
}
