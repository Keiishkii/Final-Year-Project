using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public InputActionReference _onQuit;
    
    [SerializeField] private string _mainMenuSceneName;
    private string _subSceneName;
    
    
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene(_mainMenuSceneName, LoadSceneMode.Additive);
        _subSceneName = _mainMenuSceneName;

        _onQuit.action.started += QuitGame;
    }

    public void LoadNewSubScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(_subSceneName);
        _subSceneName = sceneName;
    }

    private void QuitGame(InputAction.CallbackContext context)
    {
        Application.Quit();
    }
}
