using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.Gameplay.Entities.Player;
using nseh.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using nseh.Managers.Audio;

namespace nseh.Gameplay.Gameflow
{
    public class Tar : TarComponent
    {
        //public Animator animator;

        #region Private Properties

        private float _nextApplyEffect = 0;
        private List<GameObject> _playersInTar;
        private Animator _animator;

        public AudioClip alarm;

        #endregion

        #region Protected Methods

        protected override bool TarUp(float elapsedTime)
        {
            //animator.SetBool("Motion", true);
            SoundManager.Instance.PlayAudioFX(alarm, 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);
            _animator.SetTrigger("Start");
            //targetTarPosition = new Vector3(transform.position.x, platformPosition.y, transform.position.z);
            //transform.position = Vector3.Lerp(transform.position, targetTarPosition, elapsedTime / 80.0f);

            if (transform.position == targetTarPosition)
            {
                //Debug.Log("Tar is up. " + "(" + elapsedTime + ")");
                return true;
            }

            return false;
        }

        protected override bool TarDown(float elapsedTime)
        {
            //targetTarPosition = new Vector3(transform.position.x, platformPosition.y, transform.position.z);
            //transform.position = Vector3.Lerp(transform.position, initialTarPosition, elapsedTime / 120.0f);
            if (transform.position == initialTarPosition)
            {
                //Debug.Log("Tar is down. " + "(" + elapsedTime + ")");
                _playersInTar = new List<GameObject>();
                //animator.SetBool("Motion", false);
                return false;
            }
            //animator.SetBool("Motion", false);
            return true;
        }

        protected override void TarReset()
        {
            //animator.SetBool("Motion", false);
            transform.position = initialTarPosition;
            _nextApplyEffect = 0;
            _playersInTar = new List<GameObject>();
        }

        #endregion

        #region Private Methods

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            //There are players in Tar
            if (_playersInTar.Any())
            {
                DealDamagePeriodically();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.Tags.PLAYER_BODY))
            {
                other.GetComponent<PlayerMovement>().DecreaseSpeedTar(Constants.Events.Tar_Event.TAR_SLOWDOWN);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(Constants.Tags.PLAYER_BODY) && !PlayerListContains(other.gameObject))
            {
                _playersInTar.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Constants.Tags.PLAYER_BODY))
            {
                other.GetComponent<PlayerMovement>().RestoreBaseSpeed();
            }
            _playersInTar.Remove(other.gameObject);
        }

        private bool PlayerListContains(GameObject playerToRegister)
        {
            PlayerInfo player = playerToRegister.GetComponent<PlayerInfo>();

            if (_playersInTar.Any())
            {
                foreach (PlayerInfo element in _playersInTar.Select(t => t.GetComponent<PlayerInfo>()))
                {
                    if (element.Player == player.Player)
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
                foreach (PlayerHealth element in _playersInTar.Select(t => t.GetComponent<PlayerHealth>()))
                {
                    element.DecreaseHealth(Constants.Events.Tar_Event.TAR_DAMAGE);
                }
            }
        }

        #endregion
    }
}
