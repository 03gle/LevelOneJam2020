using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carryable : MonoBehaviour
{
    public float gravity = -9.81f;

    public float altGravity = 0.5f;

    public bool useGravity = false;


    [HideInInspector] new public Rigidbody rigidbody;

    [HideInInspector] public bool beingCarried = false;


    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rigidbody.useGravity = false;
        if (useGravity) rigidbody.AddForce(Physics.gravity * (rigidbody.mass * rigidbody.mass));
        else
        {
            float dist = 1 - transform.position.y;

            rigidbody.AddForce(Physics.gravity * (rigidbody.mass * rigidbody.mass) * (-dist -0.3f));
        }
    }

    public void EnableGravity()
    {
        useGravity = true;
    }

    public void DisableGravity()
    {
        useGravity = false;
    }

    public void Push(Vector3 dir)
    {
        rigidbody.AddForce(dir.x * 20f, 50f, dir.z * 20f);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
