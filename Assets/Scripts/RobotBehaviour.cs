using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehaviour : MonoBehaviour
{
    public float detectionRange;
    public float speed;
    public float suckingRange;
    public float suckingPower;
    public float suckingHeight;
    public LayerMask wallMask;
    public float counterRotationTorque = 100f;
    public GameObject setupObject;

    public AudioSource eatingSound;

    private bool sockDetected = false;
    private bool playerDied = false;

    private GameObject player;
    private float timeSincePlayerDeath = 0.0f;
    private float timeSinceLastCollision = 0.0f;
    [HideInInspector] new public Rigidbody rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        //moveDirection = transform.forward;
        rigidbody = GetComponent<Rigidbody>();
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        setupObject = GameObject.FindGameObjectWithTag("Setup");
    }

    private void FixedUpdate()
    {

        // Counter rotation to avoid toppling over
        //var rot = Quaternion.FromToRotation(transform.up, Vector3.up);
        //rigidbody.AddTorque(new Vector3(rot.x, rot.y, rot.z) * counterRotationTorque);

        if (sockDetected == true)
        {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y , player.transform.position.z), Vector3.up);
            GetComponent<Rigidbody>().velocity = transform.forward * speed;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (playerDied) timeSincePlayerDeath += Time.deltaTime;
        if (timeSincePlayerDeath > 1f)
        {
            playerDied = false; 
            timeSincePlayerDeath = 0f;
        }
        timeSinceLastCollision = timeSinceLastCollision + Time.deltaTime;
        if (player == null) player = GameObject.FindWithTag("Player");
        
        // Robot sucking
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < suckingRange && player.transform.position.y < suckingHeight)
        {
            Vector3 dir = transform.position - player.transform.position;
            Vector3 suckingVector = dir * (suckingRange - distance) * suckingPower;
            var ayy = player.GetComponent<Rigidbody>();
            ayy.AddForce(suckingVector.x, -2f, suckingVector.z);
        }

        //Debug.Log("dist = "+distance + "; dRange = "+detectionRange);
        //Debug.Log("dist < dRange = " + (distance < detectionRange));

        if (distance < detectionRange && player.transform.position.y < suckingHeight)
        {
            sockDetected = true;
        }
        if (sockDetected && ((distance > detectionRange + 1) || player.transform.position.y > suckingHeight + 1))
        {
            sockDetected = false;
        }
    }

    IEnumerator Rotate(Vector3 axis, float angle, float duration = 1.0f)
    {
        // Back up roboto
        float backingTimer = 0f;
        while (backingTimer < duration)
        {
            if (sockDetected) break;
            GetComponent<Rigidbody>().velocity = -transform.forward * speed;
            backingTimer += Time.deltaTime;
            yield return null;
        }

        // Turn roboto
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        Quaternion from = transform.rotation;
        Quaternion to = transform.rotation;
        to *= Quaternion.Euler(axis * angle);

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            if (sockDetected) break;
            transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = to;

        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (timeSinceLastCollision >= 0.5f)
        {
            bool hasCollided = false;

            foreach (ContactPoint contact in collision.contacts)
            {
                if (collision.gameObject.layer == 10 && !playerDied)
                {
                    eatingSound.PlayOneShot(eatingSound.clip);
                    hasCollided = true;
                    playerDied = true;
                    sockDetected = false;
                    setupObject.GetComponent<SetupScript>().SendMessage("ResetPlayer");
                }
                if (!hasCollided && collision.gameObject.layer == 9)
                {
                    timeSinceLastCollision = 0f;
                    hasCollided = true;
                    StartCoroutine(Rotate(Vector3.up, 110, 0.5f));
                }
            }
        }
        

    }
}
