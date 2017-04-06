using nseh.Gameplay.Base.Abstract.Gameflow;
using UnityEngine;

namespace nseh.Gameplay.Gameflow
{
    public class StonePlatform : TarComponent
    {

        protected override bool TarUp(float elapsedTime)
        {
            this.targetTarPosition = new Vector3(this.transform.position.x, this.platformPosition.y, this.transform.position.z);
            this.transform.position = Vector3.Lerp(this.transform.position, this.targetTarPosition, elapsedTime / 80.0f);
            if (this.transform.position == this.targetTarPosition)
            {
                //Debug.Log("Tar is up. " + "(" + elapsedTime + ")");
                return true;
            }

            return false;
        }

        protected override bool TarDown(float elapsedTime)
        {
            this.targetTarPosition = new Vector3(this.transform.position.x, this.platformPosition.y, this.transform.position.z);
            this.transform.position = Vector3.Lerp(this.transform.position, this.initialTarPosition, elapsedTime / 120.0f);
            if (this.transform.position == this.initialTarPosition)
            {
                //Debug.Log("Tar is down. " + "(" + elapsedTime + ")");
                return false;
            }
            return true;
        }

        protected override void TarReset()
        {
            this.transform.position = this.initialTarPosition;
        }
    } 
}
