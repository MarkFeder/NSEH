using UnityEngine;
using nseh.Gameplay.Entities.Player;

public class GroundedComponent : MonoBehaviour {

    PlayerMovement _playerMovement;
    Rigidbody _body;

	// Use this for initialization
	void Start ()
    {
        _playerMovement = GetComponentInParent<PlayerMovement>();
        _body = GetComponentInParent<Rigidbody>();

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "OneWayPlatform" && _body.velocity.y <= 0.1f)
        {
            _playerMovement.grounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "OneWayPlatform" && Mathf.Abs(_body.velocity.y) >= 0.1f)
        {
            _playerMovement.grounded = false;

        }
    }
}
