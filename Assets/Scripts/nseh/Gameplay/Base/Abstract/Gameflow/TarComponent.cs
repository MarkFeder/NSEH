using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nseh.Gameplay.Gameflow;

public abstract class TarComponent : MonoBehaviour {

    public Transform platformTarget;
    public Vector3 initialTarPosition;
    public Vector3 platformPosition;
    public Vector3 targetTarPosition;

    // Use this for initialization
    void Start () {
        platformPosition = this.platformTarget.position;
        initialTarPosition = this.transform.position;
    }

    //Component suscribes to event on enable
    void OnEnable()
    {
        Tar_Event.TarUp += TarUp;
        Tar_Event.TarDown += TarDown;
        Tar_Event.ResetTarComponents += TarReset;
    }

    //Component unsuscribes to event on disable
    void OnDisable()
    {
        Tar_Event.TarUp -= TarUp;
        Tar_Event.TarDown -= TarDown;
        Tar_Event.ResetTarComponents -= TarReset;

    }
    
    abstract protected bool TarUp(float elapsedTime);
    abstract protected bool TarDown(float elapsedTime);
    abstract protected void TarReset();
}
