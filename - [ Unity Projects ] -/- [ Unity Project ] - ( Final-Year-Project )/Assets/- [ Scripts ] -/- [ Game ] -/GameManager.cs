using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Object _mainMenuScene;
    private Object _subScene;
    
    
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene(_mainMenuScene.name, LoadSceneMode.Additive);
        _subScene = _mainMenuScene;
    }

    public void LoadNewSubScene(Object newScene)
    {
        SceneManager.LoadScene(newScene.name, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(_subScene.name);
        _subScene = newScene;
    }
}
