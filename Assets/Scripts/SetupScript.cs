using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupScript : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject player;
    private bool reset = false;
    private GameObject reference;
    private bool gameWon = false;

    public Image img1;
    public Image img2;
    public Image img3;
    public Image img4;
    public Image img5;
    public Image img6;
    public Image img7;
    public Image img8;
    public Image img9;
    public Image img10;
    public Image img11;
    public Image img12;
    public Image img13;
    public Image img14;
    public Image img15;

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
        if (gameWon)
        {
            Debug.Log("Game won!");
        }
    }

    public void ResetPlayer()
    {
        reset = true;
    }

    public void Win()
    {
        gameWon = true;
    }
}
