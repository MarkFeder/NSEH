using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour {

    [SerializeField]
    private List<GameObject> TeleportPoints;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetAxis("Vertical")>0)
        {
            Debug.Log(Input.GetAxis("Vertical"));
            int randomStandardItem = (int)Random.Range(0, TeleportPoints.Count);
            other.transform.position = TeleportPoints[randomStandardItem].transform.position;
        }
    }
}
