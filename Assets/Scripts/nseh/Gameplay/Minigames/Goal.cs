using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Movement;
using nseh.Managers;

namespace nseh.Gameplay.Minigames
{

    public class Goal : MonoBehaviour {

        #region Private Properties
        private Rigidbody _myRigidBody;
        #endregion

        #region Public Properties
        public bool started = false;
        public int num;
        public float speed;
        #endregion

        #region Public Methods
        // Use this for initialization
        public void Start()
        {
            _myRigidBody = GetComponent<Rigidbody>();
            num = 400;
        }

        // Update is called once per frame
        public void Update()
        {
            if (started == true)
            {
                _myRigidBody.velocity = new Vector3(speed, _myRigidBody.velocity.y, _myRigidBody.velocity.z);
            }
            else
            {
                _myRigidBody.velocity = Vector3.zero;
            }
        }
        #endregion

        #region Private Methods
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "PlayerBody")
            {
                StartCoroutine(DestroyCharacter(other);
            }
        }

        private IEnumerator DestroyCharacter(Collider other)
        {
            other.GetComponent<Minigame>().position = num;
            num -= 100;
            yield return new WaitForSeconds(1);
            Destroy(other.gameObject);
        }
        #endregion
    }
}