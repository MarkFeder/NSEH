using UnityEngine;
using UnityEngine.SceneManagement;
using nseh.Managers.Main;
using nseh.Managers.Level;

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

            set
            {
                _particle = value;
            }
        }
        #endregion

        void Start()
        {
            _isFree = true;

            if (SceneManager.GetActiveScene().name == "Game")
            {
                Debug.Log("PlayerSpawnPoint registered");
                GameManager.Instance.Find<LevelManager>().RegisterPlayerSpawnPoint(this.gameObject);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("PlayerBody"))
            {
                _isFree = false;
                Debug.Log("Character inside spawn Point. Property IsFree = " + _isFree);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("PlayerBody"))
            {
                _isFree = true;
                Debug.Log("Character has left the spawn Point. Property IsFree = " + _isFree);
            }
        }

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