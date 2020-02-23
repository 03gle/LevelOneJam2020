using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupScript : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject player;
    private bool reset = false;
    private GameObject reference;

    // Start is called before the first frame update
    void Start()
    {
        reference = Instantiate(player, spawnPoint.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (reset)
        {
            reference.transform.position = spawnPoint.transform.position;
            reset = false;
        }
    }

    public void ResetPlayer()
    {
        reset = true;
    }
}
