using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Movement;
using nseh.Managers;


namespace nseh.Gameplay.Minigames {

    public class CubeDeath : MonoBehaviour {

        #region Public Properties
        public bool started = false;
        public int num;
        public float speed;  
        #endregion

        #region Private Properties
        private Rigidbody _myRigidBody;
        #endregion

        #region Public Methods
        // Use this for initialization
        public void Start () {
            _myRigidBody = GetComponent<Rigidbody>();
        }
	
	    // Update is called once per frame
	    public void Update ()
        {
            if(started == true)
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
            StartCoroutine(DestroyCharacter(other));
        }

        private IEnumerator DestroyCharacter(Collider other)
        {
            if (other.tag == "PlayerBody")
            {
                other.GetComponent<Minigame>().velocityCube = -10f;
                other.GetComponent<Minigame>().position = num;
                num+=50;
                if (num == 0)
                {
                    //FIN MINIJUEGO
                }
                yield return new WaitForSeconds(1);
                Destroy(other.gameObject);
            }            
        }
        #endregion

    }
}