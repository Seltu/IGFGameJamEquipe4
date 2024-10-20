using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    [Header("Variables and References")]
    [SerializeField] private GameObject _PausePanel;
    [SerializeField] private float _soundDelay;
    [SerializeField] private AudioSource clickSound;
    private bool _gameIsPaused;

    private void Start()
    {
        _gameIsPaused = false;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            if(!_gameIsPaused)
                PauseGame();
            else
                UnpauseGame();
        }
    }

    public void PauseGame()
    {   
        Time.timeScale = 0;
        _PausePanel.SetActive(true);
    }

    public void UnpauseGame()
    {
        _PausePanel.SetActive(false);
        Time.timeScale = 1;
        clickSound.Play();
    }

    public void GoToMenuButton()
    {
        //play sound
        clickSound.Play();
        StartCoroutine(MenuSoundDelay());
    }

    public void ExitGameButton()
    {
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
