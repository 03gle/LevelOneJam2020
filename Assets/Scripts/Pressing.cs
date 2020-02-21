using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pressing : MonoBehaviour
{
    public Camera cam;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 10.0F))
        {
            if (hit.transform.tag == "Pressable")
            {  
                if (Input.GetKeyDown(KeyCode.E))
                { 
                    //Debug.Log("Pressed E on: " + hit.transform.name);
                    hit.transform.SendMessage("Press");
                    Debug.DrawRay(cam.transform.position, cam.transform.forward*2, Color.green, 5.0F);
                }
            }
            else if (hit.transform.tag == "Carryable")
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    //Debug.Log("Pressed M0 on: " + hit.transform.name);
                    hit.transform.SendMessage("Push", transform.forward);
                    Debug.DrawRay(cam.transform.position, cam.transform.forward * 2, Color.green, 5.0F);
                }
            }
        }
    }
}
