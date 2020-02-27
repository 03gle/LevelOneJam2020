// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameObject setupObject;
    private GameObject robot;

    // Ground check variables
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public LayerMask groundMaskShelf;

    // Public movement variables
    public float jumpHeight = 400f;
    public float jumpDistance = 150f;
    public float counterRotationTorque = 100f;

    // Private variables
    private float timeTracker = 0.4f;
    private bool isGrounded;
    private AudioSource audioSource;

    [HideInInspector] new public Rigidbody rigidbody;

    private Vector3 scaleChange = new Vector3(0f, -0.5f, 0f);
    private float timeSinceLastJump = 0f;
    private bool isFullyStretched = true;

    private void Awake()
    {   
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        setupObject = GameObject.FindGameObjectWithTag("Setup");
        robot = GameObject.FindGameObjectWithTag("Robot");
    }

    // Frame-rate independent update
    void FixedUpdate()
    {
        // Apply gravity manually to ridgidbody
        rigidbody.useGravity = false;
        rigidbody.AddForce(Physics.gravity * (rigidbody.mass * rigidbody.mass));

        // Counter rotation to avoid toppling over
        var rot = Quaternion.FromToRotation(transform.up, Vector3.up);
        rigidbody.AddTorque(new Vector3(rot.x, rot.y, rot.z) * counterRotationTorque);
    }
    
    // Update is called once per frame
    void Update()
    {
        isGrounded = (Physics.CheckSphere(groundCheck.position, groundDistance, groundMask) || 
            Physics.CheckSphere(groundCheck.position, groundDistance, groundMaskShelf));

        timeSinceLastJump += Time.deltaTime;

        if (transform.localScale.y >= 1f) isFullyStretched = true;

        if (timeSinceLastJump > 0.05f && !isFullyStretched)
        {
            transform.localScale -= scaleChange * Time.deltaTime * 4;
        }

        if (isGrounded)
        {
            // While button is held down increase timer
            if (Input.GetButton("Jump"))
            {
                if (timeTracker <= 1) transform.localScale += scaleChange*Time.deltaTime;
                timeTracker += Time.deltaTime;
            }
            // When button is released, jump acording to timer
            if (Input.GetButtonUp("Jump"))
            {
                if (timeTracker > 1) timeTracker = 1f;
                rigidbody.AddForce(
                    transform.forward.x * jumpDistance * timeTracker,
                    jumpHeight * timeTracker,
                    transform.forward.z * jumpDistance * timeTracker
                    );
                timeTracker = 0.4f;
                audioSource.Play();
                //transform.localScale = new Vector3(1f, 1f, 1f);
                isFullyStretched = false;
                timeSinceLastJump = 0f;
            }
            
            if (Input.GetButtonDown("Jump"))
            {

            }
        }
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Win")
        {
            setupObject.GetComponent<SetupScript>().SendMessage("Win");
        }
        if (collider.gameObject.tag == "room2")
        {
            robot.GetComponent<RobotBehaviour>().SendMessage("ActivateSuck");
        }

    }
}
