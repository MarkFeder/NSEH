using UnityEngine;

namespace nseh.Managers.General
{
    public class CameraComponent : MonoBehaviour
    {

        #region Private Properties
        private Vector3 velocity = Vector3.zero;
        //[SerializeField]
        private float xMin;
        //[SerializeField]
        private float xMax;
        //[SerializeField]
        private float yMin;
        //[SerializeField]
        private float yMax;
        #endregion

        #region Public Properties
        public Vector3 DebugPlayer1;
        public Vector3 DebugPlayer2;
        public Vector3 DebugPlayer3;
        public Vector3 DebugPlayer4;
        public Vector3 distance;
        public Vector3 Midpoint;
        public Vector3 Position;
        #endregion

        #region Public Methods


        public void RefreshCamera(Vector3 PositionPlayer1, Vector3 PositionPlayer2, Vector3 PositionPlayer3, Vector3 PositionPlayer4)
        {
            DebugPlayer1 = PositionPlayer1;
            DebugPlayer2 = PositionPlayer2;
            DebugPlayer3 = PositionPlayer3;
            DebugPlayer4 = PositionPlayer4;

            if (PositionPlayer1 != PositionPlayer2)
            {
                float xMaxPlayers = Mathf.Max(PositionPlayer1.x, PositionPlayer2.x, PositionPlayer3.x, PositionPlayer4.x);
                float xMinPlayers = Mathf.Min(PositionPlayer1.x, PositionPlayer2.x, PositionPlayer3.x, PositionPlayer4.x);
                float yMaxPlayers = Mathf.Max(PositionPlayer1.y, PositionPlayer2.y, PositionPlayer3.y, PositionPlayer4.y);
                float yMinPlayers = Mathf.Min(PositionPlayer1.y, PositionPlayer2.y, PositionPlayer3.y, PositionPlayer4.y);
                distance = new Vector3(xMaxPlayers, yMaxPlayers, 0) - new Vector3(xMinPlayers, yMinPlayers, 0);
                Midpoint = new Vector3((xMaxPlayers + xMinPlayers) / 2, (yMaxPlayers + yMinPlayers) / 2, 0);
            }

            else
            {
                Midpoint = new Vector3(PositionPlayer1.x,PositionPlayer1.y,0);
            }

            if (Mathf.Abs(distance.x) < 4 && Mathf.Abs(distance.y) < 4)
            {

                if(PositionPlayer1== PositionPlayer2)
                {
                    xMin = -25;
                    xMax = 15;
                    yMin = 0f;
                    yMax = 20;
                    Position = new Vector3(Mathf.Clamp(Midpoint.x, xMin, xMax), Mathf.Clamp(Midpoint.y, yMin, yMax)+1, 30);
                    transform.position = Vector3.SmoothDamp(transform.position, Position, ref velocity, 0.15f);
                }

                else
                {
                    xMin = -25;
                    xMax = 22;
                    yMin = 0f;
                    yMax = 20;
                    Position = new Vector3(Mathf.Clamp(Midpoint.x, xMin, xMax), Mathf.Clamp(Midpoint.y, yMin, yMax)+1, 20);
                    transform.position = Vector3.SmoothDamp(transform.position, Position, ref velocity, 0.15f);
                }
                
            }

            else if (Mathf.Abs(distance.x) < 10 && Mathf.Abs(distance.y) < 10)
            {
                xMin = -22;
                xMax = 22;
                yMin = 0f;
                yMax = 15;
                Position = new Vector3(Mathf.Clamp(Midpoint.x, xMin, xMax), Mathf.Clamp(Midpoint.y, yMin, yMax)+2, 40);
                transform.position = Vector3.SmoothDamp(transform.position, Position, ref velocity, 0.15f);
            }

            else if (Mathf.Abs(distance.x) < 20 && Mathf.Abs(distance.y) < 20)
            {
                xMin = -15;
                xMax = 10;
                yMin = 0f;
                yMax = 10;
                Position = new Vector3(Mathf.Clamp(Midpoint.x, xMin, xMax), Mathf.Clamp(Midpoint.y, yMin, yMax)+3, 60);
                transform.position = Vector3.SmoothDamp(transform.position, Position, ref velocity, 0.15f);
            }

            else
            {
                xMin = -10;
                xMax = 6;
                yMin = 0f;
                yMax = 10;
                Position = new Vector3(Mathf.Clamp(Midpoint.x, xMin, xMax), Mathf.Clamp(Midpoint.y, yMin, yMax)+4, 80);
                transform.position = Vector3.SmoothDamp(transform.position, Position, ref velocity, 0.15f);
            }
        }
        #endregion
    }
}
