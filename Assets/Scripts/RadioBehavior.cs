using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioBehavior : MonoBehaviour
{
    public AudioClip music1;
    public AudioClip music2;
    public AudioClip music3;
    public AudioClip news1;
    public AudioClip news2;
    public AudioClip news3;

    private List<AudioClip> soundFiles = new List<AudioClip>();
    public AudioSource audioSource;
    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        soundFiles.Add(music1);
        soundFiles.Add(news1);
        soundFiles.Add(music2);
        soundFiles.Add(news2);
        soundFiles.Add(news3);
    }

    // Update is called once per frame
    void Update()
    {
        if (index >= soundFiles.Count) index = 0;
        if (!audioSource.isPlaying)
        {
            audioSource.clip = soundFiles[index++];
            audioSource.Play();
        }

    }
}
