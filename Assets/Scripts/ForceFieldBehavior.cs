using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldBehavior : MonoBehaviour
{
    public BoxCollider Col;
    public Light Li;
    public AudioSource HumSource;
    public AudioSource ActiSource;
    
    bool isOn = false;
    
    // Set intensity of light
    public void SetLightIntensity(float intensity)
    {
        Li.intensity = intensity;
    }

    // Toggle components on/off
    public void ToggleOn()
    {
        isOn = !isOn;

        Col.enabled = !Col.enabled;                 // Toggle collider
        GetComponent<Renderer>().enabled = isOn;    // Toggle mesh renderer
        Li.enabled = isOn;                          // Toggle light source

        HumSource.enabled = isOn;                   // Toggle audio sources
        ActiSource.enabled = isOn;
        if (isOn)
        {
            ActiSource.Play();
            HumSource.Play();
        }
    }

}