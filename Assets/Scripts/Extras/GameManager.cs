using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Scene actualScene;

    //
    [Header("MENU")]
    public Animator menuAnimator;

    private void Start()
    {
        actualScene = SceneManager.GetActiveScene();
    }

    private void Update()
    {
        RestartScene();

        if (Input.GetKey(KeyCode.O))
        {
            SceneManager.LoadScene("Level1");
        }

        if (Input.GetKey(KeyCode.U))
        {
            SceneManager.LoadScene("Level");
        }
    }

    //para reiniciar el nivel
    public void RestartScene()
    {
        if(Input.GetKey(KeyCode.P))
        {
            SceneManager.LoadScene(actualScene.name);
        }
    }

    //MENU
    public void Credits()
    {
        menuAnimator.SetBool("Credits", true);
    }

    public void BackToMenu()
    {
        menuAnimator.SetBool("Credits", false);
    }

    public void GoToPlay()
    {
        SceneManager.LoadScene("IntroCinematic");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
