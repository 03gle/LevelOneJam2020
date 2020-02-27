using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBehavior : MonoBehaviour
{
    private float timeSinceLastCollision = 0.0f;

    public AudioSource audioSource;
    public AudioClip sockHitSound;
    public AudioClip floorHitSound;

    public GameObject cat;


    // Update is called once per frame
    void Update()
    {
        timeSinceLastCollision = timeSinceLastCollision + Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (timeSinceLastCollision >= 1f)
        {
            bool hasCollided = false;

            foreach (ContactPoint contact in collision.contacts)
            {
                if (!hasCollided && collision.gameObject.layer == 8)
                {
                    audioSource.PlayOneShot(floorHitSound);
                    hasCollided = true;
                    cat.GetComponent<CatBehavior>().SendMessage("SetPassive");
                    timeSinceLastCollision = 0f;
                }
                if (!hasCollided && collision.gameObject.tag == "Player")
                {
                    audioSource.PlayOneShot(sockHitSound);
                    timeSinceLastCollision = 0f;
                    hasCollided = true;
                    
                }
            }
        }


    }
}
