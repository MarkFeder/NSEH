﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlatforms : MonoBehaviour {

    public GameObject destructionPoint;

    // Use this for initialization
    void Start () {
        destructionPoint = GameObject.Find("PlatformDestructionPoint");
		
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.x < destructionPoint.transform.position.x)
        {
            Destroy(gameObject);
        }
	}
}
