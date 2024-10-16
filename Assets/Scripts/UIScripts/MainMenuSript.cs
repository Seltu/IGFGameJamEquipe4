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


    #region Play Button
    public void PlayButtonClicked()
    {
        StartCoroutine(PlayButtonSoundDelayCoroutine());
    }

    private IEnumerator PlayButtonSoundDelayCoroutine()
    {
        //play sound
        yield return new WaitForSeconds(_buttonSoundDelayTime);
        SceneManager.LoadScene(_gameSceneName);
    }
    #endregion



    #region Credits Button
    public void CreditsButton()
    {
        //play sound 
        _anim.SetTrigger("OpenCredits");
    }

    public void CreditsBackButton()
    {
        //play sound
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