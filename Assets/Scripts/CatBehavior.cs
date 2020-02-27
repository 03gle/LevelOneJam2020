using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBehavior : MonoBehaviour
{

    public float watchingRange;
    public float watchingHeight;
    public float grabRange;
    public float speed;
    public AudioSource audioSource;
    public AudioClip roarSound;
    public AudioClip meowSound1;
    public AudioClip meowSound2;
    public AudioClip meowSound3;
    public GameObject catFood;

    private bool playerDied = false;
    private GameObject player;
    private GameObject setupObject;
    private Vector3 startPos;
    private float timeSinceLastCollision = 0.0f;
    private float timeSincePlayerDeath = 0.0f;
    private bool isAggressive = true;

    // Start is called before the first frame update
    void Start()
    {
        transform.up = Vector3.down;
        startPos = transform.position;
        setupObject = GameObject.FindGameObjectWithTag("Setup");
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), Vector3.down);
        }

        if (playerDied) timeSincePlayerDeath += Time.deltaTime;
        if (timeSincePlayerDeath > 1f)
        {
            playerDied = false;
            timeSincePlayerDeath = 0f;
        }
        timeSinceLastCollision = timeSinceLastCollision + Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < watchingRange && player.transform.position.y < watchingHeight && isAggressive)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = roarSound;
                audioSource.Play();
            }
            
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), Vector3.down);
            if (distance < grabRange)
            {
                GetComponent<Rigidbody>().velocity = transform.forward * speed;
            }
            else GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        } else if (!isAggressive)
        {
            float dist = Vector3.Distance(transform.position, catFood.transform.position);
            transform.LookAt(new Vector3(catFood.transform.position.x, transform.position.y, catFood.transform.position.z), Vector3.down);
            if (dist <= 1.5f)
            {
                GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            } else
            {
                GetComponent<Rigidbody>().velocity = transform.forward * speed;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (timeSinceLastCollision >= 0.5f)
        {

            foreach (ContactPoint contact in collision.contacts)
            {
                if (collision.gameObject.layer == 10 && !playerDied && isAggressive)
                {
                    audioSource.PlayOneShot(meowSound1);
                    playerDied = true;
                    setupObject.GetComponent<SetupScript>().SendMessage("ResetPlayer");
                    GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    transform.position = startPos;
                    return;
                }
            }
        }


    }

    public void SetPassive()
    {
        audioSource.PlayOneShot(meowSound3);
        isAggressive = false;
    }
}
