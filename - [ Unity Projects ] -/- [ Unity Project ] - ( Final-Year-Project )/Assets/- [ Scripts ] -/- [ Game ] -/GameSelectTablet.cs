using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

// Scene loading game object component. On being activated the player will load the scene name given to it form the inspector.
public class GameSelectTablet : Interactable_Interface
{
    [SerializeField] private string _sceneName;
    private GameManager _gameManager;
    
    
    
    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    
    
    // Loads the scene with the same name as that of the _sceneName variable.
    public override void Activate()
    {
        _gameManager.LoadNewSubScene(_sceneName);
    }
}
