using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Movement;

namespace nseh.Gameplay.Minigames
{
    public class ChestMinigame : MonoBehaviour
    {

        #region Private Properties
        private Collider character;
        #endregion

        #region Public Methods
        public void ChangeVelocity()
        {
            character.GetComponent<Minigame>().velocityCube = 0;
        }
        #endregion

        #region Private Methods
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "PlayerBody")
            {
                float randomBuff = Random.value;
                Debug.Log(randomBuff);
                character = other;
                if (randomBuff < 0.25f)
                {
                    character.GetComponent<Minigame>().velocityCube = -2;
                    //StartCoroutine(ChangeVelocity(other, 1, -2));
                    Invoke("ChangeVelocity", 1f);
                }
                else if (randomBuff > 0.25f && randomBuff < 0.5f)
                {
                    character.GetComponent<Minigame>().velocityCube = -3;
                    //StartCoroutine(ChangeVelocity(other, 1, -2));
                    Invoke("ChangeVelocity", 1f);

                }
                else if (randomBuff > 0.5f && randomBuff < 0.75f)
                {
                    character.GetComponent<Minigame>().velocityCube = 2;
                    //StartCoroutine(ChangeVelocity(other, 1, -2));
                    Invoke("ChangeVelocity", 1f);
                }
                else if (randomBuff > 0.75f && randomBuff < 1f)
                {
                    character.GetComponent<Minigame>().velocityCube = 3;
                    //StartCoroutine(ChangeVelocity(other, 1, -2));
                    Invoke("ChangeVelocity", 1f);
                }
                gameObject.SetActive(false);
            }
        }
        #endregion

    }
}


