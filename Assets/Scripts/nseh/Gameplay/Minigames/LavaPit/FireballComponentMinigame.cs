using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballComponentMinigame : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Physics.IgnoreLayerCollision(12, 9, false);
    }
}
