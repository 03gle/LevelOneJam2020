using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainmentBehavior : Activatable
{
    float totalTime = 0f;
    bool isOn = false;                  // Forcefields active or not
    public float bootUpTime = 0.5f;     // Animation length
    public List<ForceFieldBehavior> forceFields;    // List of Forcefield objects to animate

    // Variables for controlling color alpha
    float colorAlpha;
    Material ffMat;
    public Material Mat;
    public float colorAlphaStart = 0.05f;
    public float colorAlphayEnd = 0.15f;

    // Variables for controlling light intensity
    float lightIntensity;
    public float lightIntensityStart = 6f;
    public float lightIntensityEnd = 10f;
    
    void Start()
    {
        // Forcefields in same module share material
        ffMat = new Material(Mat);
        ffMat.SetColor("_Color", new Color(0, 1, 0, colorAlphaStart));

        // Starting values
        colorAlpha = colorAlphaStart;
        lightIntensity = lightIntensityStart;
    }

    void Update()
    {
        totalTime += Time.deltaTime;

        if (!isOn)                  // Reset animation timer and set variables to start values
        {                       
            totalTime = 0f;
            colorAlpha = colorAlphaStart;
            lightIntensity = lightIntensityStart;
        }
        else if (totalTime < bootUpTime)    // Linear easing effects playing
        {
            
            colorAlpha += Time.deltaTime * ((colorAlphayEnd - colorAlphaStart) / bootUpTime);
            ffMat.SetColor("_Color", new Color(0, 1, 0, colorAlpha));

            lightIntensity += Time.deltaTime * ((lightIntensityEnd - lightIntensityStart) / bootUpTime);
            foreach (ForceFieldBehavior forcefield in forceFields)
            {                   
                forcefield.SetLightIntensity(lightIntensity);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Carryable")
        {
            //Debug.Log("Carryable Enter");
            if (isOn) other.transform.SendMessage("DisableGravity");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Carryable")
        {
            //Debug.Log("Carryable Exit");
            other.transform.SendMessage("EnableGravity");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Carryable")
        {
            if (isOn)
            {
                other.transform.SendMessage("DisableGravity");
                //Debug.Log("Carryable Stay DisableGravity");
            }
            else 
            {
                other.transform.SendMessage("EnableGravity");
                //Debug.Log("Carryable Stay EnableGravity");
            }
        }
    }

    // Called when activated by button
    public override void Activate()
    {
        isOn = !isOn;
        foreach (ForceFieldBehavior forcefield in forceFields)
        {
            forcefield.ToggleOn();
        }
    }
}
