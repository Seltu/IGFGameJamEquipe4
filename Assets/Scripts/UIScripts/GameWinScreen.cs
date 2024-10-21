using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinScreen : MonoBehaviour
{
    [SerializeField] private float _soundDelay;
    [SerializeField] private AudioSource clickSound;

    public void GoToMenuButton()
    {
        //play sound
        StartCoroutine(MenuSoundDelay());
    }

    public void ExitGameButton()
    {
        clickSound.Play();
        StartCoroutine(ExitSoundDelay());
    }

    private IEnumerator MenuSoundDelay()
    {
        yield return new WaitForSeconds(_soundDelay);
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }

    private IEnumerator ExitSoundDelay()
    {
        yield return new WaitForSeconds(_soundDelay);
        Application.Quit();
    }
}
