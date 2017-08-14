using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Tags = nseh.Utils.Constants.Tags;

namespace nseh.Gameplay.Combat.Special.Myson
{
    public class HeadComponent : MonoBehaviour {


        #region Private Properties

        private Collider _collider;
        private Rigidbody _body;
        private List<GameObject> _enemies;

        [SerializeField]
        private GameObject _particle;
        [SerializeField]
        private GameObject _explosion;
        [SerializeField]
        private Renderer _headRenderer;

        #endregion

        #region Private Methods

        private void Start()
        {
            _body = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _enemies = new List<GameObject>();
            _enemies.Add(this.gameObject.transform.root.gameObject);

            _body.isKinematic = false;
            _body.angularDrag = 0;
            _body.mass = 1;
            _collider.enabled = false;
            _collider.isTrigger = false;
        }

        private void OnCollisionEnter(Collision collider)
        {
            GameObject hit = collider.transform.root.gameObject;
            ContactPoint position = collider.contacts[0];

            if (hit.tag == Tags.PLAYER_BODY || hit.tag == Tags.ENEMY)
            {
                FireParticles(position.point);
                _collider.enabled = false;
                _headRenderer.enabled = false;
                StartCoroutine(Explosion(position.point));
            }
        }

        private void FireParticles(Vector3 position)
        {
            GameObject particleGameObject = Instantiate(_particle, position, transform.rotation);
            foreach (ParticleSystem particle_aux in particleGameObject.GetComponentsInChildren<ParticleSystem>())
            {
                particle_aux.Play();
            }

            Destroy(particleGameObject, 3f);
        }

        private IEnumerator Explosion(Vector3 positionExplosion)
        {
            GameObject explosionAux = Instantiate(_explosion, positionExplosion, Quaternion.identity, this.gameObject.transform.root);

            Debug.Log("que estoy to loco");
            yield return new WaitForSeconds(0.25f);

            Destroy(explosionAux);

        }

        #endregion

    }
}