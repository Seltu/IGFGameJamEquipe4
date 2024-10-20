using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _soundDelay;

    private void OnEnable()
    {
        _animator.Play("GameOverUI");
    }

    public void GoToMenuButton()
    {
        //play sound
        StartCoroutine(MenuSoundDelay());
    }

    public void ExitGameButton()
    {
        StartCoroutine(ExitSoundDelay());
    }

    public void ReestartGameButton()
    {
        StartCoroutine(RestartSoundDelay());
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

    private IEnumerator RestartSoundDelay()
    {
        yield return new WaitForSecondsRealtime(_soundDelay);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
