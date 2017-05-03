using nseh.Gameplay.Base.Abstract;
using nseh.Gameplay.Entities.Player;
using System;
using System.Collections;
using UnityEngine;

namespace nseh.Gameplay.Combat
{
    public enum AttackType
    {
        None = 0,
        CharacterAttackAStep1 = 1,
        CharacterAttackAStep2 = 2,
        CharacterAttackAStep3 = 3,
        CharacterAttackBStep1 = 4,
        CharacterAttackBStep2 = 5,
        CharacterAttackBSharp = 6,
        CharacterHability = 7,
        CharacterDefinitive = 8,
    }

    public class CharacterAttack : HandledAction 
    {
        #region Protected Properties

        [SerializeField]
        protected float _initialDamage;
        [SerializeField]
        protected AttackType _attackType;

        protected float _currentDamage;

        protected bool _critical;
        protected bool _enabled;

        #endregion

        #region Private Properties



        #endregion

        #region Public C# Properties

        public float InitialDamage { get { return _initialDamage; } }

        public float CurrentDamage { get { return _currentDamage; } set { _currentDamage = value; } }

        public bool Critical { get { return _critical;  } set { _critical = value; } }

        public bool EnabledAttack { get { return _enabled; } set { _enabled = value; } }

        public bool IsCombo
        {
            get
            {
                return AttackType == AttackType.CharacterAttackAStep1 ||
                       AttackType == AttackType.CharacterAttackAStep2 ||
                       AttackType == AttackType.CharacterAttackAStep3 ||
                       AttackType == AttackType.CharacterAttackBStep1 ||
                       AttackType == AttackType.CharacterAttackBStep2;
            }
        }

        public bool IsSimpleAttack
        {
            get
            {
                return !(AttackType == AttackType.CharacterDefinitive ||
                       AttackType == AttackType.CharacterHability);
            }
        }

        public AttackType AttackType { get { return _attackType; } }

        #endregion

        protected virtual void Start()
        {
            _playerInfo = gameObject.transform.root.GetComponent<PlayerInfo>();
            _animator = _playerInfo.Animator;

            _currentDamage = _initialDamage;

            _hash = _playerInfo.GetHash(_attackType);
            _stateName = _playerInfo.GetStateNameInfo(_attackType);
            _button = _playerInfo.GetButton(_attackType);

            _paramType = TypeOfParamAnimator(_hash);

            _enabled = true;
            _critical = false;
        }

        /// <summary>
        /// Init this attack action (for external calls).
        /// </summary>
        /// <param name="currentDamage">The current damage of this attack.</param>
        /// <param name="initialDamage">The initial damage of this attack.</param>
        /// <param name="attackType">The type of this attack.</param>
        public void InitAttackAction(float currentDamage, float initialDamage, AttackType attackType)
        {
            _playerInfo = gameObject.transform.root.GetComponent<PlayerInfo>();
            _animator = _playerInfo.Animator;

            _attackType = attackType;
            _currentDamage = currentDamage;
            _initialDamage = initialDamage;

            _hash = _playerInfo.GetHash(_attackType);
            _stateName = _playerInfo.GetStateNameInfo(_attackType);
            _button = _playerInfo.GetButton(_attackType);

            _paramType = TypeOfParamAnimator(_hash);

            _enabled = true;
            _critical = false;
        }

        #region Public Methods

        /// <summary>
        /// Start attack action.
        /// </summary>
        public override void StartAction()
        {
            if (_enabled)
            {
                if (IsSimpleAttack && _critical)
                {
                    base.StartAction();

                    _critical = false;
                    _currentDamage = _initialDamage;
                }
                else
                {
                    base.StartAction();
                } 
            }
        }

        /// <summary>
        /// Increase damage by percent for a total of seconds.
        /// It is used for external coroutines.
        /// </summary>
        /// <param name="percent">The percent to be incremented.</param>
        /// <param name="seconds">The number of seconds this increase take.</param>
        /// <returns></returns>
        public IEnumerator IncreaseDamageForSecondsExternal(float percent, float seconds)
        {
            var oldDamage = _currentDamage;

            IncreaseDamage(percent);

            yield return new WaitForSeconds(seconds);

            Debug.Log(string.Format("[{0}] damage of {1} has been restored to: {2}", AttackType.ToString(), _playerInfo.PlayerName, oldDamage));

            _currentDamage = oldDamage;
        }

        /// <summary>
        /// Increase damage by percent for a total of seconds.
        /// </summary>
        /// <param name="percent">The percent to be incremented.</param>
        /// <param name="seconds">The number of seconds this increase take.</param>
        public void IncreaseDamageForSeconds(float percent, float seconds)
        {
            StartCoroutine(IncreaseDamageForSecondsInternal(percent, seconds));
        }

        /// <summary>
        /// Decrease damage by percent for a total of seconds.
        /// </summary>
        /// <param name="percent">The percent to be decreased.</param>
        /// <param name="seconds">The number of seconds this decrease takes.</param>
        public void DecreaseDamageForSeconds(float percent, float seconds)
        {
            StartCoroutine(DecreaseDamageForSecondsInternal(percent, seconds));
        }

        /// <summary>
        /// Increase Damage.
        /// </summary>
        /// <param name="percent">The percent to be incremented.</param>
        public void IncreaseDamage(float percent)
        {
            if (percent > 0.0f)
            {
                var oldDamage = _currentDamage;

                _currentDamage += (_currentDamage * percent / 100.0f);

                Debug.Log(String.Format("[{0}] damage of {1} is: {2} and applying {3}% more has changed to: {4}",
                    AttackType.ToString(), _playerInfo.PlayerName, oldDamage, percent, _currentDamage));
            }
        }

        /// <summary>
        /// Decrease damage.
        /// </summary>
        /// <param name="percent">The percent to be decreased.</param>
        public void DecreaseDamage(float percent)
        {
            if (percent > 0.0f)
            {
                var oldDamage = _currentDamage;

                _currentDamage -= (_currentDamage * percent/100.0f);

                Debug.Log(String.Format("[{0}] damage of {1} is: {2} and reducing {3}% has changed to: {4}",
                    AttackType.ToString(), _playerInfo.PlayerName, oldDamage, percent, _currentDamage));
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator IncreaseDamageForSecondsInternal(float percent, float seconds)
        {
            var oldDamage = _currentDamage;

            IncreaseDamage(percent);

            yield return new WaitForSeconds(seconds);

            Debug.Log(string.Format("[{0}] damage of {1} has been restored to: {2}", AttackType.ToString(), _playerInfo.PlayerName, oldDamage));

            _currentDamage = oldDamage;
        }

        private IEnumerator DecreaseDamageForSecondsInternal(float percent, float seconds)
        {
            int counterSeconds = 0;
            var oldDamage = _currentDamage;

            while (counterSeconds < seconds)
            {
                DecreaseDamage(percent);
                counterSeconds++;

                yield return new WaitForSeconds(1.0f);
            }
        }

        #endregion
    }
}
