using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{



    public GameObject platform;
    public GameObject platformChest;
    public GameObject platformInitial;
    public Transform generationPoint;
    [SerializeField]
    private List<GameObject> SpecialPlatforms;

    private int count;
    private int countSpecial;

    private float platformWidth;

    private int randomPlatform;

    // Use this for initialization
    void Start()
    {
        platformWidth = platformInitial.GetComponent<MeshRenderer>().bounds.size.x;
        count = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < generationPoint.position.x)
        {
            platformWidth = platform.GetComponent<MeshRenderer>().bounds.size.x;

            transform.position = new Vector3(transform.position.x + platformWidth, transform.position.y, transform.position.z);
            if (count < 5)
            {
                Instantiate(platform, transform.position, transform.rotation);
                count++;
            }
            else
            {
                countSpecial++;
                count = 0;
                if (countSpecial == 5 && SpecialPlatforms.Count != 0)
                {
                    countSpecial = 0;
                    Instantiate(platformChest, transform.position, transform.rotation);
                }
                else if (SpecialPlatforms.Count == 0)
                {
                    Instantiate(platform, transform.position, transform.rotation);
                }
                else
                {
                    randomPlatform = UnityEngine.Random.Range(0, SpecialPlatforms.Count);
                    platformWidth = SpecialPlatforms[randomPlatform].GetComponent<MeshRenderer>().bounds.size.x;
                    Instantiate(SpecialPlatforms[randomPlatform], transform.position, transform.rotation);

                }
            }
        }

    }
}

