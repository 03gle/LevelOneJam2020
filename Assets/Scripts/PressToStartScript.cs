﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressToStartScript : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButtonDown("Jump"))
        {
            SceneManager.LoadScene("level design prototype");
        }

    }
}
