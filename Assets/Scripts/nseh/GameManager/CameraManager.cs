using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nseh.GameManager
{
    public class CameraManager : Service

    {

        // Use this for initialization
        public override void Setup(GameManager myGame)
        {
            base.Setup(myGame);
        }

        public override void Activate()
        {
            IsActivated = true;
        }

        public override void Tick()
        {


        }

        public override void Release()
        {
            IsActivated = false;
        }
    }
}