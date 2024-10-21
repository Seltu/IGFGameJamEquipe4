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
    private bool _gameCanBePaused = true;

    private void Start()
    {
        EventManager.onGameOverEvent += GameCannotBePaused;
        EventManager.onBossSpawnEvent += BossCutsceneCannotPause;
        _gameIsPaused = false;
        _gameCanBePaused = true;
    }

    private void OnDestroy()
    {
        EventManager.onGameOverEvent -= GameCannotBePaused;
        EventManager.onBossSpawnEvent -= BossCutsceneCannotPause;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && _gameCanBePaused)
        {
            if(!_gameIsPaused)
            {
                PauseGame();
            }
            else if(_gameIsPaused)
            {
                Debug.Log("help");
                UnpauseGame();
            }
        }
    }

    public void PauseGame()
    {   
        Time.timeScale = 0;
        _gameIsPaused = true;
        _PausePanel.SetActive(true);
    }

    public void UnpauseGame()
    {
        _PausePanel.SetActive(false);
        Time.timeScale = 1;
        clickSound.Play();
        _gameIsPaused = false;
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
        yield return new WaitForSecondsRealtime(_soundDelay);
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }

    private IEnumerator ExitSoundDelay()
    {
        yield return new WaitForSecondsRealtime(_soundDelay);
        Application.Quit();
    }

    private void GameCannotBePaused()
    {
        _gameCanBePaused = false;
    }

    private void BossCutsceneCannotPause()
    {
        StartCoroutine(GameCannotPauseForTime(1f));
    }

    private IEnumerator GameCannotPauseForTime(float time)
    {
        _gameCanBePaused = false;
        yield return new WaitForSeconds(time);
        _gameCanBePaused = true;
    }
}
