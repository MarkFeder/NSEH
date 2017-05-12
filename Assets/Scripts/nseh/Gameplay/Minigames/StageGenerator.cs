using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nseh.Gameplay.Minigames
{

    public class StageGenerator : MonoBehaviour
    {

        #region Privates Properties
        private int _randomPlatform;
        private int _count;
        private int _countSpecial;
        private float _platformWidth;
        [SerializeField]
        private List<GameObject> _specialPlatforms;
        #endregion

        #region Public Properties
        public GameObject platform;
        public GameObject platformChest;
        public GameObject platformInitial;
        public Transform generationPoint;
        #endregion

        #region Public Methods
        // Use this for initialization
        public void Start()
        {
            _count = 0;
        }

        // Update is called once per frame
        public void Update()
        {
            if (transform.position.x < generationPoint.position.x)
            {
                _platformWidth = platform.GetComponent<MeshRenderer>().bounds.size.x;

                transform.position = new Vector3(transform.position.x + _platformWidth, transform.position.y, transform.position.z);
                if (_count < 2)
                {
                    Instantiate(platform, transform.position, platform.transform.rotation);
                    _count++;
                }
                else
                {
                    _countSpecial++;
                    _count = 0;
                    if (_countSpecial == 3 && _specialPlatforms.Count != 0)
                    {
                        _countSpecial = 0;
                        Instantiate(platformChest, transform.position, platform.transform.rotation);
                    }
                    else if (_specialPlatforms.Count == 0)
                    {
                        Instantiate(platform, transform.position, platform.transform.rotation);
                    }
                    else
                    {
                        _randomPlatform = UnityEngine.Random.Range(0, _specialPlatforms.Count);
                        Instantiate(_specialPlatforms[_randomPlatform], transform.position, platform.transform.rotation);
                    }
                }
            }
        }
        #endregion

    }
}

