using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballsGenerator : MonoBehaviour {

    private float _nextBall;
    private float _starting;
    private float _decrement;

    public GameObject fireBall;
    public Transform minimun;
    public Transform maximun;

	// Use this for initialization
	void Start ()
    {
        _nextBall = Time.time;
        _decrement = -0.5f;
        _starting = 5;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Time.time > _nextBall)
        {
            _starting = Mathf.Clamp(_starting+_decrement, 1F, _starting);
            _nextBall = (Time.time + _starting);
            Debug.Log(_starting);
            Vector3 auxPosition = new Vector3(Random.Range(minimun.position.x, maximun.position.x), Random.Range(minimun.position.y, maximun.position.y), Random.Range(minimun.position.z, maximun.position.z));
            GameObject ball = Instantiate(fireBall, auxPosition, fireBall.transform.rotation);
            ball.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -100000));
            ball.GetComponent<Rigidbody>().AddTorque(new Vector3(0, 0, -10000000));
            Destroy(ball, 5);
        }
	}
}
