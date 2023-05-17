using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSFX : MonoBehaviour
{
    public AudioSource audioSource;
    // Start is called before the first frame update
    private void Awake()
    {
        audioSource.Play();
    }

}
