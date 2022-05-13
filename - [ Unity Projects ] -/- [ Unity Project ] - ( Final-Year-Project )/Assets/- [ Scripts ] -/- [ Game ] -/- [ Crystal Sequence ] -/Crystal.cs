using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalSequence
{
    public class Crystal : Interactable_Interface
    {
        [SerializeField] private CrystalColours_Enum _colour;
        private MiniGameController_CrystalSequence _miniGameController;
        private ParticleSystem _particleSystem;
        private AudioSource _audioSource;



        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void Start()
        {
            _miniGameController = FindObjectOfType<MiniGameController_CrystalSequence>();
        }

        

        public override void Activate()
        {
            _miniGameController.ActivatedCrystal(_colour);
            _audioSource.PlayOneShot(_audioSource.clip);
            _particleSystem.Play();
        }
    }
}
