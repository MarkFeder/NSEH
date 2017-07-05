using UnityEngine;
using System.Collections.Generic;

namespace nseh.Managers.General
{
    public class CameraComponent : MonoBehaviour
    {
      
        #region Private Properties

        private Vector3 velocity = Vector3.zero;
        private float xMin;
        private float xMax;
        private float yMin;
        private float yMax;

        #endregion

        #region Public Properties

        public List<Transform> positions;
        public Vector3 distance;
        public Vector3 Midpoint;
        public Vector3 Position;

        #endregion

        #region Public Methods

        public void Start()
        {
            positions = new List<Transform>(); 
        }

        public void Update()
        {
            switch (positions.Count)
            {
                case 1:

                    Midpoint = new Vector3(positions[0].position.x, positions[0].position.y, 0);
                    break;

                case 2:

                    float xMaxPlayers = Mathf.Max(positions[0].position.x, positions[1].position.x);
                    float xMinPlayers = Mathf.Min(positions[0].position.x, positions[1].position.x);
                    float yMaxPlayers = Mathf.Max(positions[0].position.y, positions[1].position.y);
                    float yMinPlayers = Mathf.Min(positions[0].position.y, positions[1].position.y);
                    distance = new Vector3(xMaxPlayers, yMaxPlayers, 0) - new Vector3(xMinPlayers, yMinPlayers, 0);
                    Midpoint = new Vector3((xMaxPlayers + xMinPlayers) / 2, (yMaxPlayers + yMinPlayers) / 2, 0);
                    break;

                case 4:

                    xMaxPlayers = Mathf.Max(positions[0].position.x, positions[1].position.x, positions[2].position.x, positions[3].position.x);
                    xMinPlayers = Mathf.Min(positions[0].position.x, positions[1].position.x, positions[2].position.x, positions[3].position.x);
                    yMaxPlayers = Mathf.Max(positions[0].position.y, positions[1].position.y, positions[2].position.y, positions[3].position.y);
                    yMinPlayers = Mathf.Min(positions[0].position.y, positions[1].position.y, positions[2].position.y, positions[3].position.y);
                    distance = new Vector3(xMaxPlayers, yMaxPlayers, 0) - new Vector3(xMinPlayers, yMinPlayers, 0);
                    Midpoint = new Vector3((xMaxPlayers + xMinPlayers) / 2, (yMaxPlayers + yMinPlayers) / 2, 0);
                    break;
            }
           

            if (Mathf.Abs(distance.x) < 4 && Mathf.Abs(distance.y) < 4)
            {
                if (positions[0] == positions[1])
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
