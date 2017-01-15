using UnityEngine;
using System.Collections;
using nseh.Gameplay.Combat;
using System.Linq;
using nseh.Gameplay.Base.Abstract;

namespace nseh.Gameplay.Entities.Characters
{
    public class CharacterProofs : MonoBehaviour
    {
        public float force = 20;
        public float walkSpeed = 0.15f;
        public float runSpeed = 1.0f;
        public float sprintSpeed = 2.0f;

        public float turnSmoothing = 3.0f;
        public float aimTurnSmoothing = 15.0f;
        public float speedDampTime = 0.1f;

        public float jumpHeight = 5.0f;
        public float jumpCooldown = 1.0f;

        private float timeToNextJump = 0;

        private float speed;

        private Vector3 lastDirection;
        private bool facingRight = true;

        private Animator anim;
        private int speedFloat;
        private int jumpBool;
        private int hFloat;
        private int groundedBool;
        private int animDead;
        private int animAttackA;
        private int animAttackB;
        private int hitsComboAAA;
        private Transform cameraTransform;
        private Rigidbody body;
        private CharacterController characterController;
        private Transform transform;

        private GameObject[] enemies;
        private GameObject currentEnemy;
        private CharacterHealth characterHealth;
        private int amountDamage = 10;
        private bool playerInRange;

        private float h;

        private bool run;
        private bool sprint;
        private bool dead;
        private bool attackA;
        private bool attackB;
        private float comboAAA = 0.0f;

        private bool isMoving;

        private float distToGround;
        private float sprintFactor;

        void Awake()
        {
            anim = GetComponent<Animator>();
            body = GetComponent<Rigidbody>();
            body.isKinematic = false;
            transform = GetComponent<Transform>();

            enemies = GameObject.FindGameObjectsWithTag("Player");
            characterHealth = GetComponent<CharacterHealth>();
            //characterController = GetComponent<CharacterController>();

            cameraTransform = Camera.main.transform;

            speedFloat = Animator.StringToHash("Speed");
            jumpBool = Animator.StringToHash("Jump");
            hFloat = Animator.StringToHash("H");
            groundedBool = Animator.StringToHash("Grounded");
            animDead = Animator.StringToHash("Dead");
            animAttackA = Animator.StringToHash("Attack_A");
            animAttackB = Animator.StringToHash("Attack_B");
            hitsComboAAA = Animator.StringToHash("Hits_Combo_AAA");

            distToGround = GetComponent<Collider>().bounds.extents.y;
            sprintFactor = sprintSpeed / runSpeed;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Player") && !enemies.Contains(gameObject))
            {
                Debug.Log("OnTriggerEnter--Player: " + other.gameObject.name + " is in range");
                playerInRange = true;
                currentEnemy = enemies.Where(g => g.activeSelf && g.Equals(other.gameObject)).FirstOrDefault();
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.collider.CompareTag("Player") && !enemies.Contains(gameObject))
            {
                Debug.Log("OnTriggerExit--Player: " + other.gameObject.name + " is in range");
                playerInRange = false;
                currentEnemy = null;
            }
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.CompareTag("Player") && !enemies.Contains(gameObject))
        //    {
        //        Debug.Log("OnTriggerEnter--Player: " + other.gameObject.name + " is in range");
        //        playerInRange = true;
        //        currentEnemy = enemies.Where(g => g.activeSelf && g.Equals(other.gameObject)).FirstOrDefault();
        //    }
        //}

        //private void OnTriggerExit(Collider other)
        //{
        //    if (other.CompareTag("Player") && !enemies.Contains(gameObject))
        //    {
        //        Debug.Log("OnTriggerExit--Player: " + other.gameObject.name + " is in range");
        //        playerInRange = false;
        //        currentEnemy = null;
        //    }
        //}

        void Update()
        {
            h = Input.GetAxis("Horizontal");
            run = Input.GetButton("Run");
            dead = Input.GetKeyDown(KeyCode.D);
            attackA = Input.GetKeyDown(KeyCode.V);
            attackB = Input.GetKeyDown(KeyCode.B);
            
            isMoving = Mathf.Abs(h) > 0.1;
        }

        void FixedUpdate()
        {
            anim.SetFloat(hFloat, h);
            anim.SetBool(groundedBool, IsGrounded());

            Dead();

            MovementManagement(h, run);

            JumpManagement();

            AttackManagement();
        }

        private void Dead()
        {
            //if (characterHealth.currentHealth <= 0)
            //{
            //    anim.SetTrigger(animDead);
            //}
        }

        private void AttackManagement()
        {
            if (playerInRange && attackA)
            {
                anim.SetTrigger(animAttackA);

                if (currentEnemy != null)
                {
                    var currentEnemyHealth = currentEnemy.GetComponent<CharacterHealth>();

                    //if (currentEnemyHealth.currentHealth > 0.0f)
                    //{
                    //    currentEnemyHealth.TakeDamage(amountDamage);
                    //}
                }
            }

            if (attackA)
            {
                anim.SetTrigger(animAttackA);
            }
            else if(attackB)
            {
                anim.SetTrigger(animAttackB);
            }
        }

        private void JumpManagement()
        {
            if (body.velocity.y < 10) // already jumped
            {
                anim.SetBool(jumpBool, false);
                if (timeToNextJump > 0)
                    timeToNextJump -= Time.deltaTime;
            }
            if (Input.GetButtonDown("Jump"))
            {
                anim.SetBool(jumpBool, true);
                if (speed > 0 && timeToNextJump <= 0)
                {
                    body.velocity = new Vector3(0, jumpHeight, 0);
                    timeToNextJump = jumpCooldown;
                }
            }
        }

        private void MovementManagement(float horizontal, bool running)
        {
            MakeCharacterToFlip(horizontal);

            if (isMoving)
            {
                if (running)
                {
                    speed = runSpeed;
                }
                else
                {
                    speed = walkSpeed;
                }

                anim.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);
            }
            else
            {
                speed = 0f;
                anim.SetFloat(speedFloat, 0f);
            }

            if (facingRight)
            {
                body.AddForce(transform.right * speed);
            }
            else
            {
                body.AddForce(-transform.right * speed);
            }
        }

        private void MakeCharacterToFlip(float horizontal)
        {
            if (horizontal > 0.0f && !facingRight)
            {
                Flip();
            }
            else if (horizontal < 0.0f && facingRight)
            {
                Flip();
            }
        }

        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;

            theScale.z *= -1;
            transform.localScale = theScale;
        }

        bool IsGrounded()
        {
            return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
        }

    }
}
