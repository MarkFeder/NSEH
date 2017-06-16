using UnityEngine;
using nseh.Gameplay.Gameflow;
using nseh.Managers.Main;
using nseh.Managers.General;
using nseh.Managers.Level;

namespace nseh.Gameplay.Minigames
{

    public class CameraScript : MonoBehaviour
    {
        #region Public Properties
        public bool started = false;
        public float speed;
        public GameObject center;
        public GameObject goal;
        private float _maxDistance;
        private float _distance;
        private float _auxmaxDistance;
        private float _auxdistance;
        [SerializeField]
        private BarComponent _distanceBar;
        #endregion

        public float Distance
        {
            get { return _distance; }
            set
            {
                _distance = value;

                if (_distanceBar != null)
                {
                    _distanceBar.Value = _distance;
                }
            }
        }

        public float MaxDistance
        {
            get { return _maxDistance; }
            set
            {
                _maxDistance = value;

                if (_distanceBar != null)
                {
                    _distanceBar.MaxValue = _maxDistance;
                }
            }
        }


        #region Public Methods
        // Use this for initialization
        public void Start()
        {
            MaxDistance = 1;
            Distance = 0;
            _auxdistance = center.transform.position.z;
            _auxmaxDistance = goal.transform.position.z;
        }

        // Update is called once per frame
        public void Update()
        {
            if (started == true)
            {

                //Distance = 1 / Mathf.Abs(center.transform.position.z - goal.transform.position.z); 

                transform.position = new Vector3(transform.position.x , transform.position.y, transform.position.z + Time.deltaTime * speed);
                //Distance =  1/center.transform.position.z - goal.transform.position.z;
                Distance = (center.transform.position.z - _auxdistance) / (_auxmaxDistance - _auxdistance);
                if(Distance >= MaxDistance)
                {
                    GameManager.Instance.Find<LevelManager>().Find<MinigameEvent>().StopMinigame();
                }
            }
        }
        #endregion

    }
}
