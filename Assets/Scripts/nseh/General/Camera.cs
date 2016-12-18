using UnityEngine;
using System.Collections;

namespace nseh.General
{
    public class Camera : MonoBehaviour
    {
        public Transform Player1;
        public Transform Player2;
        public Vector3 PositionPlayer1;
        public Vector3 PositionPlayer2;
        public Vector3 distance;
        public Vector3 Midpoint;
        public Vector3 Position;
        private Vector3 velocity = Vector3.zero;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            PositionPlayer1 = Player1.position;
            PositionPlayer2 = Player2.position;
            distance = Player1.position - Player2.position;
            Midpoint =(Player2.position + Player1.position );
            //MOVE CAMERA
            if (distance.x > -1)
            {
               
                Position =  new Vector3 (Midpoint.x, 1.5f, -10);
                transform.position = Vector3.SmoothDamp(transform.position, Position, ref velocity, 0.15f);
            }
            else
            {
                Position = new Vector3(Midpoint.x, 1.5f, -20);
                transform.position = Vector3.SmoothDamp(transform.position, Position, ref velocity, 0.15f);
            }
           

    }
    }
}
