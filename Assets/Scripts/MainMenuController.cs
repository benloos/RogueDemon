using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private float transitionTime = 1f;

    //[SerializeField]
    //private Animator transition;

    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerPrefs.SetInt("PlayerDMG", 10);
        PlayerPrefs.SetInt("PlayerHP", 100);
        PlayerPrefs.SetInt("PlayermaxHP", 100);
        PlayerPrefs.SetInt("PlayerFirerate", 2);
    }

    IEnumerator LoadLevel()
    {
        //transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayGame()
    {
        StartCoroutine(LoadLevel());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}