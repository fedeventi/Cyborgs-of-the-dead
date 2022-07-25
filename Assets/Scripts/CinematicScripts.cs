using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicScripts : MonoBehaviour
{
    public List<AudioClip> audioClips = new List<AudioClip>();
    public  AudioSource audioSource;

    public void Start()
    {
        
    }
    IEnumerator LoadYourAsyncScene()
    {
      
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level");

        
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    public void StepSound()
    {
        audioSource.PlayOneShot(audioClips[0], 0.1f);
    }

    public void ShieldButtonSound()
    {
        audioSource.PlayOneShot(audioClips[1], 0.1f);
    }

    public void ShieldSound()
    {
        audioSource.PlayOneShot(audioClips[2], 0.1f);
    }

    public void GoLevel()
    {
       //SceneManager.LoadScene("Level");
       StartCoroutine(LoadYourAsyncScene());
    }
}
