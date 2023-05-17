using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SoundController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI infoText;
    [SerializeField] private AudioMixer audioMixer;
    private float currentVolume;
    // Start is called before the first frame update

    private void Start()
    {
        audioMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
        audioMixer.SetFloat("AmbientVolume", PlayerPrefs.GetFloat("AmbientVolume"));
    }

    public void setMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("MasterVolume", volume);
        currentVolume = volume;

    }

    public void setAmbientVolume(float volume)
    {
        audioMixer.SetFloat("AmbientVolume", volume);
        PlayerPrefs.SetFloat("AmbientVolume", volume);

    }

    public void setMasterVolumeUp()
    {
        if (currentVolume <= -5f)
        {
            audioMixer.SetFloat("MasterVolume", currentVolume += 20f);
        }

    }

    public void setMasterVolumeDown()
    {
        if (currentVolume >= -80f)
        {
            audioMixer.SetFloat("MasterVolume", currentVolume -= 20f);
        }

    }



    public void StopMasterVolume()
    {
        if (currentVolume > -80f)// if less or equal to 100%
        {
            currentVolume = -80f;
            audioMixer.SetFloat("MasterVolume", currentVolume);
            infoText.text = "Sound Stopped!";
            StartCoroutine(DisplayText(2f));
        }

    }

    public void ResumeMusic()
    {
        infoText.text = "Putting volume to 50 %!";
        currentVolume = -20f;
        audioMixer.SetFloat("MasterVolume", currentVolume);
        StartCoroutine(DisplayText(2f));


    }

    IEnumerator DisplayText(float value)
    {
        yield return new WaitForSeconds(0.5f);
        infoText.gameObject.SetActive(true);
        yield return new WaitForSeconds(value);
        infoText.gameObject.SetActive(false);
    }
}
