using System.Collections.Generic;
using UnityEngine;
using nseh.Managers.Main;
using System.Linq;
using nseh.Utils;
using nseh.Gameplay.Entities.Player;

namespace nseh.Gameplay.Base.Abstract.Gameflow
{
    public class LavaGameComponent : MonoBehaviour
    {

        #region Private Properties

        private List<GameObject> _playersInLava;
        private float _nextApplyEffect = 0;
        private Animator _animator;

        #endregion

        #region Public Properties

        public AudioClip alarm;

        #endregion

        #region Private Methods

        void Start()
        {
            _playersInLava = new List<GameObject>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_playersInLava.Any())
            {
                DealDamagePeriodically();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(Constants.Tags.PLAYER_BODY) && !PlayerListContains(other.gameObject))
            {
                _playersInLava.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _playersInLava.Remove(other.gameObject);
        }

        private bool PlayerListContains(GameObject playerToRegister)
        {
            PlayerInfo player = playerToRegister.GetComponent<PlayerInfo>();

            if (_playersInLava.Any())
            {
                foreach (PlayerInfo element in _playersInLava.Select(t => t.GetComponent<PlayerInfo>()))
                {
                    if (element.GamepadIndex == player.GamepadIndex)
                    { return true; }
                }
            }

            return false;
        }

        private void DealDamagePeriodically()
        {
            if (Time.time >= _nextApplyEffect)
            {
                _nextApplyEffect = Time.time + Constants.Events.Tar_Event.TAR_TICKDAMAGE;
                foreach (PlayerInfo element in _playersInLava.Select(t => t.GetComponent<PlayerInfo>()))
                {
                    element.DecreaseHealth(Constants.Events.Tar_Event.TAR_DAMAGE);
                }
            }
        }

        #endregion

        #region public Methods

        public void LavaMotion()
        {
            _animator.SetTrigger("Start");
            GameManager.Instance.SoundManager.PlayAudioFX(alarm, 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);
        }

        public void ResetLava()
        {
            _animator.SetTrigger("Restart");
            _nextApplyEffect = 0;
            _playersInLava = new List<GameObject>();
        }

        #endregion

    }
}





