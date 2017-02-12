using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using nseh.GameManager;


public class EnemyComponent : MonoBehaviour {

    GameObject player;
    NavMeshAgent _agent;
	// Use this for initialization
	void Start () {
        _agent = GetComponent<NavMeshAgent>();
        //player = GameObject.FindGameObjectWithTag("Player");
        //_agent.destination = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            if(Mathf.Abs(_agent.destination.x-player.transform.position.x)>1 )
            _agent.destination = player.transform.position;
        }
	}


    void Die()
    {
        Destroy(gameObject);
    }
}
