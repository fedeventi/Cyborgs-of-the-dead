using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicScripts : MonoBehaviour
{
    public List<AudioClip> audioClips = new List<AudioClip>();
    AudioSource audioSource;

    public void StepSound()
    {
        audioSource.PlayOneShot(audioClips[0], 0.1f);
    }

    public void GoLevel()
    {
        SceneManager.LoadScene("Level");
    }
}
