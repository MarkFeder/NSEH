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
        public GameObject spike;
        public GameObject platform;
        public float Health;
        public float maxPlayers;
        public float numPlayers;

        private Transform nextPoint;
        private Rigidbody myRigidBody;
        private NavMeshAgent agent;
        private float dice;
        private float prob_Patrol_in;
        private float prob_AttackSpikes_in;
        private float prob_AttackRoll_in;
        private bool frenzy;
        private bool isDeath;
        private int wait;
        private GameObject throne;
        private Animator animator;
        private bool isDice;

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
            prob_AttackSpikes_in = 1f;
            Health = 100;
            frenzy = false;
            wait = 2;
            animator.SetBool("Appear", true);
            animator.SetBool("Appear", false);
            throne = GameObject.Find("throne");
            //maxPlayers = GameObject.Find("GameManager").GetComponent<GameManager>()._numberPlayers;
            maxPlayers = 2;
            animator.SetBool("Walk", true);
            isDice = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (Health < 0 && isDeath == false)
            {
                isDeath = true;
                animator.SetBool("Death", true);
            }
            else
            {
                if (Health < 30 && frenzy == false)
                {
                    frenzy = true;
                    animator.SetBool("Frenzy", true);
                    prob_Patrol_in = 0.25f;
                    prob_AttackSpikes_in = 1f;
                    agent.speed = 10;
                    wait = 1;
                
                }
                //Debug.Log("0 " + animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle")+ " "+isDice);
                if (Mathf.Abs(this.gameObject.transform.position.x - nextPoint.transform.position.x) <= 1f)
                {
                    animator.SetBool("Walk", false);
                    animator.SetBool("AttackRoll", false);
                    

                    if (nextPoint == right_Limit)
                    {
                        nextPoint = left_Limit;
                        isDice = false;
                        Debug.Log("1");
                    }
                    else if (nextPoint == left_Limit)
                    {
                        nextPoint = right_Limit;
                        isDice = false;
                        Debug.Log("2");
                    }
                    
                }


                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle") && isDice ==false)
                {
                  
                        Debug.Log("3 " + " " + animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle"));
                        isDice = true;
                        Invoke("SelectAttack", wait);
                    

                }
            }
        }

        void SelectAttack()
        {
            
            dice = Random.Range(0.0f, 1.0f);
            //numPlayers = throne.GetComponent<Throne>().players_throne;
            numPlayers = 1;
            float prob_AttackRoll = prob_AttackSpikes_in - ((prob_AttackSpikes_in - prob_Patrol_in) * numPlayers / maxPlayers);
            Debug.Log("Dice " + dice + " " + prob_AttackSpikes_in + " "+ prob_Patrol_in+" "+ numPlayers+" " +maxPlayers);
            float angle = 179;
            if (Vector3.Angle(this.transform.forward, nextPoint.transform.position - this.transform.position) > angle)
            {
                this.gameObject.transform.Rotate(0, 180, 0);

            }

            if (dice < prob_Patrol_in)
            {
                //Debug.Log(this.gameObject.transform.rotation);
                
                animator.SetBool("Walk", true);
                //funcion caminar
                Debug.Log("PATROL");
                agent.SetDestination(nextPoint.position);
            }
            else if (dice < prob_AttackRoll)
            {
               
                animator.SetBool("AttackRoll", true);
                //funcion rodar
                Debug.Log("ATTACKROLL");
                agent.SetDestination(nextPoint.position);
            }
            else
            {

                animator.SetTrigger("AttackSpikes");
                isDice = false;
                //funcion pinchos
                Debug.Log("ATTACKSPIKES");
                //animator.SetBool("AttackSpikes", false);
                /* for (int i = 0; i < 3; i++)
                 {
                     GameObject clone = Instantiate(spike, sp.transform.position, transform.rotation);
                     clone.rigidbody.AddForce(transform.forward * 8000); ;
                 }*/

            }
            

        }


        IEnumerator Wait(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }
    }
}
