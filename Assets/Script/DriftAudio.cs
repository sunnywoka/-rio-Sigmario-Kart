using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftAudio : MonoBehaviour
{
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        //UnityEngine.Random.InitState(System.DateTime.Now.Second);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio(float v = 0.6f, float p = 1.2f)
    {
        audioSource.volume = v;
        audioSource.pitch = p;
        audioSource.Play();
    }
}
