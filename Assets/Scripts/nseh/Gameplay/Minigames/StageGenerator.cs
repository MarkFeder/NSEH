using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour {



    public GameObject platform;
    public Transform generationPoint;
    [SerializeField]
    private List<GameObject> SpecialPlatforms;

    private int count;

    private float platformWidth;

    // Use this for initialization
    void Start()
    {
        platformWidth = platform.GetComponent<BoxCollider>().size.x;
        count = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < generationPoint.position.x)
        {
            transform.position = new Vector3(transform.position.x + platformWidth, transform.position.y, transform.position.z);
            if (count < 15)
            {
                Instantiate(platform, transform.position, transform.rotation);
                count++;
            }
            else
            {
                count = 0;
                if (SpecialPlatforms.Count != 0) { 
                int randomTeleportPoint = UnityEngine.Random.Range(0, SpecialPlatforms.Count);
                Instantiate(SpecialPlatforms[randomTeleportPoint], transform.position, transform.rotation);
               }
                else
                {
                    Instantiate(platform, transform.position, transform.rotation);
                }
            }
            
        }
    }
}
