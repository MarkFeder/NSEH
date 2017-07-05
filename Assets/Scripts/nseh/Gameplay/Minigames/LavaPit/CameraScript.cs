using UnityEngine;
using nseh.Gameplay.Gameflow;
using nseh.Managers.General;

namespace nseh.Gameplay.Minigames
{
    public class CameraScript : MonoBehaviour
    {

        #region Public Properties

        public bool started = false;
        public float speed;
        public GameObject center;
        public GameObject goal;
        public MinigameVolcano eventAux;

        #endregion

        #region Private Properties

        private float _maxDistance;
        private float _distance;
        private float _auxmaxDistance;
        private float _auxdistance;
        [SerializeField]
        private BarComponent _distanceBar;

        #endregion

        #region Public C# Properties

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

        #endregion

        #region Public Methods

        public void Start()
        {
            MaxDistance = 1;
            Distance = 0;
            _auxdistance = center.transform.position.z;
            _auxmaxDistance = goal.transform.position.z;
        }

        public void Update()
        {
            if (started == true)
            {
                transform.position = new Vector3(transform.position.x , transform.position.y, transform.position.z + Time.deltaTime * speed);
                Distance = (center.transform.position.z - _auxdistance) / (_auxmaxDistance - _auxdistance);
                if(Distance >= MaxDistance)
                {
                    eventAux.StopMinigame();
                }
            }
        }

        #endregion

    }
}
