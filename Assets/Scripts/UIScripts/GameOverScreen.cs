using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _soundDelay;
    [SerializeField] private AudioSource clickSound;

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
        clickSound.Play();
        StartCoroutine(ExitSoundDelay());
    }

    public void ReestartGameButton()
    {
        clickSound.Play();
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
