using UnityEngine;

namespace nseh.Gameplay.Minigames
{
    public class FireballsGenerator : MonoBehaviour
    {

        #region Private Properties

        private float _nextBall;
        private float _starting;
        private float _decrement;

        #endregion

        #region Public Properties

        public GameObject fireBall;
        public Transform minimun;
        public Transform maximun;
        public bool started;

        #endregion

        #region Private Methods

        void Start()
        {
            _nextBall = Time.time;
            _decrement = -0.5f;
            _starting = 2;
        }

        void Update()
        {
            if (started == true)
            {
                if (Time.time > _nextBall)
                {
                    _starting = Mathf.Clamp(_starting + _decrement, 0.75F, _starting);
                    _nextBall = (Time.time + _starting);
                    RaycastHit hit;
                    Ray downRay = new Ray(transform.position, Vector3.right);

                    if (Physics.Raycast(downRay, out hit, 1000))
                    {
                        float auxMinimun = gameObject.transform.position.x - hit.point.x;
                        float auxMaximun = -auxMinimun;
                        Vector3 auxPosition = new Vector3(Random.Range(auxMinimun, auxMaximun), gameObject.transform.position.y, gameObject.transform.position.z);
                        GameObject ball = Instantiate(fireBall, auxPosition, fireBall.transform.rotation);
                        ball.GetComponent<AudioSource>().pitch = ball.GetComponent<AudioSource>().pitch + (Random.Range(-1f, 1f));
                        ball.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -250000));
                        Destroy(ball, 5);
                    }
                }
            }
        }

        #endregion

    }
}
