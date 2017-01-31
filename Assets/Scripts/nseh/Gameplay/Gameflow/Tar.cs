using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tar : MonoBehaviour {
    public Transform platformTarget;
    public Vector3 initialTarPosition;
    public Vector3 platformPosition;
    public Vector3 targetTarPosition;
    //private Vector3 velocity = new Vector3(1f,1f,1f);
    //float GameTime = 0;
    //int eventStartedAt = 0;
    float eventDuration = 10.0f;
    //bool eventFinished = false;
    //float elapsedTime;
    //bool goingUp = true;

    // Use this for initialization
    void Start () {
        platformPosition = platformTarget.position;
        initialTarPosition = transform.position;
    }

    //Component suscribes to event on enable
    void OnEnable()
    {
        Tar_Event.TarUp += TarUp;
        Tar_Event.TarDown += TarDown;
    }

    //Component unsuscribes to event on disable
    void OnDisable()
    {
        Tar_Event.TarUp -= TarUp;
        Tar_Event.TarDown -= TarDown;
    }
	
	// Update is called once per frame
	void Update () {

    }
 
    bool TarUp(float elapsedTime)
    {
        //tar.SetActive(true);
        targetTarPosition = new Vector3(transform.position.x, platformPosition.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetTarPosition, elapsedTime / 80.0f);
        if (transform.position == targetTarPosition)
        {
            Debug.Log("Tar is up. " + "(" + elapsedTime + ")" );
            return true;
        }

        return false;
        //transform.position = Vector3.SmoothDamp(transform.position, targetTarPosition, ref velocity, 0.15f);
    }

    bool TarDown(float elapsedTime)
    {
        targetTarPosition = new Vector3(transform.position.x, platformPosition.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, initialTarPosition, elapsedTime / 120.0f);
        if (transform.position == initialTarPosition)
        {
            Debug.Log("Tar is down. " + "(" + elapsedTime + ")");
            return false;
        }
        return true;
        //transform.position = Vector3.SmoothDamp(transform.position, initialTarPosition, ref velocity, 0.15f);
        //tar.SetActive(false);
    }
}
