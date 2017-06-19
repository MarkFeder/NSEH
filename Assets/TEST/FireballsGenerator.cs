using UnityEngine;
using System.Collections.Generic;

namespace nseh.Gameplay.Minigames
{
    public class FireballsGenerator : MonoBehaviour
    {

        private float _nextBall;
        private float _starting;
        private float _decrement;

        public GameObject fireBall;
        public Transform minimun;
        public Transform maximun;
        public bool started;

        // Use this for initialization
        void Start()
        {
            _nextBall = Time.time;
            _decrement = -0.5f;
            _starting = 2;
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log("TEST " + Vector3.Distance(gameObject.transform.position, minimun.transform.position) + " " + Vector3.Distance(gameObject.transform.position, maximun.transform.position));
            if (started == true)
            {
                if (Time.time > _nextBall)
                {
                        _starting = Mathf.Clamp(_starting + _decrement, 0.75F, _starting);
                        _nextBall = (Time.time + _starting);
                        Debug.Log(_starting);
                        RaycastHit hit;
                        Ray downRay = new Ray(transform.position, Vector3.right);
                        if (Physics.Raycast(downRay, out hit, 1000))
                        {
                            float auxMinimun = gameObject.transform.position.x - hit.point.x;
                            float auxMaximun = -auxMinimun;
                            Vector3 auxPosition = new Vector3(Random.Range(auxMinimun, auxMaximun), gameObject.transform.position.y, gameObject.transform.position.z);
                            //Vector3 auxRotation = new Vector3(fireBall.transform.eulerAngles.x, -180 + Random.Range(-20, 20), fireBall.transform.eulerAngles.z);
                            GameObject ball = Instantiate(fireBall, auxPosition, fireBall.transform.rotation);
                            ball.GetComponent<AudioSource>().pitch = ball.GetComponent<AudioSource>().pitch + (Random.Range(-1f, 1f));
                            ball.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -250000));
                            Destroy(ball, 5);
                            
                        }
                    
                }
            }

        }
    }
}
