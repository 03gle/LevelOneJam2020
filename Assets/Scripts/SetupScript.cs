using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupScript : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject player;
    private bool reset = false;
    private bool gameWon = false;
    private bool alreadyWon = false;

    public bool SkipIntro = false;
    public AudioSource radioAudioSource;
    private AudioSource robotAudioSource;
    public GameObject robot;
    public float cutsceneDuration;

    public AudioSource audioSource;
    public AudioClip romanticEndingSong;
    public AudioClip lifeSocksSong;

    public GameObject img1;
    public GameObject img2;
    public GameObject img3;
    public GameObject img4;
    public GameObject img5;
    public GameObject img6;
    public GameObject img7;
    public GameObject img8;
    public GameObject img9;
    public GameObject img10;
    public GameObject img11;
    public GameObject img12;
    public GameObject img13;

    private List<GameObject> cutsceneFiles = new List<GameObject>();

    public GameObject img_o1;
    public GameObject img_o2;
    public GameObject img_o3;
    public GameObject img_o4;
    public GameObject img_o5;
    public GameObject img_o6;

    private List<GameObject> outroCutsceneFiles = new List<GameObject>();

    public GameObject fadeImg;

    enum fadeType
    {
        In,
        Out,
        OutIn
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;

        robotAudioSource = robot.GetComponent<AudioSource>();

        cutsceneFiles.Add(img1);
        cutsceneFiles.Add(img2);
        cutsceneFiles.Add(img3);
        cutsceneFiles.Add(img4);
        cutsceneFiles.Add(img5);
        cutsceneFiles.Add(img6);
        cutsceneFiles.Add(img7);
        cutsceneFiles.Add(img8);
        cutsceneFiles.Add(img9);
        cutsceneFiles.Add(img10);
        cutsceneFiles.Add(img11);
        cutsceneFiles.Add(img12);
        cutsceneFiles.Add(img12);

        outroCutsceneFiles.Add(img_o1);
        outroCutsceneFiles.Add(img_o2);
        outroCutsceneFiles.Add(img_o3);
        outroCutsceneFiles.Add(img_o4);
        outroCutsceneFiles.Add(img_o5);
        outroCutsceneFiles.Add(img_o6);
        outroCutsceneFiles.Add(img_o6);
        outroCutsceneFiles.Add(img_o6);

        if (!SkipIntro)
        {
            SetPlayerControl(false);
            StartCoroutine(PlayCutscenes(cutsceneFiles, 20, false, 0));
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (reset)
        {
            player.transform.position = spawnPoint.transform.position;
            player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            reset = false;
        }
        if (gameWon)
        {
            //Debug.Log("Game won!");
        }
    }

    public void ResetPlayer()
    {
        reset = true;
    }

    public void Win()
    {
        gameWon = true;
        if (!alreadyWon)
        {
            SetTimedPlayerControl(false, 1f);
            StartCoroutine(PlayFade(2, fadeType.OutIn));
            StartCoroutine(PlayCutscenes(outroCutsceneFiles, 14, true, 1));
        }
        alreadyWon = true;
    }

    IEnumerator PlayCutscenes(List<GameObject> list, float duration, bool isEnding, float startAfter)
    {
        float startTimer = 0.0f;
        while (startTimer < startAfter)
        {
            startTimer += Time.deltaTime;
            yield return null;
        }

        if (isEnding)
        {
            radioAudioSource.transform.parent.gameObject.SetActive(false);
            robotAudioSource.gameObject.SetActive(false);
            audioSource.PlayOneShot(romanticEndingSong);
        }
        else
        {
            StartCoroutine(PlayFade(2, fadeType.In));
        }

        int index = 0;
        float timer = -2.0f;
        float timePerImage = duration / list.Count;
        //Debug.Log("timePerImage = " + timePerImage);

        list[index].SetActive(true);

        while (index < list.Count)
        {
            timer += Time.deltaTime;
            
            if (timer > timePerImage)
            {
                timer = 0f;
                list[index].SetActive(false);
                if (!(index == list.Count - 1)) list[++index].SetActive(true);

                if (index == list.Count - 2)
                {
                    list[index].SetActive(true);
                    if (!isEnding) StartCoroutine(PlayFade(2, fadeType.OutIn));
                    else StartCoroutine(PlayFade(2, fadeType.Out));
                    float hackedTimer = 0.0f;
                    while (hackedTimer < 1)
                    {
                        hackedTimer += Time.deltaTime;
                        yield return null;
                    }
                    list[index].SetActive(false);
                    break;
                }
            }
            if (index == list.Count - 1) list[index].SetActive(false);

            yield return null;
        }

        if (isEnding)
        {
            float hackedTimer = 0.0f;
            while (hackedTimer < 3)
            {
                hackedTimer += Time.deltaTime;
                yield return null;
            }
            Application.Quit();
        }
        else
        {
            SetPlayerControl(true);
            radioAudioSource.GetComponent<AudioLowPassFilter>().enabled = false;
        }
    }

    IEnumerator PlayFade(float duration, fadeType type)
    {
        var ehh = fadeImg.GetComponent<RawImage>();

        if (type == fadeType.In) // fade in
        {
            for (float i = duration; i >= 0; i -= Time.deltaTime)
            {
                ehh.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
        else if (type == fadeType.Out) // fade out
        {
            for (float i = 0; i <= duration; i += Time.deltaTime)
            {
                ehh.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
        else if (type == fadeType.OutIn) // fade out, fade in
        {
            for (float i = 0; i <= duration; i += Time.deltaTime)
            {
                ehh.color = new Color(0, 0, 0, i);
                yield return null;
            }
            for (float i = duration; i >= 0; i -= Time.deltaTime)
            {
                ehh.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
    }

    private void SetPlayerControl(bool setToActive)
    {
        if (setToActive)
        {
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponentInChildren<MouseLook>().enabled = true;
        }
        else
        {
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponentInChildren<MouseLook>().enabled = false;
        }
    }

    IEnumerator SetTimedPlayerControl(bool setToActive, float time)
    {
        float hackedTimer = 0.0f;
        while (hackedTimer < time)
        {
            hackedTimer += Time.deltaTime;
            yield return null;
        }
        if (setToActive)
        {
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponentInChildren<MouseLook>().enabled = true;
        }
        else
        {
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponentInChildren<MouseLook>().enabled = false;
        }
    }
}
