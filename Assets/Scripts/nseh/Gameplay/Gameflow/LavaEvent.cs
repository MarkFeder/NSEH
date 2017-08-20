using nseh.Gameplay.Base.Abstract.Gameflow;
using nseh.Utils;
using UnityEngine;
using nseh.Gameplay.Base.Interfaces;
using nseh.Managers.Main;
using System.Collections;

namespace nseh.Gameplay.Gameflow
{
    public class LavaEvent : MonoBehaviour, IEvent
    {

        #region Private Properties

        private float eventDuration;
        public  LavaGameComponent lava;
        private bool _lavaUp;
        private bool _isActivated;

        #endregion

        #region Public Properties

        public float elapsedTime;
        public GameObject volcano;
        public GameObject particleSmoke;

        #endregion

        #region Public Methods

        public void Start()
        {
            _isActivated = false;
            foreach (ParticleSystem particle_aux in particleSmoke.GetComponentsInChildren<ParticleSystem>())
            {
                particle_aux.Stop();
            }
        }

        public void ActivateEvent()
        {
            _isActivated = true;
            _lavaUp = false;
            eventDuration = Constants.Events.Tar_Event.EVENT_DURATION;

            elapsedTime = 0;

        }

        public void Update()
        {
            if (_isActivated && !GameManager.Instance.isPaused)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= Constants.Events.Tar_Event.EVENT_START && !_lavaUp)
                {
                    _lavaUp = true;
                    StartCoroutine(FadeTo(1.0f, 1f));
                    foreach (ParticleSystem particle_aux in particleSmoke.GetComponentsInChildren<ParticleSystem>())
                    {
                        particle_aux.Play();
                    }
                    lava.LavaMotion();

                }
                else if (elapsedTime >= Constants.Events.Tar_Event.EVENT_START + eventDuration)
                {
                    elapsedTime = 0;
                    StartCoroutine(FadeTo(0.0f, 1f));
                    foreach (ParticleSystem particle_aux in particleSmoke.GetComponentsInChildren<ParticleSystem>())
                    {
                        particle_aux.Stop();
                    }
                    _lavaUp = false;
                }
            }
        }

        public void EventRelease()
        {
            eventDuration = Constants.Events.Tar_Event.EVENT_DURATION;
            volcano.transform.GetComponent<SpriteRenderer>().color= new Color (1,1,1,0);
            foreach (ParticleSystem particle_aux in particleSmoke.GetComponentsInChildren<ParticleSystem>())
            {
                particle_aux.Stop();
            }
            elapsedTime = 0;
            lava.ResetLava();
            _lavaUp = false;
            _isActivated = false;

        }

        private IEnumerator FadeTo(float aValue, float aTime)
        {
            float alpha = volcano.transform.GetComponent<SpriteRenderer>().color.a;
            for (float t = 0.0f; t < 1f; t += Time.deltaTime / aTime)
            {
                Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
                volcano.transform.GetComponent<SpriteRenderer>().color = newColor;
                yield return null;
            }
        }

        #endregion

    } 
}
