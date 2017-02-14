using System;
using UnityEngine;

namespace nseh.GameManager.Other
{
    public class CharacterMovement : MonoBehaviour
    {
        public int playerNumber;
        public float speed = 12.0f;
        private Rigidbody body;

        private string verticalAxisName;
        private string horizontalAxisName;
        private float verticalInputValue;
        private float horizontalInputValue;

        private void Awake()
        {
            this.body = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            // When the character is turned on, make sure it's not kinematic
            this.body.isKinematic = false;

            // Reset input values
            this.horizontalInputValue = 0.0f;
            this.verticalInputValue = 0.0f;
        }

        private void OnDisable()
        {
            // When the character is turned off, set it to kinematic so it stops moving
            this.body.isKinematic = true;
        }

        private void Start()
        {
            this.verticalAxisName = "Vertical";
            this.horizontalAxisName = "Horizontal";
        }

        private void Update()
        {
            // Store the values of the input axis
            this.horizontalInputValue = Input.GetAxis(this.horizontalAxisName);
            this.verticalInputValue = Input.GetAxis(this.verticalAxisName);
        }

        private void FixedUpdate()
        {
            this.Move();
        }

        private void Move()
        {
            // TODO
        }
    }

    [Serializable]
    public class CharacterManager
    {
        public Transform spawnPoint;
        [HideInInspector]
        public int playerNumber;
        [HideInInspector]
        public GameObject Instance;
        [HideInInspector]
        public int nWins;

        private CharacterMovement movement;
        private GameObject canvasGameObject;

        public void Setup()
        {
            // Get references to the components
            this.movement = Instance.GetComponent<CharacterMovement>();
            this.canvasGameObject = Instance.GetComponentInChildren<Canvas>().gameObject;

            // Set the player numbers to be consistent across the scripts
            this.movement.playerNumber = this.playerNumber;
        }

        public void DisableControl()
        {
            this.movement.enabled = false;

            this.canvasGameObject.SetActive(false);
        }

        public void EnableControl()
        {
            this.movement.enabled = true;

            this.canvasGameObject.SetActive(true);
        }

        public void Reset()
        {
            this.Instance.transform.position = this.spawnPoint.position;
            this.Instance.transform.rotation = this.spawnPoint.rotation;

            this.Instance.SetActive(false);
            this.Instance.SetActive(true);
        }
    } 
}
