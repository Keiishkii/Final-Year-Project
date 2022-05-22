using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameSelectTablet : Interactable_Interface
{
    [SerializeField] private string _sceneName;
    private GameManager _gameManager;
    
    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    
    
    public override void Activate()
    {
        _gameManager.LoadNewSubScene(_sceneName);
    }
}
