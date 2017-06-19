using System.Collections.Generic;
using UnityEngine;
using nseh.Managers.Audio;
using System.Linq;
using nseh.Utils;
using nseh.Gameplay.Entities.Player;
using nseh.Managers.Main;
using nseh.Managers.Level;
using nseh.Gameplay.Gameflow;

namespace nseh.Gameplay.Base.Abstract.Gameflow
{
    public class TarComponent : MonoBehaviour
    {

        private List<GameObject> _playersInLava;
        private float _nextApplyEffect = 0;
        private Animator _animator;
        

        public AudioClip alarm;

        // Use this for initialization
        void Start()
        {
            _playersInLava = new List<GameObject>();
            _animator = GetComponent<Animator>();
            GameManager.Instance.LevelManager.Find<Tar_Event>().lava = this;
            
        }

        public void LavaMotion()
        {
            
            _animator.SetTrigger("Start");
            SoundManager.Instance.PlayAudioFX(alarm, 1f, false, new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z), 0);

        }

        public void ResetLava()
        {
            //animator.SetBool("Motion", false);
            _animator.SetTrigger("Restart");
            
            _nextApplyEffect = 0;
            _playersInLava = new List<GameObject>();
        }

        private void Update()
        {
            //There are players in Tar
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
            if (other.CompareTag(Constants.Tags.PLAYER_BODY))
            {
                other.GetComponent<PlayerMovement>().RestoreBaseSpeed();
            }
            _playersInLava.Remove(other.gameObject);
        }

        private bool PlayerListContains(GameObject playerToRegister)
        {
            PlayerInfo player = playerToRegister.GetComponent<PlayerInfo>();

            if (_playersInLava.Any())
            {
                foreach (PlayerInfo element in _playersInLava.Select(t => t.GetComponent<PlayerInfo>()))
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
                foreach (PlayerHealth element in _playersInLava.Select(t => t.GetComponent<PlayerHealth>()))
                {
                    element.DecreaseHealth(Constants.Events.Tar_Event.TAR_DAMAGE);
                }
            }
        }
    } 
}





