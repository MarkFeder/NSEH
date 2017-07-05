using UnityEngine;

namespace nseh.Gameplay.Entities.Environment
{
    public class PlayerSpawnPoint : MonoBehaviour
    {

        #region Private Properties

        [SerializeField] //Just debug purposes. Don't change its value on Unity Inspector please.
        private bool _isFree;
        [SerializeField]
        private GameObject _particle;

        #endregion

        #region Public C# Properties

        public bool IsFree
        {
            get
            {
                return _isFree;
            }
        }

        public GameObject Particle
        {
            get
            {
                return _particle;
            }
        }

        #endregion

        #region Private Methods

        void Start()
        {
            _isFree = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("PlayerBody") || other.CompareTag("Player"))
            {
                _isFree = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("PlayerBody") || other.CompareTag("Player"))
            {
                _isFree = true;
            }
        }

        #endregion

        #region Public Methods

        public void SetFree()
        {
            _isFree = true;
        }


        public void ParticleAnimation(Transform player)
        {
            GameObject particleGameObject = Instantiate(Particle, transform.position, transform.rotation, player);
            foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
            {
                particle_aux.Play();
            }

            Destroy(particleGameObject, 1f);
        }

        #endregion

    }
}