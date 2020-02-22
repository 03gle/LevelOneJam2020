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

    private Vector3 moveDirection;
    private bool rotating = false;

    private GameObject player;
    private float timeCount = 0.0f;
    [HideInInspector] new public Rigidbody rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        moveDirection = Vector3.forward;
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Robot movement
        if (rotating)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        } 
        if (!rotating)
        {
            moveDirection = Vector3.forward;
            Debug.Log("hey!");
            GetComponent<Rigidbody>().velocity = moveDirection * speed;
        }

        // Counter rotation to avoid toppling over
        var rot = Quaternion.FromToRotation(transform.up, Vector3.up);
        rigidbody.AddTorque(new Vector3(rot.x, rot.y, rot.z) * counterRotationTorque);

    }

    // Update is called once per frame
    void Update()
    {
        timeCount = timeCount + Time.deltaTime;
        if (player == null) player = GameObject.FindWithTag("Player"); ;
        
        // Robot sucking
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < suckingRange && player.transform.position.y < suckingHeight)
        {
            Vector3 dir = transform.position - player.transform.position;
            Vector3 suckingVector = dir * (detectionRange - distance) * suckingPower;
            var ayy = player.GetComponent<Rigidbody>();
            ayy.AddForce(suckingVector.x, -2f, suckingVector.z);
        }
    }

    IEnumerator Rotate(Vector3 axis, float angle, float duration = 1.0f)
    {
        float backingTimer = 0f;
        while (backingTimer < duration)
        {
            GetComponent<Rigidbody>().velocity = -transform.forward * speed;
            backingTimer += Time.deltaTime;
            yield return null;
        }

        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        Quaternion from = transform.rotation;
        Quaternion to = transform.rotation;
        to *= Quaternion.Euler(axis * angle);

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = to;
        rotating = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (timeCount >= 1f)
        {
            
        }
        bool hasCollided = false;

        foreach (ContactPoint contact in collision.contacts)
        {
            //Debug.Log(collision.gameObject.layer);
            if (!hasCollided && collision.gameObject.layer == 9)
            {
                moveDirection = -Vector3.forward;
                hasCollided = true;
                StartCoroutine(Rotate(Vector3.up, 135, 0.5f));
                rotating = true;
            }
        }

    }
}
