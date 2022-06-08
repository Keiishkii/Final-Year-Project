using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// The global script for loading into the game and loading new scenes.
// This class manages the exciting of the game too.
public class GameManager : MonoBehaviour
{
    public InputActionReference _onQuit;
    
    [SerializeField] private string _mainMenuSceneName;
    private string _subSceneName;
    
    
    // Loads in the main menu on start in adaptive mode, so that this can be removed at a later time when entering the mini-games.
    // Also subscribes the script to the 'esc' key for the exiting of the program.
    void Start()
    {
        SceneManager.LoadScene(_mainMenuSceneName, LoadSceneMode.Additive);
        _subSceneName = _mainMenuSceneName;

        _onQuit.action.started += QuitGame;
    }

    // Removes the previous sub scene and loads in a new scene on top of the base scene.
    public void LoadNewSubScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(_subSceneName);
        _subSceneName = sceneName;
    }

    // Closes the game.
    private void QuitGame(InputAction.CallbackContext context)
    {
        Application.Quit();
    }
}
