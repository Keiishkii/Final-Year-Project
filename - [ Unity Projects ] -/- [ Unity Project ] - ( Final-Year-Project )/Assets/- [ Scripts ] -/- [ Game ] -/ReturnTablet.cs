using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

// Interactable game object component that loads the scene given to it from within the unity inspector.
// On loading this scene, also sets the players control state back to its basic value.
public class ReturnTablet : Interactable_Interface
{
    [SerializeField] private string _sceneName;
    private GameManager _gameManager;
    
    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    
    
    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.SetControlState = player.ControlStateBasic;
        }
    }

    

    public override void Activate()
    {
        _gameManager.LoadNewSubScene(_sceneName);
    }
}