using UnityEngine;
using System.Collections;

namespace nseh.General
{
    public class CameraComponent : MonoBehaviour
    {
        public Transform Player1;
        public Transform Player2;
        public Vector3 PositionPlayer1;
        public Vector3 PositionPlayer2;

        public Vector3 distance;
        public Vector3 Midpoint;
        public Vector3 Position;
        private Vector3 velocity = Vector3.zero;
        [SerializeField]
        private float xMin;
        [SerializeField]
        private float xMax;
        [SerializeField]
        private float yMin;
        [SerializeField]
        private float yMax;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            PositionPlayer1 = Player1.position;
            PositionPlayer2 = Player2.position;
           
            float xMaxPlayers = Mathf.Max(PositionPlayer1.x, PositionPlayer2.x);
            float xMinPlayers = Mathf.Min(PositionPlayer1.x, PositionPlayer2.x);
            float yMaxPlayers = Mathf.Max(PositionPlayer1.y, PositionPlayer2.y);
            float yMinPlayers = Mathf.Min(PositionPlayer1.y, PositionPlayer2.y);

            distance = Player1.position - Player2.position;
            Midpoint = new Vector3((xMaxPlayers + xMinPlayers) /2, (yMaxPlayers + yMinPlayers) / 2, 0);

            float tempZ = -30f;

            //MOVE CAMERA
            if (Mathf.Abs(distance.x) < 2 && Mathf.Abs(distance.y) < 2)
            {

                Position = new Vector3(Mathf.Clamp(Midpoint.x, xMin, xMax), Mathf.Clamp(Midpoint.y, yMin, yMax), tempZ/*-10*/);
                transform.position = Vector3.SmoothDamp(transform.position, Position, ref velocity, 0.15f);
            }
            else if (Mathf.Abs(distance.x) < 4 && Mathf.Abs(distance.y) < 4 )
            {

                Position = new Vector3(Mathf.Clamp(Midpoint.x, xMin, xMax), Mathf.Clamp(Midpoint.y, yMin, yMax), -15);
                transform.position = Vector3.SmoothDamp(transform.position, Position, ref velocity, 0.15f);
            }
            else if (Mathf.Abs(distance.x) < 10 && Mathf.Abs(distance.y) < 10)
            {
                //Position = new Vector3(Mathf.Clamp(Midpoint.x,xMin,xMax), Mathf.Clamp(Midpoint.y,yMin,yMax), -100);
                Position = new Vector3(Mathf.Clamp(Midpoint.x, xMin, xMax), Mathf.Clamp(Midpoint.y, yMin, yMax), -20);
                transform.position = Vector3.SmoothDamp(transform.position, Position, ref velocity, 0.15f);
            }

            else if(Mathf.Abs(distance.x) < 20 && Mathf.Abs(distance.y) < 20)
            {
                Position = new Vector3(Mathf.Clamp(Midpoint.x,xMin,xMax), Mathf.Clamp(Midpoint.y,yMin,yMax), -40);
                //Position = new Vector3(Mathf.Clamp(Midpoint.x, xMin, xMax), Mathf.Clamp(Midpoint.y, yMin, yMax), -20);
                transform.position = Vector3.SmoothDamp(transform.position, Position, ref velocity, 0.15f);
            }

            else 
            {
                Position = new Vector3(Mathf.Clamp(Midpoint.x, xMin, xMax), Mathf.Clamp(Midpoint.y, yMin, yMax), -80);
                //Position = new Vector3(Mathf.Clamp(Midpoint.x, xMin, xMax), Mathf.Clamp(Midpoint.y, yMin, yMax), -20);
                transform.position = Vector3.SmoothDamp(transform.position, Position, ref velocity, 0.15f);
            }


        }
    }
}
