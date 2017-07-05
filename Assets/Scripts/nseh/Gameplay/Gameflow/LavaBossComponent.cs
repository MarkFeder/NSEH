using nseh.Gameplay.Entities.Player;
using nseh.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace nseh.Gameplay.Gameflow
{
    public class LavaBossComponent : MonoBehaviour
    {

        #region Private Properties

        private float _nextApplyEffect = 0;
        private List<GameObject> _playersInLava;

        #endregion

        #region Private Methods

        private void Start()
        {
            _playersInLava = new List<GameObject>();
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
            if (other.CompareTag(Constants.Tags.PLAYER) && !PlayerListContains(other.gameObject))
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
                _nextApplyEffect = Time.time + 2f;
                foreach (PlayerInfo element in _playersInLava.Select(t => t.GetComponent<PlayerInfo>()))
                {
                    element.DecreaseHealth(10f);
                }
            }
        }

        #endregion

    }
}
