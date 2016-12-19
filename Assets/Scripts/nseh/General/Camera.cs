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
            distance = Player1.position - Player2.position;
            Midpoint =(Player2.position + Player1.position )/2;
            //MOVE CAMERA
            if (Mathf.Abs(distance.x) < 2 && Mathf.Abs(distance.y) < 2)
            {

                Position = new Vector3(Mathf.Clamp(Midpoint.x, xMin, xMax), Mathf.Clamp(Midpoint.y, yMin, yMax), -10);
                transform.position = Vector3.SmoothDamp(transform.position, Position, ref velocity, 0.15f);
            }
            else if (Mathf.Abs(distance.x) < 4 && Mathf.Abs(distance.y) < 4 )
            {

                Position = new Vector3(Mathf.Clamp(Midpoint.x, xMin, xMax), Mathf.Clamp(Midpoint.y, yMin, yMax), -15);
                transform.position = Vector3.SmoothDamp(transform.position, Position, ref velocity, 0.15f);
            }
            else
            {
                Position = new Vector3(Mathf.Clamp(Midpoint.x,xMin,xMax), Mathf.Clamp(Midpoint.y,yMin,yMax), -20);
                transform.position = Vector3.SmoothDamp(transform.position, Position, ref velocity, 0.15f);
            }
           

    }
    }
}
