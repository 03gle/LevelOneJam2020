using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Ground check variables
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    // Public movement variables
    public float jumpHeight = 400f;
    public float jumpDistance = 150f;
    public float counterRotationTorque = 100f;

    // Private variables
    private float timeTracker = 0.4f;
    private bool isGrounded;

    [HideInInspector] new public Rigidbody rigidbody;

    private void Awake()
    {   
        rigidbody = GetComponent<Rigidbody>();
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
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded)
        {
            // While button is held down increase timer
            if (Input.GetButton("Jump"))
            {
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
            }
            // TODO: Add check to see if sock is grounded
            if (Input.GetButtonDown("Jump")) //&& isGrounded)
            {

            }
        }
    }
}
