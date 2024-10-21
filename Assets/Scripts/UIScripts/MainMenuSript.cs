using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSript : MonoBehaviour
{
    [Header("Variables and References")]
    [SerializeField] private float _buttonSoundDelayTime;
    [SerializeField] private string _gameSceneName;
    [SerializeField] private Animator _anim;
    [SerializeField] private AudioSource clickSound;
    private AsyncOperation asyncOperation;

    void Start()
    {
        asyncOperation = SceneManager.LoadSceneAsync(_gameSceneName);
        asyncOperation.allowSceneActivation = false;
    }


    // Call this function when you're ready to switch to the preloaded scene
    public void ActivateScene()
    {
        if (asyncOperation != null)
        {
            asyncOperation.allowSceneActivation = true;
        }
    }



    #region Play Button
    public void PlayButtonClicked()
    {
        StartCoroutine(PlayButtonSoundDelayCoroutine());
    }

    private IEnumerator PlayButtonSoundDelayCoroutine()
    {
        //play sound
        yield return new WaitForSeconds(_buttonSoundDelayTime);
        ActivateScene();
    }
    #endregion



    #region Credits Button
    public void CreditsButton()
    {
        //play sound 
        clickSound.Play();
        _anim.SetTrigger("OpenCredits");
    }

    public void CreditsBackButton()
    {
        //play sound
        clickSound.Play();
        _anim.SetTrigger("CloseCredits");
    }
    #endregion



    #region Exit Button
    public void ExitButtonClicked()
    {
        StartCoroutine(ExitButtonSoundDelayCoroutine());
    }   

    private IEnumerator ExitButtonSoundDelayCoroutine()
    {
        yield return new WaitForSeconds(_buttonSoundDelayTime);
        Application.Quit();
    }
    #endregion
}