using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    public Material ButtonMat;

    public Activatable activatable;

    Material ButMat;

    bool isOn = false;

    void Start()
    {
        ButMat = new Material(ButtonMat);
        ButMat.color = Color.red;
        GetComponent<Renderer>().material = ButMat;
    }

    public void Press()
    {
        isOn = !isOn;
        if (isOn) ButMat.color = Color.green;
        else ButMat.color = Color.red;

        activatable.Activate();
    }
}
