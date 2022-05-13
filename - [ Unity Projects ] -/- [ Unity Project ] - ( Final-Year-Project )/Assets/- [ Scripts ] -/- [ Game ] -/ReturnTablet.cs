using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnTablet : Interactable_Interface
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