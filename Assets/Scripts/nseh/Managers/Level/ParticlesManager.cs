using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.Managers.Main;
using System.Collections;
using UnityEngine;

namespace nseh.Managers.Level
{
    public class ParticlesManager : SubLevelManager
    {
        #region SubLevelManager Methods

        public override void ActivateSubManager()
        {
            _isActivated = true;
        }

        public override void SubManagerTick() {}

        public override void ReleaseSubManager()
        {
            GameManager.Instance.StopAllCoroutines();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Play a particle at some position.
        /// </summary>
        /// <param name="poolName">The name of the pool this particle belongs to.</param>
        /// <param name="particle">The particle itself.</param>
        /// <param name="position">The position where we want to play the particle.</param>
        public void PlayParticleAtPosition(GameObject particle, string poolName, Vector3 position)
        {
            if (particle != null)
            {
                // Setup particle's pos and rot
                particle.transform.position = position;
                particle.transform.rotation = Quaternion.identity;

                // Play particle
                ParticleSystem system = particle.GetComponent<ParticleSystem>();
                ParticleSystem.MainModule main = system.main;

                float destructionTime = main.duration + main.startLifetime.constant;

                // Destroy particle
                GameManager.Instance.StartChildCoroutine(DestroyParticle(destructionTime, poolName, particle));
            }
            else
            {
                Debug.Log("Particle is null!");
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Destroy particle after x seconds.
        /// </summary>
        /// <param name="seconds"></param>
        private IEnumerator DestroyParticle(float seconds, string poolName, GameObject particle)
        {
            yield return new WaitForSeconds(seconds);

            // Destroy particle
            _levelManager.ObjectPoolManager.GetPool(poolName).DestroyObject(particle);
        }

        #endregion
    }
}
