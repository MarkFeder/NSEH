using UnityEngine;

//namespace nseh.GameManager
//{
public class CameraControl : MonoBehaviour
    {

        // Time to refocus
        public float dampTime = 0.2f;
        // Space between the top/bottom most target and the screen edge
        public float screenEdgeBuffer = 0.4f;
        // The smallest size the camera can be
        public float minSize = 6.5f;
        // All the targets the camera needs to encompass
        [HideInInspector]
        public Transform[] targets;

        private Camera camera;
        private float zoomSpeed;
        private Vector3 moveVelocity;
        private Vector3 desiredPosition;

        private void Awake()
        {
            if (this.camera == null)
            {
                this.camera = GetComponentInChildren<Camera>();
            }
        }

        private void FixedUpdate()
        {
            // Move the camera and zoom it if needed

            this.Move();

            this.Zoom();
        }

        private void Move()
        {
            this.FindAveragePosition();

            this.transform.position = Vector3.SmoothDamp(this.transform.position, this.desiredPosition, ref this.moveVelocity, this.dampTime);
        }

        private void FindAveragePosition()
        {
            Vector3 averagePos = new Vector3();
            int numTargets = 0;

            for (int i = 0; i < this.targets.Length; i++)
            {
                if (!this.targets[i].gameObject.activeSelf)
                {
                    continue;
                }

                // Add to the average and increment the number of items
                averagePos += this.targets[i].position;
                numTargets++;
            }

            if (numTargets > 0)
            {
                averagePos /= numTargets;
            }

            averagePos.y = this.transform.position.y;

            this.desiredPosition = averagePos;
        }

        private void Zoom()
        {
            float requiredSize = this.FindRequiredSize();
            this.camera.orthographicSize = Mathf.SmoothDamp(this.camera.orthographicSize, requiredSize, ref this.zoomSpeed, this.dampTime);
        }

        private float FindRequiredSize()
        {
            // Find the position the camera rig is moving towards in its local space.
            Vector3 desiredLocalPos = transform.InverseTransformPoint(this.desiredPosition);

            // Start the camera's size calculation at zero.
            float size = 0f;

            for (int i = 0; i < this.targets.Length; i++)
            {
                if (!targets[i].gameObject.activeSelf)
                    continue;

                // Otherwise, find the position of the target in the camera's local space.
                Vector3 targetLocalPos = transform.InverseTransformPoint(targets[i].position);

                // Find the position of the target from the desired position of the camera's local space.
                Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

                // Choose the largest out of the current size and the calculated size based on the tank being to the left or right of the camera.
                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / this.camera.aspect);
            }

            // Add the edge buffer to the size.
            size += this.screenEdgeBuffer;

            // Make sure the camera's size isn't below the minimum.
            size = Mathf.Max(size, this.minSize);

            return size;
        }

        public void SetStartPositionAndSize()
        {
            this.FindAveragePosition();

            // Set the camera's position to the desired position without damping.
            this.transform.position = this.desiredPosition;

            // Find and set the required size of the camera.
            this.camera.orthographicSize = FindRequiredSize();
        }
   // } 
}
