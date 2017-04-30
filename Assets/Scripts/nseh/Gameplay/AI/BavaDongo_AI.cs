using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using nseh.Managers.Main;


namespace nseh.Gameplay.AI
{
    public class BavaDongo_AI : MonoBehaviour
    {

        public Transform left_Limit;
        public Transform right_Limit;
        public GameObject platform;
        public float Health;
        public float maxPlayers;
        public float numPlayers;

        private Transform nextPoint;
        private Rigidbody myRigidBody;
        private NavMeshAgent agent;
        private float dice;
        private float prob_Patrol_in;
        private float prob_Attack1_in;
        private float prob_Attack2_in;
        private bool frenzy;
        private bool isDeath;
        private int wait;
        private GameObject throne;
        private Animator animator;

        // Use this for initialization
        void Start()
        {
            animator = gameObject.GetComponent<Animator>();
            nextPoint = right_Limit;
            isDeath = false;
            myRigidBody = GetComponent<Rigidbody>();
            agent = GetComponent<NavMeshAgent>();
            agent.SetDestination(nextPoint.position);
            prob_Patrol_in = 0.5f;
            prob_Attack1_in = 1f;
            Health = 100;
            frenzy = false;
            wait = 2;
            throne = GameObject.Find("throne");
            maxPlayers = GameObject.Find("GameManager").GetComponent<GameManager>()._numberPlayers;
            animator.SetTrigger("appear");
        }

        // Update is called once per frame
        void Update()
        {
            if (Health < 0 && isDeath == false)
            {
                isDeath = true;
                animator.SetTrigger("Death");
            }
            else
            {
                if (Health < 30 && frenzy == false)
                {
                    frenzy = true;
                    animator.SetTrigger("Frenzy");
                    prob_Patrol_in = 0.25f;
                    prob_Attack1_in = 1f;
                    agent.speed = 10;
                    wait = 1;
                
                }

                if (Mathf.Abs(this.gameObject.transform.position.x - nextPoint.transform.position.x) < 0.5f)
                {
                    if (nextPoint == right_Limit)
                    {
                        animator.SetBool("Walk", false);
                        Invoke("SelectAttack", wait);
                        nextPoint = left_Limit;

                        Debug.Log("1");
                    }
                    else
                    {
                        animator.SetBool("Walk", false);
                        Invoke("SelectAttack", wait);
                        nextPoint = right_Limit;
                        Debug.Log("2");
                    }
                }
            }
        }

        void SelectAttack()
        {
            dice = Random.Range(0.0f, 1.0f);
            numPlayers = throne.GetComponent<Throne>().players_throne;
            float prob_Attack1 = prob_Attack1_in - ((prob_Attack1_in - prob_Patrol_in) * numPlayers / maxPlayers);
            Debug.Log("Dice " + dice + " " + prob_Attack1_in+" "+ prob_Patrol_in+" "+ numPlayers+" " +maxPlayers);
            if (dice < prob_Patrol_in)
            {
                animator.SetBool("Walk", true);
                Debug.Log("PATROL");
            }
            else if (dice < prob_Attack1)
            {
                animator.SetTrigger("Attack_1");
                Debug.Log("ATTACK1");
            }
            else
            {
                animator.SetTrigger("Attack_2");
                Debug.Log("ATTACK2");
            }
            agent.SetDestination(nextPoint.position);

        }
    }
}
