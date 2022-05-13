using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameSelectTablet : Interactable_Interface
{
    [SerializeField] private Object _scene;
    private GameManager _gameManager;
    
    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    
    
    public override void Activate()
    {
        _gameManager.LoadNewSubScene(_scene);
    }
}
