using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Movement;

public class CubeDeath : MonoBehaviour {


    public float speed;
    private Rigidbody myRigidBody;
    // Use this for initialization
    void Start () {
        myRigidBody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        myRigidBody.velocity = new Vector3(speed, myRigidBody.velocity.y, myRigidBody.velocity.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(DestroyCharacter(other));
        //Destroy(other.gameObject);
    }

    IEnumerator DestroyCharacter(Collider other)
    {
        other.GetComponent<Minigame>().velocityCube = -10f;
        yield return new WaitForSeconds(0);
        Destroy(other.gameObject);
    }
}
