using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusRock : MonoBehaviour
{
    [SerializeField] private float maxWidth;
    [SerializeField] private RectTransform _rectTransform;
    private Player _player;


    
    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    
    
    private void Update()
    {
        float targetSpeed = Mathf.Lerp(0, maxWidth, _player.focusActivation);
        _rectTransform.sizeDelta = new Vector2(Mathf.Lerp(_rectTransform.sizeDelta.x, targetSpeed, 0.025f), _rectTransform.sizeDelta.y) ;
    }
}
