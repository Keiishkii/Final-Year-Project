using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalSequence
{
    // The interactable game object the player uses to select a given input.
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


        // Used as the override for the Interactable_Interfaces Activate function.
        // On the player interacting with this object and audio cue is played and the the colour enum given in the inspector is passed to the mini-game controller for Crystal Sequence.
        public override void Activate()
        {
            _miniGameController.ActivatedCrystal(_colour);
            _audioSource.PlayOneShot(_audioSource.clip);
            _particleSystem.Play();
        }
    }
}
