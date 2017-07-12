using nseh.Managers.General;
using nseh.Managers.Level;
using nseh.Managers.Main;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace nseh.Gameplay.Entities.Player
{
    public enum HealthMode
    {
        Normal = 0,
        Invulnerability = 1,
        Defense = 2
    }

    public partial class PlayerInfo : MonoBehaviour
    {

        #region Private Properties

        private int _deathCount;
        private float _bonificationDefense;
        private BarComponent _healthBar;
        private List<Image> _playerLives;
        private HealthMode _healthMode;
        private bool _isDead;

        #endregion

        #region Public C# Properties

        public float BonificationDefense
        {
            get { return _bonificationDefense; }
            set { _bonificationDefense = value; }
        }

        public float CurrentHealth
        {
            get { return _currentHealth; }
            set
            {
                _currentHealth = value;

                if (_healthBar != null)
                {
                    _healthBar.Value = _currentHealth;
                }
            }
        }

        public HealthMode HealthMode
        {
            get { return _healthMode; }
            set { _healthMode = value; }
        }

        public BarComponent HealthBar
        {
            set { _healthBar = value; }
        }

        public int MaxHealth
        {
            get { return _maxHealth; }
            set
            {
                _maxHealth = value;

                if (_healthBar != null)
                {
                    _healthBar.MaxValue = _maxHealth;
                }
            }
        }

        public List<Image> PlayerLives
        {
            get { return _playerLives; }
            set { _playerLives = value; }
        }

        #endregion

        #region Public Methods

        public void ResetHealth()
        {
            MaxHealth = _maxHealth;
            CurrentHealth = _maxHealth;
            _isDead = false;
        }

        public void ResetDeathCounter()
        {
            _deathCount = 0;
        }

        public void RestoreAllLives()
        {
            foreach (Image life in PlayerLives)
            {
                if (life.enabled == false)
                {
                    life.enabled = true;
                }
            }
        }

        public void IncreaseHealth(float percent)
        {
            if (percent > 0.0f)
            {
                var oldHealth = CurrentHealth;

                float amount = (MaxHealth * percent / 100.0f);
                CurrentHealth += amount;
                CurrentHealth = (int)Mathf.Clamp(CurrentHealth, 0.0f, _maxHealth);

            }
        }

        public void DecreaseHealth(float percent)
        {
            if (_healthMode == HealthMode.Normal && percent > 0.0f)
            {
                var oldHealth = CurrentHealth;

                float amount = (MaxHealth * percent / 100.0f);
                CurrentHealth -= amount;
                CurrentHealth = (int)Mathf.Clamp(CurrentHealth, 0.0f, _maxHealth);
                if (CurrentHealth == 0.0f && !_isDead )
                {
                    DecreaseScore(_penalization);
                    StartCoroutine(LoseLife(3));
                    _animator.SetTrigger("Dead");
                }
            }
        }

        public void TakeDamage(float pureDamage)
        {
            CurrentHealth -= pureDamage;
            CurrentHealth = (int)Mathf.Clamp(CurrentHealth, 0.0f, _maxHealth);
            IncreaseEnergy(pureDamage);
            GameManager.Instance.SoundManager.PlayAudioFX(_hitClip[Random.Range(0, _hitClip.Count)], 1f, false, transform.position, 0, Random.Range(-0.1F, 0.1F));

            if (CurrentHealth == 0)
            {
                DecreaseScore(_penalization);
                StartCoroutine(LoseLife(3));
                _animator.SetTrigger("Dead");
            }
            else
            {
                _animator.SetTrigger("Hurt");
                //GameManager.Instance.SoundManager.PlayAudioFX(_takeDamageClip[Random.Range(0, _takeDamageClip.Count)], 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);
            }
        }

        public void TakeDamage(float pureDamage, PlayerInfo sender)
        {
            CurrentHealth -= pureDamage;
            CurrentHealth = (int)Mathf.Clamp(CurrentHealth, 0.0f, _maxHealth);
            IncreaseEnergy(pureDamage);
            sender.IncreaseEnergy(pureDamage / 2);
            sender.IncreaseScore((int)pureDamage);
            GameManager.Instance.SoundManager.PlayAudioFX(_hitClip[Random.Range(0, _hitClip.Count)], 1f, false, transform.position, 0, Random.Range(-0.1F, 0.1F));

            if (CurrentHealth == 0)
            {
                DecreaseScore(_penalization);
                StartCoroutine(LoseLife(3));
                _animator.SetTrigger("Dead");
            }
            else
            {
                _animator.SetTrigger("Hurt");
                //GameManager.Instance.SoundManager.PlayAudioFX(_takeDamageClip[Random.Range(0, _takeDamageClip.Count)], 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);
            }
        }


        public void TakeDamage(float amount, PlayerInfo sender, Vector3 position)
        {
            if(_healthMode == HealthMode.Normal && !_isDead)
            {
                float damage = (float)(amount - (amount * 0.025 * CurrentEndurance));
                
                CurrentHealth -= damage;
                CurrentHealth = (int)Mathf.Clamp(CurrentHealth, 0.0f, _maxHealth);
                IncreaseEnergy(damage);
                sender.IncreaseEnergy(damage/2);
                sender.IncreaseScore((int)damage);
                GameManager.Instance.SoundManager.PlayAudioFX(_hitClip[Random.Range(0, _hitClip.Count)], 1f, false, position, 0, Random.Range(-0.1F, 0.1F));

                GameObject particleGameObject = Instantiate(sender._hitParticle, position, _hitParticle.transform.rotation, this.gameObject.transform);
                foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
                {
                    particle_aux.Play();
                }

                Destroy(particleGameObject, 1f);


                if (CurrentHealth == 0)
                {
                    DecreaseScore(_penalization);
                    StartCoroutine(LoseLife(3));
                    _animator.SetTrigger("Dead");
                }
                else
                {
                    _animator.SetTrigger("Hurt");
                    //GameManager.Instance.SoundManager.PlayAudioFX(_takeDamageClip[Random.Range(0, _takeDamageClip.Count)], 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);
                }

            }

            else if (_healthMode == HealthMode.Defense && !_isDead)
            {
                sender.Animator.SetTrigger("Parry");
            }

        }

        public IEnumerator DecreaseHealthForEverySecond(float percent, float seconds)
        {
            int counterSeconds = 0;

            while (counterSeconds < seconds)
            {
                DecreaseHealth(percent);
                counterSeconds++;

                yield return new WaitForSeconds(1.0f);
            }
        }

        public IEnumerator IncreaseHealthForEverySecond(float percent, float seconds)
        {
            int counterSeconds = 0;

            while (counterSeconds < seconds)
            {
                IncreaseHealth(percent);
                counterSeconds++;

                yield return new WaitForSeconds(1.0f);
            }
        }

        public IEnumerator InvulnerabilityModeForSeconds(float seconds)
        {
            _healthMode = HealthMode.Invulnerability;

            yield return new WaitForSeconds(seconds);

            _healthMode = HealthMode.Normal;
        }

        public IEnumerator BonificationDefenseForSeconds(int points, float seconds)
        {
            _currentEndurance = _baseEndurance;
            _currentEndurance += points;

            yield return new WaitForSeconds(seconds);

            _currentEndurance = _baseEndurance;
        }

        public IEnumerator BonificationBaseDefenseForSeconds(int points, float seconds)
        {
            _baseEndurance += points;
            _currentEndurance += points;

            yield return new WaitForSeconds(seconds);

            _baseEndurance -= points;
            _currentEndurance -= points;
        }

        public IEnumerator BonificationAttackForSeconds(int points, float seconds)
        {
            _currentStrength = _baseStrength;
            _currentStrength += points;

            yield return new WaitForSeconds(seconds);

            _currentStrength = _baseStrength;
        }

        public IEnumerator BonificationBaseAttackForSeconds(int points, float seconds)
        {
            _baseStrength += points;
            _currentStrength += points;

            yield return new WaitForSeconds(seconds);

            _baseStrength -= points;
            _currentStrength -= points;
        }

        #endregion

        #region Private Methods

        private void DisableLife(int life)
        {
            switch (life)
            {
                case 1:
                    PlayerLives[0].enabled = false;
                    break;
                case 2:
                    PlayerLives[1].enabled = false;
                    break;
                case 3:
                    PlayerLives[2].enabled = false;
                    break;
                default:
                    return;
            }
        }

        private void Death()
        {
            _deathCount++;
            _isDead = true;

            GameManager.Instance.SoundManager.PlayAudioFX(_deathClip, 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);
        }

        private IEnumerator LoseLife(float respawnTime)
        {
            Death();

            yield return new WaitForSeconds(respawnTime);

            switch (SceneManager.GetActiveScene().name)
            {
                case "Game":
                    GameManager.Instance.GameEvent.ResetFromDeath(this.gameObject);
                    break;

                case "Boss":
                    GameManager.Instance.BossEvent.ResetFromDeath(this.gameObject);
                    break;
            }

            _isDead = false;
        }

        #endregion

    }
}
