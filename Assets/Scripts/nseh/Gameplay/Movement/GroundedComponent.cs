using UnityEngine;
using nseh.Gameplay.Entities.Player;

namespace nseh.Gameplay.Movement
{
    public class GroundedComponent : MonoBehaviour
    {

        #region Private Properties

        PlayerMovement _playerMovement;
        Rigidbody _body;

        #endregion

        #region Private Methods

        // Use this for initialization
        void Start()
        {
            _playerMovement = GetComponentInParent<PlayerMovement>();
            _body = GetComponentInParent<Rigidbody>();

        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "OneWayPlatform" && _body.velocity.y <= 0.1f)
            {
                _playerMovement.grounded = true;
            }

            else if ((other.tag == "PlayerBody" || other.tag == "Player" || other.tag == "Enemy") && _body.velocity.y < 0)
            {
                Vector3 repulsion = new Vector3(1000 * _playerMovement.transform.rotation.y, 0, 0);
                _body.AddForce(repulsion, ForceMode.Impulse);
            }
        }

        private void OnTriggerExit(Collider other)
        {

            if (other.tag == "OneWayPlatform" && Mathf.Abs(_body.velocity.y) >= 0.1f)
            {
                _playerMovement.grounded = false;

            }
        }

        #endregion

    }
}
