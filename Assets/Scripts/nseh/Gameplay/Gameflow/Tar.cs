using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.Gameplay.Entities.Player;
using nseh.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace nseh.Gameplay.Gameflow
{
    public class Tar : TarComponent
    {
        private float _nextApplyEffect = 0;
        private List<GameObject> _playersInTar;

        protected override bool TarUp(float elapsedTime)
        {
            this.targetTarPosition = new Vector3(this.transform.position.x, this.platformPosition.y, this.transform.position.z);
            this.transform.position = Vector3.Lerp(this.transform.position, this.targetTarPosition, elapsedTime / 80.0f);
            if (this.transform.position == this.targetTarPosition)
            {
                //Debug.Log("Tar is up. " + "(" + elapsedTime + ")");
                return true;
            }

            return false;
        }

        protected override bool TarDown(float elapsedTime)
        {
            this.targetTarPosition = new Vector3(this.transform.position.x, this.platformPosition.y, this.transform.position.z);
            this.transform.position = Vector3.Lerp(this.transform.position, this.initialTarPosition, elapsedTime / 120.0f);
            if (this.transform.position == this.initialTarPosition)
            {
                //Debug.Log("Tar is down. " + "(" + elapsedTime + ")");
                _playersInTar = new List<GameObject>();
                return false;
            }
            return true;
        }

        protected override void TarReset()
        {
            this.transform.position = this.initialTarPosition;
            _nextApplyEffect = 0;
            _playersInTar = new List<GameObject>();
        }

        void Update()
        {
            //There are players in Tar
            if (_playersInTar.Any())
            {
                DealDamagePeriodically();
            }
        }
        
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.Tags.PLAYER_BODY))
            {
                other.GetComponent<PlayerMovement>().DecreaseSpeed(Constants.Events.Tar_Event.TAR_SLOWDOWN);
            }
        }

        void OnTriggerStay(Collider other)
        {
            if(other.CompareTag(Constants.Tags.PLAYER_BODY) && !PlayerListContains(other.gameObject))
            {
                _playersInTar.Add(other.gameObject);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Constants.Tags.PLAYER_BODY))
            {
                other.GetComponent<PlayerMovement>().RestoreBaseSpeed();
            }
            _playersInTar.Remove(other.gameObject);
        }

        bool PlayerListContains(GameObject playerToRegister)
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

        void DealDamagePeriodically()
        {
            if(Time.time >= _nextApplyEffect)
            {
                _nextApplyEffect = Time.time + Constants.Events.Tar_Event.TAR_TICKDAMAGE;
                foreach (PlayerHealth element in _playersInTar.Select(t => t.GetComponent<PlayerHealth>()))
                {
                    element.DecreaseHealth(Constants.Events.Tar_Event.TAR_DAMAGE);
                }
            }
        }


    }
}
